namespace Figlut.Server.Toolkit.Data.CSV
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Data;

    #endregion //Using Directives

    public class CsvWriter
    {
        #region Methods

        public static void WriteToFile(DataTable table, bool header, bool quoteball, string file, string footer)
        {
            using (StreamWriter writer = new StreamWriter(file, false))
            {
                writer.Write(WriteToString(table, header, quoteball, footer));
            }
        }

        public static string WriteToString(DataTable table, bool header, bool quoteall, string footer)
        {
            using (StringWriter writer = new StringWriter())
            {
                WriteToStream(writer, table, header, quoteall, footer);
                return writer.ToString();
            }
        }

        public static void WriteToStream(TextWriter stream, DataTable table, bool header, bool quoteall, string footer)
        {
            if (header)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    WriteItem(stream, table.Columns[i].Caption, quoteall);
                    if (i < table.Columns.Count - 1)
                    {
                        stream.Write(',');
                    }
                    else
                    {
                        stream.Write('\n');
                    }
                }
            }
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    WriteItem(stream, row[i], quoteall);
                    if (i < table.Columns.Count - 1)
                    {
                        stream.Write(',');
                    }
                    else
                    {
                        stream.Write("\n");
                    }
                }
            }
            if (!string.IsNullOrEmpty(footer))
            {
                stream.WriteLine();
                stream.WriteLine(footer);
            }
        }

        private static void WriteItem(TextWriter stream, object item, bool quoteall)
        {
            if (item == null)
            {
                return;
            }
            string s = item.ToString();
            if (quoteall || s.IndexOfAny("\",\x0A\x0D".ToCharArray()) > -1)
            {
                stream.Write("\"" + s.Replace("\"", "\"\"") + "\"");
            }
            else
            {
                stream.Write(s);
            }
        }

        public static string GetCsvStringFromValues(List<object> values, bool quoteall, string endOfLineTerminator)
        {
            if (values == null)
            {
                return string.Empty;
            }
            int valuesCount = values.Count;
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < valuesCount; i++)
            {
                string s = values[i].ToString();
                if (quoteall || s.IndexOfAny("\",\x0A\x0D".ToCharArray()) > -1)
                {
                    result.Append("\"" + s.Replace("\"", "\"\"") + "\"");
                }
                else
                {
                    result.Append(s);
                }
                if (i < (valuesCount - 1))
                {
                    result.Append(',');
                }
                else if(!string.IsNullOrEmpty(endOfLineTerminator))
                {
                    result.Append(endOfLineTerminator);
                }
            }
            return result.ToString();
        }

        public static string WriteToStringFromEntities(
            List<object> entities,
            Type entityType,
            bool header,
            bool quoteall,
            string footer)
        {
            DataTable table = DataHelper.GetDataTableFromEntities(entities, entityType);
            return CsvWriter.WriteToString(table, header, quoteall, footer);
        }

        public static string WriteToStringFromEntities<E>(
            List<E> entities,
            bool header,
            bool quoteall,
            string footer) where E : class
        {
            DataTable table = DataHelper.GetDataTableFromEntities<E>(entities);
            return CsvWriter.WriteToString(table, header, quoteall, footer);
        }

        public static void WriteToFileFromEntities(
            List<object> entities,
            Type entityType,
            bool header,
            bool quoteall,
            string file,
            string footer)
        {
            DataTable table = DataHelper.GetDataTableFromEntities(entities, entityType);
            CsvWriter.WriteToFile(table, header, quoteall, file, footer);
        }

        public static void WriteToFileFromEntities<E>(
            List<E> entities,
            bool header,
            bool quoteall,
            string file,
            string footer) where E : class
        {
            DataTable table = DataHelper.GetDataTableFromEntities<E>(entities);
            CsvWriter.WriteToFile(table, header, quoteall, file, footer);
        }
        
        #endregion //Methods
    }
}
