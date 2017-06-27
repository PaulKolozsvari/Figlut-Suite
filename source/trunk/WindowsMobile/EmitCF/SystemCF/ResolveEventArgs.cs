using System;

namespace SystemCF
{
	/// <summary>
	/// Provides data for the <see cref="AppDomain.TypeResolve"/>, <see cref="AppDomain.ResourceResolve"/>, and <see cref="AppDomain.AssemblyResolve"/> events. 
	/// </summary>
	public class ResolveEventArgs : EventArgs
	{
		private string name;

		/// <summary>
		/// Initializes a new instance of the <see cref="ResolveEventArgs"/> class.
		/// </summary>
		/// <param name="name">The name of an item to resolve.</param>
		public ResolveEventArgs(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// Gets the name of the item to resolve. 
		/// </summary>
		public string Name 
		{
			get
			{
				return name;
			}
		}
	}
}
