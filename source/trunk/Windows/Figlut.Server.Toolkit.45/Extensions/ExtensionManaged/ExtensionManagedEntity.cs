namespace Figlut.Server.Toolkit.Extensions.ExtensionManaged
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Utilities;

    #endregion //Using Directives

    public class ExtensionManagedEntity : EntityCache<string, ExtensionManagedEntityProperty>
    {
        #region Constructors

        public ExtensionManagedEntity(string entityFullTypeName)
            : base()
        {
            if (string.IsNullOrEmpty(entityFullTypeName))
            {
                throw new NullReferenceException(string.Format(
                    "{0} may not be null when constructing an {1}.",
                    EntityReader<ExtensionManagedEntity>.GetPropertyName(p => p.EntityFullTypeName, false),
                    typeof(ExtensionManagedEntity).FullName));
            }
            _entityFullTypeName = entityFullTypeName;
        }

        #endregion //Constructors

        #region Fields

        protected string _entityFullTypeName;

        #endregion //Fields

        #region Properties

        public string EntityFullTypeName
        {
            get { return _entityFullTypeName; }
        }

        #endregion //Properties

        #region Methods

        public ExtensionManagedEntityProperty AddExtensionManagedProperty(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new NullReferenceException(string.Format(
                    "{0} of {1} to be added may not be null or empty.",
                    EntityReader<ExtensionManagedEntityProperty>.GetPropertyName(p => p.PropertyName, false),
                    typeof(ExtensionManagedEntityProperty).FullName));
            }
            if (Exists(propertyName))
            {
                throw new ArgumentException(string.Format(
                    "A {0} of {1} has already been added to this {2} for {3}.",
                    EntityReader<ExtensionManagedEntityProperty>.GetPropertyName(p => p.PropertyName, false),
                    propertyName,
                    this.GetType().FullName,
                    _entityFullTypeName));
            }
            ExtensionManagedEntityProperty result = new ExtensionManagedEntityProperty(_entityFullTypeName, propertyName);
            base.Add(propertyName, result);
            return result;
        }

        #endregion //Methods
    }
}