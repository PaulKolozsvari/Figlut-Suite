using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Defines and creates generic type parameters for dynamically defined generic types and methods. 
	/// </summary>
	public sealed class GenericTypeParameterBuilder : Type
	{
		#region Private&Internal
		internal GenericParameter GenPar { get { return peType as GenericParameter; } }

		internal GenericTypeParameterBuilder(string name, MethodDefinition md) 
		{
		    GenericParameter gp = new GenericParameter(name, md);
			md.GenericParameters.Add(gp);
			peType = gp;
		}

		internal GenericTypeParameterBuilder(string name, TypeBuilder tb)
		{
		    GenericParameter gp = new GenericParameter(name, tb.TypeDef);
			tb.TypeDef.GenericParameters.Add(gp);
			peType = gp;
		}
		#endregion

		/// <summary>
		/// Sets the base type that a type must inherit in order to be substituted for the type parameter. 
		/// </summary>
		/// <param name="baseTypeConstraint">The <see cref="Type"/> that must be inherited by any type that is to be substituted for the type parameter.</param>
		public void SetBaseTypeConstraint(Type baseTypeConstraint)
		{
			GenPar.Constraints.Add(baseTypeConstraint.peType);
		}

		/// <summary>
		/// Sets the variance characteristics and special constraints of the generic parameter, such as the parameterless constructor constraint.
		/// </summary>
		/// <param name="genericParameterAttributes">A bitwise combination of <see cref="GenericParameterAttributes"/> values that represent the variance characteristics and special constraints of the generic type parameter.</param>
		public void SetGenericParameterAttributes(GenericParameterAttributes genericParameterAttributes)
		{
			GenPar.Attributes = GenericParameterAttributesConverter.Convert(genericParameterAttributes);
		}

		/// <summary>
		/// Sets the interfaces a type must implement in order to be substituted for the type parameter. 
		/// </summary>
		/// <param name="interfaceConstraints">An array of Type objects that represent the interfaces a type must implement in order to be substituted for the type parameter.</param>
		public void SetInterfaceConstraints(params Type[] interfaceConstraints)
		{
			foreach (Type type in interfaceConstraints)
				GenPar.Constraints.Add(type.peType);
		}

	}
}
