namespace Figlut.Mobile.Toolkit.Data.CSV
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Data;
    using System.Collections;
    using Figlut.Mobile.Toolkit.Utilities;

    #endregion //Using Directives

    public class CsvParser
    {
        #region Methods

        public static DataTable ParseFromFile(string file, bool headers, int columnCount)
        {
            FileSystemHelper.ValidateFileExists(file);
            StringBuilder fileContents = null;
            using (StreamReader reader = new StreamReader(file))
            {
                fileContents = new StringBuilder(reader.ReadToEnd());
                reader.Close();
            }
            return Parse(fileContents.ToString(), headers, columnCount);
        }

        public static DataTable Parse(string data, bool headers, int columnCount)
        {
            return Parse(new StringReader(data), headers, columnCount);
        }

        public static DataTable Parse(string data, int columnCount)
        {
            return Parse(new StringReader(data), columnCount);
        }

        public static DataTable Parse(TextReader stream, int columnCount)
        {
            return Parse(stream, false, columnCount);
        }

        public static DataTable Parse(TextReader stream, bool headers, int columnCount)
        {
            DataTable table = new DataTable();
            CsvStream csv = new CsvStream(stream);
            string[] row = csv.GetNextRow();
            if (row == null)
                return null;
            if (headers)
            {
                foreach (string header in row)
                {
                    if (header != null && header.Length > 0 && !table.Columns.Contains(header))
                        table.Columns.Add(header, typeof(string));
                    else
                        table.Columns.Add(GetNextColumnHeader(table), typeof(string));
                }
                row = csv.GetNextRow();
            }
            while ((row != null) && (row.Length == columnCount))
            {
                while (row.Length > table.Columns.Count)
                {
                    table.Columns.Add(GetNextColumnHeader(table), typeof(string));
                }
                table.Rows.Add(row);
                row = csv.GetNextRow();
            }
            return table;
        }

        private static string GetNextColumnHeader(DataTable table)
        {
            int c = 1;
            while (true)
            {
                string h = "Column" + c++;
                if (!table.Columns.Contains(h))
                    return h;
            }
        }

        public static List<object> ParseEntitiesFromString(string data, bool headers, Type entityType)
        {
            int columnCount = entityType.GetProperties().Length;
            DataTable table = Parse(data, headers, columnCount);
            return DataHelper.GetEntitiesFromDataTable(table, entityType);
        }

        public static List<E> ParseEntitiesFromString<E>(string data, bool headers) where E : class
        {
            Type entityType = typeof(E);
            int columnCount = entityType.GetProperties().Length;
            DataTable table = Parse(data, headers, columnCount);
            return DataHelper.GetEntitiesFromDataTable<E>(table);
        }

        public static List<object> ParseEntitiesFromFile(string file, bool headers, Type entityType)
        {
            int columnCount = entityType.GetProperties().Length;
            DataTable table = ParseFromFile(file, headers, columnCount);
            return DataHelper.GetEntitiesFromDataTable(table, entityType);
        }

        public static List<E> ParseEntitiesFromFile<E>(string file, bool headers) where E : class
        {
            Type entityType = typeof(E);
            int columnCount = entityType.GetProperties().Length;
            DataTable table = ParseFromFile(file, headers, columnCount);
            return DataHelper.GetEntitiesFromDataTable<E>(table);
        }

        #endregion //Methods
    }
}