using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Defines the properties for a type. 
	/// </summary>
	public class PropertyBuilder: PropertyInfo
	{
		#region Private & Internal
		internal PropertyBuilder(string name, System.Reflection.PropertyAttributes attr, Type returnType, Type[] parameterTypes, TypeBuilder containingType)
		{
			containingType.TypeDef.Properties.Add(this.prop = new PropertyDefinition(name, returnType.peType, PropertyAttributesConverter.Convert(attr)));
			if (parameterTypes != null)
				foreach (Type parameterType in parameterTypes)
					prop.Parameters.Add(new ParameterDefinition(parameterType.peType));
		}
		#endregion

		/// <summary>
		/// Sets the default value of this property. 
		/// </summary>
		/// <param name="defaultValue">The default value of this property.</param>
		public void SetConstant(object defaultValue)
		{
			prop.Constant = defaultValue;
		}

		/// <summary>
		/// Set a custom attribute using a custom attribute builder. 
		/// </summary>
		/// <param name="customBuilder">An instance of a helper class to define the custom attribute. </param>
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
	    	CustomAttribute ca;
	    	prop.CustomAttributes.Add(ca = new CustomAttribute(customBuilder.ctor));
	    	foreach (object arg in customBuilder.args) ca.ConstructorParameters.Add(arg);
		}

		/// <summary>
		/// Set a custom attribute using a specified custom attribute blob. 
		/// </summary>
		/// <param name="con">Set a custom attribute using a specified custom attribute blob. </param>
		/// <param name="binaryAttribute">A byte blob representing the attributes. </param>
	    public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
	    	CustomAttribute ca;
			prop.CustomAttributes.Add(ca = new CustomAttribute(con.peMeth, binaryAttribute));
		}

		/// <summary>
		/// Sets the method that gets the property value. 
		/// </summary>
		/// <param name="mdBuilder">A <see cref="MethodBuilder"/> object that represents the method that gets the property value. </param>
		public void SetGetMethod(MethodBuilder mdBuilder)
		{
			prop.GetMethod = mdBuilder.meth;
		}

		/// <summary>
		/// Sets the method that sets the property value. 
		/// </summary>
		/// <param name="mdBuilder">A <see cref="MethodBuilder"/> object that represents the method that sets the property value. </param>
		public void SetSetMethod(MethodBuilder mdBuilder)
		{
			prop.SetMethod = mdBuilder.meth;
		}

		/// <summary>
		/// Get the CLR-equivalent <see cref="System.Reflection.PropertyInfo"/> representation of a <see cref="PropertyBuilder"/> object.
		/// </summary>
		/// <param name="type">The <see cref="PropertyBuilder"/> to convert.</param>
		/// <returns>A <see cref="System.Reflection.PropertyInfo"/> representation equivalent to the <paramref name="prop"/> object.</returns>
		public static implicit operator System.Reflection.PropertyInfo(PropertyBuilder prop)
		{
			if (prop == null) return null;

			// Get CLR Type
			System.Type clrType = (TypeBuilder) prop.DeclaringType;

			// Return property from its signature
			System.Type[] clrTypes = ParameterInfo.ToCLRType(prop.GetIndexParameters());
			if (clrTypes != null)
				return clrType.GetProperty(prop.Name, prop.PropertyType, clrTypes);
			return clrType.GetProperty(prop.Name, prop.PropertyType);
		}
	}
}
