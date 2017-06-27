﻿using System;

namespace SystemCF.Diagnostics.SymbolStore
{
	/// <summary>
	/// Represents a symbol reader for managed code. 
	/// </summary>
	public interface ISymbolReader
	{
		/// <summary>
		/// Gets a document specified by the language, vendor, and type. 
		/// </summary>
		/// <param name="url">The URL that identifies the document. </param>
		/// <param name="language">The document language. </param>
		/// <param name="languageVendor">The identity of the vendor for the document language. </param>
		/// <param name="documentType">The type of the document.</param>
		/// <returns>The specified document. </returns>
		ISymbolDocument GetDocument(string url, Guid language, Guid languageVendor, Guid documentType);

		/// <summary>
		/// Gets an array of all documents defined in the symbol store. 
		/// </summary>
		/// <returns>An array of all documents defined in the symbol store. </returns>
		ISymbolDocument[] GetDocuments();

		/// <summary>
		/// Gets all global variables in the module. 
		/// </summary>
		/// <returns>An array of all variables in the module. </returns>
		ISymbolVariable[] GetGlobalVariables();

		/// <summary>
		/// Gets a symbol reader method object when given the identifier of a method. 
		/// </summary>
		/// <param name="method">The metadata token of the method. </param>
		/// <returns>The symbol reader method object for the specified method identifier. </returns>
		ISymbolMethod GetMethod(SymbolToken method);

		/// <summary>
		/// Gets a symbol reader method object when given the identifier of a method and its edit and continue version. 
		/// </summary>
		/// <param name="method">The metadata token of the method. </param>
		/// <param name="version">The edit and continue version of the method.</param>
		/// <returns>The symbol reader method object for the specified method identifier. </returns>
		ISymbolMethod GetMethod(SymbolToken method, int version);

		/// <summary>
		/// Gets a symbol reader method object that contains a specified position in a document. 
		/// </summary>
		/// <param name="document">The document in which the method is located. </param>
		/// <param name="line">The position of the line within the document. </param>
		/// <param name="column">The position of column within the document. </param>
		/// <returns>The reader method object for the specified position in the document. </returns>
		ISymbolMethod GetMethodFromDocumentPosition(ISymbolDocument document, int line, int column);

		/// <summary>
		/// Gets the namespaces that are defined in the global scope within the current symbol store. 
		/// </summary>
		/// <returns>The namespaces defined in the global scope within the current symbol store. </returns>
		ISymbolNamespace[] GetNamespaces();

		/// <summary>
		/// Gets an attribute value when given the attribute name. 
		/// </summary>
		/// <param name="parent">The metadata token for the object for which the attribute is requested. </param>
		/// <param name="name">The attribute name. </param>
		/// <returns>The value of the attribute. </returns>
		byte[] GetSymAttribute(SymbolToken parent, string name);

		/// <summary>
		/// Gets the variables that are not local when given the parent. 
		/// </summary>
		/// <param name="parent">The metadata token for the type for which the variables are requested. </param>
		/// <returns>An array of variables for the parent. </returns>
		ISymbolVariable[] GetVariables(SymbolToken parent);

		/// <summary>
		/// Gets the metadata token for the method that was specified as the user entry point for the module, if any.
		/// </summary>
		SymbolToken UserEntryPoint { get; }
	}

}
