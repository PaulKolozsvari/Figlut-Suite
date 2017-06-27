namespace Figlut.Server.Toolkit.Utilities.Serialization
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;

    #endregion //Using Directives

    public interface ISerializer
    {
        #region Methods

        void SerializeToFile(object obj, string filename);

        void SerializeToFile(object obj, Type[] extraTypes, string filename);

        string SerializeToText(object obj);

        string SerializeToText(object obj, Type[] extraTypes);

        object DeserializeFromText(Type type, string text);

        object DeserializeFromText(Type type, Type[] extraTypes, string text);

        object DeserializeFromFile(Type type, string filename);

        object DeserializeFromFile(Type type, Type[] extraTypes, string filename);

        #endregion //Methods
    }
}