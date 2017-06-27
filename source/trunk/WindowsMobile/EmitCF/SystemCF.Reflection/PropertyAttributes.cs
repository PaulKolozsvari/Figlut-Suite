using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	internal class PropertyAttributesConverter
	{
		static bool Match(Mono.Cecil.PropertyAttributes value, Mono.Cecil.PropertyAttributes @enum) { return (value & @enum) == @enum; }
		static bool Match(System.Reflection.PropertyAttributes value, System.Reflection.PropertyAttributes @enum) { return (value & @enum) == @enum; }

		public static System.Reflection.PropertyAttributes Convert(Mono.Cecil.PropertyAttributes peAttrs)
		{
			System.Reflection.PropertyAttributes attrs = (System.Reflection.PropertyAttributes) 0;
			if (Match(peAttrs, Mono.Cecil.PropertyAttributes.SpecialName)) attrs |= System.Reflection.PropertyAttributes.SpecialName;			 
			if (Match(peAttrs, Mono.Cecil.PropertyAttributes.RTSpecialName)) attrs |= System.Reflection.PropertyAttributes.RTSpecialName;			 
			if (Match(peAttrs, Mono.Cecil.PropertyAttributes.HasDefault)) attrs |= System.Reflection.PropertyAttributes.HasDefault;			 
			return attrs;
		}

		public static Mono.Cecil.PropertyAttributes Convert(System.Reflection.PropertyAttributes attrs)
		{
			Mono.Cecil.PropertyAttributes peAttrs = (Mono.Cecil.PropertyAttributes) 0;
			if (Match(attrs, System.Reflection.PropertyAttributes.SpecialName)) peAttrs |= Mono.Cecil.PropertyAttributes.SpecialName;			 
			if (Match(attrs, System.Reflection.PropertyAttributes.RTSpecialName)) peAttrs |= Mono.Cecil.PropertyAttributes.RTSpecialName;			 
			if (Match(attrs, System.Reflection.PropertyAttributes.HasDefault)) peAttrs |= Mono.Cecil.PropertyAttributes.HasDefault;			 
			return peAttrs;
		}
	}
}
