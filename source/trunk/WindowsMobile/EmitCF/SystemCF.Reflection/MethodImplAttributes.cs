using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	internal class MethodImplAttributesConverter
	{
		static bool Match(Mono.Cecil.MethodImplAttributes value, Mono.Cecil.MethodImplAttributes @enum) { return (value & @enum) == @enum; }
		static bool Match(System.Reflection.MethodImplAttributes value, System.Reflection.MethodImplAttributes @enum) { return (value & @enum) == @enum; }

		public static System.Reflection.MethodImplAttributes Convert(Mono.Cecil.MethodImplAttributes peAttrs)
		{
			System.Reflection.MethodImplAttributes attrs = (System.Reflection.MethodImplAttributes) 0;
			if (Match(peAttrs, Mono.Cecil.MethodImplAttributes.ForwardRef)) attrs |= System.Reflection.MethodImplAttributes.ForwardRef;			 
			if (Match(peAttrs, Mono.Cecil.MethodImplAttributes.InternalCall)) attrs |= System.Reflection.MethodImplAttributes.InternalCall;			 
			if (Match(peAttrs, Mono.Cecil.MethodImplAttributes.Native)) attrs |= System.Reflection.MethodImplAttributes.Native;			 
			if (Match(peAttrs, Mono.Cecil.MethodImplAttributes.NoInlining)) attrs |= System.Reflection.MethodImplAttributes.NoInlining;			 
			if (Match(peAttrs, Mono.Cecil.MethodImplAttributes.OPTIL)) attrs |= System.Reflection.MethodImplAttributes.OPTIL;			 
			if (Match(peAttrs, Mono.Cecil.MethodImplAttributes.PreserveSig)) attrs |= System.Reflection.MethodImplAttributes.PreserveSig;			 
			if (Match(peAttrs, Mono.Cecil.MethodImplAttributes.Runtime)) attrs |= System.Reflection.MethodImplAttributes.Runtime;			 
			if (Match(peAttrs, Mono.Cecil.MethodImplAttributes.Synchronized)) attrs |= System.Reflection.MethodImplAttributes.Synchronized;			 
			if (Match(peAttrs, Mono.Cecil.MethodImplAttributes.Unmanaged)) attrs |= System.Reflection.MethodImplAttributes.Unmanaged;			 
			return attrs;
		}
		public static Mono.Cecil.MethodImplAttributes Convert(System.Reflection.MethodImplAttributes attrs)
		{
			Mono.Cecil.MethodImplAttributes peAttrs = (Mono.Cecil.MethodImplAttributes) 0;
			if (Match(attrs, System.Reflection.MethodImplAttributes.ForwardRef)) peAttrs |= Mono.Cecil.MethodImplAttributes.ForwardRef;			 
			if (Match(attrs, System.Reflection.MethodImplAttributes.InternalCall)) peAttrs |= Mono.Cecil.MethodImplAttributes.InternalCall;			 
			if (Match(attrs, System.Reflection.MethodImplAttributes.Native)) peAttrs |= Mono.Cecil.MethodImplAttributes.Native;			 
			if (Match(attrs, System.Reflection.MethodImplAttributes.NoInlining)) peAttrs |= Mono.Cecil.MethodImplAttributes.NoInlining;			 
			if (Match(attrs, System.Reflection.MethodImplAttributes.OPTIL)) peAttrs |= Mono.Cecil.MethodImplAttributes.OPTIL;			 
			if (Match(attrs, System.Reflection.MethodImplAttributes.PreserveSig)) peAttrs |= Mono.Cecil.MethodImplAttributes.PreserveSig;			 
			if (Match(attrs, System.Reflection.MethodImplAttributes.Runtime)) peAttrs |= Mono.Cecil.MethodImplAttributes.Runtime;			 
			if (Match(attrs, System.Reflection.MethodImplAttributes.Synchronized)) peAttrs |= Mono.Cecil.MethodImplAttributes.Synchronized;			 
			if (Match(attrs, System.Reflection.MethodImplAttributes.Unmanaged)) peAttrs |= Mono.Cecil.MethodImplAttributes.Unmanaged;			 
			return peAttrs;
		}
	}
}
