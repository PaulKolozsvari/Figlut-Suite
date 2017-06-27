using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCF.Reflection
{
	internal class FieldAttributesConverter
	{
		static bool Match(Mono.Cecil.FieldAttributes value, Mono.Cecil.FieldAttributes @enum) { return (value & @enum) == @enum; }
		static bool Match(System.Reflection.FieldAttributes value, System.Reflection.FieldAttributes @enum) { return (value & @enum) == @enum; }

		public static System.Reflection.FieldAttributes Convert(Mono.Cecil.FieldAttributes peAttrs)
		{
			System.Reflection.FieldAttributes attrs = (System.Reflection.FieldAttributes) 0;
			if (Match(peAttrs, Mono.Cecil.FieldAttributes.Assembly))
				attrs |= System.Reflection.FieldAttributes.Assembly;
			if (Match(peAttrs, Mono.Cecil.FieldAttributes.FamANDAssem))
				attrs |= System.Reflection.FieldAttributes.FamANDAssem;
			if (Match(peAttrs, Mono.Cecil.FieldAttributes.Family))
				attrs |= System.Reflection.FieldAttributes.Family;
			if (Match(peAttrs, Mono.Cecil.FieldAttributes.FamORAssem))
				attrs |= System.Reflection.FieldAttributes.FamORAssem;
			if (Match(peAttrs, Mono.Cecil.FieldAttributes.HasDefault))
				attrs |= System.Reflection.FieldAttributes.HasDefault;
			if (Match(peAttrs, Mono.Cecil.FieldAttributes.HasFieldMarshal))
				attrs |= System.Reflection.FieldAttributes.HasFieldMarshal;
			if (Match(peAttrs, Mono.Cecil.FieldAttributes.HasFieldRVA))
				attrs |= System.Reflection.FieldAttributes.HasFieldRVA;
			if (Match(peAttrs, Mono.Cecil.FieldAttributes.InitOnly))
				attrs |= System.Reflection.FieldAttributes.InitOnly;
			if (Match(peAttrs, Mono.Cecil.FieldAttributes.Literal))
				attrs |= System.Reflection.FieldAttributes.Literal;
			if (Match(peAttrs, Mono.Cecil.FieldAttributes.NotSerialized))
				attrs |= System.Reflection.FieldAttributes.NotSerialized;
			if (Match(peAttrs, Mono.Cecil.FieldAttributes.Private))
				attrs |= System.Reflection.FieldAttributes.Private;
			if (Match(peAttrs, Mono.Cecil.FieldAttributes.Public))
				attrs |= System.Reflection.FieldAttributes.Public;
			if (Match(peAttrs, Mono.Cecil.FieldAttributes.RTSpecialName))
				attrs |= System.Reflection.FieldAttributes.RTSpecialName;
			if (Match(peAttrs, Mono.Cecil.FieldAttributes.SpecialName))
				attrs |= System.Reflection.FieldAttributes.SpecialName;
			if (Match(peAttrs, Mono.Cecil.FieldAttributes.Static))
				attrs |= System.Reflection.FieldAttributes.Static;			 
			return attrs;
		}
		public static Mono.Cecil.FieldAttributes Convert(System.Reflection.FieldAttributes attrs)
		{
			Mono.Cecil.FieldAttributes peAttrs = (Mono.Cecil.FieldAttributes) 0;
			if (Match(attrs, System.Reflection.FieldAttributes.Assembly))
				peAttrs |= Mono.Cecil.FieldAttributes.Assembly;
			if (Match(attrs, System.Reflection.FieldAttributes.FamANDAssem))
				peAttrs |= Mono.Cecil.FieldAttributes.FamANDAssem;
			if (Match(attrs, System.Reflection.FieldAttributes.Family))
				peAttrs |= Mono.Cecil.FieldAttributes.Family;			 
			if (Match(attrs, System.Reflection.FieldAttributes.FamORAssem)) peAttrs |= Mono.Cecil.FieldAttributes.FamORAssem;
			if (Match(attrs, System.Reflection.FieldAttributes.HasDefault)) peAttrs |= Mono.Cecil.FieldAttributes.HasDefault;
			if (Match(attrs, System.Reflection.FieldAttributes.HasFieldMarshal)) peAttrs |= Mono.Cecil.FieldAttributes.HasFieldMarshal;
			if (Match(attrs, System.Reflection.FieldAttributes.HasFieldRVA)) peAttrs |= Mono.Cecil.FieldAttributes.HasFieldRVA;
			if (Match(attrs, System.Reflection.FieldAttributes.InitOnly)) peAttrs |= Mono.Cecil.FieldAttributes.InitOnly;			 
			if (Match(attrs, System.Reflection.FieldAttributes.Literal)) peAttrs |= Mono.Cecil.FieldAttributes.Literal;			 
			if (Match(attrs, System.Reflection.FieldAttributes.NotSerialized)) peAttrs |= Mono.Cecil.FieldAttributes.NotSerialized;			 
			if (Match(attrs, System.Reflection.FieldAttributes.Private)) peAttrs |= Mono.Cecil.FieldAttributes.Private;			 
			if (Match(attrs, System.Reflection.FieldAttributes.Public)) peAttrs |= Mono.Cecil.FieldAttributes.Public;			 
			if (Match(attrs, System.Reflection.FieldAttributes.RTSpecialName)) peAttrs |= Mono.Cecil.FieldAttributes.RTSpecialName;			 
			if (Match(attrs, System.Reflection.FieldAttributes.SpecialName)) peAttrs |= Mono.Cecil.FieldAttributes.SpecialName;			 
			if (Match(attrs, System.Reflection.FieldAttributes.Static)) peAttrs |= Mono.Cecil.FieldAttributes.Static;			 
			return peAttrs;
		}
	}
}
