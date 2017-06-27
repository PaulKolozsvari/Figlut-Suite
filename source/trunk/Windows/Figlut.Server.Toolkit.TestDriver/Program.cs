namespace Figlut.Server.Toolkit.TestDriver
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection.Emit;
    using System.Reflection;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using System.Data;
    using Figlut.Server.Toolkit.Data.DB;
    using Figlut.Server.Toolkit.Data.ORM;

    #endregion //Using Directives

    class Program
    {
        #region Methods

        static void Main(string[] args)
        {
            try
            {


                CreateOrmAssemblyFromDatabase();
                Console.ReadLine();
                //CreateOrmObjects();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }

        private static void CreateOrmAssemblyFromDatabase()
        {
            //string connectionString = @"Data Source=PAULKOLOZSV4DD3\SQL2008STANDARD;Initial Catalog=DigisticsStoreDelivery;Persist Security Info=True;User ID=DigisticsStoreDeliveryUser;Password=password";
            //using (SqlDatabase database = new SqlDatabase(
            //    connectionString,
            //    true,
            //    true,
            //    true,
            //    true))

            //{
            //    foreach (DatabaseTable table in database.Tables)
            //    {
            //        Console.WriteLine("*** Table Name: {0} ***", table.TableName);
            //        table.Columns.ToList().ForEach(c => Console.WriteLine(c.ToString()));
            //        Console.WriteLine();
            //    }
            //}
        }

        private static void CreateOrmObjects()
        {
            //string libraryName = "OrmLibrary";
            //OrmAssembly assembly = new OrmAssembly(
            //    libraryName, string.Format("{0}.dll", libraryName), AssemblyBuilderAccess.RunAndSave);
            //OrmType ormType = assembly.CreateOrmType("User", false);
            //ormType.CreateOrmProperty("UserName", typeof(string));
            //ormType.CreateOrmProperty("Password", typeof(string));
            //ormType.CreateOrmProperty("RoleId", typeof(int));
            //Type userType = ormType.CreateType();
            //object user = Activator.CreateInstance(userType);
            //EntityReader<object>.SetPropertyValue("UserName", user, "PaulK");
            //assembly.Save();
        }

        #endregion //Methods
    }
}
