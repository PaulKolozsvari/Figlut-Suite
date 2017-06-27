using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCF.Diagnostics.SymbolStore
{
	/// <summary>
	/// Holds the public GUIDs for document types to be used with the symbol store. 
	/// </summary>
	public class SymDocumentType
	{
		/// <summary>
		/// Specifies the GUID of the document type to be used with the symbol store. 
		/// </summary>
		public static readonly Guid Text = new Guid(0x5a869d0b, 0x6611, 0x11d3, 0xbd, 0x2a, 0, 0, 0xf8, 8, 0x49, 0xbd);
	}
}
