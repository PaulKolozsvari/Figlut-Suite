using System;

namespace SystemCF.Diagnostics.SymbolStore
{
	/// <summary>
	/// Represents a document referenced by a symbol store.
	/// </summary>
	public interface ISymbolDocument
	{
		/// <summary>
		/// Returns the closest line that is a sequence point, given a line in the current document that might or might not be a sequence point. 
		/// </summary>
		/// <param name="line">The specified line in the document. </param>
		/// <returns>The closest line that is a sequence point. </returns>
		int FindClosestLine(int line);

		/// <summary>
		/// Gets the checksum. 
		/// </summary>
		/// <returns>The checksum. </returns>
		byte[] GetCheckSum();

		/// <summary>
		/// Gets the embedded document source for the specified range. 
		/// </summary>
		/// <param name="startLine">The starting line in the current document.</param>
		/// <param name="startColumn">The starting column in the current document. </param>
		/// <param name="endLine">The ending line in the current document. </param>
		/// <param name="endColumn">The ending column in the current document.</param>
		/// <returns>The document source for the specified range. </returns>
		byte[] GetSourceRange(int startLine, int startColumn, int endLine, int endColumn);
		
		/// <summary>
		/// Gets the checksum algorithm identifier.
		/// </summary>
		Guid CheckSumAlgorithmId { get; }

		/// <summary>
		/// Gets the type of the current document.
		/// </summary>
		Guid DocumentType { get; }

		/// <summary>
		/// Checks whether the current document is stored in the symbol store.
		/// </summary>
		bool HasEmbeddedSource { get; }

		/// <summary>
		/// Gets the language of the current document.
		/// </summary>
		Guid Language { get; }

		/// <summary>
		/// Gets the language vendor of the current document.
		/// </summary>
		Guid LanguageVendor { get; }

		/// <summary>
		/// Gets the length, in bytes, of the embedded source.
		/// </summary>
		int SourceLength { get; }

		/// <summary>
		/// Gets the URL of the current document.
		/// </summary>
		string URL { get; }
	}
}
