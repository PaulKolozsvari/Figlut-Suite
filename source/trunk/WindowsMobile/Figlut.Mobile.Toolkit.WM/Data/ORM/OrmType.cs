namespace Figlut.Mobile.Toolkit.Data.ORM
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection.Emit;
    using System.Reflection;
    using SystemCF.Reflection.Emit;

    #endregion //Using Directives

    /// <summary>
    /// http://olondono.blogspot.com/2008/02/creating-code-at-runtime.html
    /// </summary>
    public class OrmType
    {
        #region Constructors

        public OrmType(string typeName, TypeBuilder typeBuilder)
        {
            Initialize(typeName, typeBuilder);
        }

        #endregion //Constructors

        #region Fields

        protected string _typeName;
        protected TypeBuilder _typeBuilder;
        protected EntityCache<string, OrmProperty> _properties;
        protected Type _dotNetType;

        #endregion //Fields

        #region Properties

        public string TypeName
        {
            get { return _typeName; }
        }

        public TypeBuilder TypeBuilder
        {
            get { return _typeBuilder; }
        }

        public EntityCache<string, OrmProperty> Properties
        {
            get { return _properties; }
        }

        public Type DotNetType
        {
            get { return _dotNetType; }
        }

        #endregion //Properties

        #region Methods

        public void Clean()
        {
            _properties.ToList().ForEach(p => p.Clean());
            _properties.Clear();
            _properties = null;
            _typeName = null;
            _typeBuilder = null;
            _dotNetType = null;
        }

        protected void Initialize(string typeName, TypeBuilder typeBuilder)
        {
            if (typeName != null)
            {
                typeName = typeName.Trim();
            }
            if (string.IsNullOrEmpty(typeName))
            {
                throw new NullReferenceException(string.Format(
                    "{0} may not be null when constructing a {1}.",
                    EntityReader<OrmType>.GetPropertyName(p => p.TypeName, false),
                    this.GetType().FullName));
            }
            if (typeBuilder == null)
            {
                throw new NullReferenceException(string.Format(
                    "{0} may not be null when constructing a {1}.",
                    EntityReader<OrmType>.GetPropertyName(p => p.TypeBuilder, false),
                    this.GetType().FullName));
            }
            _typeName = typeName;
            _typeBuilder = typeBuilder;
            _properties = new EntityCache<string, OrmProperty>();
        }

        public OrmProperty CreateOrmProperty(string propertyName, Type propertyType)
        {
            return CreateOrmProperty(propertyName, propertyType, PropertyAttributes.HasDefault);
        }

        public OrmProperty CreateOrmProperty(string propertyName, Type propertyType, PropertyAttributes propertyAttributes)
        {
            if (_properties.Exists(propertyName))
            {
                throw new ArgumentException(string.Format(
                    "{0} with {1} {2} already added to {3}.",
                    typeof(OrmProperty).FullName,
                    EntityReader<OrmProperty>.GetPropertyName(p => p.PropertyName, false),
                    propertyName,
                    this.GetType().FullName));
            }
            OrmProperty result = new OrmProperty(propertyName, propertyAttributes, propertyType);
            FieldBuilder fieldBuilder = _typeBuilder.DefineField(result.FieldName, result.PropertyType, result.FieldAttributes);
            PropertyBuilder propertyBuilder = _typeBuilder.DefineProperty(result.PropertyName, result.PropertyAttributes, result.PropertyType, null);
            MethodAttributes methodAttributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName;
            MethodBuilder getMethodBuilder = _typeBuilder.DefineMethod(result.GetMethodName, methodAttributes, result.PropertyType, SystemCF.Type.EmptyTypes);
            MethodBuilder setMethodBuilder = _typeBuilder.DefineMethod(result.SetMethodName, methodAttributes, null, new SystemCF.Type[] { result.PropertyType });

            ILGenerator ilGenerator = setMethodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1); //Load argument.
            ilGenerator.Emit(OpCodes.Stfld, fieldBuilder); //Set field.
            ilGenerator.Emit(OpCodes.Ret);

            ilGenerator = getMethodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder); //Load field.
            ilGenerator.Emit(OpCodes.Ret); //Return the field.

            //Set the methods on the properties.
            propertyBuilder.SetSetMethod(setMethodBuilder);
            propertyBuilder.SetGetMethod(getMethodBuilder);

            _properties.Add(result.PropertyName, result);
            return result;
        }

        /// <summary>
        /// //Returns a SystemCF.Reflection.Emit.TypeBuilder and an operator overload 
        /// converts it to a CLR System.Type by closing/completing the assembly and module, 
        /// saving it and reloading it with the CLR Reflection. Therefore only call this method
        /// after all types have been added, otherwise you cannot add more methods after the
        /// assembly/module has been completed, saved and reloaded.
        /// </summary>
        /// <returns></returns>
        public Type CreateType()
        {
            _dotNetType = _typeBuilder.CreateType();
            return _dotNetType;
        }

        public override string ToString()
        {
            return _typeName;
        }

        #endregion //Methods

        #region Destructors

        //~OrmType()
        //{
        //    int i = 0;
        //}

        #endregion //Destructors
    }
}