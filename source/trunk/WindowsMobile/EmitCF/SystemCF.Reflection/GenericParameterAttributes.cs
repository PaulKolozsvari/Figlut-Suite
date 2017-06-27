using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	/// <summary>
	/// Describes the constraints on a generic type parameter of a generic type or method. 
	/// </summary>
	[System.Flags]
	public enum GenericParameterAttributes
	{
		/// <summary>
		/// A type can be substituted for the generic type parameter only if it has a parameterless constructor. 
		/// </summary>
		None = 0,
		/// <summary>
		/// The generic type parameter is covariant. A covariant type parameter can appear as the result type of a method, the type of a read-only field, a declared base type, or an implemented interface. 
		/// </summary>
		Covariant = 1,
		/// <summary>
		/// The generic type parameter is contravariant. A contravariant type parameter can appear as a parameter type in method signatures. 
		/// </summary>
		Contravariant = 2,
		/// <summary>
		/// Selects the combination of all variance flags. This value is the result of using logical OR to combine the following flags: <see cref="Contravariant"/> and <see cref="Covariant"/>.
		/// </summary>
		VarianceMask = 3,
		/// <summary>
		/// A type can be substituted for the generic type parameter only if it is a reference type. 
		/// </summary>
		ReferenceTypeConstraint = 4,
		/// <summary>
		/// A type can be substituted for the generic type parameter only if it is a value type and is not nullable. 
		/// </summary>
		NotNullableValueTypeConstraint = 8,
		/// <summary>
		/// A type can be substituted for the generic type parameter only if it has a parameterless constructor. 
		/// </summary>
		DefaultConstructorConstraint = 0x10,
		/// <summary>
		/// Selects the combination of all special constraint flags. This value is the result of using logical OR to combine the following flags: <see cref="DefaultConstructorConstraint"/>, <see cref="ReferenceTypeConstraint"/>, and <see cref="NotNullableValueTypeConstraint"/>. 
		/// </summary>
		SpecialConstraintMask = 0x1c,
	}

	internal class GenericParameterAttributesConverter
	{
		static bool Match(Mono.Cecil.GenericParameterAttributes value, Mono.Cecil.GenericParameterAttributes @enum) { return (value & @enum) == @enum; }
		static bool Match(GenericParameterAttributes value, GenericParameterAttributes @enum) { return (value & @enum) == @enum; }

		public static GenericParameterAttributes Convert(Mono.Cecil.GenericParameterAttributes peAttrs)
		{
			GenericParameterAttributes attrs = (GenericParameterAttributes)0;
			if (Match(peAttrs, Mono.Cecil.GenericParameterAttributes.Contravariant)) attrs |= GenericParameterAttributes.Contravariant;
			if (Match(peAttrs, Mono.Cecil.GenericParameterAttributes.Covariant)) attrs |= GenericParameterAttributes.Covariant;
			if (Match(peAttrs, Mono.Cecil.GenericParameterAttributes.DefaultConstructorConstraint)) attrs |= GenericParameterAttributes.DefaultConstructorConstraint;
			if (Match(peAttrs, Mono.Cecil.GenericParameterAttributes.NotNullableValueTypeConstraint)) attrs |= GenericParameterAttributes.NotNullableValueTypeConstraint;
			if (Match(peAttrs, Mono.Cecil.GenericParameterAttributes.ReferenceTypeConstraint)) attrs |= GenericParameterAttributes.ReferenceTypeConstraint;
			return attrs;
		}

		public static Mono.Cecil.GenericParameterAttributes Convert(GenericParameterAttributes attrs)
		{
			Mono.Cecil.GenericParameterAttributes peAttrs = (Mono.Cecil.GenericParameterAttributes)0;
			if (Match(attrs, GenericParameterAttributes.Contravariant)) peAttrs |= Mono.Cecil.GenericParameterAttributes.Contravariant;
			if (Match(attrs, GenericParameterAttributes.Covariant)) peAttrs |= Mono.Cecil.GenericParameterAttributes.Covariant;
			if (Match(attrs, GenericParameterAttributes.DefaultConstructorConstraint)) peAttrs |= Mono.Cecil.GenericParameterAttributes.DefaultConstructorConstraint;
			if (Match(attrs, GenericParameterAttributes.NotNullableValueTypeConstraint)) peAttrs |= Mono.Cecil.GenericParameterAttributes.NotNullableValueTypeConstraint;
			if (Match(attrs, GenericParameterAttributes.ReferenceTypeConstraint)) peAttrs |= Mono.Cecil.GenericParameterAttributes.ReferenceTypeConstraint;
			return peAttrs;
		}
	}
}
