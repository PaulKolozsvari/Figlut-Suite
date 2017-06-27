using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	/// <summary>
	/// Identifies the nature of the code in an executable file. 
	/// </summary>
	[System.Flags]
	public enum PortableExecutableKinds
	{
		/// <summary>
		/// The file is not in portable executable (PE) file format. 
		/// </summary>
		NotAPortableExecutableImage = 0,
		/// <summary>
		/// The executable contains only Microsoft intermediate language (MSIL), and is therefore neutral with respect to 32-bit or 64-bit platforms. 
		/// </summary>
		ILOnly = 1,
		/// <summary>
		/// The executable can be run on a 32-bit platform, or in the 32-bit Windows on Windows (WOW) environment on a 64-bit platform. 
		/// </summary>
		Required32Bit = 2,
		/// <summary>
		/// The executable requires a 64-bit platform. 
		/// </summary>
		PE32Plus = 4,
		/// <summary>
		/// The executable contains pure unmanaged code. 
		/// </summary>
		Unmanaged32Bit = 8
	}
}
