using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	/// <summary>
	/// Discovers the attributes of a method and provides access to method metadata. 
	/// </summary>
	public abstract class MethodInfo : MethodBase
	{
		internal Mono.Cecil.MethodDefinition meth { get { return peMeth as Mono.Cecil.MethodDefinition; } }

		#region Equality
		/// <summary>
		/// Returns <c>true</c> if the objects are equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are equal, <c>false</c> otherwise.</returns>
		public static bool operator ==(MethodInfo t1, MethodInfo t2)
		{
			if ((object) t1 == null || (object) t2 == null)
				return (object) t1 == (object) t2;
			if (t1 is CLRMethodInfo)
				return (t2 is CLRMethodInfo) && ((CLRMethodInfo) t1).meth == ((CLRMethodInfo) t2).meth;
			return (t2 is PEMethodInfo) && ((PEMethodInfo) t1).meth == ((PEMethodInfo) t2).meth;
		}

		/// <summary>
		/// Returns <c>true</c> if the objects are not equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are not equal, <c>false</c> otherwise.</returns>
		public static bool operator !=(MethodInfo t1, MethodInfo t2)
		{
			if ((object) t1 == null || (object) t2 == null)
				return (object) t1 != (object) t2;
			if (t1 is CLRMethodInfo)
				return !(t2 is CLRMethodInfo) || ((CLRMethodInfo) t1).meth != ((CLRMethodInfo) t2).meth;
			return !(t2 is PEMethodInfo) || ((PEMethodInfo) t1).meth != ((PEMethodInfo) t2).meth;
		}

		/// <summary>
		/// Determines whether two <see cref="System.Object"/> instances are equal. 
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public override bool Equals(object o)
		{
			if (o is MethodInfo)
				return this == (MethodInfo) o;
			return false;
		}

		/// <summary>
		/// Serves as a hash function for a particular type. <see cref="GetHashCode"/> is suitable for use in hashing algorithms and data structures like a hash table. 
		/// </summary>
		/// <returns>A hash code for the current <see cref="MethodInfo"/>. </returns>
		public override int GetHashCode()
		{
			if (this is CLRMethodInfo) return ((CLRMethodInfo)this).meth.GetHashCode();
			return ((PEMethodInfo)this).meth.GetHashCode();
		}
		#endregion

		#region MethodBase
		/// <summary>
		/// Gets the attributes associated with this method.
		/// </summary>
		public override System.Reflection.MethodAttributes Attributes { get { return MethodAttributesConverter.Convert(meth.Attributes); } }

		/// <summary>
		/// Gets a value indicating the calling conventions for this method.
		/// </summary>
		public override System.Reflection.CallingConventions CallingConvention { get { return CallingConventionsConverter.Extract(meth); } }

		/// <summary>
		/// Gets a value indicating whether the generic method contains unassigned generic type parameters.
		/// </summary>
		public override bool ContainsGenericParameters { get { return meth.GenericParameters.Count > 0; } }

		/// <summary>
		/// Gets a value indicating whether the method is abstract.
		/// </summary>
		public override bool IsAbstract { get { return meth.IsAbstract; } }

		/// <summary>
		/// Gets a value indicating whether this method can be called by other classes in the same assembly.
		/// </summary>
		public override bool IsAssembly { get { return meth.IsAssembly; } }

		/// <summary>
		/// Gets a value indicating whether this method can be called by other classes in the same assembly.
		/// </summary>
		public override bool IsConstructor { get { return true; } }

		/// <summary>
		/// Gets a value indicating whether access to this method is restricted to members of the class and members of its derived classes.
		/// </summary>
		public override bool IsFamily { get { return meth.IsFamily; } }

		/// <summary>
		/// Gets a value indicating whether this method can be called by derived classes if they are in the same assembly.
		/// </summary>
		public override bool IsFamilyAndAssembly { get { return meth.IsFamilyAndAssembly; } }

		/// <summary>
		/// Gets a value indicating whether this method can be called by derived classes, wherever they are, and by all classes in the same assembly.
		/// </summary>
		public override bool IsFamilyOrAssembly { get { return meth.IsFamilyOrAssembly; } }

		/// <summary>
		/// Gets a value indicating whether this method is <c>final</c>.
		/// </summary>
		public override bool IsFinal { get { return meth.IsFinal; } }

		/// <summary>
		/// Gets a value indicating whether the method is generic.
		/// </summary>
		public override bool IsGenericMethod { get { return meth.GenericParameters.Count > 0; } }

		/// <summary>
		/// Gets a value indicating whether the method is a generic method definition.
		/// </summary>
		public override bool IsGenericMethodDefinition { get { return GetGenericMethodDefinition() != null; } }

		/// <summary>
		/// Gets a value indicating whether only a member of the same kind with exactly the same signature is hidden in the derived class. 
		/// </summary>
		public override bool IsHideBySig { get { return meth.IsHideBySig; } }

		/// <summary>
		/// Gets a value indicating whether this member is private.
		/// </summary>
		public override bool IsPrivate { get { return meth.IsPrivate; } }

		/// <summary>
		/// Gets a value indicating whether this is a public method.
		/// </summary>
		public override bool IsPublic { get { return meth.IsPublic; } }

		/// <summary>
		/// Gets a value indicating whether this method has a special name.
		/// </summary>
		public override bool IsSpecialName { get { return meth.IsSpecialName; } }

		/// <summary>
		/// Gets a value indicating whether the method is <c>static</c>.
		/// </summary>
		public override bool IsStatic { get { return meth.IsStatic; } }

		/// <summary>
		/// Gets a value indicating whether the method is <c>virtual</c>.
		/// </summary>
		public override bool IsVirtual { get { return meth.IsVirtual; } }

		/// <summary>
		/// Returns an array of <see cref="Type"/> objects that represent the type arguments of a generic method or the type parameters of a generic method definition. 
		/// </summary>
		/// <returns>
		/// An array of <see cref="Type"/> objects that represent the type arguments of a generic method or the type parameters of a generic method definition. 
		/// -or-
		/// An empty array if the current method is not a generic method. 
		/// </returns>
		public override Type[] GetGenericArguments() 
		{
			List<Type> list = new List<Type>();
			foreach (GenericParameter p in meth.GenericParameters)
				list.Add(Type.Import(meth.Parameters[p.Position-1].ParameterType));
			return list.ToArray();
		}

		/// <summary>
		/// returns the <see cref="MethodImplAttributes"/> flags. 
		/// </summary>
		/// <returns>The <see cref="MethodImplAttributes"/> flags. </returns>
		public override System.Reflection.MethodImplAttributes GetMethodImplementationFlags() { return MethodImplAttributesConverter.Convert(meth.ImplAttributes); }

		/// <summary>
		/// Gets the parameters of the specified method or constructor. 
		/// </summary>
		/// <returns>An array of type <see cref="ParameterInfo"/> containing information that matches the signature of the method (or constructor) reflected by this <see cref="MethodBase"/> instance. </returns>
		public override ParameterInfo[] GetParameters() { return ParameterInfo.Wrap(meth.Parameters); }
		#endregion

		#region MemberInfo
		/// <summary>
		/// Gets the class that declares this member. 
		/// </summary>
		public override Type DeclaringType { get { return meth.DeclaringType; } }

		/// <summary>
		/// Gets a <see cref="System.Reflection.MemberTypes"/> value indicating the type of the member — method, constructor, event, and so on. 
		/// </summary>
		public override System.Reflection.MemberTypes MemberType { get { return System.Reflection.MemberTypes.Method; } }

		/// <summary>
		/// Gets the name of the current member.
		/// </summary>
		public override string Name { get { return meth.Name; } }

		/// <summary>
		/// Gets the class object that was used to obtain this instance of <see cref="MemberInfo"/>.
		/// </summary>
		public override Type ReflectedType { get { return null; } }

		/// <summary>
		///  returns all attributes applied to this member. 
		/// </summary>
		/// <param name="inherit">Specifies whether to search this member's inheritance chain to find the attributes. </param>
		/// <returns>An array that contains all the custom attributes, or an array with zero elements if no attributes are defined.</returns>
		public override object[] GetCustomAttributes(bool inherit) { return null; }

		/// <summary>
		/// Returns an array of custom attributes identified by <see cref="Type"/>. 
		/// </summary>
		/// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned. </param>
		/// <param name="inherit">Specifies whether to search this member's inheritance chain to find the attributes. </param>
		/// <returns>An array that contains all the custom attributes, or an array with zero elements if no attributes are defined.</returns>
		public override object[] GetCustomAttributes(Type attributeType, bool inherit) { return null; }

		/// <summary>
		/// Indicates whether one or more instance of attributeType is applied to this member.
		/// </summary>
		/// <param name="attributeType">The <see cref="Type"/> object to which the custom attributes are applied. </param>
		/// <param name="inherit">Specifies whether to search this member's inheritance chain to find the attributes. </param>
		/// <returns><c>true</c> if one or more instance of <paramref name="attributeType"/> is applied to this member; otherwise <c>false</c>. </returns>
		public override bool IsDefined(Type attributeType, bool inherit) { return false; }
		#endregion

		#region MethodInfo
		/// <summary>
		/// Gets the return type of this method. 
		/// </summary>
		public virtual Type ReturnType { get { return Type.Import(meth.ReturnType.ReturnType); } }
		/// <summary>
		/// Returns a <see cref="MethodInfo"/> object that represents a generic method definition from which the current method can be constructed. 
		/// </summary>
		/// <returns>a <see cref="MethodInfo"/> object that represents a generic method definition from which the current method can be constructed.</returns>
		public virtual MethodInfo GetGenericMethodDefinition() { return IsGenericMethod ? this : null; }

		/// <summary>
		/// Substitutes the elements of an array of types for the type parameters of the current generic method definition, and returns a <see cref="MethodInfo"/> object representing the resulting constructed method. 
		/// </summary>
		/// <param name="typeArguments">An array of types to be substituted for the type parameters of the current generic method definition.</param>
		/// <returns>A <see cref="MethodInfo"/> object that represents the constructed method formed by substituting the elements of <paramref name="typeArguments"/> for the type parameters of the current generic method definition. </returns>
		public virtual MethodInfo MakeGenericMethod(params Type[] typeArguments) 
		{ 
			return new PESpecMethodinfo(new GenericInstanceMethod(meth));
		}
		#endregion

		#region Cast & Wrap
		/// <summary>
		/// Cast a <see cref="System.Reflection.MethodInfo"/> into a <see cref="MethodInfo"/>.
		/// </summary>
		/// <param name="o">A <see cref="System.Reflection.MethodInfo"/> to be casted.</param>
		/// <returns>A <see cref="MethodInfo"/>.</returns>
		public static implicit operator MethodInfo(System.Reflection.MethodInfo o) { return new CLRMethodInfo(o); }

		/// <summary>
		/// Cast a <see cref="Mono.Cecil.MethodDefinition"/> into a <see cref="MethodInfo"/>.
		/// </summary>
		/// <param name="o">A <see cref="Mono.Cecil.MethodDefinition"/> to be casted.</param>
		/// <returns>A <see cref="MethodInfo"/>.</returns>
		public static implicit operator MethodInfo(Mono.Cecil.MethodDefinition o) { return new PEMethodInfo(o); }

		internal static MethodInfo Wrap(System.Reflection.MethodInfo meth)
		{
			if (meth == null)
				return null;
			return new CLRMethodInfo(meth);
		}

		internal static MethodInfo[] Wrap(System.Reflection.MethodInfo[] meths)
		{
			if (meths == null)
				return null;
			CLRMethodInfo[] wmeths = new CLRMethodInfo[ meths.Length ];
			for (int i = 0; i < meths.Length; i++)
				wmeths[ i ] = new CLRMethodInfo(meths[ i ]);
			return wmeths;
		}

		internal static MethodInfo Wrap(Mono.Cecil.MethodDefinition meth)
		{
			if (meth == null)
				return null;
			return new PEMethodInfo(meth);
		}

		internal static MethodInfo[] Wrap(Mono.Cecil.MethodDefinition[] meths)
		{
			if (meths == null)
				return null;
			PEMethodInfo[] wmeths = new PEMethodInfo[ meths.Length ];
			for (int i = 0; i < meths.Length; i++)
				wmeths[ i ] = new PEMethodInfo(meths[ i ]);
			return wmeths;
		}
		#endregion

		#region Inner Classes
		internal class CLRMethodInfo: MethodInfo
		{
			internal new System.Reflection.MethodInfo meth;
			private bool imported = false;

			public CLRMethodInfo(System.Reflection.MethodInfo meth) 
			{ 
				this.meth = meth; 				
			}

			internal override Mono.Cecil.MethodReference peMeth
			{
				get { 
					if (!imported) base.peMeth = Type.Find(meth);
					return base.peMeth;
				}
				set {
					base.peMeth = value;
				}				
			}

			#region MethodBase
			public override System.Reflection.MethodAttributes Attributes { get { return (System.Reflection.MethodAttributes) meth.Attributes; } }
			public override System.Reflection.CallingConventions CallingConvention { get { return (System.Reflection.CallingConventions) meth.CallingConvention; } }
			public override bool ContainsGenericParameters { get { return meth.ContainsGenericParameters; } }
			public override bool IsAbstract { get { return meth.IsAbstract; } }
			public override bool IsAssembly { get { return meth.IsAssembly; } }
			public override bool IsConstructor { get { return meth.IsConstructor; } }
			public override bool IsFamily { get { return meth.IsFamily; } }
			public override bool IsFamilyAndAssembly { get { return meth.IsFamilyAndAssembly; } }
			public override bool IsFamilyOrAssembly { get { return meth.IsFamilyOrAssembly; } }
			public override bool IsFinal { get { return meth.IsFinal; } }
			public override bool IsGenericMethod { get { return meth.IsGenericMethod; } }
			public override bool IsHideBySig { get { return meth.IsHideBySig; } }
			public override bool IsPrivate { get { return meth.IsPrivate; } }
			public override bool IsPublic { get { return meth.IsPublic; } }
			public override bool IsSpecialName { get { return meth.IsSpecialName; } }
			public override bool IsStatic { get { return meth.IsStatic; } }
			public override bool IsVirtual { get { return meth.IsVirtual; } }
			public override Type[] GetGenericArguments() { return Type.Wrap(meth.GetGenericArguments()); }
			public override System.Reflection.MethodImplAttributes GetMethodImplementationFlags() { return (System.Reflection.MethodImplAttributes) 0; }
			public override ParameterInfo[] GetParameters() { return ParameterInfo.Wrap(meth.GetParameters()); }
			#endregion

			#region MemberInfo
			public override Type DeclaringType { get { return meth.DeclaringType; } }
			public override System.Reflection.MemberTypes MemberType { get { return System.Reflection.MemberTypes.Method; } }
			public override string Name { get { return meth.Name; } }
			public override Type ReflectedType { get { return meth.ReflectedType; } }

			public override object[] GetCustomAttributes(bool inherit) { return meth.GetCustomAttributes(inherit); }
			public override object[] GetCustomAttributes(Type attributeType, bool inherit) { return meth.GetCustomAttributes(((Type.CLRType) attributeType).clrType, inherit); }
			public override bool IsDefined(Type attributeType, bool inherit) { return meth.IsDefined(((Type.CLRType) attributeType).clrType, inherit); }
			#endregion

			#region MethodInfo
			public override bool IsGenericMethodDefinition { get { return meth.IsGenericMethodDefinition; } }
			public override Type ReturnType { get { return Type.Import(meth.ReturnType); } }
			public override MethodInfo GetGenericMethodDefinition() { return meth.GetGenericMethodDefinition(); }
			public override MethodInfo MakeGenericMethod(params Type[] typeArguments) { return null; }
			#endregion
		}

		internal class PEMethodInfo: MethodInfo
		{
			public PEMethodInfo(Mono.Cecil.MethodDefinition meth)
			{ 
				peMeth = meth;
			}
		}

		internal class PERefMethodInfo : MethodInfo
		{
			public PERefMethodInfo(Mono.Cecil.MethodReference meth)
			{
				peMeth = meth;
			}

			#region MethodBase
			public override System.Reflection.MethodAttributes Attributes { get { return (System.Reflection.MethodAttributes) 0; } }
			public override System.Reflection.CallingConventions CallingConvention { get { return System.Reflection.CallingConventions.Standard; } }
			public override bool ContainsGenericParameters { get { return false; } }
			public override bool IsAbstract { get { return false; } }
			public override bool IsAssembly { get { return false; } }
			public override bool IsConstructor { get { return true; } }
			public override bool IsFamily { get { return false; } }
			public override bool IsFamilyAndAssembly { get { return false; } }
			public override bool IsFamilyOrAssembly { get { return false; } }
			public override bool IsFinal { get { return false; } }
			public override bool IsGenericMethod { get { return false; } }
			public override bool IsHideBySig { get { return false; } }
			public override bool IsPrivate { get { return false; } }
			public override bool IsPublic { get { return true; } }
			public override bool IsSpecialName { get { return true; } }
			public override bool IsStatic { get { return !peMeth.HasThis; } }
			public override bool IsVirtual { get { return false; } }
			public override Type[] GetGenericArguments()
			{
				List<Type> list = new List<Type>();
				foreach (GenericParameter p in peMeth.GenericParameters)
					list.Add(Type.Import(peMeth.Parameters[p.Position].ParameterType));
				return list.ToArray();
			}
			public override System.Reflection.MethodImplAttributes GetMethodImplementationFlags() { return (System.Reflection.MethodImplAttributes) 0; }
			public override ParameterInfo[] GetParameters() { return new ParameterInfo[0]; }
			#endregion

			#region MemberInfo
			public override Type DeclaringType { get { return null; } }
			public override System.Reflection.MemberTypes MemberType { get { return System.Reflection.MemberTypes.Constructor; } }
			public override string Name { get { return peMeth.Name; } }
			public override Type ReflectedType { get { return null; } }

			public override object[] GetCustomAttributes(bool inherit) { return null; }
			public override object[] GetCustomAttributes(Type attributeType, bool inherit) { return null; }
			public override bool IsDefined(Type attributeType, bool inherit) { return false; }
			#endregion

			#region MethodInfo
			public override bool IsGenericMethodDefinition { get { return true; } }
			public override Type ReturnType { get { return peMeth.ReturnType.ReturnType; } }
			public override MethodInfo GetGenericMethodDefinition() { return this; }
			public override MethodInfo MakeGenericMethod(params Type[] typeArguments)
			{
				GenericInstanceMethod gi = new GenericInstanceMethod(peMeth);
				foreach (Type argType in typeArguments)
				{
					gi.GenericArguments.Add(argType.peType);
				}
				return new PESpecMethodinfo(gi);
			}
			#endregion
		}

		internal class PESpecMethodinfo : PERefMethodInfo
		{
			public PESpecMethodinfo(GenericInstanceMethod inst) : base(inst) {}

			public override Type[] GetGenericArguments()
			{
				List<Type> list = new List<Type>();
				foreach (TypeReference t in ((GenericInstanceMethod) peMeth).GenericArguments)
					list.Add(Type.Import(t));
				return list.ToArray();
			}
		}
		#endregion
	}
}
