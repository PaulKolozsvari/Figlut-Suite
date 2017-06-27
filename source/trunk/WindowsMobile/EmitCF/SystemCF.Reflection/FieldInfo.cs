using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	/// <summary>
	/// Discovers the attributes of a field and provides access to field metadata. 
	/// </summary>
	public abstract class FieldInfo : MemberInfo
	{
		internal Mono.Cecil.FieldDefinition field;

		#region Equality
		/// <summary>
		/// Returns <c>true</c> if the objects are equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are equal, <c>false</c> otherwise.</returns>
		public static bool operator ==(FieldInfo t1, FieldInfo t2)
		{
			if ((object) t1 == null || (object) t2 == null)
				return (object) t1 == (object) t2;
			if (t1 is CLRFieldInfo)
				return (t2 is CLRFieldInfo) && ((CLRFieldInfo) t1).field == ((CLRFieldInfo) t2).field;
			return (t2 is PEFieldInfo) && ((PEFieldInfo) t1).field == ((PEFieldInfo) t2).field;
		}

		/// <summary>
		/// Returns <c>true</c> if the objects are not equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are not equal, <c>false</c> otherwise.</returns>
		public static bool operator !=(FieldInfo t1, FieldInfo t2)
		{
			if ((object) t1 == null || (object) t2 == null)
				return (object) t1 != (object) t2;
			if (t1 is CLRFieldInfo)
				return !(t2 is CLRFieldInfo) || ((CLRFieldInfo) t1).field != ((CLRFieldInfo) t2).field;
			return !(t2 is PEFieldInfo) || ((PEFieldInfo) t1).field != ((PEFieldInfo) t2).field;
		}

		/// <summary>
		/// Determines whether two <see cref="System.Object"/> instances are equal. 
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public override bool Equals(object o)
		{
			if (o is FieldInfo)
				return this == (FieldInfo) o;
			return false;
		}

		/// <summary>
		/// Serves as a hash function for a particular type. <see cref="GetHashCode"/> is suitable for use in hashing algorithms and data structures like a hash table. 
		/// </summary>
		/// <returns>A hash code for the current <see cref="FieldInfo"/>. </returns>
		public override int GetHashCode()
		{
			if (this is CLRFieldInfo) return ((CLRFieldInfo)this).field.GetHashCode();
			return ((PEFieldInfo)this).field.GetHashCode();
		}
		#endregion
	
		#region MemberInfo
		/// <summary>
		/// Gets the class that declares this member. 
		/// </summary>
		public override Type DeclaringType { get { return field.DeclaringType; } }

		/// <summary>
		/// Gets a <see cref="System.Reflection.MemberTypes"/> value indicating the type of the member — method, constructor, event, and so on. 
		/// </summary>
		public override System.Reflection.MemberTypes MemberType { get { return System.Reflection.MemberTypes.Field; } }

		/// <summary>
		/// Gets the name of the current member.
		/// </summary>
		public override string Name { get { return field.Name; } }

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

		#region FieldInfo
		/// <summary>
		/// Gets the attributes associated with this field. 
		/// </summary>
		public virtual System.Reflection.FieldAttributes Attributes { get { return FieldAttributesConverter.Convert(field.Attributes); } }

		/// <summary>
		/// Returns the value of a field supported by a given object. 
		/// </summary>
		/// <param name="obj">The object whose field value will be returned. </param>
		/// <returns>An object containing the value of the field reflected by this instance. </returns>
		public virtual object GetValue(object obj)
		{
			return null;
		}

		/// <summary>
		/// Gets the type of this field object. 
		/// </summary>
		public virtual Type FieldType { get { return Type.Import(field.FieldType); } }

		/// <summary>
		/// Gets a value indicating whether this field has Assembly level visibility. 
		/// </summary>
		public virtual bool IsAssembly { get { return field.IsAssembly; } }

		/// <summary>
		/// Gets a value indicating whether this field has Family level visibility. 
		/// </summary>
		public virtual bool IsFamily { get { return field.IsFamily; } }

		/// <summary>
		/// Gets a value indicating whether this field has <c>FamilyAndAssembly</c> level visibility. 
		/// </summary>
		public virtual bool IsFamilyAndAssembly { get { return field.IsFamilyAndAssembly; } }

		/// <summary>
		/// Gets a value indicating whether this field has <c>FamilyOrAssembly</c> level visibility. 
		/// </summary>
		public virtual bool IsFamilyOrAssembly { get { return field.IsFamilyOrAssembly; } }

		/// <summary>
		/// Gets a value indicating whether the field can only be set in the body of the constructor.
		/// </summary>
		public virtual bool IsInitOnly { get { return field.IsInitOnly; } }

		/// <summary>
		/// Gets a value indicating whether the value is written at compile time and cannot be changed. 
		/// </summary>
		public virtual bool IsLiteral { get { return field.IsLiteral; } }

		/// <summary>
		/// Gets a value indicating whether this field has the <c>NotSerialized</c> attribute. 
		/// </summary>
		public virtual bool IsNotSerialized { get { return field.IsNotSerialized; } }

		/// <summary>
		/// Gets a value indicating whether the corresponding <c>PinvokeImpl</c> attribute is set in <see cref="FieldAttributes"/>. 
		/// </summary>
 		public virtual bool IsPinvokeImpl { get { return field.IsPInvokeImpl; } }

		/// <summary>
		/// Gets a value indicating whether the field is private. 
		/// </summary>
   		public virtual bool IsPrivate { get { return field.IsPrivate; } }

		/// <summary>
		/// Gets a value indicating whether the field is public. 
		/// </summary>
		public virtual bool IsPublic { get { return field.IsPublic; } }

		/// <summary>
		/// Gets a value indicating whether the corresponding <c>SpecialName</c> attribute is set in the <see cref="FieldAttributes"/> enumerator. 
		/// </summary>
		public virtual bool IsSpecialName { get { return field.IsSpecialName; } }

		/// <summary>
		/// Gets a value indicating whether the field is static. 
		/// </summary>
		public virtual bool IsStatic { get { return field.IsStatic; } }
		#endregion

		#region Cast & Wrap
		/// <summary>
		/// Cast a <see cref="System.Reflection.FieldInfo"/> into a <see cref="FieldInfo"/>.
		/// </summary>
		/// <param name="o">A <see cref="System.Reflection.FieldInfo"/> to be casted.</param>
		/// <returns>A <see cref="FieldInfo"/>.</returns>
		public static implicit operator FieldInfo(System.Reflection.FieldInfo o) { return new CLRFieldInfo(o); }

		/// <summary>
		/// Cast a <see cref="Mono.Cecil.FieldDefinition"/> into a <see cref="FieldInfo"/>.
		/// </summary>
		/// <param name="o">A <see cref="Mono.Cecil.FieldDefinition"/> to be casted.</param>
		/// <returns>A <see cref="FieldInfo"/>.</returns>
		public static implicit operator FieldInfo(Mono.Cecil.FieldDefinition o) { return new PEFieldInfo(o); }

		internal static FieldInfo Wrap(System.Reflection.FieldInfo field)
		{
			if (field == null)
				return null;
			return new CLRFieldInfo(field);
		}

		internal static FieldInfo[] Wrap(System.Reflection.FieldInfo[] fields)
		{
			if (fields == null)
				return null;
			CLRFieldInfo[] wfields = new CLRFieldInfo[ fields.Length ];
			for (int i = 0; i < fields.Length; i++)
				wfields[ i ] = new CLRFieldInfo(fields[ i ]);
			return wfields;
		}

		internal static FieldInfo Wrap(Mono.Cecil.FieldDefinition field)
		{
			if (field == null)
				return null;
			return new PEFieldInfo(field);
		}

		internal static FieldInfo[] Wrap(Mono.Cecil.FieldDefinition[] fields)
		{
			if (fields == null)
				return null;
			PEFieldInfo[] wfields = new PEFieldInfo[ fields.Length ];
			for (int i = 0; i < fields.Length; i++)
				wfields[ i ] = new PEFieldInfo(fields[ i ]);
			return wfields;
		}
		#endregion

		#region Inner Classes
		internal class CLRFieldInfo: FieldInfo
		{
			internal new System.Reflection.FieldInfo field;
			public CLRFieldInfo(System.Reflection.FieldInfo field) { this.field = field; }

			#region MemberInfo
			public override Type DeclaringType { get { return field.DeclaringType; } }
			public override System.Reflection.MemberTypes MemberType { get { return System.Reflection.MemberTypes.Field; } }
			public override string Name { get { return field.Name; } }
			public override Type ReflectedType { get { return field.ReflectedType; } }

			public override object[] GetCustomAttributes(bool inherit) { return field.GetCustomAttributes(inherit); }
			public override object[] GetCustomAttributes(Type attributeType, bool inherit) { return field.GetCustomAttributes(((Type.CLRType) attributeType).clrType, inherit); }
			public override bool IsDefined(Type attributeType, bool inherit) { return field.IsDefined(((Type.CLRType) attributeType).clrType, inherit); }
			#endregion

			#region FieldInfo
			public override object GetValue(object obj)
			{
				return field.GetValue(obj);
			}

			public override System.Reflection.FieldAttributes Attributes { get { return (System.Reflection.FieldAttributes) field.Attributes; } }
			public override Type FieldType { get { return field.FieldType; } }
    		public override bool IsAssembly { get { return field.IsAssembly; } }
    		public override bool IsFamily { get { return field.IsFamily; } }
    		public override bool IsFamilyAndAssembly { get { return field.IsFamilyAndAssembly; } }
    		public override bool IsFamilyOrAssembly { get { return field.IsFamilyOrAssembly; } }
    		public override bool IsInitOnly { get { return field.IsInitOnly; } }
    		public override bool IsLiteral { get { return field.IsLiteral; } }
    		public override bool IsPinvokeImpl { get { return field.IsPinvokeImpl; } }
    		public override bool IsNotSerialized { get { return field.IsNotSerialized; } }
    		public override bool IsPrivate { get { return field.IsPrivate; } }
    		public override bool IsPublic { get { return field.IsPublic; } }
    		public override bool IsSpecialName { get { return field.IsSpecialName; } }
    		public override bool IsStatic { get { return field.IsStatic; } }
			#endregion
		}

		internal class PEFieldInfo: FieldInfo
		{
			public PEFieldInfo(Mono.Cecil.FieldDefinition field) { this.field = field; }
		}

		internal class PERefFieldInfo : FieldInfo
		{
			internal new Mono.Cecil.FieldReference field;
			public PERefFieldInfo(Mono.Cecil.FieldReference field){ this.field = field; }
	
			#region MemberInfo
			public override Type DeclaringType { get { return field.DeclaringType; } }
			public override System.Reflection.MemberTypes MemberType { get { return System.Reflection.MemberTypes.Field; } }
			public override string Name { get { return field.Name; } }
			public override Type ReflectedType { get { return null; } }

			public override object[] GetCustomAttributes(bool inherit) { return null; }
			public override object[] GetCustomAttributes(Type attributeType, bool inherit) { return null; }
			public override bool IsDefined(Type attributeType, bool inherit) { return false; }
			#endregion

			#region FieldInfo
			public override System.Reflection.FieldAttributes Attributes { get { return (System.Reflection.FieldAttributes) 0; } }
			public override Type FieldType { get { return field.FieldType; } }
			public override bool IsAssembly { get { return false; } }
    		public override bool IsFamily { get { return false; } }
    		public override bool IsFamilyAndAssembly { get { return false; } }
    		public override bool IsFamilyOrAssembly { get { return false; } }
    		public override bool IsInitOnly { get { return false; } }
    		public override bool IsLiteral { get { return false; } }
    		public override bool IsNotSerialized { get { return false; } }
      		public override bool IsPinvokeImpl { get { return false; } }
	   		public override bool IsPrivate { get { return false; } }
    		public override bool IsPublic { get { return false; } }
    		public override bool IsSpecialName { get { return false; } }
    		public override bool IsStatic { get { return false; } }
			#endregion
		}
		#endregion	
	}
}
