using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF
{
	/// <summary>
	/// Represents an application domain, which is an isolated environment where applications execute. This class cannot be inherited. 
	/// </summary>
	public sealed class AppDomain
	{
		private static AppDomain theDomain;

		/// <summary>
		/// Gets the current application domain for the current Thread. 
		/// <remarks>
		/// Modified from original: gets the unique static instance.
		/// </remarks>
		/// </summary>
		public static AppDomain CurrentDomain
		{
			get { 
				if (theDomain == null) theDomain = new AppDomain();
				return theDomain;
			}
		}

		/// <summary>
		/// Defines a dynamic assembly with the specified name and access mode. 
		/// </summary>
		/// <param name="name">The unique identity of the dynamic assembly.</param>
		/// <param name="access">The access mode for the dynamic assembly.</param>
		/// <returns>Represents the dynamic assembly created.</returns>
		/// <remarks>
		/// Modified from original: the <paramref name="access"/> parameter is not used.
		/// </remarks>
		public AssemblyBuilder DefineDynamicAssembly(System.Reflection.AssemblyName name, AssemblyBuilderAccess access)
		{
			return DefineDynamicAssembly(name, access, null);
		}

		/// <summary>
		/// Defines a dynamic assembly using the specified name, access mode, and storage directory. 
		/// </summary>
		/// <param name="name">The unique identity of the dynamic assembly.</param>
		/// <param name="access">The access mode for the dynamic assembly.</param>
		/// <param name="path">The name of the directory where the assembly will be saved. If <paramref name="dir"/> is <c>null</c>, the directory defaults to the current directory.</param>
		/// <returns>Represents the dynamic assembly created.</returns>
		/// <remarks>
		/// Modified from original: the <paramref name="access"/> parameter is not used.
		/// </remarks>
		public AssemblyBuilder DefineDynamicAssembly(System.Reflection.AssemblyName name, AssemblyBuilderAccess access, string path)
		{
			if (name == null)
			{
				throw new System.ArgumentNullException("name");
			}
				
			return new AssemblyBuilder(name, access, path);
		}

		/// <summary>
		/// Occurs when the resolution of a type fails.
		/// </summary>
		public event ResolveEventHandler TypeResolve;

	}

	/// <summary>
	/// Represents the method that handles the <see cref="AppDomain.TypeResolve"/>, ResourceResolve, and AssemblyResolve events of an <see cref="AppDomain"/>. 
	/// </summary>
	/// <param name="sender">The source of the event. </param>
	/// <param name="args">A <see cref="ResolveEventArgs"/> that contains the event data. </param>
	/// <returns>The <see cref="SystemCF.Reflection.Assembly"/> that resolves the type, assembly, or resource. </returns>
	public delegate Assembly ResolveEventHandler(object sender, ResolveEventArgs args);
}
