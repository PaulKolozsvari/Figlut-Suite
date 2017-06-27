using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Describes a Microsoft intermediate language (MSIL) instruction. 
	/// </summary>
	public struct OpCode
	{
		#region Private&Internal
		internal Mono.Cecil.Cil.OpCode op;	
		#endregion

		#region Equality
		/// <summary>
		/// Returns <c>true</c> if the objects are equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="a">an object.</param>
		/// <param name="b">an object.</param>
		/// <returns><c>true</c> if the objects are equal, <c>false</c> otherwise.</returns>
		public static bool operator ==(OpCode a, OpCode b)
		{
			return a.op == b.op;
		}

		/// <summary>
		/// Returns <c>true</c> if the objects are not equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="a">an object.</param>
		/// <param name="b">an object.</param>
		/// <returns><c>true</c> if the objects are not equal, <c>false</c> otherwise.</returns>
		public static bool operator !=(OpCode a, OpCode b)
		{
			return a.op != b.op;
		}

		/// <summary>
		/// Determines whether two <see cref="System.Object"/> instances are equal. 
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public override bool Equals(object o)
        {
            if (o is OpCode) return this == (OpCode)o;
            return false;
        }

		/// <summary>
		/// Serves as a hash function for a particular type. <see cref="GetHashCode"/> is suitable for use in hashing algorithms and data structures like a hash table. 
		/// </summary>
		/// <returns>A hash code for the current <see cref="OpCode"/>. </returns>
		public override int GetHashCode()
        {
            return op.GetHashCode();
        }
		#endregion

		#region Cast & Wrap
		/// <summary>
		/// Cast a <see cref="Mono.Cecil.Cil.OpCode"/> into a <see cref="OpCode"/>.
		/// </summary>
		/// <param name="code">A <see cref="Mono.Cecil.Cil.OpCode"/> to be casted.</param>
		/// <returns>A <see cref="OpCode"/>.</returns>
		public static implicit operator OpCode(Mono.Cecil.Cil.OpCode code)
		{
			OpCode _code;
			_code.op = code;
			return _code;
		}
		#endregion
	}
}
