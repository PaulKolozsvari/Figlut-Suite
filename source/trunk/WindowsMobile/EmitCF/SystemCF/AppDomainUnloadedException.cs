using System;

namespace SystemCF
{
	/// <summary>
	/// The exception that is thrown when an attempt is made to access an unloaded application domain. 
	/// </summary>
	public class AppDomainUnloadedException : SystemException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AppDomainUnloadedException"/> class with a specified error message. 
		/// </summary>
		/// <param name="message">The message that describes the error. </param>
		public AppDomainUnloadedException(string message) : base(message)
		{
		}
	}
}
