namespace Figlut.Server.Toolkit.Extensions.ExtensionManaged
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data;

    #endregion //Using Directives

    public class ExtensionManagedEntityCache : EntityCache<string, ExtensionManagedEntity>
    {
        #region Methods

        public ExtensionManagedEntity AddExtensionManagedEntity(string entityFullTypeName)
        {
            if (string.IsNullOrEmpty(entityFullTypeName))
            {
                throw new NullReferenceException(string.Format(
                    "{0} of {1} to be added may not be null or empty.",
                    EntityReader<ExtensionManagedEntity>.GetPropertyName(p => p.EntityFullTypeName, false),
                    typeof(ExtensionManagedEntity).FullName));
            }
            if (Exists(entityFullTypeName))
            {
                throw new ArgumentException(string.Format(
                    "A {0} of {1} has already been added to this {2}.",
                    EntityReader<ExtensionManagedEntity>.GetPropertyName(p => p.EntityFullTypeName, false),
                    entityFullTypeName,
                    this.GetType().FullName));
            }
            ExtensionManagedEntity result = new ExtensionManagedEntity(entityFullTypeName);
            base.Add(entityFullTypeName, result);
            return result;
        }

        #endregion //Methods
    }
}
