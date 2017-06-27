namespace Figlut.Mobile.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    #endregion //Using Directives

    public class AssemblyReader
    {
        #region Methods

        public static E CreateClassInstance<E>(Assembly assembly, string extensionFullTypeName) where E : class
        {
            Type extensionType = assembly.GetType(extensionFullTypeName, false);
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

        #endregion //Methods
    }
}
