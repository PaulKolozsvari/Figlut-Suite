using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Defines events for a class. 
	/// </summary>
	public class EventBuilder: EventInfo
	{
		#region Private & Internal
		internal EventBuilder(string name, System.Reflection.EventAttributes attrs, Type eventType, TypeBuilder type)
		{
			type.TypeDef.Events.Add(ev = new EventDefinition(name, eventType.peType, EventAttributesConverter.Convert(attrs)));
		}
		#endregion

		/// <summary>
		/// Sets the method used to subscribe to this event. 
		/// </summary>
		/// <param name="mdBuilder">A <see cref="MethodBuilder"/> object that represents the method used to subscribe to this event. </param>
		public void SetAddOnMethod(MethodBuilder mdBuilder)
		{
			ev.AddMethod = mdBuilder.meth;
		}

		/// <summary>
		/// Set a custom attribute using a custom attribute builder. 
		/// </summary>
		/// <param name="customBuilder">An instance of a helper class to define the custom attribute. </param>
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
	    	CustomAttribute ca;
	    	ev.CustomAttributes.Add(ca = new CustomAttribute(customBuilder.ctor));
	    	foreach (object arg in customBuilder.args) ca.ConstructorParameters.Add(arg);
		}

		/// <summary>
		/// Set a custom attribute using a specified custom attribute blob. 
		/// </summary>
		/// <param name="con">Set a custom attribute using a specified custom attribute blob. </param>
		/// <param name="binaryAttribute">A byte blob representing the attributes. </param>
	    public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
	    	CustomAttribute ca;
			ev.CustomAttributes.Add(ca = new CustomAttribute(con.peMeth, binaryAttribute));
		}

		/// <summary>
		/// Sets the method used to raise this event. 
		/// </summary>
		/// <param name="mdBuilder">A <see cref="MethodBuilder"/> object that represents the method used to raise this event.</param>
		public void SetRaiseMethod(MethodBuilder mdBuilder)
		{
			ev.InvokeMethod = mdBuilder.meth;
		}

		/// <summary>
		/// Sets the method used to unsubscribe to this event. 
		/// </summary>
		/// <param name="mdBuilder">A <see cref="MethodBuilder"/> object that represents the method used to unsubscribe to this event. </param>
		public void SetRemoveOnMethod(MethodBuilder mdBuilder)
		{
			ev.RemoveMethod = mdBuilder.meth;
		}

		/// <summary>
		/// Get the CLR-equivalent <see cref="System.Reflection.EventInfo"/> representation of a <see cref="EventBuilder"/> object.
		/// </summary>
		/// <param name="type">The <see cref="EventBuilder"/> to convert.</param>
		/// <returns>A <see cref="System.Reflection.EventInfo"/> representation equivalent to the <paramref name="ev"/> object.</returns>
		public static implicit operator System.Reflection.EventInfo(EventBuilder ev)
		{
			if (ev == null) return null;

			// Get CLR Type
			System.Type clrType = (TypeBuilder) ev.DeclaringType;

			// Return event from its signature
			return clrType.GetEvent(ev.Name);
		}

	}
}
