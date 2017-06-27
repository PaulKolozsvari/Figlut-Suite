using System;

namespace SystemCF.Diagnostics.SymbolStore
{
	/// <summary>
	/// Represents a symbol binder for managed code. 
	/// </summary>
	public interface ISymbolBinder
	{
		/// <summary>
		/// Gets the interface of the symbol reader for the current file. 
		/// </summary>
		/// <param name="importer">The metadata import interface. </param>
		/// <param name="filename">The name of the file for which the reader interface is required. </param>
		/// <param name="searchPath">The search path used to locate the symbol file. </param>
		/// <returns>The <see cref="ISymbolReader"/> interface that reads the debugging symbols. </returns>
		ISymbolReader GetReader(int importer, string filename, string searchPath);
	}
}
