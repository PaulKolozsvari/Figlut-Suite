namespace Figlut.Server.Toolkit.Data.ORM
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection;
    using System.Reflection.Emit;

    #endregion //Using Directives

    /// <summary>
    /// http://olondono.blogspot.com/2008/02/creating-code-at-runtime.html
    /// </summary>
    public class OrmProperty
    {
        #region Constructors

        public OrmProperty(
            string propertyName, 
            PropertyAttributes propertyAttributes,
            Type propertyType)
        {
            Initialize(propertyName, propertyAttributes, propertyType);
        }

        #endregion //Constructors

        #region Fields

        protected string _propertyName;
        protected PropertyAttributes _propertyAttributes;
        protected Type _propertyType;
        protected string _fieldName;
        protected FieldAttributes _fieldAttributes;
        protected string _getMethodName;
        protected string _setMethodName;

        #endregion //Fields

        #region Properties

        public string PropertyName
        {
            get { return _propertyName; }
        }

        public PropertyAttributes PropertyAttributes
        {
            get { return _propertyAttributes; }
        }

        public Type PropertyType
        {
            get { return _propertyType; }
        }

        public string FieldName
        {
            get { return _fieldName; }
        }

        public FieldAttributes FieldAttributes
        {
            get { return _fieldAttributes; }
        }

        public string GetMethodName
        {
            get { return _getMethodName; }
        }

        public string SetMethodName
        {
            get { return _setMethodName; }
        }

        #endregion //Properties

        #region Methods

        public void Clean()
        {
            _propertyName = null;
            _propertyAttributes = PropertyAttributes.None;
            _propertyType = null;
            _fieldName = null;
            _getMethodName = null;
            _setMethodName = null;
        }

        protected void Initialize(
            string propertyName,
            PropertyAttributes propertyAttributes,
            Type propertyType)
        {
            if (propertyName != null)
            {
                propertyName = propertyName.Trim();
            }
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new NullReferenceException(string.Format(
                    "{0} may not be null when constructing a {1}.",
                    EntityReader<OrmProperty>.GetPropertyName(p => p.PropertyName, false),
                    this.GetType().FullName));
            }
            if (propertyType == null)
            {
                throw new NullReferenceException(string.Format(
                    "{0} may not be null when constructing a {1}.",
                    EntityReader<OrmProperty>.GetPropertyName(p => p.PropertyType, false),
                    this.GetType().FullName));                
            }
            _propertyName = propertyName;
            _propertyAttributes = propertyAttributes;
            _propertyType = propertyType;

            _fieldName = string.Format(
                "_{0}{1}",
                _propertyName[0].ToString().ToLower(),
                _propertyName.Substring(1));
            _fieldAttributes = FieldAttributes.Private;

            _getMethodName = string.Format("get_{0}", _propertyName);
            _setMethodName = string.Format("set_{0}", _propertyName);
        }

        public override string ToString()
        {
            return _propertyName;
        }

        #endregion //Methods

        #region Destructors

        //~OrmProperty()
        //{
        //    int i = 0;
        //}

        #endregion //Destructors
    }
}