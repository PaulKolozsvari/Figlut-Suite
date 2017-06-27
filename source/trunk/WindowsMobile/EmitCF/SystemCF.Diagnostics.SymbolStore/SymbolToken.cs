using System;

namespace SystemCF.Diagnostics.SymbolStore
{
	/// <summary>
	/// The <see cref="SymbolToken"/> structure is an object representation of a token that represents symbolic information. 
	/// </summary>
	public struct SymbolToken
	{
		internal int _token;

		/// <summary>
		/// Initializes a new instance of the <see cref="SymbolToken"/> structure when given a value.
		/// </summary>
		/// <param name="val">The value to be used for the token. </param>
		public SymbolToken(int val)
		{
			_token = val;
		}

		/// <summary>
		/// Gets the value of the current token. 
		/// </summary>
		/// <returns>The value of the current token. </returns>
		public int GetToken()
		{
			return _token;
		}

		/// <summary>
		/// Generates the hash code for the current token.
		/// </summary>
		/// <returns>he hash code for the current token. </returns>
		public override int GetHashCode()
		{
			return _token;
		}

		/// <summary>
		/// Determines whether <paramref name="obj"/> is an instance of <see cref="SymbolToken"/> and is equal to this instance.
		/// </summary>
		/// <param name="obj">The object to check. </param>
		/// <returns><c>true</c> if <paramref name="obj"/> is an instance of <see cref="SymbolToken"/> and is equal to this instance; otherwise, <c>false</c>. </returns>
		public override bool Equals(object obj)
		{
			return ((obj is SymbolToken) && this.Equals((SymbolToken) obj));
		}

		/// <summary>
		/// Determines whether <paramref name="obj"/> is equal to this instance. 
		/// </summary>
		/// <param name="obj">The <see cref="SymbolToken"/> to check.</param>
		/// <returns><c>true</c> if <paramref name="obj"/> is equal to this instance; otherwise, <c>false</c>. </returns>
		public bool Equals(SymbolToken obj)
		{
			return (obj._token == _token);
		}

		/// <summary>
		/// Returns a value indicating whether two <see cref="SymbolToken"/> objects are equal. 
		/// </summary>
		/// <param name="a">A <see cref="SymbolToken"/> structure.</param>
		/// <param name="b">A <see cref="SymbolToken"/> structure.</param>
		/// <returns><c>true</c> if <paramref name="a"/> and <paramref name="b"/> are equal; otherwise, <c>false</c>. </returns>
		public static bool operator ==(SymbolToken a, SymbolToken b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Returns a value indicating whether two <see cref="SymbolToken"/> objects are not equal. 
		/// </summary>
		/// <param name="a">A <see cref="SymbolToken"/> structure.</param>
		/// <param name="b">A <see cref="SymbolToken"/> structure.</param>
		/// <returns><c>true</c> if <paramref name="a"/> and <paramref name="b"/> are not equal; otherwise, <c>false</c>. </returns>
		public static bool operator !=(SymbolToken a, SymbolToken b)
		{
			return !(a == b);
		}
	}
}