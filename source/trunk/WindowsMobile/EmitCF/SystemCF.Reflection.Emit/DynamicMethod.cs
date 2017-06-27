using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Defines and represents a dynamic method. 
	/// <remarks>The implementation of this class doesn't support the private members of the containing class!</remarks>
	/// </summary>
	public class DynamicMethod : MethodInfo
	{
		#region Private&Internal
		internal ModuleBuilder mb;
		internal TypeBuilder tb;
		internal System.Reflection.MethodInfo methCLR;
		internal MethodBuilderHelper helper;

		private void CheckName(ref string name)
		{
			if (name == string.Empty) name = "MyDynamicMethod";
		}

		private void BuildModuleBuilder(string methodName)
		{
			System.Reflection.AssemblyName name = new System.Reflection.AssemblyName();
			name.Name = "MyDynamicAssembly_" + methodName;
			AssemblyBuilder ab = new AssemblyBuilder(name, AssemblyBuilderAccess.RunAndSave, null);
			mb = ab.DefineDynamicModule("MyDynamicModule_" + methodName + ".dll");
		}

		private void GetMethodInfo()
		{
			System.Reflection.Assembly assembly = mb.GetAssembly();
			if (tb != null) methCLR = assembly.GetType(tb.FullName).GetMethod(helper.md.Name);
			else methCLR = assembly.GetModules()[0].GetTypes()[0].GetMethod(helper.md.Name);
		}

		#endregion

		/// <summary>
		/// Creates a dynamic method that is global to a module, specifying the method name, return type, parameter types, and module. 
		/// </summary>
		/// <param name="name">The name of the dynamic method. </param>
		/// <param name="returnType">A <see cref="Type"/> object that specifies the return type of the dynamic method, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the method has no return type. </param>
		/// <param name="parameterTypes">An array of <see cref="Type"/> objects specifying the types of the parameters of the dynamic method, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the method has no parameters. </param>
		/// <param name="m">A <see cref="Module"/> representing the module with which the dynamic method is to be logically associated.</param>
		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Module m)
			: this(name, System.Reflection.MethodAttributes.Static | System.Reflection.MethodAttributes.Public, System.Reflection.CallingConventions.Standard, returnType, parameterTypes, m, false)
		{
		}

		/// <summary>
		/// Creates a dynamic method, specifying the method name, return type, parameter types, and the type with which the dynamic method is logically associated. 
		/// </summary>
		/// <param name="name">The name of the dynamic method. </param>
		/// <param name="returnType">A <see cref="Type"/> object that specifies the return type of the dynamic method, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the method has no return type. </param>
		/// <param name="parameterTypes">An array of <see cref="Type"/> objects specifying the types of the parameters of the dynamic method, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the method has no parameters. </param>
		/// <param name="owner">A <see cref="Type"/> with which the dynamic method is logically associated. 
		/// The dynamic method has access to all members of the type.
		/// <remarks>False in this implementation!</remarks>
		/// </param>
		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, System.Type owner)
			: this(name, System.Reflection.MethodAttributes.Static | System.Reflection.MethodAttributes.Public, System.Reflection.CallingConventions.Standard, returnType, parameterTypes, owner, false)
		{
		}

		/// <summary>
		/// Creates a dynamic method that is global to a module, specifying the method name, return type, parameter types, module, and whether just-in-time (JIT) visibility checks should be skipped for members of all types in the module. 
		/// </summary>
		/// <param name="name">The name of the dynamic method. </param>
		/// <param name="returnType">A <see cref="Type"/> object that specifies the return type of the dynamic method, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the method has no return type. </param>
		/// <param name="parameterTypes">An array of <see cref="Type"/> objects specifying the types of the parameters of the dynamic method, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the method has no parameters. </param>
		/// <param name="m">A <see cref="Module"/> representing the module with which the dynamic method is to be logically associated.</param>
		/// <param name="skipVisibility">** Not used **</param>
		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Module m, bool skipVisibility)
			: this(name, System.Reflection.MethodAttributes.Static | System.Reflection.MethodAttributes.Public, System.Reflection.CallingConventions.Standard, returnType, parameterTypes, m, skipVisibility)
		{
		}

		/// <summary>
		/// Creates a dynamic method, specifying the method name, return type, parameter types, the type with which the dynamic method is logically associated, and whether just-in-time (JIT) visibility checks should be skipped for members of other types in the module. 
		/// </summary>
		/// <param name="name">The name of the dynamic method. </param>
		/// <param name="returnType">A <see cref="Type"/> object that specifies the return type of the dynamic method, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the method has no return type. </param>
		/// <param name="parameterTypes">An array of <see cref="Type"/> objects specifying the types of the parameters of the dynamic method, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the method has no parameters. </param>
		/// <param name="owner">A <see cref="Type"/> with which the dynamic method is logically associated. </param>
		/// <param name="skipVisibility">** Not used **</param>
		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, System.Type owner, bool skipVisibility)
			: this(name, System.Reflection.MethodAttributes.Static | System.Reflection.MethodAttributes.Public, System.Reflection.CallingConventions.Standard, returnType, parameterTypes, owner, skipVisibility)
		{
		}

		/// <summary>
		/// Creates a dynamic method that is global to a module, specifying the method name, attributes, calling convention, return type, parameter types, module, and whether just-in-time (JIT) visibility checks should be skipped for members of all types in the module. 
		/// </summary>
		/// <param name="name">The name of the dynamic method. </param>
		/// <param name="attributes">A bitwise combination of <see cref="MethodAttributes"/> values that specifies the attributes of the dynamic method. The only combination allowed is <c>Public</c> and <c>Static</c>.</param>
		/// <param name="callingConvention"></param>
		/// <param name="returnType">A <see cref="Type"/> object that specifies the return type of the dynamic method, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the method has no return type. </param>
		/// <param name="parameterTypes">An array of <see cref="Type"/> objects specifying the types of the parameters of the dynamic method, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the method has no parameters. </param>
		/// <param name="m">A <see cref="Module"/> representing the module with which the dynamic method is to be logically associated.</param>
		/// <param name="skipVisibility">** Not used **</param>
		public DynamicMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Module m, bool skipVisibility)
		{
			CheckName(ref name);
			BuildModuleBuilder(name);

			helper = new MethodBuilderHelper(false, name, attributes, callingConvention, returnType, parameterTypes, mb);
			this.peMeth = helper.md;
		}

		/// <summary>
		/// Creates a dynamic method, specifying the method name, attributes, calling convention, return type, parameter types, the type with which the dynamic method is logically associated, and whether just-in-time (JIT) visibility checks should be skipped for members of other types in the module. 
		/// </summary>
		/// <param name="name">The name of the dynamic method. </param>
		/// <param name="attributes">A bitwise combination of <see cref="MethodAttributes"/> values that specifies the attributes of the dynamic method. The only combination allowed is <c>Public</c> and <c>Static</c>.</param>
		/// <param name="callingConvention"></param>
		/// <param name="returnType">A <see cref="Type"/> object that specifies the return type of the dynamic method, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the method has no return type. </param>
		/// <param name="parameterTypes">An array of <see cref="Type"/> objects specifying the types of the parameters of the dynamic method, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the method has no parameters. </param>
		/// <param name="owner">A <see cref="Type"/> with which the dynamic method is logically associated. </param>
		/// <param name="skipVisibility">** Not used **</param>
		public DynamicMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, Type returnType, Type[] parameterTypes, System.Type owner, bool skipVisibility)
		{
			CheckName(ref name);
			BuildModuleBuilder(name);
			tb = mb.DefineType(name, System.Reflection.TypeAttributes.Public, owner);

			helper = new MethodBuilderHelper(false, name, attributes, callingConvention, returnType, parameterTypes, tb);
			this.peMeth = helper.md;
		}
 
		/// <summary>
		/// Completes the dynamic method and creates a delegate that can be used to execute it. 
		/// </summary>
		/// <param name="delegateType">A delegate type whose signature matches that of the dynamic method.</param>
		/// <returns>A delegate of the specified type, which can be used to execute the dynamic method. </returns>
		public System.Delegate CreateDelegate(System.Type delegateType)
		{
			return CreateDelegate(delegateType, null);
		}

		/// <summary>
		/// Completes the dynamic method and creates a delegate that can be used to execute it, specifying the delegate type and an object the delegate is bound to. 
		/// </summary>
		/// <param name="delegateType">A delegate type whose signature matches that of the dynamic method.</param>
		/// <param name="target">An object the delegate is bound to. Must be of the same type as the first parameter of the dynamic method. </param>
		/// <returns>A delegate of the specified type, which can be used to execute the dynamic method with the specified target object. </returns>
		public System.Delegate CreateDelegate(System.Type delegateType, object target)
		{
			if (methCLR == null)
			{
				GetMethodInfo();
			}
#if CF_3_5
			return (System.MulticastDelegate)System.Delegate.CreateDelegate(delegateType, target, methCLR);
#else
			throw new System.NotSupportedException();
#endif
		}

		/// <summary>
		/// Defines a parameter of the dynamic method. 
		/// </summary>
		/// <param name="position">The position of the parameter in the parameter list. Parameters are indexed beginning with the number 1 for the first parameter.</param>
		/// <param name="attributes">A bitwise combination of <see cref="ParameterAttributes"/> values that specifies the attributes of the parameter. </param>
		/// <param name="strParamName">The name of the parameter.</param>
		/// <returns>A <see cref="ParameterBuilder"/> object that represents the parameter. </returns>
		public ParameterBuilder DefineParameter(int position, System.Reflection.ParameterAttributes attributes, string strParamName)
		{
			return helper.DefineParameter(position, attributes, strParamName);
		}

		/// <summary>
		/// Returns an MSIL generator for the method with a default MSIL stream size of 64 bytes. 
		/// </summary>
		/// <returns>An <see cref="ILGenerator"/> object for the method. </returns>
		public ILGenerator GetILGenerator()
		{
			return helper.GetILGenerator();
		}

		/// <summary>
		/// Returns an MSIL generator for the method with the specified MSIL stream size. 
		/// </summary>
		/// <param name="size">The size of the MSIL stream, in bytes. </param>
		/// <returns>An <see cref="ILGenerator"/> object for the method, with the specified MSIL stream size. </returns>
		public ILGenerator GetILGenerator(int size)
		{
			return helper.GetILGenerator(size);
		}

		/// <summary>
		/// Invokes the method or constructor represented by the current instance, using the specified parameters. 
		/// </summary>
		/// <param name="obj">The object on which to invoke the method or constructor</param>
		/// <param name="parameters">An argument list for the invoked method or constructor.</param>
		/// <returns>An object containing the return value of the invoked method, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) in the case of a constructor. </returns>
		public object Invoke(object obj, object[] parameters)
		{
			if (methCLR == null)
			{
				GetMethodInfo();
			}
			return methCLR.Invoke(obj, parameters);
		}

		// Properties

		/// <summary>
		/// Gets or sets a value indicating whether the local variables in the method are zero-initialized. 
		/// </summary>
		public bool InitLocals { 
			get { return helper.InitLocals; }
			set { helper.InitLocals = value; } 
		}

		/// <summary>
		/// Get the CLR-equivalent <see cref="System.Reflection.MethodInfo"/> representation of a <see cref="DynamicMethod"/> object.
		/// </summary>
		/// <param name="type">The <see cref="DynamicMethod"/> to convert.</param>
		/// <returns>A <see cref="System.Reflection.MethodInfo"/> representation equivalent to the <paramref name="meth"/> object.</returns>
		public static implicit operator System.Reflection.MethodInfo(DynamicMethod meth)
		{
			if (meth == null) return null;

			// Get CLR Type
			System.Type clrType = (TypeBuilder) meth.DeclaringType;

			// Return method from its signature
			return clrType.GetMethod(meth.Name, ParameterInfo.ToCLRType(meth.GetParameters()));
		}
	}
}
