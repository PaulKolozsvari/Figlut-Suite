using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	/// <summary>
	/// Discovers the attributes of a class constructor and provides access to constructor metadata. 
	/// </summary>
	public abstract class ConstructorInfo : MethodBase
	{
		internal Mono.Cecil.MethodDefinition ctor;
	
		/// <summary>
		/// Represents the name of the class constructor method as it is stored in metadata. This name is always ".ctor".
		/// </summary>
		public static readonly string ConstructorName = ".ctor"; 

		/// <summary>
		/// Represents the name of the type constructor method as it is stored in metadata. This name is always ".cctor". 
		/// </summary>
		public static readonly string TypeConstructorName = ".cctor";

		#region Equality
		/// <summary>
		/// Returns <c>true</c> if the objects are equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are equal, <c>false</c> otherwise.</returns>
		public static bool operator ==(ConstructorInfo t1, ConstructorInfo t2)
		{
			if ((object)t1 == null || (object)t2 == null)
				return (object)t1 == (object)t2;
			if (t1 is CLRConstructorInfo)
				return (t2 is CLRConstructorInfo) && ((CLRConstructorInfo)t1).ctor == ((CLRConstructorInfo)t2).ctor;
			return (t2 is PEConstructorInfo) && ((PEConstructorInfo)t1).ctor == ((PEConstructorInfo)t2).ctor;
		}

		/// <summary>
		/// Returns <c>true</c> if the objects are not equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are not equal, <c>false</c> otherwise.</returns>
		public static bool operator !=(ConstructorInfo t1, ConstructorInfo t2)
		{
			if ((object)t1 == null || (object)t2 == null)
				return (object)t1 != (object)t2;
			if (t1 is CLRConstructorInfo)
				return !(t2 is CLRConstructorInfo) || ((CLRConstructorInfo)t1).ctor != ((CLRConstructorInfo)t2).ctor;
			return !(t2 is PEConstructorInfo) || ((PEConstructorInfo)t1).ctor != ((PEConstructorInfo)t2).ctor;
		}

		/// <summary>
		/// Determines whether two <see cref="System.Object"/> instances are equal. 
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public override bool Equals(object o)
		{
			if (o is EventInfo)
				return this == (EventInfo)o;
			return false;
		}

		/// <summary>
		/// Serves as a hash function for a particular type. <see cref="GetHashCode"/> is suitable for use in hashing algorithms and data structures like a hash table. 
		/// </summary>
		/// <returns>A hash code for the current <see cref="ConstructorInfo"/>. </returns>
		public override int GetHashCode()
		{
			if (this is CLRConstructorInfo) return ((CLRConstructorInfo)this).ctor.GetHashCode();
			return ((PEConstructorInfo)this).ctor.GetHashCode();
		}
		#endregion

		#region Cast & Wrap
		/// <summary>
		/// Cast a <see cref="System.Reflection.ConstructorInfo"/> into a <see cref="ConstructorInfo"/>.
		/// </summary>
		/// <param name="o">A <see cref="System.Reflection.ConstructorInfo"/> to be casted.</param>
		/// <returns>A <see cref="ConstructorInfo"/>.</returns>
		public static implicit operator ConstructorInfo(System.Reflection.ConstructorInfo o) { return new CLRConstructorInfo(o); }

		/// <summary>
		/// Cast a <see cref="Mono.Cecil.MethodDefinition"/> into a <see cref="ConstructorInfo"/>.
		/// </summary>
		/// <param name="o">A <see cref="Mono.Cecil.MethodDefinition"/> to be casted.</param>
		/// <returns>A <see cref="ConstructorInfo"/>.</returns>
		public static implicit operator ConstructorInfo(Mono.Cecil.MethodDefinition o) { return new PEConstructorInfo(o); }

		internal static ConstructorInfo Wrap(System.Reflection.ConstructorInfo ctor)
		{
			if (ctor == null)
				return null;
			return new CLRConstructorInfo(ctor);
		}

		internal static ConstructorInfo[] Wrap(System.Reflection.ConstructorInfo[] ctors)
		{
			if (ctors == null)
				return null;
			CLRConstructorInfo[] wctors = new CLRConstructorInfo[ ctors.Length ];
			for (int i = 0; i < ctors.Length; i++)
				wctors[ i ] = new CLRConstructorInfo(ctors[ i ]);
			return wctors;
		}

		internal static ConstructorInfo Wrap(Mono.Cecil.MethodDefinition ctor)
		{
			if (ctor == null)
				return null;
			return new PEConstructorInfo(ctor);
		}

		internal static ConstructorInfo[] Wrap(Mono.Cecil.MethodDefinition[] ctors)
		{
			if (ctors == null)
				return null;
			PEConstructorInfo[] wctors = new PEConstructorInfo[ ctors.Length ];
			for (int i = 0; i < ctors.Length; i++)
				wctors[ i ] = new PEConstructorInfo(ctors[ i ]);
			return wctors;
		}
		#endregion

		#region MemberInfo
		/// <summary>
		/// Gets the class that declares this member. 
		/// </summary>
		public override Type DeclaringType { get { return ctor.DeclaringType; } }

		/// <summary>
		/// Gets a <see cref="System.Reflection.MemberTypes"/> value indicating the type of the member — method, constructor, event, and so on. 
		/// </summary>
		public override System.Reflection.MemberTypes MemberType { get { return System.Reflection.MemberTypes.Constructor; } }

		/// <summary>
		/// Gets the name of the current member.
		/// </summary>
		public override string Name { get { return ctor.Name; } }

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

		#region MethodBase
		/// <summary>
		/// Gets the attributes associated with this method.
		/// </summary>
		public override System.Reflection.MethodAttributes Attributes { get { return MethodAttributesConverter.Convert(ctor.Attributes); } }

		/// <summary>
		/// Gets a value indicating the calling conventions for this method.
		/// </summary>
		public override System.Reflection.CallingConventions CallingConvention { get { return CallingConventionsConverter.Extract(ctor); } }

		/// <summary>
		/// Gets a value indicating whether the generic method contains unassigned generic type parameters.
		/// </summary>
		public override bool ContainsGenericParameters { get { return ctor.GenericParameters.Count > 0; } }

		/// <summary>
		/// Gets a value indicating whether the method is abstract.
		/// </summary>
		public override bool IsAbstract { get { return ctor.IsAbstract; } }

		/// <summary>
		/// Gets a value indicating whether this method can be called by other classes in the same assembly.
		/// </summary>
		public override bool IsAssembly { get { return ctor.IsAssembly; } }

		/// <summary>
		/// Gets a value indicating whether this method can be called by other classes in the same assembly.
		/// </summary>
		public override bool IsConstructor { get { return true; } }

		/// <summary>
		/// Gets a value indicating whether access to this method is restricted to members of the class and members of its derived classes.
		/// </summary>
		public override bool IsFamily { get { return ctor.IsFamily; } }

		/// <summary>
		/// Gets a value indicating whether this method can be called by derived classes if they are in the same assembly.
		/// </summary>
		public override bool IsFamilyAndAssembly { get { return ctor.IsFamilyAndAssembly; } }

		/// <summary>
		/// Gets a value indicating whether this method can be called by derived classes, wherever they are, and by all classes in the same assembly.
		/// </summary>
		public override bool IsFamilyOrAssembly { get { return ctor.IsFamilyOrAssembly; } }

		/// <summary>
		/// Gets a value indicating whether this method is <c>final</c>.
		/// </summary>
		public override bool IsFinal { get { return ctor.IsFinal; } }

		/// <summary>
		/// Gets a value indicating whether the method is generic.
		/// </summary>
		public override bool IsGenericMethod { get { return ctor.GenericParameters.Count > 0; } }

		/// <summary>
		/// Gets a value indicating whether the method is a generic method definition.
		/// </summary>
		public override bool IsGenericMethodDefinition { get { return ctor.GenericParameters.Count > 0; } }

		/// <summary>
		/// Gets a value indicating whether only a member of the same kind with exactly the same signature is hidden in the derived class. 
		/// </summary>
		public override bool IsHideBySig { get { return ctor.IsHideBySig; } }

		/// <summary>
		/// Gets a value indicating whether this member is private.
		/// </summary>
		public override bool IsPrivate { get { return ctor.IsPrivate; } }

		/// <summary>
		/// Gets a value indicating whether this is a public method.
		/// </summary>
		public override bool IsPublic { get { return ctor.IsPublic; } }

		/// <summary>
		/// Gets a value indicating whether this method has a special name.
		/// </summary>
		public override bool IsSpecialName { get { return ctor.IsSpecialName; } }

		/// <summary>
		/// Gets a value indicating whether the method is <c>static</c>.
		/// </summary>
		public override bool IsStatic { get { return ctor.IsStatic; } }

		/// <summary>
		/// Gets a value indicating whether the method is <c>virtual</c>.
		/// </summary>
		public override bool IsVirtual { get { return ctor.IsVirtual; } }

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
			foreach (GenericParameter p in ctor.GenericParameters)
				list.Add(Type.Import(ctor.Parameters[p.Position-1].ParameterType));
			return list.ToArray();
		}

		/// <summary>
		/// returns the <see cref="MethodImplAttributes"/> flags. 
		/// </summary>
		/// <returns>The <see cref="MethodImplAttributes"/> flags. </returns>
		public override System.Reflection.MethodImplAttributes GetMethodImplementationFlags() { return MethodImplAttributesConverter.Convert(ctor.ImplAttributes); }

		/// <summary>
		/// Gets the parameters of the specified method or constructor. 
		/// </summary>
		/// <returns>An array of type <see cref="ParameterInfo"/> containing information that matches the signature of the method (or constructor) reflected by this <see cref="MethodBase"/> instance. </returns>
		public override ParameterInfo[] GetParameters() { return ParameterInfo.Wrap(ctor.Parameters); }
		#endregion

		#region Inner Classes
		internal class CLRConstructorInfo: ConstructorInfo
		{
			internal new System.Reflection.ConstructorInfo ctor;
			public CLRConstructorInfo(System.Reflection.ConstructorInfo ctor) 
			{ 
				this.ctor = ctor; 
				peMeth = Type.Find(ctor);
			}

			#region MethodBase
			public override System.Reflection.MethodAttributes Attributes { get { return (System.Reflection.MethodAttributes) ctor.Attributes; } }
			public override System.Reflection.CallingConventions CallingConvention { get { return (System.Reflection.CallingConventions) ctor.CallingConvention; } }
			public override bool ContainsGenericParameters { get { return ctor.ContainsGenericParameters; } }
			public override bool IsAbstract { get { return ctor.IsAbstract; } }
			public override bool IsAssembly { get { return ctor.IsAssembly; } }
			public override bool IsConstructor { get { return ctor.IsConstructor; } }
			public override bool IsFamily { get { return ctor.IsFamily; } }
			public override bool IsFamilyAndAssembly { get { return ctor.IsFamilyAndAssembly; } }
			public override bool IsFamilyOrAssembly { get { return ctor.IsFamilyOrAssembly; } }
			public override bool IsFinal { get { return ctor.IsFinal; } }
			public override bool IsGenericMethod { get { return ctor.IsGenericMethod; } }
			public override bool IsHideBySig { get { return ctor.IsHideBySig; } }
			public override bool IsPrivate { get { return ctor.IsPrivate; } }
			public override bool IsPublic { get { return ctor.IsPublic; } }
			public override bool IsSpecialName { get { return ctor.IsSpecialName; } }
			public override bool IsStatic { get { return ctor.IsStatic; } }
			public override bool IsVirtual { get { return ctor.IsVirtual; } }
			public override Type[] GetGenericArguments() { return Type.Wrap(ctor.GetGenericArguments()); }
			public override System.Reflection.MethodImplAttributes GetMethodImplementationFlags() { return (System.Reflection.MethodImplAttributes) 0; }
			public override ParameterInfo[] GetParameters() { return ParameterInfo.Wrap(ctor.GetParameters()); }
			public override bool IsGenericMethodDefinition { get { return ctor.IsGenericMethodDefinition; } }
			#endregion

			#region MemberInfo
			public override Type DeclaringType { get { return ctor.DeclaringType; } }
			public override System.Reflection.MemberTypes MemberType { get { return System.Reflection.MemberTypes.Constructor; } }
			public override string Name { get { return ctor.Name; } }
			public override Type ReflectedType { get { return ctor.ReflectedType; } }

			public override object[] GetCustomAttributes(bool inherit) { return ctor.GetCustomAttributes(inherit); }
			public override object[] GetCustomAttributes(Type attributeType, bool inherit) { return ctor.GetCustomAttributes(((Type.CLRType) attributeType).clrType, inherit); }
			public override bool IsDefined(Type attributeType, bool inherit) { return ctor.IsDefined(((Type.CLRType) attributeType).clrType, inherit); }
			#endregion
		}

		internal class PEConstructorInfo: ConstructorInfo
		{
			internal PEConstructorInfo(Mono.Cecil.MethodDefinition ctor) 
			{ 
				this.ctor = ctor;
				peMeth = ctor;
			}
		}

		internal class PERefConstructorInfo: ConstructorInfo
		{
			public PERefConstructorInfo(Mono.Cecil.MethodReference ctor) 
			{ 
				peMeth = ctor;
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
			public override Type[] GetGenericArguments() { return Type.EmptyTypes; }
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
			public override bool IsGenericMethodDefinition { get { return peMeth.GenericParameters.Count > 0; } }
			#endregion
		}
		#endregion
	}
}
