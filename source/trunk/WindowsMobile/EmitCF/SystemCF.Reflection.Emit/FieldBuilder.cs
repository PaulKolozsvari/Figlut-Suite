using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Defines and represents a field. This 
	/// </summary>
	public sealed class FieldBuilder: FieldInfo
	{
		#region Private & Internal
		internal FieldBuilder(string fieldName, Type type, System.Reflection.FieldAttributes attributes, TypeBuilder typeBuilder)
		{
			typeBuilder.TypeDef.Fields.Add(this.field = new FieldDefinition(fieldName, type.peType, FieldAttributesConverter.Convert(attributes)));
		}
		internal FieldBuilder(string fieldName, Type type, System.Reflection.FieldAttributes attributes, ModuleBuilder module)
		{
			module.peModule.Types[0].Fields.Add(this.field = new FieldDefinition(fieldName, type.peType, FieldAttributesConverter.Convert(attributes)));
		}
		internal FieldBuilder(FieldDefinition field)
		{
		}
		#endregion

		/// <summary>
		/// Sets the default value of this field. 
		/// </summary>
		/// <param name="defaultValue">The new default value for this field. </param>
		public void SetConstant(object defaultValue)
		{
			field.Constant = defaultValue;
		}

		/// <summary>
		/// Set a custom attribute using a custom attribute builder. 
		/// </summary>
		/// <param name="customBuilder">An instance of a helper class to define the custom attribute. </param>
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
	    	CustomAttribute ca;
	    	field.CustomAttributes.Add(ca = new CustomAttribute(customBuilder.ctor));
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
			field.CustomAttributes.Add(ca = new CustomAttribute(con.peMeth, binaryAttribute));
		}

		/// <summary>
		/// Get the CLR-equivalent <see cref="System.Reflection.FieldInfo"/> representation of a <see cref="FieldBuilder"/> object.
		/// </summary>
		/// <param name="type">The <see cref="FieldBuilder"/> to convert.</param>
		/// <returns>A <see cref="System.Reflection.FieldInfo"/> representation equivalent to the <paramref name="field"/> object.</returns>
		public static implicit operator System.Reflection.FieldInfo(FieldBuilder field)
		{
			if (field == null) return null;

			// Get CLR Type
			System.Type clrType = (TypeBuilder) field.DeclaringType;

			// Return field from its signature
			return clrType.GetField(field.Name);
		}
	}
}
