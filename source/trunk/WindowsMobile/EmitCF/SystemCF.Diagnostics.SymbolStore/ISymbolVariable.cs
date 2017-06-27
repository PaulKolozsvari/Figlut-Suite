using System;

namespace SystemCF.Diagnostics.SymbolStore
{
	/// <summary>
	/// Represents a variable within a symbol store.
	/// </summary>
	public interface ISymbolVariable
	{
		/// <summary>
		/// Gets the variable signature. 
		/// </summary>
		/// <returns>The variable signature as an opaque blob.</returns>
		byte[] GetSignature();

		/// <summary>
		/// Gets the first address of a variable.
		/// </summary>
		int AddressField1 { get; }

		/// <summary>
		/// Gets the second address of a variable.
		/// </summary>
		int AddressField2 { get; }

		/// <summary>
		/// Gets the third address of a variable.
		/// </summary>
		int AddressField3 { get; }

		/// <summary>
		/// Gets the SymAddressKind value describing the type of the address.
		/// </summary>
		SymAddressKind AddressKind { get; }

		/// <summary>
		/// Gets the attributes of the variable.
		/// </summary>
		object Attributes { get; }

		/// <summary>
		/// Gets the end offset of a variable within the scope of the variable.
		/// </summary>
		int EndOffset { get; }

		/// <summary>
		/// Gets the name of the variable.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the start offset of the variable within the scope of the variable.
		/// </summary>
		int StartOffset { get; }
	}
}
