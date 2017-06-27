using System.Collections.Generic;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;
using SystemCF.Runtime.InteropServices;
using SystemCF.Diagnostics.SymbolStore;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Defines and represents a module. 
	/// </summary>
	public class ModuleBuilder : Module
	{
		#region Private & Internal
		internal List<TypeBuilder> types = new List<TypeBuilder>();
		internal List<StreamedResource> resources = new List<StreamedResource>();
		internal AssemblyBuilder assem;
		internal System.Reflection.Assembly clrAssembly;
		internal bool completed;

		internal ModuleBuilder(Mono.Cecil.ModuleDefinition peFile, AssemblyBuilder assem) 
		{ 
			this.peModule = peFile; 
			this.assem = assem; 
		}

		internal System.Reflection.Assembly GetAssembly()
		{
            if (clrAssembly == null)
            {
                assem.Save(peModule);
                clrAssembly = System.Reflection.Assembly.LoadFrom(assem.assemblyFileName);
            }
            assem.Save(peModule);
            clrAssembly = System.Reflection.Assembly.LoadFrom(assem.assemblyFileName);
			return clrAssembly;
		}

		internal void Complete()
		{
            if (completed) return;

			foreach (TypeBuilder tb in types)
				tb.Complete();
			foreach (StreamedResource res in resources)
				res.Complete();

			completed = true;
		}

		internal TypeDefinition GetTypeData(string name, int size)
		{
			string strTypeName = "$ArrayType$" + size;
			TypeDefinition type = null;
			foreach (TypeDefinition td in peModule.Types)
				if (td.Name == name)
				{
					type = td;
					break;
				}

			if (type == null)
			{
				System.Reflection.TypeAttributes attr = System.Reflection.TypeAttributes.Sealed | System.Reflection.TypeAttributes.ExplicitLayout | System.Reflection.TypeAttributes.Public;
				peModule.Types.Add(type = new TypeDefinition(strTypeName, null, TypeAttributesConverter.Convert(attr), Type.ValueTypeType));
				type.PackingSize = (ushort)PackingSize.Size1;
				type.ClassSize = (uint)size;
			}
			return type;
		}
		#endregion

		// Properties

		/// <summary>
		/// Gets the appropriate <see cref="Assembly"/> for this instance of <see cref="Module"/>. 
		/// </summary>
		public override Assembly Assembly
		{
		    get {
		        return assem;
		    }
		}

		// Methods

		/// <summary>
		/// Completes the global function definitions and global data definitions for this dynamic module. 
		/// </summary>
		public void CreateGlobalFunctions()
		{
			if (completed)
			{
				throw new System.InvalidOperationException("This method was called previously");
			}

			Complete();
		}

		/// <summary>
		/// Defines an enumeration type with that is a value type with a single non-static field called value__ of the specified type. 
		/// </summary>
		/// <param name="name">The full path of the enumeration type. </param>
		/// <param name="visibility">The type attributes for the enumeration. </param>
		/// <param name="underlyingType">The underlying type for the enumeration. </param>
		/// <returns>Returns the defined enumeration.</returns>
		public EnumBuilder DefineEnum(string name, System.Reflection.TypeAttributes visibility, Type underlyingType)
		{
			return new EnumBuilder(name, visibility, underlyingType, this);
		}

		/// <summary>
		/// Defines a global method given its name, attributes, return type, and parameter types. 
		/// </summary>
		/// <param name="name">The name of the method.</param>
		/// <param name="attributes">The attributes of the method. Must include <see cref="MethodAttributes.Static"/>. </param>
		/// <param name="returnType">The return type of the method. </param>
		/// <param name="parameterTypes">The types of the method's parameters. </param>
		/// <returns>Returns the defined global method. </returns>
		public MethodBuilder DefineGlobalMethod(string name, System.Reflection.MethodAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			return DefineGlobalMethod(name, attributes, System.Reflection.CallingConventions.Standard, returnType, parameterTypes);
		}

		/// <summary>
		/// Defines a global method given its name, attributes, calling convention, return type, and parameter types. 
		/// </summary>
		/// <param name="name">The name of the method.</param>
		/// <param name="attributes">The attributes of the method. Must include <see cref="MethodAttributes.Static"/>. </param>
		/// <param name="callingConvention">The calling convention for the method.</param>
		/// <param name="returnType">The return type of the method. </param>
		/// <param name="parameterTypes">The types of the method's parameters. </param>
		/// <returns>Returns the defined global method. </returns>
		public MethodBuilder DefineGlobalMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			if (completed)
			{
				throw new System.InvalidOperationException("CreateGlobalFunctions has been previously called");
			}

			return new MethodBuilder(name, attributes, callingConvention, returnType, parameterTypes, this);
		}
		
		/// <summary>
		/// Defines an initialized data field in the .sdata section of the portable executable (PE) file. 
		/// </summary>
		/// <param name="name">The name used to refer to the data.</param>
		/// <param name="data">The blob of data. </param>
		/// <param name="attributes">The attributes for the field. The default is <see cref="MethodAttributes.Static"/>. </param>
		/// <returns>A field to reference the data. </returns>
		public FieldBuilder DefineInitializedData(string name, byte[] data, System.Reflection.FieldAttributes attributes)
		{
			if (completed)
			{
				throw new System.InvalidOperationException("CreateGlobalFunctions has been previously called");
			}

			FieldBuilder builder = DefineUninitializedData(name, data.Length, attributes | System.Reflection.FieldAttributes.HasDefault);
			builder.field.Constant = data;
			return builder;
		}

		/// <summary>
		/// Defines a manifest resource blob to be embedded in the dynamic assembly. 
		/// </summary>
		/// <param name="name">The case-sensitive name for the resource.</param>
		/// <param name="stream">A stream that contains the bytes for the resource.</param>
		/// <param name="attribute">A <see cref="ResourceAttributes"/> value that specifies whether the resource is public or private.</param>
		public void DefineManifestResource(string name, Stream stream, ResourceAttributes attribute)
		{
			EmbeddedResource res = new EmbeddedResource(name, ResourceAttributesConverter.Convert(attribute));
			peModule.Resources.Add(res);
			this.resources.Add(new StreamedResource(res, stream));
		}

		/// <summary>
		/// Defines a PInvoke method given its name, the name of the DLL in which the method is defined, the attributes of the method, the calling convention of the method, the return type of the method, the types of the parameters of the method, and the PInvoke flags. 
		/// </summary>
		/// <param name="name">The name of the PInvoke method.</param>
		/// <param name="dllName">The name of the PInvoke method.</param>
		/// <param name="attributes">The attributes of the method.</param>
		/// <param name="callingConvention">The method's calling convention. </param>
		/// <param name="returnType">The method's return type. </param>
		/// <param name="parameterTypes">The types of the method's parameters. </param>
		/// <param name="nativeCallConv">The native calling convention. </param>
		/// <param name="nativeCharSet">The method's native character set.</param>
		/// <returns>The defined PInvoke method. </returns>
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			return DefinePInvokeMethod(name, dllName, name, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
		}

		/// <summary>
		/// Defines a PInvoke method given its name, the name of the DLL in which the method is defined, the attributes of the method, the calling convention of the method, the return type of the method, the types of the parameters of the method, and the PInvoke flags. 
		/// </summary>
		/// <param name="name">The name of the PInvoke method.</param>
		/// <param name="dllName">The name of the PInvoke method.</param>
		/// <param name="entryName">The name of the entry point in the DLL. </param>
		/// <param name="attributes">The attributes of the method.</param>
		/// <param name="callingConvention">The method's calling convention. </param>
		/// <param name="returnType">The method's return type. </param>
		/// <param name="parameterTypes">The types of the method's parameters. </param>
		/// <param name="nativeCallConv">The native calling convention. </param>
		/// <param name="nativeCharSet">The method's native character set.</param>
		/// <returns>The defined PInvoke method. </returns>
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			if ((attributes & System.Reflection.MethodAttributes.Static) == 0)
			{
				throw new System.ArgumentException("The method is not static.");
			}

			MethodBuilder mb = new MethodBuilder(name, attributes, callingConvention, returnType, parameterTypes, this);
			TypeBuilder.DefinePInvokeMethod(mb.peMeth as MethodDefinition, dllName, entryName, nativeCallConv, nativeCharSet, this);
			return mb;
		}

		/// <summary>
		/// Constructs a <see cref="TypeBuilder"/> for a type with the specified name. 
		/// </summary>
		/// <param name="name">The full path of the type. </param>
		/// <returns>Returns the created <see cref="TypeBuilder"/>. </returns>
		public TypeBuilder DefineType(string name)
		{
			return DefineType(name, System.Reflection.TypeAttributes.AutoLayout, null, null);
		}

		/// <summary>
		/// Constructs a <see cref="TypeBuilder"/> given the type name and the type attributes.  
		/// </summary>
		/// <param name="name">The full path of the type. </param>
		/// <param name="attr">The attributes of the defined type. </param>
		/// <returns>Returns a <see cref="TypeBuilder"/> created with all of the requested attributes. </returns>
		public TypeBuilder DefineType(string name, System.Reflection.TypeAttributes attr)
		{
			return DefineType(name, attr, null, null);
		}

		/// <summary>
		/// Constructs a <see cref="TypeBuilder"/> given type name, its attributes, and the type that the defined type extends. 
		/// </summary>
		/// <param name="name">The full path of the type. </param>
		/// <param name="attr">The attributes of the defined type. </param>
		/// <param name="parent">The <see cref="Type"/> that the defined type extends. </param>
		/// <returns>Returns a <see cref="TypeBuilder"/> created with all of the requested attributes. </returns>
		public TypeBuilder DefineType(string name, System.Reflection.TypeAttributes attr, Type parent)
		{
			return DefineType(name, attr, parent, null);
		}

		/// <summary>
		/// Constructs a <see cref="TypeBuilder"/> given the type name, the attributes, the type that the defined type extends, and the total size of the type. 
		/// </summary>
		/// <param name="name">The full path of the type. </param>
		/// <param name="attr">The attributes of the defined type. </param>
		/// <param name="parent">The <see cref="Type"/> that the defined type extends. </param>
		/// <param name="typesize">The total size of the type. </param>
		/// <returns>Returns a <see cref="TypeBuilder"/> object. </returns>
		public TypeBuilder DefineType(string name, System.Reflection.TypeAttributes attr, Type parent, int typesize)
		{
			return DefineType(name, attr, parent, PackingSize.Unspecified, typesize);
		}

		/// <summary>
		/// Constructs a <see cref="TypeBuilder"/> given the type name, the attributes, the type that the defined type extends, and the packing size of the type.
		/// </summary>
		/// <param name="name">The full path of the type. </param>
		/// <param name="attr">The attributes of the defined type. </param>
		/// <param name="parent">The <see cref="Type"/> that the defined type extends. </param>
		/// <param name="packsize">The packing size of the type. </param>
		/// <returns>Returns a <see cref="TypeBuilder"/> object. </returns>
		public TypeBuilder DefineType(string name, System.Reflection.TypeAttributes attr, Type parent, PackingSize packsize)
		{
			return DefineType(name, attr, parent, packsize, 0);
		}

		/// <summary>
		/// Constructs a <see cref="TypeBuilder"/> given the type name, attributes, the type that the defined type extends, and the interfaces that the defined type implements.
		/// </summary>
		/// <param name="name">The full path of the type. </param>
		/// <param name="attr">The attributes of the defined type. </param>
		/// <param name="parent">The <see cref="Type"/> that the defined type extends. </param>
		/// <param name="interfaces">The list of interfaces that the type implements.</param>
		/// <returns>Returns a <see cref="TypeBuilder"/> created with all of the requested attributes. </returns>
		public TypeBuilder DefineType(string name, System.Reflection.TypeAttributes attr, Type parent, Type[] interfaces)
		{
			TypeBuilder tb = new TypeBuilder(name, attr, parent, interfaces, this);
			types.Add(tb);
			return tb;
		}

		/// <summary>
		/// Constructs a <see cref="TypeBuilder"/> given the type name, attributes, the type that the defined type extends, the packing size of the defined type, and the total size of the defined type
		/// </summary>
		/// <param name="name">The full path of the type. </param>
		/// <param name="attr">The attributes of the defined type. </param>
		/// <param name="parent">The <see cref="Type"/> that the defined type extends. </param>
		/// <param name="packingSize">The packing size of the type. </param>
		/// <param name="typesize">The total size of the type. </param>
		/// <returns>Returns a <see cref="TypeBuilder"/> created with all of the requested attributes. </returns>
		public TypeBuilder DefineType(string name, System.Reflection.TypeAttributes attr, Type parent, PackingSize packingSize, int typesize)
		{
			TypeBuilder tb = DefineType(name, attr, parent);
			types.Add(tb);

			if (packingSize != PackingSize.Unspecified) tb.TypeDef.PackingSize = (ushort)packingSize;
			if (typesize != 0) tb.TypeDef.ClassSize = (uint) typesize;
			return tb;
		}

		/// <summary>
		/// Defines an uninitialized data field in the .sdata section of the portable executable (PE) file. 
		/// </summary>
		/// <param name="name">The name used to refer to the data. </param>
		/// <param name="size">The size of the data field. </param>
		/// <param name="attributes">The attributes for the field. </param>
		/// <returns>A field to reference the data.</returns>
		public FieldBuilder DefineUninitializedData(string name, int size, System.Reflection.FieldAttributes attributes)
		{
			if (completed)
			{
				throw new System.InvalidOperationException("CreateGlobalFunctions has been previously called");
			}

			TypeDefinition type = GetTypeData(name, size);
			FieldBuilder builder = new FieldBuilder(name, type, attributes | System.Reflection.FieldAttributes.Static, this);
			return builder;
		}

		/// <summary>
		/// Returns the named method on an array class. 
		/// </summary>
		/// <param name="arrayClass">An array class. </param>
		/// <param name="methodName">The name of a method on the array class. </param>
		/// <param name="callingConvention">The method's calling convention. </param>
		/// <param name="returnType">The method's calling convention. </param>
		/// <param name="parameterTypes">The method's calling convention. </param>
		/// <returns>The named method on an array class. </returns>
		public MethodInfo GetArrayMethod(Type arrayClass, string methodName, System.Reflection.CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			System.Reflection.MethodInfo candidate = null;
			foreach (System.Reflection.MethodInfo mi in typeof(System.Array).GetMethods())
			{
				if (mi.Name != methodName)
					continue;

				Type[] pars = ParameterInfo.ToType(mi.GetParameters());
				if (!Type.Match(pars, parameterTypes))
					continue;

				Type ret = Type.Import(mi.ReturnType);
				if (ret != returnType)
					continue;

				candidate = mi;
				break;
			}

			if (candidate == null)
				return null;
			return MethodInfo.Wrap(candidate);
		}

		/// <summary>
		/// Gets the named type defined in the module. 
		/// </summary>
		/// <param name="className">The name of the <see cref="Type"/> to get.</param>
		/// <returns>The requested type. Returns a null reference (Nothing in Visual Basic) if the type is not found. </returns>
		public override Type GetType(string className)
		{
			return GetType(className, false);
		}

		/// <summary>
		/// Gets the named type defined in the module optionally ignoring the case of the type name. 
		/// </summary>
		/// <param name="className">The name of the <see cref="Type"/> to get.</param>
		/// <param name="ignoreCase">If <c>true</c>, the search is case-insensitive. If <c>false</c>, the search is case-sensitive. </param>
		/// <returns>The requested type. Returns a null reference (Nothing in Visual Basic) if the type is not found. </returns>
		public Type GetType(string className, bool ignoreCase)
		{
			if (className.EndsWith("&"))
			{
				Type @ref = GetType(className.Substring(0, className.Length - 1), ignoreCase);
				if (@ref == null) return null;
				return @ref.MakeByRefType();
			}

			if (className.EndsWith("*"))
			{
				Type @ref = GetType(className.Substring(0, className.Length - 1), ignoreCase);
				if (@ref == null) return null;
				return @ref.MakePointerType();
			}

			if (className.EndsWith("]"))
			{
				string srank = className.Substring(className.IndexOf('[') + 1);
				srank = srank.Substring(0, srank.Length - 1);

				int rank = srank.Split(',').Length-1;

				Type @ref = GetType(className.Substring(0, className.IndexOf('[')), ignoreCase);
				if (@ref == null) return null;
				return @ref.MakeArrayType(rank);
			}

			int pos = className.IndexOf('+');
			string typeName = pos != -1 ? className.Substring(0, pos) : className;
			string subTypeName = pos != -1 ? className.Substring(pos + 1) : null;

			foreach (TypeBuilder tb in this.types)
			{
				if (string.Equals(tb.FullName, typeName, System.StringComparison.InvariantCultureIgnoreCase))
				{
					if (subTypeName == null)
						return tb;
					return tb.GetNestedType(subTypeName);
				}
			}
			return null;
		}

		/// <summary>
		/// Set a custom attribute using a custom attribute builder. 
		/// </summary>
		/// <param name="customBuilder">An instance of a helper class to define the custom attribute. </param>
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
	    	CustomAttribute ca;
			peModule.CustomAttributes.Add(ca = new CustomAttribute(customBuilder.ctor));
	    	foreach (object arg in customBuilder.args) ca.ConstructorParameters.Add(arg);
		}

		/// <summary>
		/// Set a custom attribute using a specified custom attribute blob. 
		/// </summary>
		/// <param name="con">The constructor for the custom attribute. </param>
		/// <param name="binaryAttribute">A byte blob representing the attributes. </param>
	    public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
	    	CustomAttribute ca;
			peModule.CustomAttributes.Add(ca = new CustomAttribute(con.peMeth, binaryAttribute));
		}

		/// <summary>
		/// Define a document for source. 
		/// </summary>
		/// <param name="url">The URL for the document. </param>
		/// <param name="language">The GUID identifying the document language. </param>
		/// <param name="languageVendor">The GUID identifying the document language vendor. </param>
		/// <param name="documentType">The GUID identifying the document type. </param>
		/// <returns></returns>
		public ISymbolDocumentWriter DefineDocument(string url, System.Guid language, System.Guid languageVendor, System.Guid documentType)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Returns the symbol writer associated with this dynamic module. 
		/// </summary>
		/// <returns>Returns the symbol writer associated with this dynamic module. </returns>
		public SystemCF.Diagnostics.SymbolStore.ISymbolWriter GetSymWriter()
		{
			return new MySymbolWriter();
		}

		/// <summary>
		/// Get the CLR-equivalent <see cref="System.Reflection.Module"/> representation of a <see cref="ModuleBuilder"/> object.
		/// </summary>
		/// <param name="type">The <see cref="ModuleBuilder"/> to convert.</param>
		/// <returns>A <see cref="System.Reflection.Module"/> representation equivalent to the <paramref name="module"/> object.</returns>
		public static implicit operator System.Reflection.Module(ModuleBuilder module)
		{
            if (module == null)
            {
                return null;
            }

			// Get CLR Assembly
			System.Reflection.Assembly clrAssembly = module.GetAssembly();

			// Return module from its name
			System.Reflection.Module[] clrModules = clrAssembly.GetModules();
			foreach (System.Reflection.Module clrModule in clrModules)
			{
				if (clrModule.Name == module.Name)
					return clrModule;
			}

			return null;
		}
	}

	#region Resource
	internal class StreamedResource
	{
		internal EmbeddedResource res;
		internal Stream stream;

		public StreamedResource(EmbeddedResource res, Stream stream)
		{
			this.res = res;
			this.stream = stream;
		}

		internal void Complete()
		{
			res.Data = ReadFully(stream, (int)stream.Length);
		}

		/// <summary>
		/// Reads data from a stream until the end is reached. The
		/// data is returned as a byte array. An IOException is
		/// thrown if any of the underlying IO calls fail.
		/// </summary>
		/// <param name="stream">The stream to read data from</param>
		/// <param name="initialLength">The initial buffer length</param>
		private static byte[] ReadFully(Stream stream, int initialLength)
		{
			stream.Position = 0;

			// If we've been passed an unhelpful initial length, just
			// use 32K.
			if (initialLength < 1)
			{
				initialLength = 32768;
			}

			byte[] buffer = new byte[initialLength];
			int read = 0;

			int chunk;
			while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
			{
				read += chunk;

				// If we've reached the end of our buffer, check to see if there's
				// any more information
				if (read == buffer.Length)
				{
					int nextByte = stream.ReadByte();

					// End of stream? If so, we're done
					if (nextByte == -1)
					{
						return buffer;
					}

					// Nope. Resize the buffer, put in the byte we've just
					// read, and continue
					byte[] newBuffer = new byte[buffer.Length * 2];
					System.Array.Copy(buffer, newBuffer, buffer.Length);
					newBuffer[read] = (byte)nextByte;
					buffer = newBuffer;
					read++;
				}
			}
			// Buffer is now too big. Shrink it.
			byte[] ret = new byte[read];
			System.Array.Copy(buffer, ret, read);
			return ret;
		}
	}
	#endregion

	#region Symbol
	internal class MySymbolWriter : SystemCF.Diagnostics.SymbolStore.ISymbolWriter
	{
		#region ISymbolWriter Members

		public void Close()
		{
		}

		public void CloseMethod()
		{
		}

		public void CloseNamespace()
		{
		}

		public void CloseScope(int endOffset)
		{
		}

		public ISymbolDocumentWriter DefineDocument(string url, System.Guid language, System.Guid languageVendor, System.Guid documentType)
		{
			return new MySymbolDocumentWriter();
		}

		public void DefineField(SymbolToken parent, string name, System.Reflection.FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
		}

		public void DefineGlobalVariable(string name, System.Reflection.FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
		}

		public void DefineLocalVariable(string name, System.Reflection.FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3, int startOffset, int endOffset)
		{
		}

		public void DefineParameter(string name, System.Reflection.ParameterAttributes attributes, int sequence, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
		}

		public void DefineSequencePoints(ISymbolDocumentWriter document, int[] offsets, int[] lines, int[] columns, int[] endLines, int[] endColumns)
		{
		}

		public void Initialize(System.IntPtr emitter, string filename, bool fFullBuild)
		{
		}

		public void OpenMethod(SymbolToken method)
		{
		}

		public void OpenNamespace(string name)
		{
		}

		public int OpenScope(int startOffset)
		{
			return 0;
		}

		public void SetMethodSourceRange(ISymbolDocumentWriter startDoc, int startLine, int startColumn, ISymbolDocumentWriter endDoc, int endLine, int endColumn)
		{
		}

		public void SetScopeRange(int scopeID, int startOffset, int endOffset)
		{
		}

		public void SetSymAttribute(SymbolToken parent, string name, byte[] data)
		{
		}

		public void SetUnderlyingWriter(System.IntPtr underlyingWriter)
		{
		}

		public void SetUserEntryPoint(SymbolToken entryMethod)
		{
		}

		public void UsingNamespace(string fullName)
		{
		}

		#endregion
	}

	internal class MySymbolDocumentWriter : ISymbolDocumentWriter
	{
		#region ISymbolDocumentWriter Members

		public void SetCheckSum(System.Guid algorithmId, byte[] checkSum)
		{
		}

		public void SetSource(byte[] source)
		{
		}

		#endregion
	}
	#endregion
}
