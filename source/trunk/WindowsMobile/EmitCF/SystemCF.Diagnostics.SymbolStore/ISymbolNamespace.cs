using System;

namespace SystemCF.Diagnostics.SymbolStore
{
	/// <summary>
	/// Represents a namespace within a symbol store. 
	/// </summary>
	public interface ISymbolNamespace
	{
		/// <summary>
		/// Gets the child members of the current namespace. 
		/// </summary>
		/// <returns>The child members of the current namespace. </returns>
		ISymbolNamespace[] GetNamespaces();

		/// <summary>
		/// Gets all the variables defined at global scope within the current namespace. 
		/// </summary>
		/// <returns>The variables defined at global scope within the current namespace. </returns>
		ISymbolVariable[] GetVariables();

		/// <summary>
		/// Gets the current namespace.
		/// </summary>
		string Name { get; }
	}
}
