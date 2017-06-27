using System;

namespace SystemCF.Diagnostics.SymbolStore
{
	/// <summary>
	/// Represents a lexical scope within <see cref="ISymbolMethod"/>, providing access to the start and end offsets of the scope, as well as its child and parent scopes. 
	/// </summary>
	public interface ISymbolScope
	{
		/// <summary>
		/// Gets the child lexical scopes of the current lexical scope. 
		/// </summary>
		/// <returns>The child lexical scopes that of the current lexical scope. </returns>
		ISymbolScope[] GetChildren();

		/// <summary>
		/// Gets the local variables within the current lexical scope. 
		/// </summary>
		/// <returns>The local variables within the current lexical scope. </returns>
		ISymbolVariable[] GetLocals();

		/// <summary>
		/// Gets the namespaces that are used within the current scope. 
		/// </summary>
		/// <returns>The namespaces that are used within the current scope. </returns>
		ISymbolNamespace[] GetNamespaces();

		/// <summary>
		/// Gets the end offset of the current lexical scope.
		/// </summary>
		int EndOffset { get; }

		/// <summary>
		/// Gets the method that contains the current lexical scope.
		/// </summary>
		ISymbolMethod Method { get; }

		/// <summary>
		/// Gets the parent lexical scope of the current scope.
		/// </summary>
		ISymbolScope Parent { get; }

		/// <summary>
		/// Gets the start offset of the current lexical scope.
		/// </summary>
		int StartOffset { get; }
	}
}
