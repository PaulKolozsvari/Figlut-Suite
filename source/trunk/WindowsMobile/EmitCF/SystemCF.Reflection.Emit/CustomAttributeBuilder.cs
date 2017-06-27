using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Helps build custom attributes. 
	/// </summary>
	public class CustomAttributeBuilder
	{
		#region Private & Internal
		internal MethodReference ctor;
		internal object[] args;
		#endregion

		/// <summary>
		/// Initializes an instance of the <see cref="CustomAttributeBuilder"/> class given the constructor for the custom attribute and the arguments to the constructor.  
		/// </summary>
		/// <param name="con">The constructor for the custom attribute. </param>
		/// <param name="constructorArgs">The arguments to the constructor of the custom attribute. </param>
		public CustomAttributeBuilder(ConstructorInfo con, object[] constructorArgs)
			: this(con, constructorArgs, null, null, null, null)
		{
		}

		/// <summary>
		/// Initializes an instance of the <see cref="CustomAttributeBuilder"/> class given the constructor for the custom attribute, the arguments to the constructor, and a set of named field/value pairs. 
		/// </summary>
		/// <param name="con">The constructor for the custom attribute. </param>
		/// <param name="constructorArgs">The arguments to the constructor of the custom attribute. </param>
		/// <param name="namedFields">Named fields of the custom attribute.</param>
		/// <param name="fieldValues">Values for the named fields of the custom attribute.</param>
		public CustomAttributeBuilder(ConstructorInfo con, object[] constructorArgs, FieldInfo[] namedFields, object[] fieldValues)
			: this(con, constructorArgs, null, null, namedFields, fieldValues)
		{
		}

		/// <summary>
		/// Initializes an instance of the <see cref="CustomAttributeBuilder"/> class given the constructor for the custom attribute, the arguments to the constructor, and a set of named property or value pairs. 
		/// </summary>
		/// <param name="con">The constructor for the custom attribute. </param>
		/// <param name="constructorArgs">The arguments to the constructor of the custom attribute. </param>
		/// <param name="namedProperties">Named properties of the custom attribute. </param>
		/// <param name="propertyValues">Values for the named properties of the custom attribute. </param>
		public CustomAttributeBuilder(ConstructorInfo con, object[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues)
			: this(con, constructorArgs, namedProperties, propertyValues, null, null)
		{
		}

		/// <summary>
		/// Initializes an instance of the <see cref="CustomAttributeBuilder"/> class given the constructor for the custom attribute, the arguments to the constructor, a set of named property or value pairs, and a set of named field or value pairs.
		/// </summary>
		/// <param name="con">The constructor for the custom attribute. </param>
		/// <param name="constructorArgs">The arguments to the constructor of the custom attribute. </param>
		/// <param name="namedProperties">Named properties of the custom attribute. </param>
		/// <param name="propertyValues">Values for the named properties of the custom attribute. </param>
		/// <param name="namedFields">Named fields of the custom attribute.</param>
		/// <param name="fieldValues">Values for the named fields of the custom attribute.</param>
		public CustomAttributeBuilder(ConstructorInfo con, object[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues, FieldInfo[] namedFields, object[] fieldValues)
		{
			if (((con.Attributes & System.Reflection.MethodAttributes.Static) == System.Reflection.MethodAttributes.Static) || ((con.Attributes & System.Reflection.MethodAttributes.MemberAccessMask) == System.Reflection.MethodAttributes.Private))
			{
				//throw new ArgumentException(Properties.Messages.Argument_BadConstructor);
			}
			if ((con.CallingConvention & System.Reflection.CallingConventions.Standard) != System.Reflection.CallingConventions.Standard)
			{
				//throw new ArgumentException(Properties.Messages.Argument_BadConstructorCallConv);
			}

			if (con is ConstructorInfo.CLRConstructorInfo)
				ctor = Type.Find(((ConstructorInfo.CLRConstructorInfo) con).ctor);
			else
				ctor = ((ConstructorInfo.PEConstructorInfo) con).ctor;

			args = constructorArgs;
		}

	}
}
