using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	/// <summary>
	/// Identifies the platform targeted by an executable. 
	/// </summary>
	[System.Flags]
	public enum ImageFileMachine
	{
		/// <summary>
		/// Targets a 32-bit Intel processor. 
		/// </summary>
		I386 = 0x14c,
		/// <summary>
		/// Targets a 64-bit Intel processor.
		/// </summary>
		IA64 = 0x200,
		/// <summary>
		/// Targets a 64-bit AMD processor. 
		/// </summary>
		AMD64 = 0x8664,
	}
}
