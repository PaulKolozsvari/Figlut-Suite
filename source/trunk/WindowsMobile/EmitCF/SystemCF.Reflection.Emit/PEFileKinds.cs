using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// PEFileKinds
	/// </summary>
	public enum PEFileKinds
	{
		/// <summary>
		/// The portable executable (PE) file is a DLL. 
		/// </summary>
		Dll = 1,
		/// <summary>
		/// The application is a console (not a Windows-based) application. 
		/// </summary>
		ConsoleApplication = 2,
		/// <summary>
		/// The application is a Windows-based application. 
		/// </summary>
		WindowApplication = 3
	}

	internal class PEFileKindsConverter
	{
		static bool Match(PEFileKinds value, PEFileKinds @enum) { return (value & @enum) == @enum; }

		public static AssemblyKind Convert(PEFileKinds peAttrs)
		{
			AssemblyKind attrs = (AssemblyKind) 0;
			if (Match(peAttrs, PEFileKinds.ConsoleApplication)) attrs |= AssemblyKind.Console;			 
			if (Match(peAttrs, PEFileKinds.Dll)) attrs |= AssemblyKind.Dll;			 
			if (Match(peAttrs, PEFileKinds.WindowApplication)) attrs |= AssemblyKind.Windows;			 
			return attrs;
		}
	}
}
