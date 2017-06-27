using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Creates or associates parameter information. 
	/// </summary>
	public class ParameterBuilder
	{
		#region Private&Internal
		internal ParameterDefinition param;

		internal ParameterBuilder(MethodDefinition md, int sequence, System.Reflection.ParameterAttributes attributes, string strParamName)
		{
			if ((sequence < 0) ||
			    ((sequence > 0) && (sequence > md.Parameters.Count)))
			{
				//throw new ArgumentOutOfRangeException(Properties.Messages.ArgumentOutOfRange_ParamSequence);
			}
			attributes &= ~System.Reflection.ParameterAttributes.ReservedMask;
  
			param = md.Parameters[ sequence - 1 ];
			param.Attributes = ParameterAttributesConverter.Convert(attributes);
			param.Name = strParamName;
			param.Sequence = sequence;
		}
		#endregion

		/// <summary>
		/// Sets the default value of the parameter. 
		/// </summary>
		/// <param name="defaultValue">The default value of this parameter. </param>
		public void SetConstant(object defaultValue)
		{
			param.Constant = defaultValue;
		}

		/// <summary>
		/// Set a custom attribute using a custom attribute builder. 
		/// </summary>
		/// <param name="customBuilder">An instance of a helper class to define the custom attribute. </param>
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
	    	CustomAttribute ca;
	    	param.CustomAttributes.Add(ca = new CustomAttribute(customBuilder.ctor));
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
			param.CustomAttributes.Add(ca = new CustomAttribute(con.peMeth, binaryAttribute));
		}

		// Properties

		/// <summary>
		/// Retrieves the attributes for this parameter.
		/// </summary>
		public int Attributes { get { return (int) ParameterAttributesConverter.Convert(param.Attributes); } }

		/// <summary>
		/// Retrieves whether this is an input parameter.
		/// </summary>
		public bool IsIn { get { return param.IsIn; } }

		/// <summary>
		/// Retrieves whether this parameter is optional.
		/// </summary>
		public bool IsOptional { get { return param.IsOptional; } }

		/// <summary>
		/// Retrieves whether this parameter is optional.
		/// </summary>
		public bool IsOut { get { return param.IsOut; } }

		/// <summary>
		/// Retrieves the name of this parameter.
		/// </summary>
		public string Name { get { return param.Name; } }

		/// <summary>
		/// Retrieves the signature position for this parameter.
		/// </summary>
		public int Position { get { return param.Sequence; } }
	}
}
