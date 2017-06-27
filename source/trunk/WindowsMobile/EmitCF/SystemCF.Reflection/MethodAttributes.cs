using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	internal class MethodAttributesConverter
	{
		static bool Match(System.Reflection.MethodAttributes value, System.Reflection.MethodAttributes @enum) { return (value & @enum) == @enum; }
		static bool Match(Mono.Cecil.MethodAttributes value, Mono.Cecil.MethodAttributes @enum) { return (value & @enum) == @enum; }

		public static Mono.Cecil.MethodAttributes Convert(System.Reflection.MethodAttributes attrs)
		{
			Mono.Cecil.MethodAttributes peAttrs = (Mono.Cecil.MethodAttributes) 0;
			if (Match(attrs, System.Reflection.MethodAttributes.Abstract)) peAttrs |= Mono.Cecil.MethodAttributes.Abstract;			 
			if (Match(attrs, System.Reflection.MethodAttributes.Assembly)) peAttrs |= Mono.Cecil.MethodAttributes.Assem;			 
			if (Match(attrs, System.Reflection.MethodAttributes.FamANDAssem)) peAttrs |= Mono.Cecil.MethodAttributes.FamANDAssem;			 
			if (Match(attrs, System.Reflection.MethodAttributes.Family)) peAttrs |= Mono.Cecil.MethodAttributes.Family;			 
			if (Match(attrs, System.Reflection.MethodAttributes.FamORAssem)) peAttrs |= Mono.Cecil.MethodAttributes.FamORAssem;			 
			if (Match(attrs, System.Reflection.MethodAttributes.Final)) peAttrs |= Mono.Cecil.MethodAttributes.Final;			 
			if (Match(attrs, System.Reflection.MethodAttributes.HideBySig)) peAttrs |= Mono.Cecil.MethodAttributes.HideBySig;			 
			if (Match(attrs, System.Reflection.MethodAttributes.NewSlot)) peAttrs |= Mono.Cecil.MethodAttributes.NewSlot;			 
			if (Match(attrs, System.Reflection.MethodAttributes.Private)) peAttrs |= Mono.Cecil.MethodAttributes.Private;			 
			if (Match(attrs, System.Reflection.MethodAttributes.Public)) peAttrs |= Mono.Cecil.MethodAttributes.Public;			 
			if (Match(attrs, System.Reflection.MethodAttributes.RequireSecObject)) peAttrs |= Mono.Cecil.MethodAttributes.RequireSecObject;			 
			if (Match(attrs, System.Reflection.MethodAttributes.RTSpecialName)) peAttrs |= Mono.Cecil.MethodAttributes.RTSpecialName;			 
			if (Match(attrs, System.Reflection.MethodAttributes.SpecialName)) peAttrs |= Mono.Cecil.MethodAttributes.SpecialName;			 
			if (Match(attrs, System.Reflection.MethodAttributes.Static)) peAttrs |= Mono.Cecil.MethodAttributes.Static;			 
			if (Match(attrs, System.Reflection.MethodAttributes.Virtual)) peAttrs |= Mono.Cecil.MethodAttributes.Virtual;	
			return peAttrs;
		}
		public static System.Reflection.MethodAttributes Convert(Mono.Cecil.MethodAttributes peAttrs)
		{
			System.Reflection.MethodAttributes attrs = (System.Reflection.MethodAttributes) 0;
			if (Match(peAttrs, Mono.Cecil.MethodAttributes.Abstract)) attrs |= System.Reflection.MethodAttributes.Abstract;			 
			if (Match(peAttrs, Mono.Cecil.MethodAttributes.Assem)) attrs |= System.Reflection.MethodAttributes.Assembly;			 
			if (Match(peAttrs, Mono.Cecil.MethodAttributes.FamANDAssem)) attrs |= System.Reflection.MethodAttributes.FamANDAssem;			 
			if (Match(peAttrs, Mono.Cecil.MethodAttributes.Family)) attrs |= System.Reflection.MethodAttributes.Family;			 
			if (Match(peAttrs, Mono.Cecil.MethodAttributes.FamORAssem)) attrs |= System.Reflection.MethodAttributes.FamORAssem;			 
			if (Match(peAttrs, Mono.Cecil.MethodAttributes.Final)) attrs |= System.Reflection.MethodAttributes.Final;			 
			if (Match(peAttrs, Mono.Cecil.MethodAttributes.HideBySig)) attrs |= System.Reflection.MethodAttributes.HideBySig;			 
			if (Match(peAttrs, Mono.Cecil.MethodAttributes.NewSlot)) attrs |= System.Reflection.MethodAttributes.NewSlot;			 
			if (Match(peAttrs, Mono.Cecil.MethodAttributes.Private)) attrs |= System.Reflection.MethodAttributes.Private;			 
			if (Match(peAttrs, Mono.Cecil.MethodAttributes.Public)) attrs |= System.Reflection.MethodAttributes.Public;			 
			if (Match(peAttrs, Mono.Cecil.MethodAttributes.RequireSecObject)) attrs |= System.Reflection.MethodAttributes.RequireSecObject;			 
			if (Match(peAttrs, Mono.Cecil.MethodAttributes.RTSpecialName)) attrs |= System.Reflection.MethodAttributes.RTSpecialName;			 
			if (Match(peAttrs, Mono.Cecil.MethodAttributes.SpecialName)) attrs |= System.Reflection.MethodAttributes.SpecialName;			 
			if (Match(peAttrs, Mono.Cecil.MethodAttributes.Static)) attrs |= System.Reflection.MethodAttributes.Static;			 
			if (Match(peAttrs, Mono.Cecil.MethodAttributes.Virtual)) attrs |= System.Reflection.MethodAttributes.Virtual;	
			return attrs;
		}
	}
}
