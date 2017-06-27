using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	/// <summary>
	/// Provides information about methods and constructors. 
	/// </summary>
	public abstract class MethodBase : MemberInfo
	{
		private Mono.Cecil.MethodReference _peMeth;
		internal virtual Mono.Cecil.MethodReference peMeth
		{
			get { 
				if (_peMeth == null) throw new System.NullReferenceException("peMeth");
				return _peMeth;
			}
			set {
				_peMeth = value;
			}				
		}

		#region Abstract
		/// <summary>
		/// Gets the attributes associated with this method.
		/// </summary>
		public abstract System.Reflection.MethodAttributes Attributes { get; }

		/// <summary>
		/// Gets a value indicating the calling conventions for this method.
		/// </summary>
		public abstract System.Reflection.CallingConventions CallingConvention { get; }

		/// <summary>
		/// Gets a value indicating whether the generic method contains unassigned generic type parameters.
		/// </summary>
		public abstract bool ContainsGenericParameters { get; }

		/// <summary>
		/// Gets a value indicating whether the method is abstract.
		/// </summary>
		public abstract bool IsAbstract { get; }

		/// <summary>
		/// Gets a value indicating whether this method can be called by other classes in the same assembly.
		/// </summary>
		public abstract bool IsAssembly { get; }

		/// <summary>
		/// Gets a value indicating whether this method can be called by other classes in the same assembly.
		/// </summary>
		public abstract bool IsConstructor { get; }

		/// <summary>
		/// Gets a value indicating whether access to this method is restricted to members of the class and members of its derived classes.
		/// </summary>
		public abstract bool IsFamily { get; }

		/// <summary>
		/// Gets a value indicating whether this method can be called by derived classes if they are in the same assembly.
		/// </summary>
		public abstract bool IsFamilyAndAssembly { get; }

		/// <summary>
		/// Gets a value indicating whether this method can be called by derived classes, wherever they are, and by all classes in the same assembly.
		/// </summary>
		public abstract bool IsFamilyOrAssembly { get; }

		/// <summary>
		/// Gets a value indicating whether this method is <c>final</c>.
		/// </summary>
		public abstract bool IsFinal { get; }

		/// <summary>
		/// Gets a value indicating whether the method is generic.
		/// </summary>
		public abstract bool IsGenericMethod { get; }

		/// <summary>
		/// Gets a value indicating whether the method is a generic method definition.
		/// </summary>
		public abstract bool IsGenericMethodDefinition { get; }

		/// <summary>
		/// Gets a value indicating whether only a member of the same kind with exactly the same signature is hidden in the derived class. 
		/// </summary>
		public abstract bool IsHideBySig { get; }

		/// <summary>
		/// Gets a value indicating whether this member is private.
		/// </summary>
		public abstract bool IsPrivate { get; }

		/// <summary>
		/// Gets a value indicating whether this is a public method.
		/// </summary>
		public abstract bool IsPublic { get; }

		/// <summary>
		/// Gets a value indicating whether this method has a special name.
		/// </summary>
		public abstract bool IsSpecialName { get; }

		/// <summary>
		/// Gets a value indicating whether the method is <c>static</c>.
		/// </summary>
		public abstract bool IsStatic { get; }

		/// <summary>
		/// Gets a value indicating whether the method is <c>virtual</c>.
		/// </summary>
		public abstract bool IsVirtual { get; }

		/// <summary>
		/// Returns an array of <see cref="Type"/> objects that represent the type arguments of a generic method or the type parameters of a generic method definition. 
		/// </summary>
		/// <returns>
		/// An array of <see cref="Type"/> objects that represent the type arguments of a generic method or the type parameters of a generic method definition. 
		/// -or-
		/// An empty array if the current method is not a generic method. 
		/// </returns>
		public abstract Type[] GetGenericArguments();

		/// <summary>
		/// returns the <see cref="MethodImplAttributes"/> flags. 
		/// </summary>
		/// <returns>The <see cref="MethodImplAttributes"/> flags. </returns>
		public abstract System.Reflection.MethodImplAttributes GetMethodImplementationFlags();

		/// <summary>
		/// Gets the parameters of the specified method or constructor. 
		/// </summary>
		/// <returns>An array of type <see cref="ParameterInfo"/> containing information that matches the signature of the method (or constructor) reflected by this <see cref="MethodBase"/> instance. </returns>
		public abstract ParameterInfo[] GetParameters();
		#endregion
	}
}
