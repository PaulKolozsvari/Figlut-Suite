namespace Figlut.Server.TestClient
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net;
    using System.IO;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Web.Client;
    using Newtonsoft.Json;
    using Figlut.Server.Toolkit.Web;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Newtonsoft.Json.Linq;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Web.Client.FiglutWebService;
    using System.Data;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Data.DB;

    #endregion //Using Directives

    class Program
    {
        #region Fields

        private const string BASE_URI = "http://localhost:8889/Figlut";
        private static List<User> _users;

        #endregion //Fields

        #region Methods

        static void Main(string[] args)
        {
            try
            {
                ExecuteSql();
                ConnectionTest();
                _users = GetDummyUsers();
                GetSchema();
                GetTest();
                PostTest();
                PutTest();
                DeleteTest();
            }
            catch (System.Net.WebException wex)
            {
                Console.Error.WriteLine(wex.ToString());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }

        private static List<User> GetDummyUsers()
        {
            List<User> users = new List<User>()
            {
                new User(){
                    UserId = Guid.NewGuid(),
                    UserName = "TestUser",
                    Password = "my password",
                    DeviceId = "device123",
                    DateCreated = DateTime.Now},
                new User(){
                    UserId = Guid.NewGuid(),
                    UserName = "TestUser2",
                    Password = "my password2",
                    DeviceId = "device456",
                    DateCreated = DateTime.Now}
            };
            return users;
        }

        private static void ConnectionTest()
        {
            FiglutWebServiceClientWrapper client = new FiglutWebServiceClientWrapper(new JsonWebServiceClient("http://localhost:2983/Figlut"), 30000);
            client.ConnectionTest(true);
        }

        private static void ExecuteSql()
        {
            FiglutWebServiceClientWrapper client = new FiglutWebServiceClientWrapper(new JsonWebServiceClient("http://localhost:2983/Figlut"), 30000);
            string rawOutput = null;
            List<Test> result = client.ExecuteSqlQuery<List<Test>>("select * from Test", typeof(Test).Name, out rawOutput, true);
            int count = result.Count;

            string insertQuery = @"INSERT INTO [Test]
                                           ([TestId]
                                           ,[Notes])
                                     VALUES
                                           (1
                                           ,'these are some notes')";
            string insertResult = client.ExecuteSqlScript(insertQuery, true);
            Console.WriteLine(insertResult);


     //       INSERT INTO [Test]
     //      ([TestId]
     //      ,[Notes])
     //VALUES
     //      (1
     //      ,'these are some notes')
        }

        private static void PostTest()
        {
            //(new WebServiceClient("http://localhost:8889/Figlut")).CallService(
            //    string.Concat(typeof(User).Name, "/json"), //queryString (table name)
            //    users, //requestPostObject
            //    HttpVerb.POST, //verb
            //    out textOutput, //textOutput
            //    30000, //timeout (in milliseconds)
            //    GOC.Instance.GetSerializer(SerializerType.JSON), //serializer
            //    null, //postExtraSerializationTypes
            //    MimeContentType.TEXT_PLAIN, //postContentType
            //    null); //resultEntityType

            HttpStatusCode statusCode;
            string statusDescription;

            (new XmlWebServiceClient("http://localhost:8889/Figlut")).CallService<object>(
                typeof(User).Name,
                _users,
                HttpVerb.POST,
                true,
                30000,
                out statusCode,
                out statusDescription,
                true);

            //(new JsonWebServiceClient("http://localhost:8889/Figlut")).CallService<object>(
            //    typeof(User).Name,
            //    _users,
            //    HttpVerb.POST,
            //    30000);

            //(new CsvWebServiceClient("http://localhost:8889/Figlut")).CallService<object>(
            //    typeof(User).Name,
            //    _users,
            //    HttpVerb.POST,
            //    30000);

            //(new JsonWebServiceClient("http://localhost:8889/Figlut")).CallService<object>(
            //    string.Concat(typeof(User).Name, "/json"),
            //    _users,
            //    HttpVerb.POST,
            //    30000);

            Console.WriteLine("Insert done!");
            Console.ReadLine();
        }

        private static void GetSchema()
        {
            string rawOutput = null;
            FiglutWebServiceClientWrapper service = new FiglutWebServiceClientWrapper(new JsonWebServiceClient("http://localhost:8889/Figlut"), 30000);
            SqlDatabase db = service.GetSqlDatabase(true, true, Environment.CurrentDirectory, true);
        }

        private static void GetTest()
        {
            string textOutput = null;
            //List<User> users = (List<User>)(new WebServiceClient("http://localhost:8889/Figlut")).CallService(
            //    typeof(User).Name,
            //    null,
            //    HttpVerb.GET,
            //    out textOutput,
            //    30000,
            //    GOC.Instance.GetSerializer(SerializerType.XML),
            //    null,
            //    MimeContentType.TEXT_PLAIN,
            //    typeof(List<User>));


            HttpStatusCode statusCode;
            string statusDescription;

            List<User> usersXml = new XmlWebServiceClient("http://localhost:8889/Figlut").CallService<List<User>>(
                typeof(User).Name,
                null,
                HttpVerb.GET,
                out textOutput,
                false,
                true,
                30000,
                out statusCode,
                out statusDescription,
                true);

            List<User> usersJson = new JsonWebServiceClient(BASE_URI).CallService<List<User>>(
                typeof(User).Name,
                null,
                HttpVerb.GET,
                out textOutput,
                false,
                true,
                30000,
                out statusCode,
                out statusDescription,
                true);

            List<User> usersCsv = new CsvWebServiceClient(BASE_URI).CallService<List<User>>(
                typeof(User).Name,
                null,
                HttpVerb.GET,
                out textOutput,
                false,
                true,
                30000,
                out statusCode,
                out statusDescription,
                true);

            Console.WriteLine("Response: {0}", textOutput);
            Console.WriteLine("Query done!");
            Console.ReadLine();
        }

        private static void PutTest()
        {
            for(int i = 0; i < _users.Count; i++)
            {
                User u = _users[i];
                u.UserName += string.Format(" updated {0}", i);
                u.Password += string.Format(" updated {0}", i);
                u.DeviceId += string.Format(" updated {0}", i);
                u.DateCreated = DateTime.Now;
            }
            string columnName = EntityReader<User>.GetPropertyName(p => p.UserName, false);

            HttpStatusCode statusCode;
            string statusDescription;

            (new XmlWebServiceClient(BASE_URI)).CallService<object>(
                string.Format("{0}/{1}", typeof(User).Name, columnName),
                _users,
                HttpVerb.PUT,
                true,
                30000,
                out statusCode,
                out statusDescription,
                true);
            Console.WriteLine("Xml update done!");

            //(new JsonWebServiceClient(BASE_URI)).CallService<object>(
            //    string.Format("{0}/{1}", typeof(User).Name, columnName),
            //    _users,
            //    HttpVerb.PUT,
            //    30000);
            //Console.WriteLine("Json update done!");

            //(new CsvWebServiceClient(BASE_URI)).CallService<object>(
            //string.Format("{0}/{1}", typeof(User).Name, columnName),
            //    _users,
            //    HttpVerb.PUT,
            //    30000);
            //Console.WriteLine("Csv update done!");
            Console.ReadLine();
        }

        private static void DeleteTest()
        {
            HttpStatusCode statusCode;
            string statusDescription;

            string columnName = EntityReader<User>.GetPropertyName(p => p.UserName, false);
            (new XmlWebServiceClient(BASE_URI)).CallService<object>(
                string.Format("{0}/{1}", typeof(User).Name, columnName),
                _users,
                HttpVerb.DELETE,
                true,
                30000,
                out statusCode,
                out statusDescription,
                true);

            //(new JsonWebServiceClient(BASE_URI)).CallService<object>(
            //    string.Format("{0}/{1}", typeof(User).Name, columnName),
            //    _users,
            //    HttpVerb.DELETE,
            //    30000);

            //(new CsvWebServiceClient(BASE_URI)).CallService<object>(
            //    string.Format("{0}/{1}", typeof(User).Name, columnName),
            //    _users,
            //    HttpVerb.DELETE,
            //    30000);

            Console.WriteLine("Delete done!");
            Console.ReadLine();
        }

        #endregion //Methods
    }
}
