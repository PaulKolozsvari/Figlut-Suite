using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF
{
	/// <summary>
	/// Represents type declarations: class types, interface types, array types, value types, and enumeration types. 
	/// </summary>
	public abstract class Type
	{
		#region Private & Internal
		internal static Mono.Cecil.TypeReference VoidType { get { return Find(typeof(void)); } }
		internal static Mono.Cecil.TypeReference EnumType { get { return Find(typeof(System.Enum)); } }
		internal static Mono.Cecil.TypeReference ValueTypeType { get { return Find(typeof(System.ValueType)); } }
		internal static Mono.Cecil.TypeReference ExceptionType { get { return Find(typeof(System.Exception)); } }
		internal static Mono.Cecil.TypeReference ObjectType { get { return Find(typeof(object)); } }
		internal static Type ArrayType { get { return Type.Find(typeof(System.Array)); } }

		internal Type underlyingType;
		private Mono.Cecil.TypeReference _peType;
		internal virtual Mono.Cecil.TypeReference peType
		{
			get { 
				if (_peType == null) throw new System.NullReferenceException("peType");
				return _peType;
			}
			set {
				_peType = value;
			}
		}

		private static TypeDefinition GetTypeDefinition(Type[] types)
		{
			foreach (Type t in types)
				if (t is GenericTypeParameterBuilder)
					return ((GenericTypeParameterBuilder) t).GenPar.Owner as TypeDefinition;
			return null;
		}
		#endregion

		#region Equality
		/// <summary>
		/// Returns <c>true</c> if the objects are equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are equal, <c>false</c> otherwise.</returns>
		public static bool operator ==(Type t1, Type t2)
        {
            if ((object)t1 == null || (object)t2 == null) return (object)t1 == (object)t2;
			if (t1 is CLRType && t2 is CLRType) return ((CLRType)t1).clrType == ((CLRType)t2).clrType;
            return t1.peType == t2.peType;
        }

		/// <summary>
		/// Returns <c>true</c> if the objects are not equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=(Type t1, Type t2)
        {
            if ((object)t1 == null || (object)t2 == null) return (object)t1 != (object)t2;
			if (t1 is CLRType && t2 is CLRType) return ((CLRType)t1).clrType != ((CLRType)t2).clrType;
			return t1.peType != t2.peType;
        }
		
		/// <summary>
		/// Determines whether two <see cref="System.Object"/> instances are equal. 
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public override bool Equals(object o)
		{
		    if (o is Type) return this == (Type) o;
		    return false;
		}

		/// <summary>
		/// Serves as a hash function for a particular type. <see cref="GetHashCode"/> is suitable for use in hashing algorithms and data structures like a hash table. 
		/// </summary>
		/// <returns>A hash code for the current <see cref="Type"/>. </returns>
        public override int GetHashCode()
        {
			if (this is Type.CLRType)
				return ((CLRType)this).clrType.GetHashCode();
			return ((PEAPIType)this)._peType.GetHashCode();
		}
		#endregion

		/// <summary>
		/// Gets the underlying type code of the specified <see cref="Type"/>. 
		/// </summary>
		/// <param name="t">The <see cref="Type"/> whose underlying type code to get.</param>
		/// <returns>The <see cref="System.TypeCode"/> value of the underlying type.</returns>
		public static System.TypeCode GetTypeCode(Type t)
		{
			if ((object)t == null)
			{
				return System.TypeCode.Empty;
			}

		    if (t is Type.CLRType)
		        return System.Type.GetTypeCode(((CLRType) t));
            Type.PEAPIType type = (Type.PEAPIType)t;
            if (type.IsEnum) return System.TypeCode.Int32;
            return System.TypeCode.Object;
		}
		
		/// <summary>
		/// Gets the <see cref="System.Type"/> of the current instance. 
		/// </summary>
		/// <returns></returns>
		public new System.Type GetType()
		{
		    return typeof(object).GetType();
		}

		/// <summary>
		/// Determines whether the current Type derives from the specified <see cref="Type"/>. 
		/// </summary>
		/// <param name="type">The <see cref="Type"/> to compare with the current <see cref="Type"/>.</param>
		/// <returns><c>true</c> if the <see cref="Type"/> represented by the <paramref name="type"/> 
		/// parameter and the current <see cref="Type"/> represent classes, and the class represented by the current <see cref="Type"/> 
		/// derives from the class represented by <paramref name="type"/>; otherwise, <c>false</c>. This method also returns 
		/// <c>false</c> if <paramref name="type"/> and the current <see cref="Type"/> represent the same class.
		/// </returns>
        public bool IsSubclassOf(Type type)
        {
			Type baseType = this;
			if (baseType != type)
			{
				while (baseType != null)
				{
					if (baseType == type)
					{
						return true;
					}
					baseType = baseType.BaseType;
				}
				return false;
			}
			return false;
        }

		/// <summary>
		/// Determines whether an instance of the current <see cref="Type"/> can be assigned from an instance of the specified <see cref="Type"/>. 
		/// </summary>
		/// <param name="type">The <see cref="Type"/> to compare with the current <see cref="Type"/>.</param>
		/// <returns><c>true</c> if the <paramref name="type"/> parameter and the current <see cref="Type"/> represent the same type, or if the current <see cref="Type"/> is 
		/// in the inheritance hierarchy of <paramref name="type"/>, or if the current <see cref="Type"/> is an interface that <paramref name="type"/> supports. <c>false</c> if none 
		/// of these conditions are the case, or if <paramref name="type"/> is <c>null</c>.
		/// </returns>
        public bool IsAssignableFrom(Type type)
        {
			if (type == null)
			{
				return false;
			}

			if (peType == type.peType)
				return true;

            Type baseType = type.BaseType;
            while ((object)baseType != null)
            {
                if (baseType == this)
                    return true;
                baseType = baseType.BaseType;
            }

			if (this.IsInterface)
			{
				Type[] interfaces = type.GetInterfaces();
				foreach (Type interf in interfaces)
				{
					if (IsAssignableFrom(interf))
						return true;
				}
			}

			return false;
        }

		/// <summary>
		/// Invokes the specified member, using the specified binding constraints and matching the specified argument list. 
		/// </summary>
		/// <param name="name">The <see cref="System.String"/> containing the name of the constructor, method, property, or field member to invoke. 
		/// -or- 
		/// An empty string ("") to invoke the default member.
		/// </param>
		/// <param name="invokeAttr">A bitmask comprised of one or more <see cref="System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
		/// <param name="binder">A <see cref="System.Reflection.Binder"/> object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.</param>
		/// <param name="target">The <see cref="System.Object"/> on which to invoke the specified member.</param>
		/// <param name="args">An array containing the arguments to pass to the member to invoke.</param>
		/// <returns></returns>
		public object InvokeMember(string name, System.Reflection.BindingFlags invokeAttr, System.Reflection.Binder binder, object target, object[] args)
		{
			return target.GetType().InvokeMember(name, invokeAttr, binder, target, args);
		}
		
		/// <summary>
		/// Represents an empty array of type <see cref="Type"/>. This field is read-only. 
		/// </summary>
		public static readonly Type[] EmptyTypes = new Type[0];

		#region Virtual members
		/// <summary>
		/// Gets the <see cref="Assembly"/> that the type is declared in.
		/// </summary>
		public virtual System.Reflection.Assembly Assembly { get { return null; } }

		/// <summary>
		/// Gets the module (the DLL) in which the current <see cref="Type"/> is defined.
		/// </summary>
		public virtual Module Module { get { return null; } }

		/// <summary>
		/// Gets the name of this member.
		/// </summary>
		public virtual string Name { get { return peType.Name; } }

		/// <summary>
		/// Gets the fully qualified name of the <see cref="Type"/>, including the namespace of the <see cref="Type"/>.
		/// </summary>
		public virtual string FullName { get { return peType.FullName.Replace('/', '+'); } }

		/// <summary>
		/// Gets the type from which the current <see cref="Type"/> directly inherits.
		/// </summary>
		public virtual Type BaseType { 
			get {
				if (peType is ArrayType) return ArrayType;

				TypeDefinition @this = peType as TypeDefinition;
				if (@this == null) return null;
				
				if (@this.BaseType == null && !IsInterface) @this.BaseType = Type.ObjectType;
				return @this.BaseType; 
			} 
		}

		/// <summary>
		/// Gets the class that declares this member.
		/// </summary>
		public virtual Type DeclaringType { get { return peType.DeclaringType; } }

		/// <summary>
		/// Indicates the type provided by the common language runtime that represents this type.
		/// </summary>
		public virtual Type UnderlyingSystemType { get { return underlyingType; } }

		/// <summary>
		/// Gets a value indicating whether the <see cref="Type"/> is an array.
		/// </summary>
		public virtual bool IsArray { get { return peType is Mono.Cecil.ArrayType; } }

		/// <summary>
		/// Gets a value indicating whether the <see cref="Type"/> is passed by reference.
		/// </summary>
		public virtual bool IsByRef { get { return peType is ReferenceType; } }

		/// <summary>
		/// Gets a value indicating whether the <see cref="Type"/> is a class; that is, not a value type or interface.
		/// </summary>
		public virtual bool IsClass { 
            get {
                if (IsArray) return ArrayType.IsClass;
				return !IsValueType && (peType is TypeDefinition) && !((TypeDefinition) peType).IsInterface; 
            } 
        }

		/// <summary>
		/// Gets a value indicating whether the current <see cref="Type"/> represents an enumeration.
		/// </summary>
		public virtual bool IsEnum { 
            get {
                if (IsArray) return ArrayType.IsValueType;
                return (peType is TypeDefinition) && ((TypeDefinition)peType).BaseType == EnumType; 
            } 
        }

		/// <summary>
		/// Gets a value indicating whether the current <see cref="Type"/> represents a type parameter in the definition of a generic type or method. 
		/// </summary>
		public virtual bool IsGenericParameter { get { return peType.GenericParameters.Count > 0; } }

		/// <summary>
		/// Gets a value indicating whether the current type is a generic type. 
		/// </summary>
		public virtual bool IsGenericType { get { return peType.GenericParameters.Count > 0; } }

		/// <summary>
		/// Gets a value indicating whether the current <see cref="Type"/> represents a generic type definition, from which other generic types can be constructed. 
		/// </summary>
		public virtual bool IsGenericTypeDefinition { get { return !(peType is GenericInstanceType); } }

		/// <summary>
		/// Gets a value indicating whether the <see cref="Type"/> is an interface; that is, not a class or a value type.
		/// </summary>
		public virtual bool IsInterface
		{ 
            get {
                if (IsArray) return ArrayType.IsInterface;
                return (peType is TypeDefinition) && ((TypeDefinition)peType).IsInterface; 
            } 
        }

		/// <summary>
		/// Gets a value indicating whether the <see cref="Type"/> is declared public. 
		/// </summary>
		public virtual bool IsPublic
		{
			get	{
				return (peType is TypeDefinition) && ((TypeDefinition)peType).IsPublic;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="Type"/> is one of the primitive types.
		/// </summary>
		public virtual bool IsPrimitive { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether the <see cref="Type"/> is a value type.
		/// </summary>
		public virtual bool IsValueType { get { return peType.IsValueType; } }

		/// <summary>
		/// Gets the attributes associated with the <see cref="Type"/>. 
		/// </summary>
		public virtual System.Reflection.TypeAttributes Attributes { 
			get {
				if (!(peType is TypeDefinition))
					return (System.Reflection.TypeAttributes) 0;
				return TypeAttributesConverter.Convert(((TypeDefinition) peType).Attributes); 
			} 
		}

		/// <summary>
		/// Gets a combination of <see cref="GenericParameterAttributes"/> flags that describe the covariance and special constraints of the current generic type parameter. 
		/// </summary>
		public virtual SystemCF.Reflection.GenericParameterAttributes GenericParameterAttributes
		{ 
			get {
				if (!(peType is GenericParameter)) return SystemCF.Reflection.GenericParameterAttributes.None;
				return GenericParameterAttributesConverter.Convert(((GenericParameter)peType).Attributes);
			} 
		}

		/// <summary>
		/// Gets the number of dimensions in an <see cref="System.Array"/>.
		/// </summary>
		/// <returns>An <see cref="System.Int32"/> containing the number of dimensions in the current Type.</returns>
		public virtual int GetArrayRank() { return (int)((ArrayType)peType).Rank; }

		/// <summary>
		/// Searches for a public instance constructor whose parameters match the types in the specified array.
		/// </summary>
		/// <param name="argTypes">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the constructor to get.</param>
		/// <returns>A <see cref="ConstructorInfo"/> object representing the public instance constructor whose parameters match the types in the parameter type array, if found; otherwise, <c>null</c>.</returns>
		public virtual ConstructorInfo GetConstructor(Type[] argTypes)
		{
			if (IsArray) return ArrayType.GetConstructor(argTypes);

			TypeDefinition td = peType as TypeDefinition;
			foreach (MethodDefinition md in td.Constructors)
				if (md.Name == ".ctor")
				{
					Type[] parTypes = ParameterInfo.ToType(md.Parameters);
					if (Type.Match(parTypes, argTypes))
						return ConstructorInfo.Wrap(md);
				}
			return null;
		}

		/// <summary>
		/// Searches for a public instance constructor whose parameters match the types in the specified array, using the specified binding constraints.
		/// </summary>
		/// <param name="argTypes">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the constructor to get.</param>
		/// <param name="flags">A <see cref="System.Reflection.Binder"/> object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.</param>
		/// <returns>A <see cref="ConstructorInfo"/> object representing the public instance constructor whose parameters match the types in the parameter type array, if found; otherwise, <c>null</c>.</returns>
		public virtual ConstructorInfo GetConstructor(Type[] argTypes, System.Reflection.BindingFlags flags)
		{
			if (IsArray) return ArrayType.GetConstructor(argTypes, flags);

			TypeDefinition td = peType as TypeDefinition;
			ConstructorInfo ci = GetConstructor(argTypes);
			if (ci != null || ((flags & System.Reflection.BindingFlags.Static) == 0)) return ci;
			foreach (MethodDefinition md in td.Constructors)
				if (md.Name == ".cctor")
				{
					Type[] parTypes = ParameterInfo.ToType(md.Parameters);
					if (Type.Match(parTypes, argTypes))
						return ConstructorInfo.Wrap(md);
				}
			return null;
		}

		/// <summary>
		/// Returns all the public constructors defined for the current <see cref="Type"/>.
		/// </summary>
		/// <returns>An array of <see cref="ConstructorInfo"/> objects representing all the public constructors defined for the current Type, including the type initializer if it is defined.</returns>
		public virtual ConstructorInfo[] GetConstructors()
		{
			if (IsArray) return ArrayType.GetConstructors();

			TypeDefinition td = peType as TypeDefinition;
			List<MethodDefinition> ctors = new List<MethodDefinition>();
			foreach (MethodDefinition md in td.Constructors)
				ctors.Add(md);
			return ConstructorInfo.Wrap(ctors.ToArray());
		}

		/// <summary>
		/// Searches for the constructors defined for the current <see cref="Type"/>, using the specified <see cref="System.Reflection.BindingFlags"/>. 
		/// </summary>
		/// <param name="flags"></param>
		/// <returns>An array of <see cref="ConstructorInfo"/> objects representing all constructors defined for the current <see cref="Type"/> that match the specified binding constraints, including the type initializer if it is defined.</returns>
		public virtual ConstructorInfo[] GetConstructors(System.Reflection.BindingFlags flags)
		{
			if (IsArray) return ArrayType.GetConstructors(flags);

			if ((flags & System.Reflection.BindingFlags.Static) == 0)
				return GetConstructors();
			TypeDefinition td = peType as TypeDefinition;
			List<MethodDefinition> cctors = new List<MethodDefinition>();
			foreach (MethodDefinition md in td.Constructors)
				if (md.Name == ".cctor") cctors.Add(md);
			return ConstructorInfo.Wrap(cctors.ToArray());
		}

		/// <summary>
		/// Returns the <see cref="Type"/> of the object encompassed or referred to by the current array, pointer or reference type. 
		/// </summary>
		/// <returns>
		/// The <see cref="Type"/> of the object encompassed or referred to by the current array, pointer or reference type.
		/// -or- 
		/// <c>null</c> if the current <see cref="Type"/> is not an array or a pointer, or is not passed by reference.
		/// </returns>
		public virtual Type GetElementType()
		{
			if (peType is TypeSpecification)
				return ((TypeSpecification) peType).ElementType;
			return null;
		}

		/// <summary>
		/// Returns the <see cref="EventInfo"/> object representing the specified event. 
		/// </summary>
		/// <param name="name">The <see cref="System.String"/> containing the name of an event which is declared or inherited by the current <see cref="Type"/>. </param>
		/// <returns>The <see cref="EventInfo"/> object representing the specified event which is declared or inherited by the current <see cref="Type"/>, if found; otherwise, <c>null</c>.</returns>
		public virtual EventInfo GetEvent(string name)
		{
			if (IsArray) return ArrayType.GetEvent(name);

			TypeDefinition td = peType as TypeDefinition;
			foreach (EventDefinition ed in td.Events)
			{
				if (ed.Name == name)
					return EventInfo.Wrap(ed);
			}
			return null;
		}

		/// <summary>
		/// Teturns the <see cref="EventInfo"/> object representing the specified event, using the specified binding constraints. 
		/// </summary>
		/// <param name="name">The <see cref="System.String"/> containing the name of an event which is declared or inherited by the current <see cref="Type"/>. </param>
		/// <param name="flags">A bitmask comprised of one or more <see cref="System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
		/// <returns>The <see cref="EventInfo"/> object representing the specified event which is declared or inherited by the current <see cref="Type"/>, if found; otherwise, <c>null</c>.</returns>
		public virtual EventInfo GetEvent(string name, System.Reflection.BindingFlags flags)
		{
			return GetEvent(name);
		}

		/// <summary>
		/// Returns all the public events that are declared or inherited by the current <see cref="Type"/>. 
		/// </summary>
		/// <returns>An array of <see cref="EventInfo"/> objects representing all the public events which are declared or inherited by the current <see cref="Type"/>.
		/// -or- 
		/// An empty array of type <see cref="EventInfo"/>, if the current <see cref="Type"/> does not have public events.
		/// </returns>
		public virtual EventInfo[] GetEvents()
		{
			if (IsArray) return ArrayType.GetEvents();

			TypeDefinition td = peType as TypeDefinition;
			List<EventDefinition> events = new List<EventDefinition>();
			foreach (EventDefinition ed in td.Events)
				events.Add(ed);
			return EventInfo.Wrap(events.ToArray());
		}

		/// <summary>
		/// Searches for events that are declared or inherited by the current <see cref="Type"/>, using the specified binding constraints. 
		/// </summary>
		/// <param name="flags">A bitmask comprised of one or more <see cref="System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
		/// <returns>An array of <see cref="EventInfo"/> objects representing all the public events which are declared or inherited by the current <see cref="Type"/> that match the specified binding constraints.
		/// -or- 
		/// An empty array of type <see cref="EventInfo"/>, if the current <see cref="Type"/> does not have public events, or if none of the events match the binding constraints.
		/// </returns>
		public virtual EventInfo[] GetEvents(System.Reflection.BindingFlags flags)
		{
			return GetEvents();
		}

		/// <summary>
		/// Searches for the field with the specified name.
		/// </summary>
		/// <param name="name">The <see cref="System.String"/> containing the name of the data field to get. </param>
		/// <returns>A <see cref="FieldInfo"/> object representing the field with the specified name, if found; otherwise, <c>null</c>. </returns>
		public virtual FieldInfo GetField(string name)
		{
			if (IsArray) return ArrayType.GetField(name);

			TypeDefinition td = peType as TypeDefinition;
			foreach (FieldDefinition fd in td.Fields)
			{
				if (fd.Name == name)
					return FieldInfo.Wrap(fd);
			}
			return null;
		}

		/// <summary>
		/// Searches for the specified field, using the specified binding constraints. 
		/// </summary>
		/// <param name="name">The <see cref="System.String"/> containing the name of the data field to get. </param>
		/// <param name="flags">A bitmask comprised of one or more <see cref="System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
		/// <returns>A <see cref="FieldInfo"/> object representing the field that matches the specified requirements, if found; otherwise, <c>null</c>. </returns>
		public virtual FieldInfo GetField(string name, System.Reflection.BindingFlags flags)
		{
			return GetField(name);
		}

		/// <summary>
		/// Returns all the public fields of the current <see cref="Type"/>. 
		/// </summary>
		/// <returns>
		/// An array of FieldInfo objects representing all the public fields defined for the current <see cref="Type"/>.
		/// -or-
		/// An empty array of type <see cref="FieldInfo"/>, if no public fields are defined for the current <see cref="Type"/>.
		/// </returns>
		public virtual FieldInfo[] GetFields()
		{
			if (IsArray) return ArrayType.GetFields();

			TypeDefinition td = peType as TypeDefinition;
			List<FieldDefinition> fields = new List<FieldDefinition>();
			foreach (FieldDefinition fd in td.Fields)
				fields.Add(fd);
			return FieldInfo.Wrap(fields.ToArray());
		}

		/// <summary>
		/// Searches for the fields defined for the current <see cref="Type"/>, using the specified binding constraints. 
		/// </summary>
		/// <param name="flags">A bitmask comprised of one or more <see cref="System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
		/// <returns>
		/// An array of <see cref="FieldInfo"/> objects representing all fields defined for the current <see cref="Type"/> that match the specified binding constraints.
		/// -or- 
		/// An empty array of type <see cref="FieldInfo"/>, if no fields are defined for the current Type, or if none of the defined fields match the binding constraints.
		/// </returns>
		public virtual FieldInfo[] GetFields(System.Reflection.BindingFlags flags)
		{
			return GetFields();
		}

		/// <summary>
		/// Gets all the interfaces implemented or inherited by the current <see cref="Type"/>. 
		/// </summary>
		/// <returns>
		/// An array of <see cref="Type"/> objects representing all the interfaces implemented or inherited by the current <see cref="Type"/>.
		/// -or-
		/// An empty array of type <see cref="Type"/>, if no interfaces are implemented or inherited by the current <see cref="Type"/>.
		/// </returns>
		public virtual Type[] GetInterfaces() 
		{
			TypeDefinition @this = peType as TypeDefinition;
			if (@this == null)
				return Type.EmptyTypes;

			List<Type> interfaces = new List<Type>();
			foreach (TypeReference tr in @this.Interfaces)
				interfaces.Add(new PEAPIType(tr));
			return interfaces.ToArray();
		}

		/// <summary>
		/// Searches for the public method with the specified name. 
		/// </summary>
		/// <param name="name">The <see cref="System.String"/> containing the name of the public method to get. </param>
		/// <returns>A <see cref="MethodInfo"/> object representing the public method with the specified name, if found; otherwise, <c>null</c>. </returns>
		public virtual MethodInfo GetMethod(string name)
		{
			if (IsArray) return ArrayType.GetMethod(name);

			TypeDefinition td = peType as TypeDefinition;
			foreach (MethodDefinition md in td.Methods)
			{
				if (md.Name == name) return MethodInfo.Wrap(md);
			}
			return null;
		}

		/// <summary>
		/// Searches for the specified public method whose parameters match the specified argument types.
		/// </summary>
		/// <param name="name">The <see cref="System.String"/> containing the name of the public method to get. </param>
		/// <param name="argTypes">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
		/// <returns>A <see cref="MethodInfo"/> object representing the public method whose parameters match the specified argument types, if found; otherwise, <c>null</c>. </returns>
		public virtual MethodInfo GetMethod(string name, Type[] argTypes)
		{
			if (IsArray) return ArrayType.GetMethod(name, argTypes);

			TypeDefinition td = peType as TypeDefinition;
			foreach (MethodDefinition md in td.Methods)
				if (md.Name == name)
				{
					Type[] parTypes = ParameterInfo.ToType(md.Parameters);
					if (Type.Match(parTypes, argTypes))
						return MethodInfo.Wrap(md);
				}
			return null;
		}

		/// <summary>
		/// Searches for the specified method, using the specified binding constraints. 
		/// </summary>
		/// <param name="name">The <see cref="System.String"/> containing the name of the public method to get. </param>
		/// <param name="flags">A bitmask comprised of one or more <see cref="System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
		/// <returns>A <see cref="MethodInfo"/> object representing the method that matches the specified requirements, if found; otherwise, <c>null</c>. </returns>
		public virtual MethodInfo GetMethod(string name, System.Reflection.BindingFlags flags)
		{
			return GetMethod(name);
		}

		/// <summary>
		/// Searches for the specified method whose parameters match the specified argument types and modifiers, using the specified binding constraints. 
		/// </summary>
		/// <param name="name">The string containing the name of the method to get. </param>
		/// <param name="bindingAttr">A bitmask comprised of one or more <see cref="System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
		/// <param name="binder">A <see cref="System.Reflection.Binder"/> object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.</param>
		/// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
		/// <param name="modifiers">An array of <see cref="System.Reflection.ParameterModifier"/> objects representing the attributes associated with the corresponding element in the types array. The default binder does not process this parameter. </param>
		/// <returns>A <see cref="MethodInfo"/> object representing the method that matches the specified requirements, if found; otherwise, a null reference.</returns>
		public virtual MethodInfo GetMethod(string name, System.Reflection.BindingFlags bindingAttr, System.Reflection.Binder binder, Type[] types, System.Reflection.ParameterModifier[] modifiers)
		{
			return GetMethod(name, types);
		}

		/// <summary>
		/// Returns all the public methods of the current <see cref="Type"/>.
		/// </summary>
		/// <returns>
		/// An array of <see cref="MethodInfo"/> objects representing all the public methods defined for the current <see cref="Type"/>.
		/// -or- 
		/// An empty array of type <see cref="MethodInfo"/>, if no public methods are defined for the current <see cref="Type"/>.
		/// </returns>
		public virtual MethodInfo[] GetMethods()
		{
			if (IsArray) return ArrayType.GetMethods();

			TypeDefinition td = peType as TypeDefinition;
			List<MethodDefinition> meths = new List<MethodDefinition>();
            foreach (MethodDefinition md in td.Methods)
			    meths.Add(md);
			return MethodInfo.Wrap(meths.ToArray());
		}

		/// <summary>
		/// Searches for the methods defined for the current Type, using the specified binding constraints.
		/// </summary>
		/// <param name="flags">A bitmask comprised of one or more <see cref="System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
		/// <returns>
		/// An array of <see cref="MethodInfo"/> objects representing all methods defined for the current <see cref="Type"/> that match the specified binding constraints.
		/// -or- 
		/// An empty array of type <see cref="MethodInfo"/>, if no methods are defined for the current <see cref="Type"/>, or if none of the defined methods match the binding constraints.
		/// </returns>
		public virtual MethodInfo[] GetMethods(System.Reflection.BindingFlags flags)
		{
			return GetMethods();
		}

		/// <summary>
		/// Searches for the nested type with the specified name. 
		/// </summary>
		/// <param name="name">The <see cref="System.String"/> containing the name of the nested type to get. </param>
		/// <returns>A <see cref="Type"/> object representing the nested type with the specified name, if found; otherwise, <c>null</c>. </returns>
		public virtual Type GetNestedType(string className) 
		{
			int pos = className.IndexOf('+');
			string typeName = pos != -1 ? className.Substring(0, pos) : className;
			string subTypeName = pos != -1 ? className.Substring(pos) : null;

			TypeDefinition td = peType as TypeDefinition;
			foreach (TypeDefinition ntd in td.NestedTypes)
				if (ntd.Name == typeName)
				{
					Type nested = Type.Import(ntd);
					if (subTypeName == null)
						return nested;
					return nested.GetNestedType(subTypeName);
				}
			return null;
		}

		/// <summary>
		/// Searches for the specified nested type, using the specified binding constraints. 
		/// </summary>
		/// <param name="name">The <see cref="System.String"/> containing the name of the nested type to get. </param>
		/// <param name="flags">A bitmask comprised of one or more <see cref="System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
		/// <returns>A <see cref="Type"/> object representing the nested type that matches the specified requirements, if found; otherwise, <c>null</c>. </returns>
		public virtual Type GetNestedType(string name, System.Reflection.BindingFlags flags) 
		{ 
			return GetNestedType(name); 
		}

		/// <summary>
		/// Returns all the types nested within the current <see cref="Type"/>. 
		/// </summary>
		/// <returns>
		/// An array of <see cref="Type"/> objects representing all the types nested within the current <see cref="Type"/>.
		/// -or- 
		/// An empty array of type <see cref="Type"/>, if no types are nested within the current <see cref="Type"/>.
		/// </returns>
		public virtual Type[] GetNestedTypes() 
		{ 
			TypeDefinition td = peType as TypeDefinition;
			List<Type> list = new List<Type>();
			foreach (TypeDefinition ntd in td.NestedTypes)
				list.Add(Type.Import(ntd));
			return list.ToArray();
		}

		/// <summary>
		/// Searches for the types nested within the current Type, using the specified binding constraints. 
		/// </summary>
		/// <param name="flags">A bitmask comprised of one or more <see cref="System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
		/// <returns>
		/// An array of <see cref="Type"/> objects representing all the types nested within the current <see cref="Type"/> that match the specified binding constraints.
		/// -or- 
		/// An empty array of type <see cref="Type"/>, if no types are nested within the current <see cref="Type"/>, or if none of the nested types match the binding constraints.
		/// </returns>
		public virtual Type[] GetNestedTypes(System.Reflection.BindingFlags flags) 
		{ 
			return GetNestedTypes(); 
		}

		/// <summary>
		/// Returns all the public properties of the current <see cref="Type"/>. 
		/// </summary>
		/// <returns>
		/// An array of <see cref="PropertyInfo"/> objects representing all public properties of the current <see cref="Type"/>.
		/// -or- 
		/// An empty array of type <see cref="PropertyInfo"/>, if the current <see cref="Type"/> does not have public properties.
		/// </returns>
		public virtual PropertyInfo[] GetProperties()
		{
			if (IsArray) return ArrayType.GetProperties();

			TypeDefinition td = peType as TypeDefinition;
			List<PropertyDefinition> props = new List<PropertyDefinition>();
			foreach (PropertyDefinition pd in td.Properties)
				props.Add(pd);
			return PropertyInfo.Wrap(props.ToArray());
		}

		/// <summary>
		/// Searches for the properties of the current Type, using the specified binding constraints.
		/// </summary>
		/// <param name="flags">A bitmask comprised of one or more <see cref="System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
		/// <returns>
		/// An array of <see cref="PropertyInfo"/> objects representing all properties of the current <see cref="Type"/> that match the specified binding constraints.
		/// -or- 
		/// An empty array of type <see cref="PropertyInfo"/>, if the current <see cref="Type"/> does not have properties, or if none of the properties match the binding constraints.
		/// </returns>
		public virtual PropertyInfo[] GetProperties(System.Reflection.BindingFlags flags)
		{
			return GetProperties();
		}

		/// <summary>
		/// Searches for the public property with the specified name. 
		/// </summary>
		/// <param name="name">The <see cref="System.String"/> containing the name of the public property to get. </param>
		/// <returns>A <see cref="PropertyInfo"/> object representing the public property with the specified name, if found; otherwise, <c>null</c>.</returns>
		public virtual PropertyInfo GetProperty(string name)
		{
			if (IsArray) return ArrayType.GetProperty(name);

			TypeDefinition td = peType as TypeDefinition;
			foreach (PropertyDefinition pd in td.Properties)
			{
				if (pd.Name == name)
					return PropertyInfo.Wrap(pd);
			}
			return null;
		}

		/// <summary>
		/// Searches for the specified property, using the specified binding constraints. 
		/// </summary>
		/// <param name="name">The <see cref="System.String"/> containing the name of the public property to get. </param>
		/// <param name="flags">A bitmask comprised of one or more <see cref="System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
		/// <returns>A <see cref="PropertyInfo"/> object representing the property that matches the specified requirements, if found; otherwise, <c>null</c>. </returns>
		public virtual PropertyInfo GetProperty(string name, System.Reflection.BindingFlags flags)
		{
			return GetProperty(name);
		}

		/// <summary>
		/// Returns an array of <see cref="Type"/> objects that represent the type arguments of a generic type or the type parameters of a generic type definition. 
		/// </summary>
		/// <returns>
		/// An array of <see cref="Type"/> objects that represent the type arguments of a generic type. 
		/// -or-
		/// An empty array if the current type is not a generic type. 
		/// </returns>
		public virtual Type[] GetGenericArguments() 
		{
			if (!IsGenericType)
				return Type.EmptyTypes;

			List<Type> types = new List<Type>();
			foreach (TypeReference peType in this.peType.GenericParameters)
				types.Add(Type.Import(peType));

			return types.ToArray();
		}

		/// <summary>
		/// Returns an array of <see cref="Type"/> objects that represent the constraints on the current generic type parameter.
		/// </summary>
		/// <returns>An array of <see cref="Type"/> objects that represent the constraints on the current generic type parameter. </returns>
		public virtual Type[] GetGenericParameterConstraints()
		{
			if (!IsGenericType) 
				return Type.EmptyTypes;

			List<Type> types = new List<Type>();
			foreach (GenericParameter genPar in peType.GenericParameters)
				foreach (TypeReference tref in genPar.Constraints)
					types.Add(Type.Import(tref));

			return types.ToArray();
		}

		/// <summary>
		/// Returns a <see cref="Type"/> object that represents a generic type definition from which the current generic type can be constructed. 
		/// </summary>
		/// <returns>A <see cref="Type"/> object representing a generic type from which the current type can be constructed. </returns>
		public virtual Type GetGenericTypeDefinition() 
		{
			return Type.Import(peType.GetOriginalType());
		}

		/// <summary>
		/// Returns a <see cref="Type"/> object representing a one-dimensional array of the current type, with a lower bound of zero. 
		/// </summary>
		/// <returns>A <see cref="Type"/> object representing a one-dimensional array of the current type, with a lower bound of zero.</returns>
		public virtual Type MakeArrayType() { return Type.Import(new Mono.Cecil.ArrayType(peType)); }

		/// <summary>
		/// Returns a <see cref="Type"/> object representing an array of the current type, with the specified number of dimensions. 
		/// </summary>
		/// <param name="rank">The number of dimensions for the array. </param>
		/// <returns>A <see cref="Type"/> object representing an array of the current type, with the specified number of dimensions. </returns>
		public virtual Type MakeArrayType(int rank) 
		{ 
			ArrayType at = new Mono.Cecil.ArrayType(peType); 
			for (int i = 0; i < rank; i++)
				at.Dimensions.Add(new ArrayDimension(0, 0));
			return Type.Import(at); 
		}

		/// <summary>
		/// Returns a <see cref="Type"/> object that represents the current type when passed as a <c>ref</c> parameter (<c>ByRef</c> parameter in Visual Basic).
		/// </summary>
		/// <returns>a <see cref="Type"/> object that represents the current type when passed as a <c>ref</c> parameter (<c>ByRef</c> parameter in Visual Basic).</returns>
		public virtual Type MakeByRefType() { return Type.Import(new Mono.Cecil.ReferenceType(peType)); }

		/// <summary>
		/// Substitutes the elements of an array of types for the type parameters of the current generic type definition and returns a Type object representing the resulting constructed type. 
		/// </summary>
		/// <param name="typeArguments">An array of types to be substituted for the type parameters of the current generic type.</param>
		/// <returns>A <see cref="Type"/> representing the constructed type formed by substituting the elements of <paramref name="typeArguments"/> for the type parameters of the current generic type. </returns>
		public virtual Type MakeGenericType(params Type[] typeArguments)
		{
			GenericInstanceType instance = new GenericInstanceType(peType);
			foreach (Type t in typeArguments)
				instance.GenericArguments.Add(t.peType);
			return new PEAPITypeSpec(instance, this is TypeBuilder ? ((TypeBuilder) this).TypeDef : GetTypeDefinition(typeArguments));
		}

		/// <summary>
		/// Returns a <see cref="Type"/> object that represents a pointer to the current type. 
		/// </summary>
		/// <returns>A <see cref="Type"/> object that represents a pointer to the current type. </returns>
		public virtual Type MakePointerType() { return Type.Import(new Mono.Cecil.PointerType(peType)); }
	
		/// <summary>
		/// Get a string representation of this object.
		/// </summary>
		public override string ToString()
		{
			if (!IsGenericType)
			{
				return FullName;
			}

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append(FullName);
			sb.Append("<");

			GenericParameterCollection genPars = peType.GenericParameters;
			for (int i = 0; i < genPars.Count; i++)
			{
				if (i > 0) sb.Append(",");
				sb.Append(genPars[ i ].FullName);
			}

			sb.Append(">");

			return sb.ToString();
		}
		#endregion

		#region Inner Classes
		internal class CLRType: Type
		{
			internal System.Type clrType;
			private bool imported = false;

			internal CLRType(System.Type clrType)
			{ 
				this.clrType = clrType;
			}

			internal override Mono.Cecil.TypeReference peType
			{
				get { 
					if (!imported) base.peType = Type.Find(clrType);
					return base.peType;
				}
				set {
					base.peType = value;
				}				
			}

			public override System.Reflection.Assembly Assembly { get { return clrType.Assembly; } }
			public override Module Module { get { return null; } }
			public override string Name { get { return clrType.Name; } }
			public override string FullName { get { return clrType.FullName; } }
			public override Type BaseType { get { return clrType.BaseType; } }
			public override Type DeclaringType { get { return clrType.DeclaringType; } }
			public override Type UnderlyingSystemType { get { return clrType.UnderlyingSystemType; } }
			public override bool IsArray { get { return clrType.IsArray; } }
			public override bool IsByRef { get { return clrType.IsByRef; } }
			public override bool IsClass { get { return clrType.IsClass; } }
			public override bool IsEnum { get { return clrType.IsEnum; } }
			public override bool IsGenericType { get { return clrType.IsGenericType; } }
			public override bool IsGenericParameter { get { return clrType.IsGenericParameter; } }
			public override bool IsGenericTypeDefinition { get { return clrType.IsGenericTypeDefinition; } }
			public override bool IsInterface { get { return clrType.IsInterface; } }
			public override bool IsPublic { get { return clrType.IsPublic; } }
			public override bool IsPrimitive { get { return clrType.IsPrimitive; } }
			public override bool IsValueType { get { return clrType.IsValueType; } }
			public override System.Reflection.TypeAttributes Attributes { get { return (System.Reflection.TypeAttributes)clrType.Attributes; } }
			public override SystemCF.Reflection.GenericParameterAttributes GenericParameterAttributes { get { return SystemCF.Reflection.GenericParameterAttributes.None; } }
			public override int GetArrayRank() { return clrType.GetArrayRank(); }
			public override ConstructorInfo GetConstructor(Type[] argTypes)
			{
				return ConstructorInfo.Wrap(clrType.GetConstructor(Type.Unwrap(argTypes)));
			}
			public override ConstructorInfo GetConstructor(Type[] argTypes, System.Reflection.BindingFlags flags)
			{
				foreach (System.Reflection.ConstructorInfo ci in clrType.GetConstructors(flags))
				{
					Type[] parTypes = ParameterInfo.ToType(ci.GetParameters());
					if (Type.Match(parTypes, argTypes)) return ConstructorInfo.Wrap(ci);
				}
				return null;
			}
			public override ConstructorInfo[] GetConstructors()
			{
				return ConstructorInfo.Wrap(clrType.GetConstructors());
			}
			public override ConstructorInfo[] GetConstructors(System.Reflection.BindingFlags flags)
			{
				return ConstructorInfo.Wrap(clrType.GetConstructors(flags));
			}
			public override Type GetElementType() { return clrType.GetElementType(); }
			public override EventInfo GetEvent(string name)
			{
				return EventInfo.Wrap(clrType.GetEvent(name));
			}
			public override EventInfo GetEvent(string name, System.Reflection.BindingFlags flags)
			{
				return EventInfo.Wrap(clrType.GetEvent(name, flags));
			}
			public override EventInfo[] GetEvents()
			{
				return EventInfo.Wrap(clrType.GetEvents());
			}
			public override EventInfo[] GetEvents(System.Reflection.BindingFlags flags)
			{
				return EventInfo.Wrap(clrType.GetEvents(flags));
			}
			public override FieldInfo GetField(string name)
			{
				return FieldInfo.Wrap(clrType.GetField(name));
			}
			public override FieldInfo GetField(string name, System.Reflection.BindingFlags flags)
			{
				return FieldInfo.Wrap(clrType.GetField(name, flags));
			}
			public override FieldInfo[] GetFields()
			{
				return FieldInfo.Wrap(clrType.GetFields());
			}
			public override FieldInfo[] GetFields(System.Reflection.BindingFlags flags)
			{
				return FieldInfo.Wrap(clrType.GetFields(flags));
			}
			public override Type[] GetInterfaces() { return Type.Wrap(clrType.GetInterfaces()); }
			public override MethodInfo GetMethod(string name)
			{
				return MethodInfo.Wrap(clrType.GetMethod(name));
			}
			public override MethodInfo GetMethod(string name, System.Reflection.BindingFlags flags)
			{
				return MethodInfo.Wrap(clrType.GetMethod(name, flags));
			}
			public override MethodInfo GetMethod(string name, Type[] argTypes)
			{
				return MethodInfo.Wrap(clrType.GetMethod(name, Type.Unwrap(argTypes)));
			}
			public override MethodInfo GetMethod(string name, System.Reflection.BindingFlags bindingAttr, System.Reflection.Binder binder, Type[] types, System.Reflection.ParameterModifier[] modifiers)
			{
				return MethodInfo.Wrap(clrType.GetMethod(name, bindingAttr, binder, Type.Unwrap(types), modifiers));
			}
			public override MethodInfo[] GetMethods()
			{
				return MethodInfo.Wrap(clrType.GetMethods());
			}
			public override MethodInfo[] GetMethods(System.Reflection.BindingFlags flags)
			{
				return MethodInfo.Wrap(clrType.GetMethods(flags));
			}
			public override Type GetNestedType(string name) { return Type.Import(clrType.GetNestedType(name, System.Reflection.BindingFlags.Public)); }
			public override Type GetNestedType(string name, System.Reflection.BindingFlags flags) { return Type.Import(clrType.GetNestedType(name, flags)); }
			public override Type[] GetNestedTypes() { return Type.Wrap(clrType.GetNestedTypes(System.Reflection.BindingFlags.Public)); }
			public override Type[] GetNestedTypes(System.Reflection.BindingFlags flags) { return Type.Wrap(clrType.GetNestedTypes(flags)); }
			public override PropertyInfo GetProperty(string name)
			{
				return PropertyInfo.Wrap(clrType.GetProperty(name));
			}
			public override PropertyInfo GetProperty(string name, System.Reflection.BindingFlags flags)
			{
				return PropertyInfo.Wrap(clrType.GetProperty(name, flags));
			}
			public override PropertyInfo[] GetProperties()
			{
				return PropertyInfo.Wrap(clrType.GetProperties());
			}
			public override PropertyInfo[] GetProperties(System.Reflection.BindingFlags flags)
			{
				return PropertyInfo.Wrap(clrType.GetProperties(flags));
			}
			public override Type[] GetGenericArguments() { return Type.Wrap(clrType.GetGenericArguments()); }
			public override Type[] GetGenericParameterConstraints() { return Type.EmptyTypes; }
			public override Type MakeGenericType(params Type[] typeArguments)
			{
				if (AreCLRTypes(typeArguments)) return clrType.MakeGenericType(Type.Unwrap(typeArguments));

				// Create a new PEType
				TypeReference tref = _module.Import(clrType);
				return Type.Import(tref).MakeGenericType(typeArguments);
			}
			public override Type GetGenericTypeDefinition() { return Type.Import(clrType.GetGenericTypeDefinition()); }
			public override Type MakeArrayType() { return MakeArrayType(1); }
			public override Type MakeArrayType(int rank)
			{
				string r = "[";
				for (int i = 1; i < rank; i++)
					r += ",";
				r += "]";
				return clrType.Assembly.GetType(clrType.FullName + r);
			}
			public override Type MakeByRefType() { return clrType.Assembly.GetType(clrType.FullName + "&"); }
			public override Type MakePointerType() { return clrType.Assembly.GetType(clrType.FullName + "*"); }

			public override string ToString()
			{
				return clrType.ToString();
			}

			// Private
			private static bool AreCLRTypes(Type[] types)
			{
				foreach (Type t in types)
					if (t is PEAPIType) return false;
				return true;
			}
		}

		internal class PEAPIType : Type
		{
			internal PEAPIType(Mono.Cecil.TypeReference peType) { this.peType = peType; }
		}

		internal class PEAPITypeSpec: PEAPIType
		{
			internal GenericInstanceType spec;
			internal TypeDefinition tdef;
			public PEAPITypeSpec(GenericInstanceType spec, TypeDefinition tdef)
				: base(spec) 
			{
				this.spec = spec;
				this.tdef = tdef;
			}

			internal Mono.Cecil.TypeReference Import(Type type)
			{
				if (type is CLRType)
					return _module.Import((CLRType) type, tdef);
				return ((PEAPIType) type).peType;
			}
		}

		#endregion

		#region Cast & Wrap	
		/// <summary>
		/// Cast a <see cref="System.Type"/> into a <see cref="Type"/>.
		/// </summary>
		/// <param name="clrType">A <see cref="System.Type"/> to be casted.</param>
		/// <returns>A <see cref="Type"/>.</returns>
		public static implicit operator Type(System.Type clrType) {	return Import(clrType);	}

		/// <summary>
		/// Cast a <see cref="Mono.Cecil.TypeReference"/> into a <see cref="Type"/>.
		/// </summary>
		/// <param name="peType">A <see cref="Mono.Cecil.TypeReference"/> to be casted.</param>
		/// <returns>A <see cref="Type"/>.</returns>
		public static implicit operator Type(Mono.Cecil.TypeReference peType) { return Import(peType); }

		/// <summary>
		/// Cast a <see cref="Type"/> into a <see cref="System.Type"/>.
		/// </summary>
		/// <param name="type">A <see cref="Type"/> to be casted.</param>
		/// <returns>A <see cref="System.Type"/>.</returns>
		public static implicit operator System.Type(Type type) 
		{
			if (type == null) return null;

		    if (type is CLRType) return ((CLRType) type).clrType;

			if (type is TypeBuilder)
				return (TypeBuilder) type;

		    return System.Type.GetType(type.FullName);
		}

		internal static Type[] Wrap(System.Type[] types)
		{
			if (types == null) return null;

			Type[] tmp = new Type[ types.Length ];
			for (int i = 0; i < types.Length; i++)
				tmp[ i ] = Type.Import(types[ i ]);
			return tmp;
		}
		internal static Type[] Wrap(Mono.Cecil.TypeReference[] types)
		{
			if (types == null) return null;

			Type[] tmp = new Type[ types.Length ];
			for (int i = 0; i < types.Length; i++)
				tmp[ i ] = Type.Import(types[ i ]);
			return tmp;
		}

		internal static System.Type Unwrap(Type type)
		{
		    return (System.Type) type;
		}

		internal static System.Type[] Convert(Type[] types)
		{
		    return Unwrap(types);
		}
		internal static System.Type[] Unwrap(Type[] types)
		{
			if (types == null) return null;

			System.Type[] tmp = new System.Type[ types.Length ];
			for (int i = 0; i < types.Length; i++)
				tmp[ i ] = (System.Type) types[ i ];
			return tmp;
		}
		
		internal static Mono.Cecil.TypeReference[] UnwrapPE(Type[] types)
		{
			if (types == null) return null;

			Mono.Cecil.TypeReference[] tmp = new Mono.Cecil.TypeReference[ types.Length ];
			for (int i = 0; i < types.Length; i++)
				tmp[ i ] = types[ i ].peType;
			return tmp;
		}

		internal static bool Match(Type[] parTypes, Type[] argTypes)
		{
			if (parTypes == null || argTypes == null) return parTypes == argTypes;

			if (parTypes.Length != argTypes.Length)
				return false;
			for (int i = 0; i < argTypes.Length; i++)
				if (parTypes[ i ].peType != argTypes[ i ].peType)
					return false;
			return true;
		}
		#endregion

		#region References
		internal static Mono.Cecil.ModuleDefinition _module;

        internal static Mono.Cecil.TypeReference Find(System.Type type)
        {
            return _module.Import(type);
        }

		internal static Mono.Cecil.FieldReference Find(System.Reflection.FieldInfo field)
		{
            return _module.Import(field);
		}

		internal static Mono.Cecil.TypeReference[] Find(System.Reflection.ParameterInfo[] pars)
		{
			Mono.Cecil.TypeReference[] t = new Mono.Cecil.TypeReference[pars.Length];
			for (int i = 0; i < pars.Length; i++)
				t[i] = Find(pars[i].ParameterType);
			return t;
		}

		internal static Mono.Cecil.MethodReference Find(System.Reflection.MethodBase meth)
		{
            return _module.Import(meth);
		}
		#endregion

		#region Import
		private static Dictionary<object, Type> types = new Dictionary<object, Type>();
		internal static Type Import(System.Type type)
		{
			if (type == null)
				return null;
			if (types.ContainsKey(type))
				return types[ type ];
			return types[ type ] = new Type.CLRType(type);
		}
		internal static Type Import(Mono.Cecil.TypeReference type)
		{
			if (type == null)
				return null;

			if (types.ContainsKey(type))
				return types[ type ];
			return types[ type ] = new Type.PEAPIType(type);
		}

		#endregion
	}
}
