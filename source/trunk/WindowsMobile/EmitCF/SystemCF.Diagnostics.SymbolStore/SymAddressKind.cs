using System;

namespace SystemCF.Diagnostics.SymbolStore
{
	/// <summary>
	/// Specifies address types for local variables, parameters, and fields in the methods <see cref="ISymbolWriter.DefineLocalVariable"/>, <see cref="ISymbolWriter.DefineParameter"/>, and <see cref="ISymbolWriter.DefineField"/> of the <see cref="ISymbolWriter"/> interface. 
	/// </summary>
	public enum SymAddressKind
	{
		/// <summary>
		/// A bit field. 
		/// </summary>
		BitField = 9,
		/// <summary>
		/// A Microsoft intermediate language (MSIL) offset. 
		/// </summary>
		ILOffset = 1,
		/// <summary>
		/// A native offset. 
		/// </summary>
		NativeOffset = 5,
		/// <summary>
		/// A native register address. 
		/// </summary>
		NativeRegister = 3,
		/// <summary>
		/// A register-relative address.
		/// </summary>
		NativeRegisterRegister = 6,
		/// <summary>
		/// A register-relative address. 
		/// </summary>
		NativeRegisterRelative = 4,
		/// <summary>
		/// A register-relative address. 
		/// </summary>
		NativeRegisterStack = 7,
		/// <summary>
		/// A native Relevant Virtual Address (RVA).
		/// </summary>
		NativeRVA = 2,
		/// <summary>
		/// A native section offset.
		/// </summary>
		NativeSectionOffset = 10,
		/// <summary>
		/// A register-relative address. 
		/// </summary>
		NativeStackRegister = 8
	}
}
