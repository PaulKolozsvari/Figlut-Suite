namespace Figlut.Mobile.Toolkit.Utilities.Serialization
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;
    using System.IO;

    #endregion //Using Directives

    public class JSerializer : ISerializer
    {
        #region Constructors

        public JSerializer()
        {
            _includeOrmTypeNamesInJsonResponse = false;
        }

        public JSerializer(bool includeOrmTypeNamesInJsonResponse)
        {
            _includeOrmTypeNamesInJsonResponse = includeOrmTypeNamesInJsonResponse;
        }

        #endregion //Constructors

        #region Fields

        protected bool _includeOrmTypeNamesInJsonResponse;

        #endregion //Fields

        #region Properties

        public bool IncludeOrmTypeNamesInJsonResponse
        {
            get { return _includeOrmTypeNamesInJsonResponse; }
            set { _includeOrmTypeNamesInJsonResponse = value; }
        }

        #endregion //Properties

        #region Methods

        public void SerializeToFile(object obj, string filename)
        {
            SerializeToFile(obj, null, filename);
        }

        public void SerializeToFile(object obj, Type[] extraTypes, string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                string json = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
                writer.Write(json);
                writer.Flush();
            }
        }

        public string SerializeToText(object obj)
        {
            return SerializeToText(obj, null);
        }

        public string SerializeToText(object obj, Type[] extraTypes)
        {
            TypeNameHandling typeNameHandling = _includeOrmTypeNamesInJsonResponse ? TypeNameHandling.All : TypeNameHandling.None;
            return JsonConvert.SerializeObject(
                obj,
                Newtonsoft.Json.Formatting.Indented,
                new JsonSerializerSettings() { TypeNameHandling = typeNameHandling });
        }

        public object DeserializeFromText(Type type, string rawText)
        {
            return DeserializeFromText(type, null, rawText);
        }

        public object DeserializeFromText(Type type, Type[] extraTypes, string rawText)
        {
            return JsonConvert.DeserializeObject(rawText, type);
        }

        public object DeserializeFromFile(Type type, string filename)
        {
            return DeserializeFromFile(type, null, filename);
        }

        public object DeserializeFromFile(Type type, Type[] extraTypes, string filename)
        {
            FileSystemHelper.ValidateFileExists(filename);
            using (StreamReader reader = new StreamReader(filename))
            {
                string json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject(json, type);
            }
        }

        #endregion //Methods
    }
}