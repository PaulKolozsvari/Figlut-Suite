using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;
using SystemCF.Runtime.InteropServices;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Defines and creates new instances of classes during runtime. 
	/// </summary>
	public class TypeBuilder: Type
	{
		#region Private & Internal
		internal static TypeDefinition GetClassDef(string name, System.Reflection.TypeAttributes attr, Type parent, Type[] interfaces, TypeDefinition enclosingType, ModuleDefinition module)
		{
			if (((attr & System.Reflection.TypeAttributes.VisibilityMask) == System.Reflection.TypeAttributes.Public) || ((attr & System.Reflection.TypeAttributes.VisibilityMask) == System.Reflection.TypeAttributes.AutoLayout))
			{
				//throw new ArgumentException(Properties.Messages.Argument_BadNestedTypeFlags, "attr");
			}

			TypeDefinition td = GetClassDef(name, attr, parent, interfaces, module);
			enclosingType.NestedTypes.Add(td);

			return td;
		}

		internal static TypeDefinition GetClassDef(string name, System.Reflection.TypeAttributes attr, Type parent, Type[] interfaces, ModuleDefinition module)
		{
			bool isValueType = parent != null && parent == Type.ValueTypeType;
			if (isValueType)
				attr |= System.Reflection.TypeAttributes.Sealed;

			string _namespace = name.LastIndexOf('.') >= 0 ? name.Substring(0, name.LastIndexOf('.')) : null;
			string _name = name.Substring(name.LastIndexOf('.') + 1);

            TypeReference parentBaseType = null;
            if ((object)parent != null)
            {
                parentBaseType = parent.peType;
            }
            else
            {
                if ((attr & System.Reflection.TypeAttributes.Interface) == 0)//Not an interface, then the base type is object.
                {
                    parentBaseType = Type.ObjectType;
                }
                else
                {
                    parentBaseType = null;
                }
            }
            TypeDefinition td = new TypeDefinition(
                _name,
                _namespace,
                TypeAttributesConverter.Convert(attr),
                parentBaseType);

            //module.Types.Add(td = new TypeDefinition(_name, _namespace, TypeAttributesConverter.Convert(attr), (object) parent != null ? parent.peType : ((attr & System.Reflection.TypeAttributes.Interface) == 0) ? Type.ObjectType : null));
            module.Types.Add(td);
			if (interfaces != null)
			{
				foreach (Type interf in interfaces)
					td.Interfaces.Add(interf.peType);
			}

			return td;
		}

		internal static System.Type CreateType(ModuleBuilder module, string typeName)
		{
			System.Reflection.Assembly assembly = module.GetAssembly();
			return assembly.GetType(typeName);
		}

		internal static void DefinePInvokeMethod(MethodDefinition item, string dllName, string entryName, CallingConvention nativeCallConv, CharSet nativeCharSet, ModuleBuilder module)
		{
			PInvokeInfo pinvoke = new PInvokeInfo(item);
			item.PInvokeInfo = pinvoke;
			item.Attributes |= Mono.Cecil.MethodAttributes.PInvokeImpl;

			CallingConventionsConverter.Assign(pinvoke, nativeCallConv, nativeCharSet);
			pinvoke.EntryPoint = entryName;
			module.peModule.ModuleReferences.Add(pinvoke.Module = new ModuleReference(dllName));
		}
		
		internal bool completed;
		internal ModuleBuilder module;
		internal List<IMethodBuilder> methods = new List<IMethodBuilder>();
		private List<TypeBuilder> nested = new List<TypeBuilder>();
		internal Type parent;

		internal TypeDefinition TypeDef 
		{ 
			get { 
				return peType as TypeDefinition; 
			} 
		}

		internal TypeBuilder(string name, System.Reflection.TypeAttributes attr, Type parent, Type[] interfaces, TypeBuilder enclosingType, ModuleBuilder module)
		{
			peType = GetClassDef(name, attr, parent, interfaces, enclosingType.TypeDef, module.peModule);
			this.module = module;
			this.parent = parent != null ? parent : ((TypeDefinition) peType).BaseType;
		}
		internal TypeBuilder(string name, System.Reflection.TypeAttributes attr, Type parent, Type[] interfaces, ModuleBuilder module)
		{
			peType = GetClassDef(name, attr, parent, interfaces, module.peModule);
			this.module = module;
            this.parent = parent != null ? parent : ((TypeDefinition)peType).BaseType;
            if (parent != null)
            {
                this.parent = parent;
            }
            else
            {
                TypeReference parentBaseType = ((TypeDefinition)peType).BaseType;
                if (parentBaseType == Type.ObjectType)
                {
                    this.parent = typeof(System.Object);
                }
                else
                {
                    this.parent = parentBaseType;
                }
            }
		}

		internal bool AcceptDefaultConstructor()
		{
			return !IsInterface && !IsValueType;
		}

		internal void Complete()
		{
			if (completed)
				return;

			// Check default constructor
			if (TypeDef.Constructors.Count == 0 && AcceptDefaultConstructor())
				DefineDefaultConstructor(System.Reflection.MethodAttributes.Public);

			foreach (TypeBuilder nested in this.nested)
				nested.Complete();

			foreach (IMethodBuilder mb in methods)
			{
				mb.GetILGenerator().Complete();
			}

			completed = true;
		}
		#endregion

		/// <summary>
		/// Adds an interface that this type implements. 
		/// </summary>
		/// <param name="interfaceType">The interface that this type implements. </param>
		public void AddInterfaceImplementation(Type interfaceType)
		{
			if (!TypeDef.Interfaces.Contains(interfaceType.peType))
				TypeDef.Interfaces.Add(interfaceType.peType);
		}

		/// <summary>
		/// Creates a Type object for the class. After defining fields and methods on the class, CreateType is called in order to load its Type object. 
		/// </summary>
		/// <returns>** Returns this instance. **</returns>
		public Type CreateType()
		{
			Complete();
			return this;
		}

		/// <summary>
		/// Adds a new constructor to the type, with the given attributes and signature. 
		/// </summary>
		/// <param name="attributes">The attributes of the constructor. </param>
		/// <param name="callingConvention">The calling convention of the constructor. </param>
		/// <param name="parameterTypes">The types of the parameters of the constructor. </param>
		/// <returns>The defined constructor. </returns>
		public ConstructorBuilder DefineConstructor(System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, Type[] parameterTypes)
		{
			ConstructorBuilder cb = new ConstructorBuilder(attributes, callingConvention, parameterTypes, this);
			methods.Add(cb);
			return cb;
		}

		/// <summary>
		/// Defines the default constructor. The constructor defined here will simply call the default constructor of the parent. 
		/// </summary>
		/// <param name="attributes">A <see cref="MethodAttributes"/> object representing the attributes to be applied to the constructor. </param>
		/// <returns>Returns the constructor. </returns>
		public ConstructorBuilder DefineDefaultConstructor(System.Reflection.MethodAttributes attributes)
		{
			ConstructorBuilder cb = new ConstructorBuilder(attributes, System.Reflection.CallingConventions.Standard, null, this);
			
			ConstructorInfo con = parent.GetConstructor(Type.EmptyTypes, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
			ILGenerator iLGenerator = cb.GetILGenerator();
			iLGenerator.Emit(OpCodes.Ldarg_0);
			iLGenerator.Emit(OpCodes.Call, con);
			iLGenerator.Emit(OpCodes.Ret);
			return cb;
		}

		/// <summary>
		/// Adds a new event to the type, with the given name, attributes and event type. 
		/// </summary>
		/// <param name="name">The name of the event.</param>
		/// <param name="attributes">The attributes of the event. </param>
		/// <param name="eventtype">The type of the event. </param>
		/// <returns>The defined event. </returns>
		public EventBuilder DefineEvent(string name, System.Reflection.EventAttributes attributes, Type eventtype)
		{
			return new EventBuilder(name, attributes, eventtype, this);
		}

		/// <summary>
		/// Adds a new field to the type, with the given name, attributes and field type. 
		/// </summary>
		/// <param name="fieldName">The name of the field. </param>
		/// <param name="type">The type of the field </param>
		/// <param name="attributes">The attributes of the field. </param>
		/// <returns>The defined field. </returns>
		public FieldBuilder DefineField(string fieldName, Type type, System.Reflection.FieldAttributes attributes)
		{
			return new FieldBuilder(fieldName, type, attributes, this);
		}
		
		/// <summary>
		/// Defines the generic type parameters for the current type, specifying their number and their names, and returns an array of <see cref="GenericTypeParameterBuilder"/> objects that can be used to set their constraints. 
		/// </summary>
		/// <param name="names">An array of names for the generic type parameters.</param>
		/// <returns>An array of <see cref="GenericTypeParameterBuilder"/> objects that can be used to define the constraints of the generic type parameters for the current type. </returns>
		public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
		{
			GenericTypeParameterBuilder[] genPars = new GenericTypeParameterBuilder[names.Length];
			int i = 0;
			foreach (string name in names)
				(genPars[i] = new GenericTypeParameterBuilder(name, this)).GenPar.Position = i++;
			return genPars;
		}
			
		/// <summary>
		/// Defines initialized data field in the .sdata section of the portable executable (PE) file. 
		/// </summary>
		/// <param name="name">The name used to refer to the data. </param>
		/// <param name="data">The blob of data. </param>
		/// <param name="attributes">The attributes for the field. </param>
		/// <returns>A field to reference the data. </returns>
		public FieldBuilder DefineInitializedData(string name, byte[] data, System.Reflection.FieldAttributes attributes)
		{
			FieldBuilder builder = DefineUninitializedData(name, data.Length, attributes);
			builder.field.Constant = data;
			return builder;
		}
		
		/// <summary>
		/// Adds a new method to the type, with the specified name and method attributes. 
		/// </summary>
		/// <param name="name">The name of the method.</param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <returns>A <see cref="MethodBuilder"/> representing the newly defined method. </returns>
		public MethodBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes)
		{
			return DefineMethod(name, attributes, System.Reflection.CallingConventions.Standard, null, null);
		}

		/// <summary>
		/// Adds a new method to the type, with the specified name, method attributes, and calling convention.
		/// </summary>
		/// <param name="name">The name of the method.</param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <param name="callingConvention">The calling convention of the method. </param>
		/// <returns>A <see cref="MethodBuilder"/> representing the newly defined method. </returns>
		public MethodBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention)
		{
			return DefineMethod(name, attributes, callingConvention, null, null);
		}

		/// <summary>
		/// Adds a new method to the type, with the specified name, method attributes, and method signature. 
		/// </summary>
		/// <param name="name">The name of the method.</param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <param name="returnType">The return type of the method. </param>
		/// <param name="parameterTypes">The types of the parameters of the method. </param>
		/// <returns>A <see cref="MethodBuilder"/> representing the newly defined method. </returns>
		public MethodBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			return DefineMethod(name, attributes, System.Reflection.CallingConventions.Standard, returnType, parameterTypes);
		}

		/// <summary>
		/// Adds a new method to the type, with the specified name, method attributes, calling convention, and method signature. 
		/// </summary>
		/// <param name="name">The name of the method.</param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <param name="callingConvention">The calling convention of the method. </param>
		/// <param name="returnType">The return type of the method. </param>
		/// <param name="parameterTypes">The types of the parameters of the method. </param>
		/// <returns>A <see cref="MethodBuilder"/> representing the newly defined method. </returns>
		public MethodBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			MethodBuilder mb = new MethodBuilder(name, attributes, callingConvention, returnType, parameterTypes, this);
			methods.Add(mb);
			return mb;
		}
		
		/// <summary>
		/// Specifies a given method body that implements a given method declaration. 
		/// </summary>
		/// <param name="methodInfoBody">The method body to be used. </param>
		/// <param name="methodInfoDeclaration">The method whose declaration is to be used. </param>
		public void DefineMethodOverride(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration)
		{
		    ((MethodDefinition) methodInfoBody.peMeth).Overrides.Add(methodInfoDeclaration.peMeth);
		}
		
		/// <summary>
		/// Defines a nested type given its name. 
		/// </summary>
		/// <param name="name">The short name of the type. </param>
		/// <returns>The defined nested type. </returns>
		public TypeBuilder DefineNestedType(string name)
		{
			return DefineNestedType(name, System.Reflection.TypeAttributes.NestedPrivate, null, null, PackingSize.Unspecified, 0);
		}

		/// <summary>
		/// Defines a nested type given its name and attributes. 
		/// </summary>
		/// <param name="name">The short name of the type. </param>
		/// <param name="attr">The attributes of the type. </param>
		/// <returns>The defined nested type. </returns>
		public TypeBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr)
		{
			return DefineNestedType(name, attr, null, null, PackingSize.Unspecified, 0);
		}

		/// <summary>
		/// Defines a nested type given its name, attributes, and the type that it extends. 
		/// </summary>
		/// <param name="name">The short name of the type. </param>
		/// <param name="attr">The attributes of the type. </param>
		/// <param name="parent">The type that the nested type extends. </param>
		/// <returns>The defined nested type. </returns>
		public TypeBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, Type parent)
		{
			return DefineNestedType(name, attr, parent, null, PackingSize.Unspecified, 0);
		}

		/// <summary>
		/// Defines a nested type given its name, attributes, the total size of the type, and the type that it extends. 
		/// </summary>
		/// <param name="name">The short name of the type. </param>
		/// <param name="attr">The attributes of the type. </param>
		/// <param name="parent">The type that the nested type extends. </param>
		/// <param name="typeSize">The total size of the type. </param>
		/// <returns>The defined nested type. </returns>
		public TypeBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, Type parent, int typeSize)
		{
			return DefineNestedType(name, attr, parent, null, PackingSize.Unspecified, typeSize);
		}

		/// <summary>
		/// Defines a nested type given its name, attributes, the type that it extends, and the packing size.
		/// </summary>
		/// <param name="name">The short name of the type. </param>
		/// <param name="attr">The attributes of the type. </param>
		/// <param name="parent">The type that the nested type extends. </param>
		/// <param name="packSize">The packing size of the type. </param>
		/// <returns>The defined nested type. </returns>
		public TypeBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, Type parent, PackingSize packSize)
		{
			return DefineNestedType(name, attr, parent, null, packSize, 0);
		}

		/// <summary>
		/// Defines a nested type given its name, attributes, the type that it extends, and the interfaces that it implements. 
		/// </summary>
		/// <param name="name">The short name of the type. </param>
		/// <param name="attr">The attributes of the type. </param>
		/// <param name="parent">The type that the nested type extends. </param>
		/// <param name="interfaces">The interfaces that the nested type implements. </param>
		/// <returns>The defined nested type. </returns>
		public TypeBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, Type parent, Type[] interfaces)
		{
			return DefineNestedType(name, attr, parent, interfaces, PackingSize.Unspecified, 0);
		}

		/// <summary>
		/// Defines a nested type given its name, attributes, the type that it extends, the interfaces that it implements, the packing size and the total size of the type. 
		/// </summary>
		/// <param name="name">The short name of the type. </param>
		/// <param name="attr">The attributes of the type. </param>
		/// <param name="parent">The type that the nested type extends. </param>
		/// <param name="interfaces">The interfaces that the nested type implements. </param>
		/// <param name="packSize">The packing size of the type. </param>
		/// <param name="typeSize">The total size of the type. </param>
		/// <returns>The defined nested type. </returns>
		public TypeBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, Type parent, Type[] interfaces, PackingSize packSize, int typeSize)
		{
			TypeBuilder tb = new TypeBuilder(name, attr, parent, interfaces, this, module);
			if (packSize != PackingSize.Unspecified) tb.TypeDef.PackingSize = (ushort)packSize;
			if (typeSize != 0) tb.TypeDef.ClassSize = (uint)typeSize;
			nested.Add(tb);
			return tb;
		}

		/// <summary>
		/// Defines a PInvoke method given its name, the name of the DLL in which the method is defined, the attributes of the method, the calling convention of the method, the return type of the method, the types of the parameters of the method, and the PInvoke flags. 
		/// </summary>
		/// <param name="name">The name of the PInvoke method. </param>
		/// <param name="dllName">The name of the DLL in which the PInvoke method is defined. </param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <param name="callingConvention">The method's calling convention. </param>
		/// <param name="returnType">The method's return type. </param>
		/// <param name="parameterTypes">The types of the method's parameters. </param>
		/// <param name="nativeCallConv">The native calling convention.</param>
		/// <param name="nativeCharSet">The method's native character set. </param>
		/// <returns>The defined PInvoke method. </returns>
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			return DefinePInvokeMethod(name, dllName, name, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
		}
		
		/// <summary>
		/// Defines a PInvoke method given its name, the name of the DLL in which the method is defined, the name of the entry point, the attributes of the method, the calling convention of the method, the return type of the method, the types of the parameters of the method, and the PInvoke flags. 
		/// </summary>
		/// <param name="name">The name of the PInvoke method. </param>
		/// <param name="dllName">The name of the DLL in which the PInvoke method is defined. </param>
		/// <param name="entryName">The name of the entry point in the DLL. </param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <param name="callingConvention">The method's calling convention. </param>
		/// <param name="returnType">The method's return type. </param>
		/// <param name="parameterTypes">The types of the method's parameters. </param>
		/// <param name="nativeCallConv">The native calling convention.</param>
		/// <param name="nativeCharSet">The method's native character set. </param>
		/// <returns>The defined PInvoke method. </returns>
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			MethodBuilder mb = new MethodBuilder(name, attributes, callingConvention, returnType, parameterTypes, this);
			DefinePInvokeMethod(mb.peMeth as MethodDefinition, dllName, entryName, nativeCallConv, nativeCharSet, this.module);
			return mb;
   		}

		/// <summary>
		/// Adds a new property to the type, with the given name and property signature. 
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <param name="attributes">The attributes of the property. </param>
		/// <param name="returnType">The return type of the property. </param>
		/// <param name="parameterTypes">The types of the parameters of the property. </param>
		/// <returns>The defined property. </returns>
		public PropertyBuilder DefineProperty(string name, System.Reflection.PropertyAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			return new PropertyBuilder(name, attributes, returnType, parameterTypes, this);
		}
		
		/// <summary>
		/// Defines the initializer for this type. 
		/// </summary>
		/// <returns>Returns a type initializer. </returns>
		public ConstructorBuilder DefineTypeInitializer()
		{
			return new ConstructorBuilder(System.Reflection.MethodAttributes.Public, System.Reflection.CallingConventions.Standard, null, this);
		}
	
		/// <summary>
		/// Defines an uninitialized data field in the .sdata section of the portable executable (PE) file. 
		/// </summary>
		/// <param name="name">The name used to refer to the data.</param>
		/// <param name="size">The size of the data field. </param>
		/// <param name="attributes">The attributes for the field. </param>
		/// <returns>A field to reference the data</returns>
		public FieldBuilder DefineUninitializedData(string name, int size, System.Reflection.FieldAttributes attributes)
		{
			TypeDefinition type = module.GetTypeData(name, size);
			FieldBuilder builder = new FieldBuilder(name, type, attributes | System.Reflection.FieldAttributes.Static, this);
			return builder;
		}

		/// <summary>
		/// Returns the constructor of the specified constructed generic type that corresponds to the specified constructor of the generic type definition. 
		/// </summary>
		/// <param name="type">The constructed generic type whose constructor is returned.</param>
		/// <param name="constructor">A constructor on the generic type definition of <paramref name="type"/>, which specifies which constructor of <paramref name="type"/> to return.</param>
		/// <returns>A <see cref="ConstructorInfo"/> object that represents the constructor of <paramref name="type"/> corresponding to <paramref name="constructor"/>, which specifies a constructor belonging to the generic type definition of <paramref name="type"/>.</returns>
		public static ConstructorInfo GetConstructor(Type type, ConstructorInfo constructor)
		{
			if (!constructor.DeclaringType.IsGenericTypeDefinition)
			{
				throw new System.ArgumentException();//Properties.Messages.Argument_ConstructorNeedGenericDeclaringType, "constructor");
			}
			if (type.IsGenericTypeDefinition)
			{
				type = type.MakeGenericType(type.GetGenericArguments());
			}
			if (type.GetGenericTypeDefinition() != constructor.DeclaringType)
			{
				throw new System.ArgumentException();//Properties.Messages.Argument_InvalidConstructorDeclaringType, "type");
			}
			if (!(type is PEAPITypeSpec))
			{
				throw new System.ArgumentException();//Properties.Messages.Argument_NeedNonGenericType, "type");
			}

			PEAPITypeSpec tspec = (PEAPITypeSpec) type;

			MethodReference mref = new MethodReference(MethodDefinition.Ctor, type.peType, VoidType, true, false, MethodCallingConvention.Default);
			mref.CallingConvention |= MethodCallingConvention.Generic;
			foreach (ParameterInfo pi in constructor.GetParameters())
			{
				mref.Parameters.Add(new ParameterDefinition(tspec.Import(pi.ParameterType)));
			}
			ConstructorInfo ctor = new ConstructorInfo.PERefConstructorInfo(mref);
			return ctor;
		}

		/// <summary>
		/// Returns the field of the specified constructed generic type that corresponds to the specified field of the generic type definition. 
		/// </summary>
		/// <param name="type">The constructed generic type whose field is returned.</param>
		/// <param name="field">A field on the generic type definition of <paramref name="type"/>, which specifies which field of <paramref name="type"/> to return.</param>
		/// <returns>A <see cref="FieldInfo"/> object that represents the field of type corresponding to <paramref name="field"/>, which specifies a field belonging to the generic type definition of <paramref name="type"/>. </returns>
		public static FieldInfo GetField(Type type, FieldInfo field)
		{
			if (!field.DeclaringType.IsGenericTypeDefinition)
			{
				throw new System.ArgumentException();//Properties.Messages.Argument_FieldNeedGenericDeclaringType, "field");
			}
			if (type.IsGenericTypeDefinition)
			{
				type = type.MakeGenericType(type.GetGenericArguments());
			}
			if (type.GetGenericTypeDefinition() != field.DeclaringType)
			{
				throw new System.ArgumentException();//Properties.Messages.Argument_InvalidfieldDeclaringType, "type");
			}
			if (!(type is PEAPITypeSpec))
			{
				throw new System.ArgumentException();//Properties.Messages.Argument_NeedNonGenericType, "type");
			}

			PEAPITypeSpec tspec = (PEAPITypeSpec)type;

			FieldReference fref = new FieldReference(field.Name, type.peType, tspec.Import(field.FieldType));
			FieldInfo fi = new FieldInfo.PERefFieldInfo(fref);
			return fi;
		}

		/// <summary>
		/// Returns the method of the specified constructed generic type that corresponds to the specified method of the generic type definition. 
		/// </summary>
		/// <param name="type">The constructed generic type whose method is returned.</param>
		/// <param name="method">A method on the generic type definition of <paramref name="type"/>, which specifies which method of <paramref name="type"/> to return.</param>
		/// <returns>A <see cref="MethodInfo"/> object that represents the method of <paramref name="type"/> corresponding to <paramref name="method"/>, which specifies a method belonging to the generic type definition of <paramref name="type"/>.</returns>
		public static MethodInfo GetMethod(Type type, MethodInfo method)
		{
			if (!method.DeclaringType.IsGenericTypeDefinition)
			{
				throw new System.ArgumentException();//Properties.Messages.Argument_MethodNeedGenericDeclaringType, "constructor");
			}
			if (type.IsGenericTypeDefinition)
			{
				type = type.MakeGenericType(type.GetGenericArguments());
			}
			if (type.GetGenericTypeDefinition() != method.DeclaringType)
			{
				throw new System.ArgumentException();//Properties.Messages.Argument_InvalidMethodDeclaringType, "type");
			}
			if (!(type is PEAPITypeSpec))
			{
				throw new System.ArgumentException();//Properties.Messages.Argument_NeedNonGenericType, "type");
			}

			PEAPITypeSpec tspec = (PEAPITypeSpec)type;

			MethodReference mref = new MethodReference(method.Name, type.peType, tspec.Import(method.ReturnType), !method.IsStatic, method.IsStatic, MethodCallingConvention.Default);
			CallingConventionsConverter.Assign(mref, method.CallingConvention);
			mref.CallingConvention |= MethodCallingConvention.Generic;

			foreach (ParameterInfo pi in method.GetParameters())
			{
				mref.Parameters.Add(new ParameterDefinition(tspec.Import(pi.ParameterType)));

				if (pi.ParameterType.IsGenericType)
				{
					mref.GenericParameters.Add(new GenericParameter(pi.Name, mref));
				}
			}

			MethodInfo meth = new MethodInfo.PERefMethodInfo(mref);
			return meth;
		}

		/// <summary>
		/// **New method** to define a 'function pointer' type before used by calli.
		/// </summary>
		/// <param name="nativeCallConv">The native calling convention.</param>
		/// <param name="returnType">The method's return type. </param>
		/// <param name="parameterTypes">The types of the method's parameters. </param>
		/// <returns>A type representing a function pointer.</returns>
		public static Type BuildFunctionPointerType(CallingConvention nativeCallConv, Type returnType, Type[] parameterTypes)
		{
			FunctionPointerType fp = new FunctionPointerType(
					false,
					false,
					CallingConventionConverter.Convert(nativeCallConv),
					new MethodReturnType(returnType.peType));

			foreach (Type paramType in parameterTypes)
				fp.Parameters.Add(new ParameterDefinition(paramType.peType));

			return Type.Import(fp);
		}

		/// <summary>
		/// **New method** to define a 'function pointer' type before used by calli.
		/// </summary>
		/// <param name="callConvs">The calling convention.</param>
		/// <param name="returnType">The method's return type. </param>
		/// <param name="parameterTypes">The types of the method's parameters. </param>
		/// <returns>A type representing a function pointer.</returns>
		public static Type BuildFunctionPointerType(System.Reflection.CallingConventions callConvs, Type returnType, Type[] parameterTypes)
		{
			FunctionPointerType fp = new FunctionPointerType(
					(callConvs & System.Reflection.CallingConventions.HasThis) != 0,
					(callConvs & System.Reflection.CallingConventions.ExplicitThis) != 0,
					MethodCallingConvention.Default,
					new MethodReturnType(returnType.peType));

			foreach (Type paramType in parameterTypes)
				fp.Parameters.Add(new ParameterDefinition(paramType.peType));

			return Type.Import(fp);
		}

		/// <summary>
		/// Set a custom attribute using a custom attribute builder. 
		/// </summary>
		/// <param name="customBuilder">An instance of a helper class to define the custom attribute. </param>
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
	    	CustomAttribute ca;
	    	TypeDef.CustomAttributes.Add(ca = new CustomAttribute(customBuilder.ctor));
	    	foreach (object arg in customBuilder.args) ca.ConstructorParameters.Add(arg);
		}

		/// <summary>
		/// Sets a custom attribute using a specified custom attribute blob. 
		/// </summary>
		/// <param name="con">The constructor for the custom attribute.</param>
		/// <param name="binaryAttribute">A byte blob representing the attributes. </param>
	    public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
	    	CustomAttribute ca;
			TypeDef.CustomAttributes.Add(ca = new CustomAttribute(con.peMeth, binaryAttribute));
		}

		/// <summary>
		/// Sets the base type of the type currently under construction. 
		/// </summary>
		/// <param name="parent">The new base type.</param>
		public void SetParent(Type parent)
		{
			TypeDef.BaseType = parent.peType;
		}

		// Properties

		/// <summary>
		/// Retrieves the dynamic assembly that contains this type definition. 
		/// </summary>
		public override System.Reflection.Assembly Assembly { get { return this.module.GetAssembly(); } }

		/// <summary>
		/// Retrieves the dynamic module that contains this type definition. 
		/// </summary>
		public override Module Module { get { return this.module; } }

		/// <summary>
		/// Retrieves the packing size of this type. 
		/// </summary>
		public PackingSize PackingSize { get { return (PackingSize) TypeDef.PackingSize; } }

		/// <summary>
		/// Retrieves the total size of a type. 
		/// </summary>
		public int Size { get { return (int) TypeDef.ClassSize; } }

		/// <summary>
		/// Returns the underlying system type for this <see cref="TypeBuilder"/>. 
		/// </summary>
		public override Type UnderlyingSystemType { get { return this.peType; } }

		/// <summary>
		/// Save the <see cref="TypeBuilder"/> and get the CLR-equivalent <see cref="System.Type"/> representation.
		/// </summary>
		/// <param name="type">The <see cref="TypeBuilder"/> to convert.</param>
		/// <returns>A <see cref="System.Type"/> representation equivalent to the <paramref name="type"/> object.</returns>
		public static implicit operator System.Type(TypeBuilder type)
		{
            if (type == null)
            {
                return null;
            }

			// Get CLR Module
			System.Reflection.Module clrModule = type.module;

			// Return type from its name
			return clrModule.GetType(type.FullName);
		}
	}
}

