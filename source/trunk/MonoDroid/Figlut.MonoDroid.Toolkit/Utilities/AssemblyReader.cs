namespace Figlut.MonoDroid.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class AssemblyReader
    {
        #region Methods

        public static E CreateClassInstance<E>(Assembly assembly, string extensionFullTypeName) where E : class
        {
            Type extensionType = assembly.GetType(extensionFullTypeName, false, true);
            if (extensionType == null)
            {
                throw new NullReferenceException(
                    string.Format(
                    "Cannot find type {0} in assembly {1}.",
                    extensionFullTypeName,
                    assembly.FullName));
            }
            object obj = Activator.CreateInstance(extensionType);
            E result = obj as E;
            if (result == null)
            {
                throw new NullReferenceException(
                    string.Format(
                    "Type {0} in assembly {1} is not of type {2}.",
                    extensionFullTypeName,
                    assembly.FullName,
                    typeof(E).FullName));
            }
            return result;
        }

        public static Type FindType(
            string assemblyName, 
            string typeNamespace, 
            string typeName,
            bool throwExceptionOnError)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyName);
            return FindType(assembly, typeNamespace, typeName, throwExceptionOnError);
        }

        public static Type FindType(
            Assembly assembly,
            string typeNamespace,
            string typeName,
            bool throwExceptionOnError)
        {
            string assemblyName = assembly.GetName().CodeBase.Remove(0, 6);
            string fullTypeName = string.Format("{0}.{1}", typeNamespace, typeName);
            Type result = assembly.GetType(fullTypeName, false, true);
            if (result == null && throwExceptionOnError)
            {
                throw new NullReferenceException(string.Format("Could find type {0} in assembly {1}.", fullTypeName, assemblyName));
            }
            return result;
        }

        #endregion //Methods
    }
}
