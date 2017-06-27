using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	internal class EventAttributesConverter
	{
		static bool Match(Mono.Cecil.EventAttributes value, Mono.Cecil.EventAttributes @enum) { return (value & @enum) == @enum; }
		static bool Match(System.Reflection.EventAttributes value, System.Reflection.EventAttributes @enum) { return (value & @enum) == @enum; }

		public static System.Reflection.EventAttributes Convert(Mono.Cecil.EventAttributes peAttrs)
		{
			System.Reflection.EventAttributes attrs = (System.Reflection.EventAttributes) 0;
			if (Match(peAttrs, Mono.Cecil.EventAttributes.RTSpecialName))
				attrs |= System.Reflection.EventAttributes.RTSpecialName;
			if (Match(peAttrs, Mono.Cecil.EventAttributes.SpecialName))
				attrs |= System.Reflection.EventAttributes.SpecialName;			 
			return attrs;
		}

		public static Mono.Cecil.EventAttributes Convert(System.Reflection.EventAttributes attrs)
		{
			Mono.Cecil.EventAttributes peAttrs = (Mono.Cecil.EventAttributes) 0;
			if (Match(attrs, System.Reflection.EventAttributes.RTSpecialName))
				peAttrs |= Mono.Cecil.EventAttributes.RTSpecialName;
			if (Match(attrs, System.Reflection.EventAttributes.SpecialName))
				peAttrs |= Mono.Cecil.EventAttributes.SpecialName;			 
			return peAttrs;
		}
	}
}
