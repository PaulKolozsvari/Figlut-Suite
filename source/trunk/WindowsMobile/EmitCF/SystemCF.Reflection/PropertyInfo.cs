using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	/// <summary>
	/// Discovers the attributes of a property and provides access to property metadata. 
	/// </summary>
	public abstract class PropertyInfo : MemberInfo
	{
		internal Mono.Cecil.PropertyDefinition prop;

		#region Equality
		/// <summary>
		/// Returns <c>true</c> if the objects are equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are equal, <c>false</c> otherwise.</returns>
		public static bool operator ==(PropertyInfo t1, PropertyInfo t2)
		{
			if ((object) t1 == null || (object) t2 == null)
				return (object) t1 == (object) t2;
			if (t1 is CLRPropertyInfo)
				return (t2 is CLRPropertyInfo) && ((CLRPropertyInfo) t1).prop == ((CLRPropertyInfo) t2).prop;
			return (t2 is PEPropertyInfo) && ((PEPropertyInfo) t1).prop == ((PEPropertyInfo) t2).prop;
		}

		/// <summary>
		/// Returns <c>true</c> if the objects are not equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are not equal, <c>false</c> otherwise.</returns>
		public static bool operator !=(PropertyInfo t1, PropertyInfo t2)
		{
			if ((object) t1 == null || (object) t2 == null)
				return (object) t1 != (object) t2;
			if (t1 is CLRPropertyInfo)
				return !(t2 is CLRPropertyInfo) || ((CLRPropertyInfo) t1).prop != ((CLRPropertyInfo) t2).prop;
			return !(t2 is PEPropertyInfo) || ((PEPropertyInfo) t1).prop != ((PEPropertyInfo) t2).prop;
		}

		/// <summary>
		/// Determines whether two <see cref="System.Object"/> instances are equal. 
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public override bool Equals(object o)
		{
			if (o is PropertyInfo)
				return this == (PropertyInfo) o;
			return false;
		}

		/// <summary>
		/// Serves as a hash function for a particular type. <see cref="GetHashCode"/> is suitable for use in hashing algorithms and data structures like a hash table. 
		/// </summary>
		/// <returns>A hash code for the current <see cref="PropertyInfo"/>. </returns>
		public override int GetHashCode()
		{
			if (this is CLRPropertyInfo) return ((CLRPropertyInfo)this).prop.GetHashCode();
			return ((PEPropertyInfo)this).prop.GetHashCode();
		}
		#endregion

		#region MemberInfo
		/// <summary>
		/// Gets the class that declares this member. 
		/// </summary>
		public override Type DeclaringType { get { return prop.DeclaringType; } }

		/// <summary>
		/// Gets a <see cref="System.Reflection.MemberTypes"/> value indicating the type of the member — method, constructor, event, and so on. 
		/// </summary>
		public override System.Reflection.MemberTypes MemberType { get { return System.Reflection.MemberTypes.Property; } }
	
		/// <summary>
		/// Gets the name of the current member.
		/// </summary>
		public override string Name { get { return prop.Name; } }

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

		#region PropertyInfo
		/// <summary>
		/// Gets the attributes for this property.
		/// </summary>
		public virtual System.Reflection.PropertyAttributes Attributes { get { return PropertyAttributesConverter.Convert(prop.Attributes); } }

		/// <summary>
		/// Gets a value indicating whether the property can be read.
		/// </summary>
		public virtual bool CanRead { get { return prop.GetMethod != null; } }

		/// <summary>
		/// Gets a value indicating whether the property can be written to.
		/// </summary>
		public virtual bool CanWrite { get  { return prop.SetMethod != null; } }

		/// <summary>
		/// Gets the type of this property.
		/// </summary>
		public virtual Type PropertyType  { get { return prop.PropertyType; } }

		/// <summary>
		/// Returns an array whose elements reflect the public <c>get</c>, <c>set</c>, and other accessors of the property reflected by the current instance. 
		/// </summary>
		/// <returns>An array of <see cref="MethodInfo"/> objects that reflect the public <c>get</c>, <c>set</c>, and other accessors of the property reflected by the current instance, if found; otherwise, this method returns an array with zero (0) elements. </returns>
		public virtual MethodInfo[] GetAccessors()
		{ 
			List<MethodInfo> accessors = new List<MethodInfo>();
			if (CanRead) accessors.Add(GetGetMethod());
			if (CanWrite) accessors.Add(GetSetMethod());
			return accessors.ToArray();
		}

		/// <summary>
		/// Returns an array whose elements reflect the public and, if specified, non-public <c>get</c>, <c>set</c>, and other accessors of the property reflected by the current instance. 
		/// </summary>
		/// <param name="nonPublic">Indicates whether non-public methods should be returned in the <see cref="MethodInfo"/> array. <c>true</c> if non-public methods are to be included; otherwise, <c>false</c>. </param>
		/// <returns>An array of <see cref="MethodInfo"/> objects whose elements reflect the <c>get</c>, <c>set</c>, and other accessors of the property reflected by the current instance. 
		/// If <paramref name="nonPublic"/> is <c>true</c>, this array contains public and non-public <c>get</c>, <c>set</c>, and other accessors. 
		/// If <paramref name="nonPublic"/> is <c>false</c>, this array contains only public <c>get</c>, <c>set</c>, and other accessors. 
		/// If no accessors with the specified visibility are found, this method returns an array with zero (0) elements. 
		/// </returns>
		public virtual MethodInfo[] GetAccessors(bool nonPublic) { return GetAccessors(); }

		/// <summary>
		/// Returns the public <c>get</c> accessor for this property. 
		/// </summary>
		/// <returns>A <see cref="MethodInfo"/> object representing the public <c>get</c> accessor for this property, or a null reference (Nothing in Visual Basic) if the <c>get</c> accessor is non-public or does not exist. </returns>
		public virtual MethodInfo GetGetMethod() { return MethodInfo.Wrap(prop.GetMethod); }

		/// <summary>
		/// Returns the public or non-public <c>get</c> accessor for this property. 
		/// </summary>
		/// <param name="nonPublic">
		/// Indicates whether a non-public <c>get</c> accessor should be returned. 
		/// <c>true</c> if a non-public accessor is to be returned; otherwise, <c>false</c>. 
		/// </param>
		/// <returns>
		/// A <see cref="MethodInfo"/> object representing the <c>get</c> accessor for this property, if <paramref name="nonPublic"/> is <c>true</c>. 
		/// Returns a null reference (Nothing in Visual Basic) if <paramref name="nonPublic"/> is <c>false</c> and the get accessor is non-public, or if <paramref name="nonPublic"/> is <c>true</c> but no get accessors exist. 
		/// </returns>
		public virtual MethodInfo GetGetMethod(bool nonPublic) { return MethodInfo.Wrap(prop.GetMethod); }

		/// <summary>
		/// Returns an array of all the index parameters for the property. 
		/// </summary>
		/// <returns>An array of type <see cref="ParameterInfo"/> containing the parameters for the indexes. </returns>
		public virtual ParameterInfo[] GetIndexParameters() { return ParameterInfo.Wrap(prop.Parameters); }

		/// <summary>
		/// Returns the public <c>set</c> accessor for this property. 
		/// </summary>
		/// <returns>A <see cref="MethodInfo"/> object representing the public <c>set</c> accessor for this property, or a null reference (Nothing in Visual Basic) if the <c>set</c> accessor is non-public or does not exist. </returns>
		public virtual MethodInfo GetSetMethod() { return MethodInfo.Wrap(prop.SetMethod); }

		/// <summary>
		/// Returns the public or non-public <c>set</c> accessor for this property. 
		/// </summary>
		/// <param name="nonPublic">
		/// Indicates whether a non-public <c>set</c> accessor should be returned. 
		/// <c>true</c> if a non-public accessor is to be returned; otherwise, <c>false</c>. 
		/// </param>
		/// <returns>
		/// A <see cref="MethodInfo"/> object representing the <c>set</c> accessor for this property, if <paramref name="nonPublic"/> is <c>true</c>. 
		/// Returns a null reference (Nothing in Visual Basic) if <paramref name="nonPublic"/> is <c>false</c> and the set accessor is non-public, or if <paramref name="nonPublic"/> is <c>true</c> but no set accessors exist. 
		/// </returns>
		public virtual MethodInfo GetSetMethod(bool nonPublic) { return MethodInfo.Wrap(prop.SetMethod); }
		#endregion

		#region Cast & Wrap
		/// <summary>
		/// Cast a <see cref="System.Reflection.PropertyInfo"/> into a <see cref="PropertyInfo"/>.
		/// </summary>
		/// <param name="o">A <see cref="System.Reflection.PropertyInfo"/> to be casted.</param>
		/// <returns>A <see cref="PropertyInfo"/>.</returns>
		public static implicit operator PropertyInfo(System.Reflection.PropertyInfo o) { return new CLRPropertyInfo(o); }

		/// <summary>
		/// Cast a <see cref="Mono.Cecil.PropertyDefinition"/> into a <see cref="PropertyInfo"/>.
		/// </summary>
		/// <param name="o">A <see cref="Mono.Cecil.PropertyDefinition"/> to be casted.</param>
		/// <returns>A <see cref="PropertyInfo"/>.</returns>
		public static implicit operator PropertyInfo(Mono.Cecil.PropertyDefinition o) { return new PEPropertyInfo(o); }

		internal static PropertyInfo Wrap(System.Reflection.PropertyInfo prop)
		{
			if (prop == null)
				return null;
			return new CLRPropertyInfo(prop);
		}

		internal static PropertyInfo[] Wrap(System.Reflection.PropertyInfo[] props)
		{
			if (props == null)
				return null;
			CLRPropertyInfo[] wprops = new CLRPropertyInfo[ props.Length ];
			for (int i = 0; i < props.Length; i++)
				wprops[ i ] = new CLRPropertyInfo(props[ i ]);
			return wprops;
		}

		internal static PropertyInfo Wrap(Mono.Cecil.PropertyDefinition prop)
		{
			if (prop == null)
				return null;
			return new PEPropertyInfo(prop);
		}

		internal static PropertyInfo[] Wrap(Mono.Cecil.PropertyDefinition[] props)
		{
			if (props == null)
				return null;
			PEPropertyInfo[] wprops = new PEPropertyInfo[ props.Length ];
			for (int i = 0; i < props.Length; i++)
				wprops[ i ] = new PEPropertyInfo(props[ i ]);
			return wprops;
		}
		#endregion

		#region Inner Classes
		internal class CLRPropertyInfo: PropertyInfo
		{
			internal new System.Reflection.PropertyInfo prop;
			public CLRPropertyInfo(System.Reflection.PropertyInfo prop) { this.prop = prop; }

			#region MemberInfo
			public override Type DeclaringType { get { return prop.DeclaringType; } }
			public override System.Reflection.MemberTypes MemberType { get { return System.Reflection.MemberTypes.Property; } }
			public override string Name { get { return prop.Name; } }
			public override Type ReflectedType { get { return prop.ReflectedType; } }

			public override object[] GetCustomAttributes(bool inherit) { return prop.GetCustomAttributes(inherit); }
			public override object[] GetCustomAttributes(Type attributeType, bool inherit) { return prop.GetCustomAttributes(((Type.CLRType) attributeType).clrType, inherit); }
			public override bool IsDefined(Type attributeType, bool inherit) { return prop.IsDefined(((Type.CLRType) attributeType).clrType, inherit); }
			#endregion

			#region PropertyInfo
			public override System.Reflection.PropertyAttributes Attributes { get { return (System.Reflection.PropertyAttributes) prop.Attributes; } }
			public override bool CanRead { get { return prop.CanRead; } }
			public override bool CanWrite { get  { return prop.CanWrite; } }
			public override Type PropertyType  { get { return prop.PropertyType; } }
			public override MethodInfo[] GetAccessors() { return MethodInfo.Wrap(prop.GetAccessors()); }
			public override MethodInfo[] GetAccessors(bool nonPublic) { return MethodInfo.Wrap(prop.GetAccessors(nonPublic)); }
			public override MethodInfo GetGetMethod() { return prop.GetGetMethod(); }
			public override MethodInfo GetGetMethod(bool nonPublic) { return prop.GetGetMethod(nonPublic); }
			public override ParameterInfo[] GetIndexParameters() { return ParameterInfo.Wrap(prop.GetIndexParameters()); }
			public override MethodInfo GetSetMethod() { return prop.GetSetMethod(); }
			public override MethodInfo GetSetMethod(bool nonPublic) { return prop.GetSetMethod(nonPublic); }
			#endregion
		}

		internal class PEPropertyInfo: PropertyInfo
		{
			public PEPropertyInfo(Mono.Cecil.PropertyDefinition prop) { this.prop = prop; }
		}
		#endregion	
	}
}
