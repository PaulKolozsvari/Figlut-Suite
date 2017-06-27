using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	/// <summary>
	/// Performs reflection on a module.
	/// </summary>
	public abstract class Module
	{
		internal Mono.Cecil.ModuleDefinition peModule;

		#region Virtual
		/// <summary>
		/// Returns the specified class, performing a case-sensitive search. 
		/// </summary>
		/// <param name="className">The name of the class to locate. The name must be fully qualified with the namespace.</param>
		/// <returns>A <see cref="Type"/> object representing the given class name, if the class is in this module; otherwise, <c>null</c>.</returns>
		public virtual Type GetType(string className) 
		{ 
			return Type.Import(peModule.Types[className]); 
		}

		/// <summary>
		/// Returns all the classes defined within this module. 
		/// </summary>
		/// <returns>An array of type <see cref="Type"/> containing classes defined within the module that is reflected by this instance.</returns>
		public virtual Type[] GetTypes() 
		{
			List<Type> types = new List<Type>();
			foreach (TypeDefinition td in peModule.Types)
				types.Add(Type.Import(td));
			return types.ToArray(); 
		}

		/// <summary>
		/// Gets the appropriate <see cref="Assembly"/> for this instance of <see cref="Module"/>. 
		/// </summary>
		public virtual Assembly Assembly { get { return new AssemblyBuilder(peModule.Assembly); } }

		/// <summary>
		/// Gets a <see cref="System.String"/> representing the fully qualified name and path to this module.
		/// </summary>
		public virtual string FullyQualifiedName { get { return peModule.Name; } }

		/// <summary>
		/// Gets a <see cref="System.String"/> representing the name of the module with the path removed. 
		/// </summary>
		public virtual string Name { get { return peModule.Name; } }
		#endregion

		#region Equality
		/// <summary>
		/// Returns <c>true</c> if the objects are equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are equal, <c>false</c> otherwise.</returns>
		public static bool operator ==(Module t1, Module t2)
		{
			if ((object)t1 == null || (object)t2 == null)
				return (object)t1 == (object)t2;
			if (t1 is CLRModule)
				return (t2 is CLRModule) && ((CLRModule)t1).clrModule == ((CLRModule)t2).clrModule;
			return (t2 is PEAPIModule) && ((PEAPIModule)t1).peModule == ((PEAPIModule)t2).peModule;
		}

		/// <summary>
		/// Returns <c>true</c> if the objects are not equal, <c>false</c> otherwise. 
		/// </summary>
		/// <param name="t1">an object.</param>
		/// <param name="t2">an object.</param>
		/// <returns><c>true</c> if the objects are not equal, <c>false</c> otherwise.</returns>
		public static bool operator !=(Module t1, Module t2)
		{
			if ((object)t1 == null || (object)t2 == null)
				return (object)t1 != (object)t2;
			if (t1 is CLRModule)
				return !(t2 is CLRModule) || ((CLRModule)t1).clrModule != ((CLRModule)t2).clrModule;
			return !(t2 is PEAPIModule) || ((PEAPIModule)t1).peModule != ((PEAPIModule)t2).peModule;
		}

		/// <summary>
		/// Determines whether two <see cref="System.Object"/> instances are equal. 
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public override bool Equals(object o)
		{
			if (o is Module)
				return this == (Module)o;
			return false;
		}

		/// <summary>
		/// Serves as a hash function for a particular type. <see cref="GetHashCode"/> is suitable for use in hashing algorithms and data structures like a hash table. 
		/// </summary>
		/// <returns>A hash code for the current <see cref="Module"/>. </returns>
		public override int GetHashCode()
		{
			if (this is CLRModule) return ((CLRModule)this).clrModule.GetHashCode();
			return ((PEAPIModule)this).peModule.GetHashCode();
		}
		#endregion

		#region Cast & Wrap
		/// <summary>
		/// Cast a <see cref="System.Reflection.Module"/> into a <see cref="Module"/>.
		/// </summary>
		/// <param name="o">A <see cref="System.Reflection.Module"/> to be casted.</param>
		/// <returns>A <see cref="Module"/>.</returns>
		public static implicit operator Module(System.Reflection.Module o) { return new CLRModule(o); }

		/// <summary>
		/// Cast a <see cref="Mono.Cecil.ModuleDefinition"/> into a <see cref="Module"/>.
		/// </summary>
		/// <param name="o">A <see cref="Mono.Cecil.ModuleDefinition"/> to be casted.</param>
		/// <returns>A <see cref="Module"/>.</returns>
		public static implicit operator Module(Mono.Cecil.ModuleDefinition o) { return new PEAPIModule(o); }
		#endregion

		#region Inner Classes
		internal class CLRModule : Module
		{
			internal System.Reflection.Module clrModule;
			internal CLRModule(System.Reflection.Module clrModule) { this.clrModule = clrModule; }

			// Methods
			public override Type GetType(string className) { return Type.Import(clrModule.GetType(className)); }
			public override Type[] GetTypes() { return Type.Wrap(clrModule.GetTypes()); }

			// Properties
			public override Assembly Assembly { get { return null; } }
			public override string FullyQualifiedName { get { return clrModule.FullyQualifiedName; } }
			public override string Name { get { return clrModule.Name; } }
		}

		internal class PEAPIModule : Module
		{
			internal PEAPIModule(Mono.Cecil.ModuleDefinition peModule) { this.peModule = peModule; }
		}
		#endregion
	}
}
