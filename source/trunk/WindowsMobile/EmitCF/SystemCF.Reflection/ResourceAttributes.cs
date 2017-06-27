using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	/// <summary>
	/// Specifies the attributes for a manifest resource. 
	/// </summary>
	[System.Flags]
	public enum ResourceAttributes
	{
		/// <summary>
		/// A mask used to retrieve public manifest resources. 
		/// </summary>
		Public = 1,
		/// <summary>
		/// A mask used to retrieve private manifest resources. 
		/// </summary>
		Private = 2
	}

	internal class ResourceAttributesConverter
	{
		static bool Match(ResourceAttributes value, ResourceAttributes @enum) { return (value & @enum) == @enum; }

		public static ManifestResourceAttributes Convert(ResourceAttributes peAttrs)
		{
			ManifestResourceAttributes attrs = (ManifestResourceAttributes) 0;
			if (Match(peAttrs, ResourceAttributes.Public)) attrs |= ManifestResourceAttributes.Public;			 
			if (Match(peAttrs, ResourceAttributes.Private)) attrs |= ManifestResourceAttributes.Private;			 
			return attrs;
		}
	}
}
