using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Runtime.InteropServices
{
	/// <summary>
	/// Specifies the calling convention required to call methods implemented in unmanaged code. 
	/// </summary>
	public enum CallingConvention
	{
		/// <summary>
		/// This member is not actually a calling convention, but instead uses the default platform calling convention. For example, on Windows the default is StdCall and on Windows CE.NET it is Cdecl. 
		/// </summary>
		Winapi = 1,
		/// <summary>
		/// The caller cleans the stack. This enables calling functions with varargs, which makes it appropriate to use for methods that accept a variable number of parameters, such as Printf. 
		/// </summary>
		Cdecl = 2,
		/// <summary>
		/// The callee cleans the stack. This is the default convention for calling unmanaged functions with platform invoke. 
		/// </summary>
		StdCall = 3,
		/// <summary>
		/// The first parameter is the this pointer and is stored in register ECX. Other parameters are pushed on the stack. This calling convention is used to call methods on classes exported from an unmanaged DLL. 
		/// </summary>
		ThisCall = 4,
		/// <summary>
		/// This calling convention is not supported. 
		/// </summary>
		FastCall = 5,
	}

	internal class CallingConventionConverter
	{
		static bool Match(CallingConvention value, CallingConvention @enum) { return (value & @enum) == @enum; }

		public static MethodCallingConvention Convert(CallingConvention nativeCallConv)
		{
			MethodCallingConvention convs = (MethodCallingConvention)0;
			if (Match(nativeCallConv, CallingConvention.Cdecl)) convs |= MethodCallingConvention.C;
			if (Match(nativeCallConv, CallingConvention.FastCall)) convs |= MethodCallingConvention.FastCall;
			if (Match(nativeCallConv, CallingConvention.StdCall)) convs |= MethodCallingConvention.StdCall;
			if (Match(nativeCallConv, CallingConvention.ThisCall)) convs |= MethodCallingConvention.ThisCall;
			return convs;
		}
	}
}
