using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Describes and represents an enumeration type. 
	/// </summary>
	public class EnumBuilder: TypeBuilder
	{
		#region Private&Internal
		internal EnumBuilder(string name, System.Reflection.TypeAttributes visibility, Type underlyingType, ModuleBuilder module)
			: base(name, visibility | System.Reflection.TypeAttributes.Sealed, typeof(System.Enum), null, module)
		{
			if ((visibility & ~System.Reflection.TypeAttributes.VisibilityMask) != System.Reflection.TypeAttributes.AutoLayout)
			{
				//throw new ArgumentException(Properties.Messages.Argument_ShouldOnlySetVisibilityFlags, "visibility");
			}

			this.underlyingType = underlyingType;
			DefineField("value__", underlyingType, System.Reflection.FieldAttributes.SpecialName | System.Reflection.FieldAttributes.Private | System.Reflection.FieldAttributes.RTSpecialName);
		}
		#endregion

		/// <summary>
		/// Describes and represents an enumeration type. 
		/// </summary>
		/// <param name="literalName">The name of the static field. </param>
		/// <param name="literalValue">The constant value of the literal. </param>
		/// <returns>The defined field. </returns>
		public FieldBuilder DefineLiteral(string literalName, object literalValue)
		{
			if (literalValue == null)
			{
				if (underlyingType.IsValueType)
				{
					//throw new ArgumentException(Properties.Messages.Argument_ConstantNull);
				}
			}
			else
			{
				Type type = literalValue.GetType();
				if (underlyingType.IsEnum)
				{
					if (underlyingType.UnderlyingSystemType != type)
					{
						//throw new ArgumentException(Properties.Messages.Argument_ConstantDoesntMatch);
					}
				}
				else
				{
					if (underlyingType != type)
					{
						//throw new ArgumentException(Properties.Messages.Argument_ConstantDoesntMatch);
					}
				}
			}
			FieldDefinition field;
			TypeDef.Fields.Add(field = new FieldDefinition(literalName, TypeDef, FieldAttributesConverter.Convert(System.Reflection.FieldAttributes.Public | System.Reflection.FieldAttributes.Literal | System.Reflection.FieldAttributes.Static | System.Reflection.FieldAttributes.HasDefault)));
			field.Constant = literalValue;
			return new FieldBuilder(field);
		}
	}
}
