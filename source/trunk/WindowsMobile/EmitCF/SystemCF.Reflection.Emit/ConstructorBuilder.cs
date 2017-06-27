using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Defines and represents a constructor of a dynamic class. 
	/// </summary>
	public class ConstructorBuilder: ConstructorInfo, IMethodBuilder
	{
		#region Private&Internal
		internal MethodBuilderHelper helper;

		internal ConstructorBuilder(System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, Type[] parameterTypes, ModuleBuilder mod)
		{
			string constructorName;
			if ((attributes & System.Reflection.MethodAttributes.Static) == System.Reflection.MethodAttributes.ReuseSlot)
			{
				constructorName = ConstructorInfo.ConstructorName;
			}
			else
			{
				constructorName = ConstructorInfo.TypeConstructorName;
			}
			attributes |= System.Reflection.MethodAttributes.SpecialName | System.Reflection.MethodAttributes.RTSpecialName;
 			
			helper = new MethodBuilderHelper(true, constructorName, attributes, callingConvention, Type.VoidType, parameterTypes, mod);
			this.ctor = helper.md;
		}

		internal ConstructorBuilder(System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, Type[] parameterTypes, TypeBuilder typ)
		{
			string constructorName;
			if ((attributes & System.Reflection.MethodAttributes.Static) == System.Reflection.MethodAttributes.ReuseSlot)
			{
				constructorName = ConstructorInfo.ConstructorName;
			}
			else
			{
				constructorName = ConstructorInfo.TypeConstructorName;
			}
			attributes |= System.Reflection.MethodAttributes.SpecialName | System.Reflection.MethodAttributes.RTSpecialName;

			helper = new MethodBuilderHelper(true, constructorName, attributes, callingConvention, Type.VoidType, parameterTypes, typ);
			this.ctor = helper.md;
		}
		#endregion

		/// <summary>
		/// Sets the number of generic type parameters for the current method, specifies their names, and returns an array of <see cref="GenericTypeParameterBuilder"/> objects that can be used to define their constraints. 
		/// </summary>
		/// <param name="names">An array of strings that represent the names of the generic type parameters.</param>
		/// <returns>An array of <see cref="GenericTypeParameterBuilder"/> objects representing the type parameters of the generic method.</returns>
		public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
		{
			return helper.DefineGenericParameters(names);
		}
 
		/// <summary>
		/// Defines a parameter of this method. 
		/// </summary>
		/// <param name="position">The position of the parameter in the parameter list. Parameters are indexed beginning with the number 1 for the first parameter. </param>
		/// <param name="attributes">The attributes of the parameter. </param>
		/// <param name="strParamName">The name of the parameter. </param>
		/// <returns>Returns a <see cref="ParameterBuilder"/> object that represents the new parameter of this method. </returns>
		public ParameterBuilder DefineParameter(int position, System.Reflection.ParameterAttributes attributes, string strParamName)
		{
			return helper.DefineParameter(position, attributes, strParamName);
		}

		/// <summary>
		/// Returns an <see cref="ILGenerator"/> for this method with a default Microsoft intermediate language (MSIL) stream size of 64 bytes. 
		/// </summary>
		/// <returns>Returns an <see cref="ILGenerator"/> object for this method. </returns>
		public ILGenerator GetILGenerator()
		{
			return helper.GetILGenerator();
		}

		/// <summary>
		/// Returns an ILGenerator for this method with the specified Microsoft intermediate language (MSIL) stream size. 
		/// </summary>
		/// <param name="size">The size of the MSIL stream, in bytes. </param>
		/// <returns>Returns an <see cref="ILGenerator"/> object for this method. </returns>
		public ILGenerator GetILGenerator(int size)
		{
			return helper.GetILGenerator(size);
		}

		/// <summary>
		/// Sets a custom attribute using a custom attribute builder.
		/// </summary>
		/// <param name="customBuilder">An instance of a helper class to describe the custom attribute.</param>
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			helper.SetCustomAttribute(customBuilder);
		}

		/// <summary>
		/// Sets a custom attribute using a specified custom attribute blob. 
		/// </summary>
		/// <param name="con">The constructor for the custom attribute. </param>
		/// <param name="binaryAttribute">A byte blob representing the attributes.</param>
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			helper.SetCustomAttribute(con, binaryAttribute);
		}

		/// <summary>
		/// Sets the implementation flags for this method. 
		/// </summary>
		/// <param name="attr">The implementation flags to set. </param>
		public void SetImplementationFlags(System.Reflection.MethodImplAttributes attr)
		{
			helper.SetImplementationFlags(attr);
		}

		/// <summary>
		/// Sets the number and types of parameters for a method. 
		/// </summary>
		/// <param name="parameterTypes">An array of <see cref="Type"/> objects representing the parameter types.</param>
		public void SetParameters(params Type[] parameterTypes)
		{
			helper.SetParameters(parameterTypes);
		}

		/// <summary>
		/// Returns the <see cref="MethodToken"/> that represents the token for this constructor. 
		/// </summary>
		/// <returns></returns>
		public MethodToken GetToken()
		{
			return helper.GetToken();
		}

		// Properties

		/// <summary>
		/// Gets or sets a Boolean value that specifies whether the local variables in this method are zero initialized. 
		/// </summary>
		public bool InitLocals { 
			get { return helper.InitLocals; }
			set { helper.InitLocals = value; } 
		}

		/// <summary>
		/// Retrieves the signature of the method. 
		/// </summary>
		public string Signature { get { return helper.Signature; } }

		/// <summary>
		/// Get the CLR-equivalent <see cref="System.Reflection.ConstructorInfo"/> representation of a <see cref="ConstructorBuilder"/> object.
		/// </summary>
		/// <param name="type">The <see cref="ConstructorBuilder"/> to convert.</param>
		/// <returns>A <see cref="System.Reflection.ConstructorInfo"/> representation equivalent to the <paramref name="ctor"/> object.</returns>
		public static implicit operator System.Reflection.ConstructorInfo(ConstructorBuilder ctor)
		{
			if (ctor == null) return null;

			// Get CLR Type
			System.Type clrType = (TypeBuilder) ctor.DeclaringType;

			// Return constructor from its signature
			return clrType.GetConstructor(ParameterInfo.ToCLRType(ctor.GetParameters()));
		}
	}
}
