using System.Collections.Generic;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Defines and represents a dynamic assembly. 
	/// </summary>
	public class AssemblyBuilder: SystemCF.Reflection.Assembly
	{
		#region Private & Internal
		private List<ModuleBuilder> modules = new List<ModuleBuilder>();
		internal Mono.Cecil.AssemblyDefinition assembly;
		internal string path;
		internal AssemblyBuilderAccess access;
		internal string assemblyFileName;
		internal bool saved;
		private MethodInfo entryPoint;

		internal AssemblyBuilder(System.Reflection.AssemblyName assemblyName, AssemblyBuilderAccess access, string path)
		{
			AssemblyKind kind = AssemblyKind.Dll;
			if (path != null && Path.GetExtension(path).ToLower() == ".exe") kind = AssemblyKind.Console;

			assembly = AssemblyFactory.DefineAssembly(assemblyName.Name, kind);
	    	Type._module = assembly.MainModule;
			
			this.access = access;
			this.path = path;
 		}

		internal AssemblyBuilder(Mono.Cecil.AssemblyDefinition assembly)
		{
			this.assembly = assembly;
			Type._module = assembly.MainModule;
		}

		static string GetApplicationPath()
		{
			//return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string codeBase = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
			if (codeBase.StartsWith("file:\\"))
				codeBase = codeBase.Substring(codeBase.IndexOf('\\') + 1);
			return codeBase;
		}

	    internal string GetFileName(ModuleDefinition module)
		{
	    	string filename = module.Name;
			if (Path.GetFileNameWithoutExtension(filename) == filename)
				filename += module.Assembly.Kind == AssemblyKind.Dll ? ".dll" : ".exe";
			if (path == null) path = GetApplicationPath();
			filename = path + @"\" + filename;
	    	return filename;
		}

	    internal void Save(ModuleDefinition module)
		{
			Save(GetFileName(module));
		}
		#endregion

		/// <summary>
		/// Adds an existing resource file to this assembly. 
		/// </summary>
		/// <param name="name">The logical name of the resource. </param>
		/// <param name="fileName">The physical file name (.resources file) to which the logical name is mapped. 
		/// This should not include a path; the file must be in the same directory as the assembly to which it is added. 
		/// </param>
		public void AddResourceFile(string name, string fileName)
		{
			AddResourceFile(name, fileName, ResourceAttributes.Public);
		}

		/// <summary>
		/// Adds an existing resource file to this assembly. 
		/// </summary>
		/// <param name="name">The logical name of the resource. </param>
		/// <param name="fileName">The physical file name (.resources file) to which the logical name is mapped. 
		/// This should not include a path; the file must be in the same directory as the assembly to which it is added. 
		/// </param>
		/// <param name="attribute">The resource attributes.</param>
	    public void AddResourceFile(string name, string fileName, ResourceAttributes attribute)
		{
	    	assembly.MainModule.Resources.Add(new LinkedResource(name, ResourceAttributesConverter.Convert(attribute), fileName));
		}

		/// <summary>
		/// Defines a named transient dynamic module in this assembly. 
		/// </summary>
		/// <param name="name">The name of the dynamic module.</param>
		/// <returns>A <see cref="ModuleBuilder"/> representing the defined dynamic module. </returns>
	    public ModuleBuilder DefineDynamicModule(string name)
		{
			return DefineDynamicModule(name, false);
		}

		/// <summary>
		/// Defines a named transient dynamic module in this assembly and specifies whether symbol information should be emitted. 
		/// </summary>
		/// <param name="name">The name of the dynamic module.</param>
		/// <param name="emitSymbolInfo">** Not used **</param>
		/// <returns>A <see cref="ModuleBuilder"/> representing the defined dynamic module. </returns>
	    public ModuleBuilder DefineDynamicModule(string name, bool emitSymbolInfo)
		{
	        string filename = path+"\\"+name;
	        if ((access & AssemblyBuilderAccess.Save) != 0) filename += ".dll";
			return DefineDynamicModule(name, filename, emitSymbolInfo);
		}

		/// <summary>
		/// Defines a persistable dynamic module with the given name that will be saved to the specified file. No symbol information is emitted. 
		/// </summary>
		/// <param name="name">The name of the dynamic module.</param>
		/// <param name="fileName">The name of the file to which the dynamic module should be saved. </param>
		/// <returns>A <see cref="ModuleBuilder"/> representing the defined dynamic module. </returns>
	    public ModuleBuilder DefineDynamicModule(string name, string fileName)
		{
			return DefineDynamicModule(name, fileName, false);
		}

		/// <summary>
		/// Defines a persistable dynamic module, specifying the module name, the name of the file to which the module will be saved, and whether symbol information should be emitted using the default symbol writer. 
		/// </summary>
		/// <param name="name">The name of the dynamic module.</param>
		/// <param name="fileName">The name of the file to which the dynamic module should be saved. </param>
		/// <param name="emitSymbolInfo">** Not used **</param>
		/// <returns>A <see cref="ModuleBuilder"/> representing the defined dynamic module. </returns>
	    public ModuleBuilder DefineDynamicModule(string name, string fileName, bool emitSymbolInfo)
		{
			ModuleDefinition module;
			
			if (assembly.Modules.Count == 1)
			{
				module = assembly.Modules[0];
				module.Name = name;
			}
			else
				assembly.Modules.Add(module = new ModuleDefinition(name, assembly, true));

	    	if (emitSymbolInfo) module.SaveSymbols(fileName);
			ModuleBuilder mb = new ModuleBuilder(module, this);
			modules.Add(mb);

			return mb;
		}

		/// <summary>
		/// Defines an unmanaged resource for this assembly as an opaque blob of bytes. 
		/// </summary>
		/// <param name="resource">The opaque blob of bytes representing the unmanaged resource.</param>
	    public void DefineUnmanagedResource(byte[] resource)
		{
	    	assembly.MainModule.Resources.Add(new EmbeddedResource(null, ManifestResourceAttributes.Public, resource));
		}

		/// <summary>
		/// Defines an unmanaged resource file for this assembly given the name of the resource file. 
		/// </summary>
		/// <param name="resourceFileName">The name of the resource file.</param>
	    public void DefineUnmanagedResource(string resourceFileName)
		{
	    	assembly.MainModule.Resources.Add(new LinkedResource(null, ManifestResourceAttributes.Public, resourceFileName));
		}
  
		/// <summary>
		/// Returns the dynamic module with the specified name. 
		/// </summary>
		/// <param name="name">The name of the requested dynamic module. </param>
		/// <returns>A <see cref="ModuleBuilder"/> object representing the requested dynamic module. </returns>
	    public ModuleBuilder GetDynamicModule(string name)
		{
			foreach (ModuleBuilder module in modules)
	    		if (module.Name == name) return module;
	    	return null;
		}

		/// <summary>
		/// Saves this dynamic assembly to disk. 
		/// </summary>
		/// <param name="assemblyFileName">The file name of the assembly.</param>
	    public void Save(string assemblyFileName)
		{
            if (saved) return;

			// Complete Modules
			foreach (ModuleBuilder module in modules)
				module.Complete();

			if (path == null) path = GetApplicationPath();
			if (Path.GetDirectoryName(assemblyFileName) == string.Empty)
				assemblyFileName = path + @"\" + assemblyFileName;

			AssemblyFactory.SaveAssembly(assembly, assemblyFileName);
			this.assemblyFileName = assemblyFileName;
			saved = true;
		}

		/// <summary>
		/// Saves this dynamic assembly to disk, specifying the nature of code in the assembly's executables and the target platform. 
		/// </summary>
		/// <param name="assemblyFileName">The file name of the assembly.</param>
		/// <param name="portableExecutableKind">** Not used **</param>
		/// <param name="imageFileMachine">** Not used **</param>
	    public void Save(string assemblyFileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
		{
			Save(assemblyFileName);
		}

		/// <summary>
		/// Set a custom attribute on this assembly using a custom attribute builder. 
		/// </summary>
		/// <param name="customBuilder">An instance of a helper class to define the custom attribute.</param>
	    public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
	    	CustomAttribute ca;
	    	assembly.CustomAttributes.Add(ca = new CustomAttribute(customBuilder.ctor));
	    	foreach (object arg in customBuilder.args) ca.ConstructorParameters.Add(arg);
		}

		/// <summary>
		/// Set a custom attribute on this assembly using a specified custom attribute blob. 
		/// </summary>
		/// <param name="con">The constructor for the custom attribute.</param>
		/// <param name="binaryAttribute">A byte blob representing the attributes.</param>
	    public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
	    	CustomAttribute ca;
			assembly.CustomAttributes.Add(ca = new CustomAttribute(con.peMeth, binaryAttribute));
		}

		/// <summary>
		/// Sets the entry point for this dynamic assembly, assuming that a console application is being built. 
		/// </summary>
		/// <param name="entryMethod">A reference to the method that represents the entry point for this dynamic assembly.</param>
	    public void SetEntryPoint(MethodInfo entryMethod)
		{
			SetEntryPoint(entryMethod, PEFileKinds.ConsoleApplication);
		}

		/// <summary>
		/// Sets the entry point for this assembly and defines the type of the portable executable (PE file) being built. 
		/// </summary>
		/// <param name="entryMethod">A reference to the method that represents the entry point for this dynamic assembly. </param>
		/// <param name="fileKind">The type of the assembly executable being built. </param>
	    public void SetEntryPoint(MethodInfo entryMethod, PEFileKinds fileKind)
		{
			assembly.EntryPoint = entryMethod.peMeth as MethodDefinition;
			assembly.Kind = PEFileKindsConverter.Convert(fileKind);
			
			this.entryPoint = entryMethod;
		}

		/// <summary>
		/// Returns the entry point of this assembly. 
		/// </summary>
		public MethodInfo EntryPoint { get { return entryPoint; } }
	}
}
