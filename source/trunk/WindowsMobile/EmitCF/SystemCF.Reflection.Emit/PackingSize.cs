using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Specifies one of two factors that determine the memory alignment of fields when a type is marshaled. 
	/// </summary>
	public enum PackingSize
	{
		/// <summary>
		/// The packing size is not specified. 
		/// </summary>
	    Unspecified = 0,
		/// <summary>
		/// The packing size is 1 byte. 
		/// </summary>
	    Size1 = 1,
		/// <summary>
		/// The packing size is 2 bytes. 
		/// </summary>
	    Size2 = 2,
		/// <summary>
		/// The packing size is 4 bytes. 
		/// </summary>
	    Size4 = 4,
		/// <summary>
		/// The packing size is 8 bytes. 
		/// </summary>
	    Size8 = 8,
		/// <summary>
		/// The packing size is 16 bytes. 
		/// </summary>
	    Size16 = 0x10,
		/// <summary>
		/// The packing size is 32 bytes. 
		/// </summary>
	    Size32 = 0x20,
		/// <summary>
		/// The packing size is 64 bytes. 
		/// </summary>
	    Size64 = 0x40,
		/// <summary>
		/// The packing size is 128 bytes. 
		/// </summary>
	    Size128 = 0x80,
	}
}
