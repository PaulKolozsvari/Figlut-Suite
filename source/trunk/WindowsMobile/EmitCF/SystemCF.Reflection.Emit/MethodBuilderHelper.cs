using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	internal interface IMethodBuilder
	{
		ILGenerator GetILGenerator();
		ILGenerator GetILGenerator(int size);
	}
	
	internal class MethodBuilderHelper
	{
		static void CheckArgs(System.Reflection.MethodAttributes attributes, ref System.Reflection.CallingConventions callingConvention, bool IsInterface)
		{
			if ((attributes & System.Reflection.MethodAttributes.Static) == System.Reflection.MethodAttributes.ReuseSlot)
			{
				callingConvention |= System.Reflection.CallingConventions.HasThis;
			}
			else if ((attributes & System.Reflection.MethodAttributes.Virtual) != System.Reflection.MethodAttributes.ReuseSlot)
			{
				//throw new ArgumentException(Properties.Messages.Arg_NoStaticVirtual);
			}
			if ((((attributes & System.Reflection.MethodAttributes.SpecialName) != System.Reflection.MethodAttributes.SpecialName) && IsInterface) && (((attributes & (System.Reflection.MethodAttributes.Abstract | System.Reflection.MethodAttributes.Virtual)) != (System.Reflection.MethodAttributes.Abstract | System.Reflection.MethodAttributes.Virtual)) && ((attributes & System.Reflection.MethodAttributes.Static) == System.Reflection.MethodAttributes.ReuseSlot)))
			{
				//throw new ArgumentException(Properties.Messages.Argument_BadAttributeOnInterfaceMethod);
			}
		}

		static MethodDefinition GetMethodDef(bool iSCtor, string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, Type returnType, Type[] parameterTypes, ModuleDefinition mod)
		{
			if ((object)returnType == null) returnType = Type.VoidType;
			MethodDefinition md = new MethodDefinition(name, MethodAttributesConverter.Convert(attributes), returnType.peType);

			if (mod.Types.Count == 0)
			{
				mod.Types.Add(new TypeDefinition("<Module>", null, Mono.Cecil.TypeAttributes.NotPublic, Type.ObjectType));
			}

			if (iSCtor)
				mod.Types[0].Constructors.Add(md);
			else
				mod.Types[0].Methods.Add(md);

			if (parameterTypes != null)
			{
				int seq = 0;
				foreach (Type parameterType in parameterTypes)
				{
					ParameterDefinition pdef;
					md.Parameters.Add(pdef = new ParameterDefinition(parameterType.peType));
					pdef.Sequence = seq++;
				}
			}
			CallingConventionsConverter.Assign(md, callingConvention);
			md.ImplAttributes = Mono.Cecil.MethodImplAttributes.IL;
			return md;
		}

		static MethodDefinition GetMethodDef(bool iSCtor, string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, Type returnType, Type[] parameterTypes, TypeDefinition cls)
		{
			if ((object)returnType == null) returnType = Type.VoidType;
			MethodDefinition md = new MethodDefinition(name, MethodAttributesConverter.Convert(attributes), returnType.peType);
			if (iSCtor)
				cls.Constructors.Add(md);
			else
				cls.Methods.Add(md);
			if (parameterTypes != null)
			{
				int seq = 0;
				foreach (Type parameterType in parameterTypes)
				{
					ParameterDefinition pdef;
					md.Parameters.Add(pdef = new ParameterDefinition(parameterType.peType));
					pdef.Sequence = seq++;
				}
			}
			CallingConventionsConverter.Assign(md, callingConvention);
			md.ImplAttributes = Mono.Cecil.MethodImplAttributes.IL;
			return md;
		}

		internal MethodDefinition md;
		internal ILGenerator ilGenerator;

		public MethodBuilderHelper(bool iSCtor, string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, Type returnType, Type[] parameterTypes, ModuleBuilder mod)
		{
			CheckArgs(attributes, ref callingConvention, false);
			md = GetMethodDef(iSCtor, name, attributes, callingConvention, returnType, parameterTypes, mod.peModule);
		}

		public MethodBuilderHelper(bool iSCtor, string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, Type returnType, Type[] parameterTypes, TypeBuilder type)
		{
			CheckArgs(attributes, ref callingConvention, type.IsInterface);
			md = GetMethodDef(iSCtor, name, attributes, callingConvention, returnType, parameterTypes, type.TypeDef);
		}

		public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
		{
			GenericTypeParameterBuilder[] genPars = new GenericTypeParameterBuilder[names.Length];
			int i = 0;
			foreach (string name in names)
			{
				(genPars[i] = new GenericTypeParameterBuilder(name, md)).GenPar.Position = i++;
			}
			return genPars;
		}

		public ParameterBuilder DefineParameter(int position, System.Reflection.ParameterAttributes attributes, string strParamName)
		{
			return new ParameterBuilder(md, position, attributes, strParamName);
		}

		public ILGenerator GetILGenerator(int size)
		{
			if (size < 0x10) size = 0x10;
			ILGenerator il = GetILGenerator();
			il.code.GetBody().CodeSize = size; ;
			return il;
		}
		public ILGenerator GetILGenerator()
		{
			if (ilGenerator != null) return ilGenerator;
			CilWorker code = null;
			if (md.HasBody) code = md.Body.CilWorker;
			return ilGenerator = new ILGenerator(code);
		}

		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			CustomAttribute ca;
			md.CustomAttributes.Add(ca = new CustomAttribute(customBuilder.ctor));
			foreach (object arg in customBuilder.args) ca.ConstructorParameters.Add(arg);
		}

		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			CustomAttribute ca;
			md.CustomAttributes.Add(ca = new CustomAttribute(con.peMeth, binaryAttribute));
		}

		public void SetImplementationFlags(System.Reflection.MethodImplAttributes attr)
		{
			md.ImplAttributes = MethodImplAttributesConverter.Convert(attr);
		}

		public void SetParameters(params Type[] parameterTypes)
		{
			md.Parameters.Clear();
			foreach (Type parameterType in parameterTypes)
				md.Parameters.Add(new ParameterDefinition(parameterType.peType));
		}

		public void SetReturnType(Type returnType)
		{
			md.ReturnType = new MethodReturnType(returnType.peType);
		}

		public MethodToken GetToken()
		{
			return MethodToken.Empty;
		}

		// Properties
		public bool InitLocals
		{
			get { return md.Body.InitLocals; }
			set { md.Body.InitLocals = value; }
		}
		public string Signature { get { return md.ToString(); } }
	}
}
