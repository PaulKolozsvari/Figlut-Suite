using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	internal class ParameterAttributesConverter
	{
		static bool Match(Mono.Cecil.ParameterAttributes value, Mono.Cecil.ParameterAttributes @enum) { return (value & @enum) == @enum; }
		static bool Match(System.Reflection.ParameterAttributes value, System.Reflection.ParameterAttributes @enum) { return (value & @enum) == @enum; }

		public static System.Reflection.ParameterAttributes Convert(Mono.Cecil.ParameterAttributes peAttrs)
		{
			System.Reflection.ParameterAttributes attrs = (System.Reflection.ParameterAttributes) 0;
			if (Match(peAttrs, Mono.Cecil.ParameterAttributes.HasDefault)) attrs |= System.Reflection.ParameterAttributes.HasDefault;			 
			if (Match(peAttrs, Mono.Cecil.ParameterAttributes.HasFieldMarshal)) attrs |= System.Reflection.ParameterAttributes.HasFieldMarshal;			 
			if (Match(peAttrs, Mono.Cecil.ParameterAttributes.In)) attrs |= System.Reflection.ParameterAttributes.In;			 
			if (Match(peAttrs, Mono.Cecil.ParameterAttributes.Optional)) attrs |= System.Reflection.ParameterAttributes.Optional;			 
			if (Match(peAttrs, Mono.Cecil.ParameterAttributes.Out)) attrs |= System.Reflection.ParameterAttributes.Out;			 
			return attrs;
		}

		public static Mono.Cecil.ParameterAttributes Convert(System.Reflection.ParameterAttributes attrs)
		{
			Mono.Cecil.ParameterAttributes peAttrs = (Mono.Cecil.ParameterAttributes) 0;
			if (Match(attrs, System.Reflection.ParameterAttributes.HasDefault)) peAttrs |= Mono.Cecil.ParameterAttributes.HasDefault;			 
			if (Match(attrs, System.Reflection.ParameterAttributes.HasFieldMarshal)) peAttrs |= Mono.Cecil.ParameterAttributes.HasFieldMarshal;			 
			if (Match(attrs, System.Reflection.ParameterAttributes.In)) peAttrs |= Mono.Cecil.ParameterAttributes.In;			 
			if (Match(attrs, System.Reflection.ParameterAttributes.Optional)) peAttrs |= Mono.Cecil.ParameterAttributes.Optional;			 
			if (Match(attrs, System.Reflection.ParameterAttributes.Out)) peAttrs |= Mono.Cecil.ParameterAttributes.Out;			 
			return peAttrs;
		}
	}
}
