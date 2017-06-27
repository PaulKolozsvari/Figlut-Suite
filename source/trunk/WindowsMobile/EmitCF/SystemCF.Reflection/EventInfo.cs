using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	/// <summary>
	/// Discovers the attributes of an event and provides access to event metadata. 
	/// </summary>
	public abstract class EventInfo : MemberInfo
	{
		internal Mono.Cecil.EventDefinition ev;

		#region Equality
		/// <summary>
		/// Returns <c>true</c> if the objects are equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are equal, <c>false</c> otherwise.</returns>
		public static bool operator ==(EventInfo t1, EventInfo t2)
		{
			if ((object) t1 == null || (object) t2 == null)
				return (object) t1 == (object) t2;
			if (t1 is CLREventInfo)
				return (t2 is CLREventInfo) && ((CLREventInfo) t1).ev == ((CLREventInfo) t2).ev;
			return (t2 is PEEventInfo) && ((PEEventInfo) t1).ev == ((PEEventInfo) t2).ev;
		}

		/// <summary>
		/// Returns <c>true</c> if the objects are not equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are not equal, <c>false</c> otherwise.</returns>
		public static bool operator !=(EventInfo t1, EventInfo t2)
		{
			if ((object) t1 == null || (object) t2 == null)
				return (object) t1 != (object) t2;
			if (t1 is CLREventInfo)
				return !(t2 is CLREventInfo) || ((CLREventInfo) t1).ev != ((CLREventInfo) t2).ev;
			return !(t2 is PEEventInfo) || ((PEEventInfo) t1).ev != ((PEEventInfo) t2).ev;
		}

		/// <summary>
		/// Determines whether two <see cref="System.Object"/> instances are equal. 
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public override bool Equals(object o)
		{
			if (o is EventInfo)
				return this == (EventInfo) o;
			return false;
		}

		/// <summary>
		/// Serves as a hash function for a particular type. <see cref="GetHashCode"/> is suitable for use in hashing algorithms and data structures like a hash table. 
		/// </summary>
		/// <returns>A hash code for the current <see cref="EventInfo"/>. </returns>
		public override int GetHashCode()
		{
			if (this is CLREventInfo) return ((CLREventInfo)this).ev.GetHashCode();
			return ((PEEventInfo)this).ev.GetHashCode();
		}
		#endregion
	
		#region MemberInfo
		/// <summary>
		/// Gets the class that declares this member. 
		/// </summary>
		public override Type DeclaringType { get { return ev.DeclaringType; } }

		/// <summary>
		/// Gets a <see cref="System.Reflection.MemberTypes"/> value indicating the type of the member — method, constructor, event, and so on. 
		/// </summary>
		public override System.Reflection.MemberTypes MemberType { get { return System.Reflection.MemberTypes.Event; } }

		/// <summary>
		/// Gets the name of the current member.
		/// </summary>
		public override string Name { get { return ev.Name; } }

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

		#region EventInfo
		/// <summary>
		/// Gets the attributes for this event.
		/// </summary>
		public virtual System.Reflection.EventAttributes Attributes { get { return EventAttributesConverter.Convert(ev.Attributes); } }

		/// <summary>
		/// Gets the Type object of the underlying event-handler delegate associated with this event.
		/// </summary>
		public virtual Type EventHandlerType { get { return Type.Import(ev.EventType); } }

		/// <summary>
		/// Returns the method used to add an event handler delegate to the event source. 
		/// </summary>
		/// <returns>A <see cref="MethodInfo"/> object representing the method used to add an event handler delegate to the event source. </returns>
		public virtual MethodInfo GetAddMethod() { return MethodInfo.Wrap(ev.AddMethod); }

		/// <summary>
		/// Retrieves the <see cref="MethodInfo"/> object for the <c>AddEventHandler</c> method of the event, specifying whether to return non-public methods. 
		/// </summary>
		/// <param name="nonPublic"><c>true</c> if non-public methods can be returned; otherwise, <c>false</c>. </param>
		/// <returns></returns>
		public virtual MethodInfo GetAddMethod(bool nonPublic) { return MethodInfo.Wrap(ev.AddMethod); }

		/// <summary>
		/// Returns the method that is called when the event is raised. 
		/// </summary>
		/// <returns>The method that is called when the event is raised. </returns>
		public virtual MethodInfo GetRaiseMethod() { return MethodInfo.Wrap(ev.InvokeMethod); }

		/// <summary>
		/// Returns the method that is called when the event is raised, specifying whether to return non-public methods. 
		/// </summary>
		/// <param name="nonPublic"><c>true</c> if non-public methods can be returned; otherwise, <c>false</c>. </param>
		/// <returns>A <see cref="MethodInfo"/> object that was called when the event was raised. </returns>
		public virtual MethodInfo GetRaiseMethod(bool nonPublic) { return MethodInfo.Wrap(ev.InvokeMethod); }

		/// <summary>
		/// Returns the method used to remove an event handler delegate from the event source. 
		/// </summary>
		/// <returns>A <see cref="MethodInfo"/> object representing the method used to remove an event handler delegate from the event source. </returns>
		public virtual MethodInfo GetRemoveMethod() { return MethodInfo.Wrap(ev.RemoveMethod); }

		/// <summary>
		/// Retrieves the <see cref="MethodInfo"/> object for removing a method of the event, specifying whether to return non-public methods. 
		/// </summary>
		/// <param name="nonPublic"><c>true</c> if non-public methods can be returned; otherwise, <c>false</c>. </param>
		/// <returns>A <see cref="MethodInfo"/> object representing the method used to remove an event handler delegate from the event source. </returns>
		public virtual MethodInfo GetRemoveMethod(bool nonPublic) { return MethodInfo.Wrap(ev.RemoveMethod); }
		#endregion

		#region Cast & Wrap
		/// <summary>
		/// Cast a <see cref="System.Reflection.EventInfo"/> into a <see cref="EventInfo"/>.
		/// </summary>
		/// <param name="o">A <see cref="System.Reflection.EventInfo"/> to be casted.</param>
		/// <returns>A <see cref="EventInfo"/>.</returns>
		public static implicit operator EventInfo(System.Reflection.EventInfo o) { return new CLREventInfo(o); }

		/// <summary>
		/// Cast a <see cref="Mono.Cecil.EventDefinition"/> into a <see cref="EventDefinition"/>.
		/// </summary>
		/// <param name="o">A <see cref="Mono.Cecil.EventDefinition"/> to be casted.</param>
		/// <returns>A <see cref="EventInfo"/>.</returns>
		public static implicit operator EventInfo(Mono.Cecil.EventDefinition o) { return new PEEventInfo(o); }

		internal static EventInfo Wrap(System.Reflection.EventInfo @event)
		{
			if (@event == null)
				return null;
			return new CLREventInfo(@event);
		}

		internal static EventInfo[] Wrap(System.Reflection.EventInfo[] events)
		{
			if (events == null)
				return null;
			CLREventInfo[] wevents = new CLREventInfo[ events.Length ];
			for (int i = 0; i < events.Length; i++)
				wevents[ i ] = new CLREventInfo(events[ i ]);
			return wevents;
		}

		internal static EventInfo Wrap(Mono.Cecil.EventDefinition @event)
		{
			if (@event == null)
				return null;
			return new PEEventInfo(@event);
		}

		internal static EventInfo[] Wrap(Mono.Cecil.EventDefinition[] events)
		{
			if (events == null)
				return null;
			PEEventInfo[] wevents = new PEEventInfo[ events.Length ];
			for (int i = 0; i < events.Length; i++)
				wevents[ i ] = new PEEventInfo(events[ i ]);
			return wevents;
		}
		#endregion

		#region Inner Classes
		internal class CLREventInfo: EventInfo
		{
			internal new System.Reflection.EventInfo ev;
			public CLREventInfo(System.Reflection.EventInfo ev) { this.ev = ev; }

			#region MemberInfo
			public override Type DeclaringType { get { return ev.DeclaringType; } }
			public override System.Reflection.MemberTypes MemberType { get { return System.Reflection.MemberTypes.Event; } }
			public override string Name { get { return ev.Name; } }
			public override Type ReflectedType { get { return ev.ReflectedType; } }

			public override object[] GetCustomAttributes(bool inherit) { return ev.GetCustomAttributes(inherit); }
			public override object[] GetCustomAttributes(Type attributeType, bool inherit) { return ev.GetCustomAttributes(((Type.CLRType) attributeType).clrType, inherit); }
			public override bool IsDefined(Type attributeType, bool inherit) { return ev.IsDefined(((Type.CLRType) attributeType).clrType, inherit); }
			#endregion

			#region EventInfo
			public override System.Reflection.EventAttributes Attributes { get { return (System.Reflection.EventAttributes) ev.Attributes; } }
			public override Type EventHandlerType { get { return ev.EventHandlerType; } }
			public override MethodInfo GetAddMethod() { return ev.GetAddMethod(); }
			public override MethodInfo GetAddMethod(bool nonPublic) { return ev.GetAddMethod(nonPublic); }
			public override MethodInfo GetRaiseMethod() { return ev.GetRaiseMethod(); }
			public override MethodInfo GetRaiseMethod(bool nonPublic) { return ev.GetRaiseMethod(nonPublic); }
			public override MethodInfo GetRemoveMethod() { return ev.GetRemoveMethod(); }
			public override MethodInfo GetRemoveMethod(bool nonPublic) { return ev.GetRemoveMethod(nonPublic); }
			#endregion
		}

		internal class PEEventInfo: EventInfo
		{
			internal PEEventInfo(Mono.Cecil.EventDefinition ev) { this.ev = ev; }
			internal PEEventInfo(SystemCF.Reflection.Emit.EventBuilder eb) { this.ev = eb.ev; }
		}
		#endregion	
	}
}
