using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Represents a local variable within a method or constructor. 
	/// </summary>
	public class LocalBuilder
	{
		#region Private&Internal
		internal Mono.Cecil.Cil.VariableDefinition local;
		internal LocalBuilder(VariableDefinition local) 
		{ 
			this.local = local;
		}
		#endregion

		// Properties

		/// <summary>
		/// Gets a value indicating whether the object referred to by the local variable is pinned in memory.
		/// </summary>
		public bool IsPinned { get { return local.VariableType is PinnedType; } }

		/// <summary>
		/// Gets the zero-based index of the local variable within the method body. 
		/// </summary>
		public int LocalIndex { get { return local.Index; } }

		/// <summary>
		/// Gets the type of the local variable. 
		/// </summary>
		public Type LocalType { get { return local.VariableType; } }

		/// <summary>
		/// Sets the name of this local variable. 
		/// </summary>
		/// <param name="name">The name of the local variable. </param>
		public void SetLocalSymInfo(string name)
		{
		}

		/// <summary>
		/// Sets the name and lexical scope of this local variable. 
		/// </summary>
		/// <param name="name">The name of the local variable. </param>
		/// <param name="startOffset">The beginning offset of the lexical scope of the local variable. </param>
		/// <param name="endOffset">The ending offset of the lexical scope of the local variable. </param>
		public void SetLocalSymInfo(string name, int startOffset, int endOffset)
		{
		}
	}

}
