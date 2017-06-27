using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	/// <summary>
	/// Obtains information about the attributes of a member and provides access to member metadata. 
	/// </summary>
	public abstract class MemberInfo
	{
		#region Equality
		/// <summary>
		/// Returns <c>true</c> if the objects are equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are equal, <c>false</c> otherwise.</returns>
		public static bool operator ==(MemberInfo t1, MemberInfo t2)
		{
			if ((object) t1 == null || (object) t2 == null)
				return (object) t1 == (object) t2;
			if (t1 is ConstructorInfo && t2 is ConstructorInfo)
				return (ConstructorInfo) t1 == (ConstructorInfo) t2;
			if (t1 is MethodInfo && t2 is MethodInfo)
				return (MethodInfo) t1 == (MethodInfo) t2;
			if (t1 is FieldInfo && t2 is FieldInfo)
				return (FieldInfo) t1 == (FieldInfo) t2;
			if (t1 is PropertyInfo && t2 is PropertyInfo)
				return (PropertyInfo) t1 == (PropertyInfo) t2;
			if (t1 is EventInfo && t2 is EventInfo)
				return (EventInfo) t1 == (EventInfo) t2;
			return false;
		}

		/// <summary>
		/// Returns <c>true</c> if the objects are not equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are not equal, <c>false</c> otherwise.</returns>
		public static bool operator !=(MemberInfo t1, MemberInfo t2)
		{
			return !(t1 == t2);
		}

		/// <summary>
		/// Determines whether two <see cref="System.Object"/> instances are equal. 
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public override bool Equals(object o)
		{
			if (o is MemberInfo)
				return this == (MemberInfo) o;
			return false;
		}

		/// <summary>
		/// Serves as a hash function for a particular type. <see cref="GetHashCode"/> is suitable for use in hashing algorithms and data structures like a hash table. 
		/// </summary>
		/// <returns>A hash code for the current <see cref="MemberInfo"/>. </returns>
		public override int GetHashCode()
		{
			if (this is ConstructorInfo)
				return ((ConstructorInfo)this).GetHashCode();
			if (this is MethodInfo)
				return ((MethodInfo)this).GetHashCode();
			if (this is FieldInfo)
				return ((FieldInfo)this).GetHashCode();
			if (this is PropertyInfo)
				return ((PropertyInfo)this).GetHashCode();
			if (this is EventInfo)
				return ((EventInfo)this).GetHashCode();
			return 0;
		}
		#endregion

		#region Abstract
		/// <summary>
		/// Gets the class that declares this member. 
		/// </summary>
		public abstract Type DeclaringType { get; }

		/// <summary>
		/// Gets a <see cref="System.Reflection.MemberTypes"/> value indicating the type of the member — method, constructor, event, and so on. 
		/// </summary>
		public abstract System.Reflection.MemberTypes MemberType { get; }

		/// <summary>
		/// Gets the name of the current member.
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// Gets the class object that was used to obtain this instance of <see cref="MemberInfo"/>.
		/// </summary>
		public abstract Type ReflectedType { get; }

		/// <summary>
		///  returns all attributes applied to this member. 
		/// </summary>
		/// <param name="inherit">Specifies whether to search this member's inheritance chain to find the attributes. </param>
		/// <returns>An array that contains all the custom attributes, or an array with zero elements if no attributes are defined.</returns>
		public abstract object[] GetCustomAttributes(bool inherit);

		/// <summary>
		/// Returns an array of custom attributes identified by <see cref="Type"/>. 
		/// </summary>
		/// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned. </param>
		/// <param name="inherit">Specifies whether to search this member's inheritance chain to find the attributes. </param>
		/// <returns>An array that contains all the custom attributes, or an array with zero elements if no attributes are defined.</returns>
		public abstract object[] GetCustomAttributes(Type attributeType, bool inherit);

		/// <summary>
		/// Indicates whether one or more instance of attributeType is applied to this member.
		/// </summary>
		/// <param name="attributeType">The <see cref="Type"/> object to which the custom attributes are applied. </param>
		/// <param name="inherit">Specifies whether to search this member's inheritance chain to find the attributes. </param>
		/// <returns><c>true</c> if one or more instance of <paramref name="attributeType"/> is applied to this member; otherwise <c>false</c>. </returns>
		public abstract bool IsDefined(Type attributeType, bool inherit);
		#endregion
	}
}
