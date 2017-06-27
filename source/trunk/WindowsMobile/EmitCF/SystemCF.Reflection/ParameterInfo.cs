using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	/// <summary>
	/// Discovers the attributes of a parameter and provides access to parameter metadata. 
	/// </summary>
	public abstract class ParameterInfo
	{
		internal Mono.Cecil.ParameterDefinition param;

		#region Equality
		/// <summary>
		/// Returns <c>true</c> if the objects are equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are equal, <c>false</c> otherwise.</returns>
		public static bool operator ==(ParameterInfo t1, ParameterInfo t2)
		{
			if ((object) t1 == null || (object) t2 == null)
				return (object) t1 == (object) t2;
			if (t1 is CLRParameterInfo)
				return (t2 is CLRParameterInfo) && ((CLRParameterInfo) t1).param == ((CLRParameterInfo) t2).param;
			return (t2 is PEParameterInfo) && ((PEParameterInfo) t1).param == ((PEParameterInfo) t2).param;
		}

		/// <summary>
		/// Returns <c>true</c> if the objects are not equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are not equal, <c>false</c> otherwise.</returns>
		public static bool operator !=(ParameterInfo t1, ParameterInfo t2)
		{
			if ((object) t1 == null || (object) t2 == null)
				return (object) t1 != (object) t2;
			if (t1 is CLRParameterInfo)
				return !(t2 is CLRParameterInfo) || ((CLRParameterInfo) t1).param != ((CLRParameterInfo) t2).param;
			return !(t2 is PEParameterInfo) || ((PEParameterInfo) t1).param != ((PEParameterInfo) t2).param;
		}

		/// <summary>
		/// Determines whether two <see cref="System.Object"/> instances are equal. 
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public override bool Equals(object o)
		{
			if (o is ParameterInfo)
				return this == (ParameterInfo) o;
			return false;
		}

		/// <summary>
		/// Serves as a hash function for a particular type. <see cref="GetHashCode"/> is suitable for use in hashing algorithms and data structures like a hash table. 
		/// </summary>
		/// <returns>A hash code for the current <see cref="ParameterInfo"/>. </returns>
		public override int GetHashCode()
		{
			if (this is CLRParameterInfo) return ((CLRParameterInfo) this).param.GetHashCode();
			return ((PEParameterInfo)this).param.GetHashCode();
		}
		#endregion

		#region ParameterInfo
		/// <summary>
		/// Gets the attributes for this parameter.
		/// </summary>
		public virtual System.Reflection.ParameterAttributes Attributes { get { return ParameterAttributesConverter.Convert(param.Attributes); } }

		/// <summary>
		/// Gets a value indicating whether this is an input parameter.
		/// </summary>
		public virtual bool IsIn { get { return param.IsIn; } }

		/// <summary>
		/// Gets a value indicating whether this parameter is optional.
		/// </summary>
		public virtual bool IsOptional { get { return param.IsOptional; } }

		/// <summary>
		/// Gets a value indicating whether this is an output parameter.
		/// </summary>
		public virtual bool IsOut { get { return param.IsOut; } }

		/// <summary>
		/// Gets a value indicating whether this is a <c>Retval</c> parameter.
		/// </summary>
		public virtual bool IsRetval { get { return false; } }

		/// <summary>
		/// Gets a value indicating the member in which the parameter is implemented.
		/// </summary>
		public virtual MemberInfo Member { get { return MethodInfo.Wrap(param.Method as MethodDefinition); } }
		/// <summary>
		/// Gets the <see cref="Type"/> of this parameter.
		/// </summary>
		public virtual Type ParameterType { get { return param.ParameterType; } }

		/// <summary>
		/// Gets the signature position for the parameter.
		/// </summary>
		public virtual int Position { get { return param.Sequence; } }

		/// <summary>
		/// Gets the name of the parameter.
		/// </summary>
		public virtual string Name { get { return param.Name; } }

		/// <summary>
		/// Gets all the custom attributes defined on this parameter. 
		/// </summary>
		/// <param name="inherit"></param>
		/// <returns></returns>
		public virtual object[] GetCustomAttributes(bool inherit) { return null; }

		/// <summary>
		/// Gets the custom attributes of the specified type defined on this parameter. 
		/// </summary>
		/// <param name="attributeType"></param>
		/// <param name="inherit"></param>
		/// <returns></returns>
		public virtual object[] GetCustomAttributes(Type attributeType, bool inherit) { return null; }

		/// <summary>
		/// Determines if the custom attribute of the specified type is defined on this member. 
		/// </summary>
		/// <param name="attributeType"></param>
		/// <param name="inherit"></param>
		/// <returns></returns>
		public virtual bool IsDefined(Type attributeType, bool inherit) { return false; }
		#endregion

		#region Cast & Wrap
		/// <summary>
		/// Cast a <see cref="System.Reflection.ParameterInfo"/> into a <see cref="ParameterInfo"/>.
		/// </summary>
		/// <param name="o">A <see cref="System.Reflection.ParameterInfo"/> to be casted.</param>
		/// <returns>A <see cref="ParameterInfo"/>.</returns>
		public static implicit operator ParameterInfo(System.Reflection.ParameterInfo o) { return new CLRParameterInfo(o); }

		/// <summary>
		/// Cast a <see cref="Mono.Cecil.ParameterDefinition"/> into a <see cref="ParameterInfo"/>.
		/// </summary>
		/// <param name="o">A <see cref="Mono.Cecil.ParameterDefinition"/> to be casted.</param>
		/// <returns>A <see cref="ParameterInfo"/>.</returns>
		public static implicit operator ParameterInfo(Mono.Cecil.ParameterDefinition o) { return new PEParameterInfo(o); }

		internal static ParameterInfo Wrap(System.Reflection.ParameterInfo par)
		{
			if (par == null)
				return null;
			return new CLRParameterInfo(par);
		}

		internal static ParameterInfo[] Wrap(System.Reflection.ParameterInfo[] pars)
		{
			if (pars == null)
				return null;
			CLRParameterInfo[] wpars = new CLRParameterInfo[ pars.Length ];
			for (int i = 0; i < pars.Length; i++)
				wpars[ i ] = new CLRParameterInfo(pars[ i ]);
			return wpars;
		}

		internal static Type[] ToType(System.Reflection.ParameterInfo[] pars)
		{
			if (pars == null)
				return null;
			Type[] wpars = new Type[ pars.Length ];
			for (int i = 0; i < pars.Length; i++)
				wpars[ i ] = Type.Import(pars[ i ].ParameterType);
			return wpars;
		}

		internal static Type[] ToType(ParameterInfo[] pars)
		{
			if (pars == null)
				return null;
			Type[] wpars = new Type[pars.Length];
			for (int i = 0; i < pars.Length; i++)
				wpars[i] = pars[i].ParameterType;
			return wpars;
		}

		internal static System.Type[] ToCLRType(ParameterInfo[] pars)
		{
			if (pars == null)
				return null;
			System.Type[] wpars = new System.Type[ pars.Length ];
			for (int i = 0; i < pars.Length; i++)
				wpars[ i ] = pars[ i ].ParameterType;
			return wpars;
		}

		internal static Type[] ToType(Mono.Cecil.ParameterDefinitionCollection pars)
		{
			if (pars == null)
				return null;
			Type[] wpars = new Type[ pars.Count ];
			for (int i = 0; i < pars.Count; i++)
				wpars[ i ] = Type.Import(pars[ i ].ParameterType);
			return wpars;
		}

		internal static ParameterInfo Wrap(Mono.Cecil.ParameterDefinition par)
		{
			if (par == null)
				return null;
			return new PEParameterInfo(par);
		}

		internal static ParameterInfo[] Wrap(Mono.Cecil.ParameterDefinitionCollection pars)
		{
			if (pars == null)
				return null;
			PEParameterInfo[] wpars = new PEParameterInfo[ pars.Count ];
			for (int i = 0; i < pars.Count; i++)
				wpars[ i ] = new PEParameterInfo(pars[ i ]);
			return wpars;
		}

		#endregion

		#region Inner Classes
		internal class CLRParameterInfo: ParameterInfo
		{
			internal new System.Reflection.ParameterInfo param;
			public CLRParameterInfo(System.Reflection.ParameterInfo param) { this.param = param; }

			#region MemberInfo
			public override string Name { get { return param.Name; } }

			public override object[] GetCustomAttributes(bool inherit) { return param.GetCustomAttributes(inherit); }
			public override object[] GetCustomAttributes(Type attributeType, bool inherit) { return param.GetCustomAttributes(((Type.CLRType) attributeType).clrType, inherit); }
			public override bool IsDefined(Type attributeType, bool inherit) { return param.IsDefined(((Type.CLRType) attributeType).clrType, inherit); }
			#endregion

			#region ParameterInfo
			public override System.Reflection.ParameterAttributes Attributes { get { return (System.Reflection.ParameterAttributes) param.Attributes; } }
			public override bool IsIn { get { return (param.Attributes & System.Reflection.ParameterAttributes.In) != 0; } }
			public override bool IsOptional { get { return (param.Attributes & System.Reflection.ParameterAttributes.Optional) != 0; } }
			public override bool IsOut { get { return (param.Attributes & System.Reflection.ParameterAttributes.Out) != 0; } }
			public override bool IsRetval { get { return (param.Attributes & System.Reflection.ParameterAttributes.Retval) != 0; } }
			public override MemberInfo Member { get { return null; } }
			public override Type ParameterType { get { return param.ParameterType; } }
			public override int Position { get { return param.Position; } }
			#endregion
		}

		internal class PEParameterInfo: ParameterInfo
		{
			internal PEParameterInfo(Mono.Cecil.ParameterDefinition param) { this.param = param; }
		}
		#endregion
	}
}
