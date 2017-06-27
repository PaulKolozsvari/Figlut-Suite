using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	internal class CallingConventionsConverter
	{
		static bool Match(System.Reflection.CallingConventions value, System.Reflection.CallingConventions @enum) { return (value & @enum) == @enum; }
		static bool Match(MethodCallingConvention value, MethodCallingConvention @enum) { return (value & @enum) == @enum; }
		static bool Match(SystemCF.Runtime.InteropServices.CallingConvention value, SystemCF.Runtime.InteropServices.CallingConvention @enum) { return (value & @enum) == @enum; }
		static bool Match(SystemCF.Runtime.InteropServices.CharSet value, SystemCF.Runtime.InteropServices.CharSet @enum) { return (value & @enum) == @enum; }

		public static void Assign(Mono.Cecil.MethodReference meth, System.Reflection.CallingConventions convs)
		{
			if (Match(convs, System.Reflection.CallingConventions.VarArgs))
				meth.CallingConvention |= MethodCallingConvention.VarArg;
			meth.ExplicitThis = Match(convs, System.Reflection.CallingConventions.ExplicitThis);
			meth.HasThis = Match(convs, System.Reflection.CallingConventions.HasThis);
		}
		public static System.Reflection.CallingConventions Extract(Mono.Cecil.MethodReference meth)
		{
			System.Reflection.CallingConventions convs = (System.Reflection.CallingConventions) 0;
			if (Match(meth.CallingConvention, MethodCallingConvention.VarArg))
				convs |= System.Reflection.CallingConventions.VarArgs;
			else
				convs |= System.Reflection.CallingConventions.Standard;
			if (meth.HasThis)
				convs |= System.Reflection.CallingConventions.HasThis;
			if (meth.ExplicitThis)
				convs |= System.Reflection.CallingConventions.ExplicitThis;			 
			return convs;
		}

		public static void Assign(Mono.Cecil.PInvokeInfo info, SystemCF.Runtime.InteropServices.CallingConvention nativeCallConv, SystemCF.Runtime.InteropServices.CharSet nativeCharSet)
		{
			if (Match(nativeCallConv, SystemCF.Runtime.InteropServices.CallingConvention.Cdecl)) info.Attributes |= PInvokeAttributes.CallConvCdecl;
			if (Match(nativeCallConv, SystemCF.Runtime.InteropServices.CallingConvention.FastCall)) info.Attributes |= PInvokeAttributes.CallConvFastcall;
			if (Match(nativeCallConv, SystemCF.Runtime.InteropServices.CallingConvention.StdCall)) info.Attributes |= PInvokeAttributes.CallConvStdCall;
			if (Match(nativeCallConv, SystemCF.Runtime.InteropServices.CallingConvention.ThisCall)) info.Attributes |= PInvokeAttributes.CallConvThiscall;
			if (Match(nativeCallConv, SystemCF.Runtime.InteropServices.CallingConvention.Winapi)) info.Attributes |= PInvokeAttributes.CallConvWinapi;

			if (Match(nativeCharSet, SystemCF.Runtime.InteropServices.CharSet.Ansi)) info.Attributes |= PInvokeAttributes.CharSetAnsi;
			if (Match(nativeCharSet, SystemCF.Runtime.InteropServices.CharSet.Auto)) info.Attributes |= PInvokeAttributes.CharSetAuto;
			if (Match(nativeCharSet, SystemCF.Runtime.InteropServices.CharSet.None)) info.Attributes |= PInvokeAttributes.CharSetNotSpec;
			if (Match(nativeCharSet, SystemCF.Runtime.InteropServices.CharSet.Unicode)) info.Attributes |= PInvokeAttributes.CharSetUnicode;
		}
	}
}
