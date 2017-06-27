using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Defines the access modes for a dynamic assembly. 
	/// </summary>
	[System.Flags]
	public enum AssemblyBuilderAccess
	{
		/// <summary>
		/// Represents that the dynamic assembly can be executed, but not saved. 
		/// </summary>
		Run = 1,
		/// <summary>
		/// Represents that the dynamic assembly can be saved, but not executed. 
		/// </summary>
		Save = 2,
		/// <summary>
		/// Represents that the dynamic assembly can be executed and saved. 
		/// </summary>
		RunAndSave = 3,
		/// <summary>
		/// Represents that the dynamic assembly is loaded into the reflection-only context, and cannot be executed.
		/// </summary>
		ReflectionOnly = 6,
	}
}
