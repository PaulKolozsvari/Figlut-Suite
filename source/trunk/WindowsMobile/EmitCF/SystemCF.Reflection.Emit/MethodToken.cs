using System;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// The <see cref="MethodToken"/> struct is an object representation of a token that represents a method. 
	/// </summary>
	public struct MethodToken
	{
		/// <summary>
		/// The default <see cref="MethodToken"/> with <see cref="MethodToken.Token"/> value 0. 
		/// </summary>
		public static readonly MethodToken Empty = new MethodToken(0);

		private int token;
		internal MethodToken(int token)
		{
			this.token = token;
		}

		/// <summary>
		/// Returns the metadata token for this method. 
		/// </summary>
		public int Token 
		{
			get { return token; } 
		}
	}
}
