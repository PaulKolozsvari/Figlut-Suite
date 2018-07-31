namespace Figlut.Server.Toolkit.Utilities.Serialization
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using System.IO;
    using System.Xml;

    #endregion //Using Directives

    public class XSerializer : ISerializer
    {
        #region Methods

        /// <summary>
        /// Serializes an object to an XML file. No extra/derived types will be provided
        /// to the underlying serializer.
        /// </summary>
        /// <param name="obj">The object to serialized.</param>
        /// <param name="filename">The the file path of the XML file to which object will be serialized to.</param>
        public void SerializeToFile(object obj, string filename)
        {
            SerializeToFile(obj, null, filename);
        }

        /// <summary>
        /// Serializes an object to an XML file. All derived types should be included in the call
        /// for serialization to complete succcessfully.
        /// </summary>
        /// <param name="obj">The object to serialized.</param>
        /// <param name="extraTypes">The derived types to be provided to the underlying serializer.</param>
        /// <param name="filename">The the file path of the XML file to which object will be serialized to.</param>
        public void SerializeToFile(object obj, Type[] extraTypes, string filename)
        {
            XmlSerializer serializer = extraTypes == null ?
                new XmlSerializer(obj.GetType()) :
                new XmlSerializer(obj.GetType(), extraTypes);
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            using (XmlTextWriter writer = new XmlTextWriter(filename, System.Text.Encoding.UTF8))
            {
                using (Stream baseStream = writer.BaseStream)
                {
                    writer.Formatting = Formatting.Indented;
                    //writer.IndentChar = '\x09'; //Tab character
                    //writer.Indentation = 1;
                    writer.Indentation = 2;
                    writer.IndentChar = ' ';
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance&quot;");
                    serializer.Serialize(writer, obj, ns);
                    writer.Close();
                }
            }
        }

        public string SerializeToText(object obj, Type[] extraTypes)
        {
            XmlSerializer serializer = extraTypes == null ?
                new XmlSerializer(obj.GetType()) :
                new XmlSerializer(obj.GetType(), extraTypes);
            StringBuilder result = new StringBuilder();
            using (TextWriter writer = new StringWriter(result))
            {
                serializer.Serialize(writer, obj);
            }
            return result.ToString();
        }

        public string SerializeToText(object obj)
        {
            return SerializeToText(obj, null);
        }

        public object DeserializeFromText(Type type, string xmlString)
        {
            return DeserializeFromText(type, null, xmlString);
        }

        public object DeserializeFromText(Type type, Type[] extraTypes, string xmlString)
        {
            XmlSerializer serializer = extraTypes ==
                null ?
                new XmlSerializer(type) :
                new XmlSerializer(type, extraTypes);
            using (TextReader reader = new StringReader(xmlString))
            {
                return serializer.Deserialize(reader);
            }
        }

        public object DeserializeFromFile(Type type, string filename)
        {
            return DeserializeFromFile(type, null, filename);
        }

        public object DeserializeFromFile(Type type, Type[] extraTypes, string filename)
        {
            XmlSerializer serializer = extraTypes ==
                null ?
                new XmlSerializer(type) :
                new XmlSerializer(type, extraTypes);
            FileSystemHelper.ValidateFileExists(filename);
            using (XmlReader reader = XmlReader.Create(filename))
            {
                return serializer.Deserialize(reader);
            }
        }

        #endregion //Methods
    }
}
