using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF
{
	/// <summary>
	/// Contains methods to create types of objects. 
	/// </summary>
	public class Activator
	{
		/// <summary>
		/// Creates an instance of the specified type using the constructor that best matches the specified parameter.
		/// </summary>
		/// <param name="type">The type of object to create.</param>
		/// <returns></returns>
		public static object CreateInstance(SystemCF.Type type)
		{
			System.Type myType = type.Assembly.GetType(
				type.FullName
				.Replace("/", "+")	// For subclasses
				);

			// Invoke default constructor and return the instance created.
			return System.Activator.CreateInstance(myType);
		}

		/// <summary>
		/// Creates an instance of the specified type using the constructor that best matches the specified parameters.
		/// </summary>
		/// <param name="type">The type of object to create.</param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args"/> is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked. </param>
		/// <returns></returns>
		public static object CreateInstance(SystemCF.Type type, params object[] args)
		{
			System.Type myType = type.Assembly.GetType(
				type.FullName
				.Replace("/", "+")	// For subclasses
				);

			if (args.Length == 0) return System.Activator.CreateInstance(myType);

			// Build array of types from arguments
			System.Type[] types = new System.Type[args.Length];
			for (int i = 0; i < args.Length; i++)
			{
				object arg = args[i];

				if (arg == null) types[i] = typeof(object);
				else types[i] = arg.GetType();
			}

			// Get Constructor
			System.Reflection.ConstructorInfo cinfo = myType.GetConstructor(types);

			// Invoke constructor and return the instance created.
			return cinfo.Invoke(args);
		}
	}
}
