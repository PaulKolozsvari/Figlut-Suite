namespace Figlut.Mobile.Toolkit.Data
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Data;

    #endregion //Using Directives

    public class EntityReader<E> : EntityReader
    {
        #region Methods

        public static DataTable GetDataTable(bool shapeColumnNames)
        {
            return GetDataTable(shapeColumnNames, typeof(E));
        }

        public static string GetPropertyName<TProp>(Expression<Func<E, TProp>> expression, bool shapePropertyName)
        {
            var body = expression.Body as MemberExpression;
            if (body == null) throw new ArgumentException("'expression' should be a member expression");
            if (shapePropertyName)
            {
                return DataShaper.ShapeCamelCaseString(body.Member.Name);
            }
            return body.Member.Name;
        }

        public static List<string> GetAllPropertyNames(bool shapeColumnNames)
        {
            return GetAllPropertyNames(shapeColumnNames, typeof(E));
        }

        public static object GetPropertyValue(string propertyName, E entity, bool useDBNull)
        {
            return GetPropertyValue(propertyName, (object)entity, useDBNull);
        }

        public static void SetPropertyValue(string propertyName, E entity, object value)
        {
            SetPropertyValue(propertyName, entity, value);
        }

        public static Type GetPropertyType<TProp>(Expression<Func<E, TProp>> expression)
        {
            var body = expression.Body as MemberExpression;
            if (body == null) throw new ArgumentException("'expression' should be a member expression");
            return typeof(E).GetProperty(body.Member.Name).PropertyType;
        }

        public static E PopulateFromDataRow(E entity, DataRow row)
        {
            return (E)EntityReader.PopulateFromDataRow(entity, row);
        }

        public static DataRow PopulateDataRow(E entity, DataRow row, bool shapePropertyNames)
        {
            return PopulateDataRow(entity, row, shapePropertyNames, typeof(E));
        }

        public static K ConvertTo<K>(E entity)
        {
            return (K)ConvertTo(entity, typeof(K));
        }

        #endregion //Methods
    }
}