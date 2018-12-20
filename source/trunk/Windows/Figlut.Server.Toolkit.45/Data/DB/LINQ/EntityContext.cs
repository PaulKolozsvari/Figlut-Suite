namespace Figlut.Server.Toolkit.Data.DB.LINQ
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Transactions;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Data.DB.LINQ;
    using Figlut.Server.Toolkit.Data.DB.LINQ.Logging;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Web.Service;
    using System.Data;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;

    #endregion //Using Directives

    //6 ways of doing locking in .NET (Pessimistic and optimistic): https://www.codeproject.com/Articles/114262/ways-of-doing-locking-in-NET-Pessimistic-and-opt
    //Overview of concurrency in LINQ to SQL: https://www.c-sharpcorner.com/article/overview-of-concurrency-in-linq-to-sql/
    //Optimistic Concurrency Overview: https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/optimistic-concurrency-overview
    //RefreshMode : https://docs.microsoft.com/en-us/dotnet/api/system.data.linq.refreshmode?view=netframework-4.7.2
    //How to: Specify When Concurrency Exceptions are Thrown: https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/how-to-specify-when-concurrency-exceptions-are-thrown
    //How to handle concurrency in LINQ to SQL: https://www.codeproject.com/Articles/38299/How-To-Handle-Concurrency-in-LINQ-to-SQL
    //Pessimistic Concurrency in LINQ to SQL by using Transactions: https://blogs.msdn.microsoft.com/wriju/2007/08/06/linq-to-sql-using-transaction/
    public class EntityContext : LinqFunnelContext
    {
        #region Constructors

        public EntityContext(
            DataContext db, 
            LinqFunnelSettings settings, 
            bool handleExceptions,
            Type userLinqToSqlType,
            Type serverActionLinqToSqlType,
            Type serverErrorLinqToSqlType) :
            base(db, settings)
        {
            _handleExceptions = handleExceptions;
            _userLinqToSqlType = userLinqToSqlType;
            _serverActionLinqToSqlType = serverActionLinqToSqlType;
            _serverErrorLinqtoSqlType = serverErrorLinqToSqlType;
        }

        #endregion //Constructors

        #region Fields

        protected bool _handleExceptions;
        protected Type _userLinqToSqlType;
        protected Type _serverActionLinqToSqlType;
        protected Type _serverErrorLinqtoSqlType;

        #endregion //Fields

        #region Properties

        public Type UserLinqToSqlType
        {
            get { return _userLinqToSqlType; }
        }

        public Type ServerActionLinqToSqlType
        {
            get { return _serverActionLinqToSqlType; }
        }

        public Type ServerErrorLinqToSqlType
        {
            get { return _serverErrorLinqtoSqlType; }
        }

        #endregion //Properties

        #region Methods

        #region Core Methods

        public ServiceProcedureResult Save<E>(
            List<E> entities, 
            Nullable<Guid> userId,
            string userName,
            bool saveChildren) where E : class
        {
            try
            {
                using (TransactionScope t = new TransactionScope())
                {
                    foreach(E e in entities)
                    {
                        base.Save<E>(e, saveChildren).ForEach(c => HandleChange(c, userId, userName));
                    }
                    t.Complete();
                }
                return new ServiceProcedureResult();
            }
            catch (Exception ex)
            {
                if (_handleExceptions)
                {
                    HandleException(ex, userId, userName);
                }
                throw;
            }
        }

        public ServiceProcedureResult Save(
            Type entityType, 
            List<object> entities, 
            Nullable<Guid> userId, 
            string userName,
            bool saveChildren)
        {
            try
            {
                using (TransactionScope t = new TransactionScope())
                {
                    foreach (object e in entities)
                    {
                        base.Save(entityType, e, false).ForEach(c => HandleChange(c, userId, userName));
                    }
                    t.Complete();
                }
                return new ServiceProcedureResult();
            }
            catch (Exception ex)
            {
                if (_handleExceptions)
                {
                    HandleException(ex, userId, userName);
                }
                throw;
            }
        }

        public ServiceProcedureResult Insert(
            Type entityType,
            List<object> entities,
            Nullable<Guid> userId,
            string userName,
            bool saveChildren)
        {
            try
            {
                using (TransactionScope t = new TransactionScope())
                {
                    foreach (object e in entities)
                    {
                        base.Insert(entityType, e, false).ForEach(c => HandleChange(c, userId, userName));
                    }
                    t.Complete();
                }
                return new ServiceProcedureResult();
            }
            catch (Exception ex)
            {
                if (_handleExceptions)
                {
                    HandleException(ex, userId, userName);
                }
                throw;
            }
        }

        public ServiceProcedureResult Delete<E>(
            List<E> entities, 
            Nullable<Guid> userId,
            string userName) where E : class
        {
            try
            {
                using (TransactionScope t = new TransactionScope())
                {
                    foreach (E e in entities)
                    {
                        base.Delete<E>(e).ForEach(c => HandleChange(c, userId, userName));
                    }
                    t.Complete();
                }
                return new ServiceProcedureResult();
            }
            catch (Exception ex)
            {
                if (_handleExceptions)
                {
                    HandleException(ex, userId, userName);
                }
                throw;
            }
        }

        public ServiceProcedureResult Delete(
            Type entityType, 
            List<object> entities,
            Nullable<Guid> userId,
            string userName)
        {
            try
            {
                using (TransactionScope t = new TransactionScope())
                {
                    foreach (object e in entities)
                    {
                        base.Delete(e).ForEach(c => HandleChange(c, userId, userName));
                    }
                    t.Complete();
                }
                return new ServiceProcedureResult();
            }
            catch (Exception ex)
            {
                if (_handleExceptions)
                {
                    HandleException(ex, userId, userName);
                }
                throw;
            }
        }

        public ServiceProcedureResult DeleteBySurrogateKey<E>(
            List<object> surrogateKeys, 
            Nullable<Guid> userId,
            string userName) where E : class
        {
            try
            {
                using (TransactionScope t = new TransactionScope())
                {
                    foreach (object keyValue in surrogateKeys)
                    {
                        base.DeleteBySurrogateKey<E>(keyValue).ForEach(c => HandleChange(c, userId, userName));
                        t.Complete();
                    }
                }
                return new ServiceProcedureResult();
            }
            catch (Exception ex)
            {
                if (_handleExceptions)
                {
                    HandleException(ex, userId, userName);
                }
                throw;
            }
        }

        public ServiceProcedureResult DeleteBySurrogateKey(
            Type entityType, 
            List<object> surrogateKeys, 
            Nullable<Guid> userId,
            string userName)
        {
            try
            {
                using (TransactionScope t = new TransactionScope())
                {
                    foreach (object key in surrogateKeys)
                    {
                        base.DeleteBySurrogateKey(key, entityType).ForEach(c => HandleChange(c, userId, userName));
                        t.Complete();
                    }
                }
                return new ServiceProcedureResult();
            }
            catch (Exception ex)
            {
                if (_handleExceptions)
                {
                    HandleException(ex, userId, userName);
                }
                throw;
            }
        }

        public ServiceProcedureResult DeleteAll<E>(
            Nullable<Guid> userId,
            string userName) where E : class
        {
            try
            {
                using (TransactionScope t = new TransactionScope())
                {
                    base.DeleteAll<E>().ForEach(c => HandleChange(c, userId, userName));
                    t.Complete();
                }
                return new ServiceProcedureResult();
            }
            catch (Exception ex)
            {
                if (_handleExceptions)
                {
                    HandleException(ex, userId, userName);
                }
                throw;
            }
        }

        public ServiceProcedureResult DeleteAll(
            Type entityType,
            Nullable<Guid> userId,
            string userName)
        {
            try
            {
                using (TransactionScope t = new TransactionScope())
                {
                    base.DeleteAll(entityType).ForEach(c => HandleChange(c, userId, userName));
                    t.Complete();
                }
                return new ServiceProcedureResult();
            }
            catch (Exception ex)
            {
                if (_handleExceptions)
                {
                    HandleException(ex, userId, userName);
                }
                throw;
            }
        }

        public ServiceFunctionResult<E> GetEntityBySurrogateKey<E>(
            object keyValue, 
            bool loadChildren, 
            Nullable<Guid> userId,
            string userName) where E : class
        {
            try
            {
                return new ServiceFunctionResult<E> { Contents = base.GetEntityBySurrogateKey<E>(keyValue, loadChildren) };

            }
            catch (Exception ex)
            {
                if (_handleExceptions)
                {
                    HandleException(ex, userId, userName);
                }
                throw;
            }
        }

        public ServiceFunctionResult<object> GetEntityBySurrogateKey(
            Type entityType, 
            object keyValue, 
            bool loadChildren, 
            Nullable<Guid> userId,
            string userName)
        {
            try
            {
                return new ServiceFunctionResult<object>() { Contents = base.GetEntityBySurrogateKey(entityType, keyValue, loadChildren) };
            }
            catch (Exception ex)
            {
                if (_handleExceptions)
                {
                    HandleException(ex, userId, userName);
                }
                throw;
            }
        }

        public ServiceFunctionResult<List<E>> GetEntitiesbyField<E>(
            string fieldName, 
            object fieldValue, 
            bool loadChildren, 
            Nullable<Guid> userId,
            string userName) where E : class
        {
            try
            {
                return new ServiceFunctionResult<List<E>>() { Contents = base.GetEntitiesByField<E>(fieldName, fieldValue, loadChildren) };
            }
            catch (Exception ex)
            {
                if (_handleExceptions)
                {
                    HandleException(ex, userId, userName);
                }
                throw;
            }
        }

        public ServiceFunctionResult<List<object>> GetEntitiesByField(
            Type entityType, 
            string fieldName, 
            object fieldValue, 
            bool loadChildren, 
            Nullable<Guid> userId,
            string userName)
        {
            try
            {
                return new ServiceFunctionResult<List<object>>() { Contents = base.GetEntitiesByField(entityType, fieldName, fieldValue, loadChildren) };
            }
            catch (Exception ex)
            {
                if (_handleExceptions)
                {
                    HandleException(ex, userId, userName);
                }
                throw;
            }
        }

        public ServiceFunctionResult<List<E>> GetAllEntities<E>(
            bool loadChildren, 
            Nullable<Guid> userId, 
            string userName) where E : class
        {
            try
            {
                return new ServiceFunctionResult<List<E>>() { Contents = base.GetAllEntities<E>(loadChildren) };
            }
            catch (Exception ex)
            {
                if (_handleExceptions)
                {
                    HandleException(ex, userId, userName);
                }
                throw;
            }
        }

        public ServiceFunctionResult<List<object>> GetAllEntities(
            Type entityType, 
            bool loadChildren, 
            Nullable<Guid> userId, 
            string userName)
        {
            try
            {
                return new ServiceFunctionResult<List<object>>() { Contents = base.GetAllEntities(entityType, loadChildren) };
            }
            catch (Exception ex)
            {
                if (_handleExceptions)
                {
                    HandleException(ex, userId, userName);
                }
                throw;
            }
        }

        public ServiceFunctionResult<int> GetTotalCount<E>(
            Nullable<Guid> userId, 
            string userName) where E : class
        {
            try
            {
                return new ServiceFunctionResult<int>() { Contents = base.GetTotalCount<E>() };
            }
            catch (Exception ex)
            {
                if (_handleExceptions)
                {
                    HandleException(ex, userId, userName);
                }
                throw;
            }
        }

        public ServiceFunctionResult<long> GetTotalCountLong<E>(
            Nullable<Guid> userId,
            string userName) where E : class
        {
            try
            {
                return new ServiceFunctionResult<long>() { Contents = base.GetTotalCountLong<E>() };
            }
            catch (Exception ex)
            {
                if (_handleExceptions)
                {
                    HandleException(ex, userId, userName);
                }
                throw ex;
            }
        }

        public Guid GetUserId(string userName)
        {
            List<object> query = GetEntitiesByField(
                _userLinqToSqlType,
                EntityReader<User>.GetPropertyName(p => p.UserName, false),
                userName,
                false);
            if (query.Count > 1)
            {
                throw new Exception(string.Format("More than one user with the user name of {0}.", userName));
            }
            if (query.Count < 1)
            {
                throw new Exception(string.Format("No user exists with the user name of {0}.", userName));
            }
            object user = query[0];
            PropertyInfo surrogateKey = GetEntitySurrogateKey(_userLinqToSqlType);
            Guid result = (Guid)EntityReader.GetPropertyValue(surrogateKey.Name, user, false);
            return result;
        }

        #endregion //Core Methods

        #region Utility Methods

        protected void HandleChange(LinqFunnelChangeResult change, Nullable<Guid> userId, string userName)
        {
            ServerAction serverAction = new ServerAction()
            {
                Function = change.Function,
                DateCreated = change.DateChanged,
                EntityChanged = change.EntityChanged,
                FieldChanged = change.FieldChanged
            };
            if(userId.HasValue && !string.IsNullOrEmpty(userName))
            {
                serverAction.UserId = userId;
                serverAction.UserName = userName;
            }
            if (change.OriginalValue != null)
            {
                serverAction.OriginalValue = change.OriginalValue.ToString();
            }
            if (change.NewValue != null)
            {
                serverAction.NewValue = change.NewValue.ToString();
            }
            if (_serverActionLinqToSqlType != null)
            {
                object serverActionLinqToSql = EntityReader<ServerAction>.ConvertTo(serverAction, _serverActionLinqToSqlType);
                base.Save(_serverActionLinqToSqlType, serverActionLinqToSql, false);
            }
        }

        protected ServiceResult HandleException(Exception ex)
        {
            return HandleException(ex, null, null);
        }

        //protected ServiceResult HandleException(Exception ex, User user)
        //{
        //    ServerError error = new ServerError()
        //    {
        //        ErrorMessage = ex.Message,
        //        DateCreated = DateTime.Now,
        //    };
        //    if (user != null)
        //    {
        //        error.UserId = user.UserId;
        //        error.UserName = user.UserName;
        //    }
        //    //HACK The saving of errors needs to be refactored. The reason for the below code is because we need to
        //    //create a new context, as the DB one has thrown an exception, therefore we are not able to use it any longer to save.
        //    //That's why the error is being saved in the first place i.e. because an exception has been thrown.
        //    DataContext context = ((DataContext)Activator.CreateInstance(DB.GetType()));
        //    context.Connection.ConnectionString = base.LinqFunnelSettings.ConnectionString;
        //    using (context)
        //    {
        //        context.GetTable<ServerError>().InsertOnSubmit(error);
        //        context.SubmitChanges();
        //    }
        //    ServiceException serviceException = ex as ServiceException;
        //    if (serviceException == null)
        //    {
        //        return new ServiceResult { Code = ServiceResultCode.FatalError, Message = ex.Message }; //Not a user thrown exception.
        //    }
        //    else
        //    {
        //        return serviceException.Result;
        //    }
        //}

        protected ServiceResult HandleException(Exception ex, Nullable<Guid> userId, string userName)
        {
            ServerError serverError = new ServerError()
            {
                ErrorMessage = ex.Message,
                DateCreated = DateTime.Now,
            };
            if (userId.HasValue && !string.IsNullOrEmpty(userName))
            {
                serverError.UserId = userId;
                serverError.UserName = userName;
            }
            //HACK The saving of errors needs to be refactored. The reason for the below code is because we need to
            //create a new context, as the DB one has thrown an exception, therefore we are not able to use it any longer to save.
            //That's why the error is being saved in the first place i.e. because an exception has been thrown.
            DataContext context = ((DataContext)Activator.CreateInstance(DB.GetType()));
            context.Connection.ConnectionString = base.LinqFunnelSettings.ConnectionString;
            using (context)
            {
                object serverErrorLinqToSql = EntityReader<ServerError>.ConvertTo(serverError, _serverErrorLinqtoSqlType);
                context.GetTable(_serverErrorLinqtoSqlType).InsertOnSubmit(serverErrorLinqToSql);
                context.SubmitChanges();
            }
            ServiceException serviceException = ex as ServiceException;
            if (serviceException == null)
            {
                return new ServiceResult { Code = ServiceResultCode.FatalError, Message = ex.Message }; //Not a user thrown exception.
            }
            else
            {
                return serviceException.Result;
            }
        }

        #endregion //Utility Methods

        #region Schema Query Methods

        public List<SqlDatabaseTableColumn> GetSqlDatabaseTableColumnNames(string tableName)
        {
            List<SqlDatabaseTableColumn> result = new List<SqlDatabaseTableColumn>();
            using (DB.Connection)
            {
                if (DB.Connection.State != ConnectionState.Open)
                {
                    DB.Connection.Open();
                }
                DataTable schemaTable = DB.Connection.GetSchema("Columns", new string[] { null, null, tableName, null });
                DB.Connection.Close();
                foreach (DataRow schemaRow in schemaTable.Rows)
                {
                    SqlDatabaseTableColumn column = new SqlDatabaseTableColumn(schemaRow);
                    result.Add(column);
                }
            }
             return result;
        }

        public List<SqlDatabaseTable> GetSqlDatabaseTableNames()
        {
            List<SqlDatabaseTable> result = new List<SqlDatabaseTable>();
            using (DB.Connection)
            {
                if (DB.Connection.State != ConnectionState.Open)
                {
                    DB.Connection.Open();
                }
                DataTable schemaTable = DB.Connection.GetSchema("Tables");
                DB.Connection.Close();
                foreach (DataRow schemaRow in schemaTable.Rows)
                {
                    SqlDatabaseTable table = new SqlDatabaseTable(schemaRow);
                    result.Add(table);
                }
            }
            return result;
        }

        #endregion //Schema Query Methods

        #endregion //Methods
    }
}