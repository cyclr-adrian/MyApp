using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace MyApp.Framework.Helpers.Script
{
    public static class CsvHelper
    {
        /// <summary>
        /// Gets records from the CSV reader.
        /// </summary>
        /// <param name="reader">Text reader for the CSV.</param>
        /// <param name="separatorString">Delimiter used to separate fields.</param>
        /// <param name="firstRowIsHeader">True if the CSV has a header record.</param>
        /// <param name="columnNames">List of column names.</param>
        /// <returns>All the records in the CSV.</returns>
        public static IList<dynamic> GetRecords(TextReader reader, string separatorString, bool firstRowIsHeader, IList<string> columnNames = null)
        {
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = separatorString,
                HasHeaderRecord = firstRowIsHeader,
                BadDataFound = c => { }, // Ignore bad data.
                MissingFieldFound = null // Ignore missing fields.
            };

            using (var csv = new CsvReader(reader, configuration))
            {
                // Get all fields if there's no header or column names.
                if (!configuration.HasHeaderRecord || columnNames == null || columnNames.Count < 1)
                {
                    var noHeaderOrColumnNamesRecords = csv.GetRecords<dynamic>().ToList();

                    return noHeaderOrColumnNamesRecords;
                }

                // Get records with the list of columns.
                var records = new List<dynamic>();
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    IDictionary<string, object> record = new ExpandoObject();

                    foreach (var c in columnNames)
                    {
                        record[c] = csv.GetField(c);
                    }

                    records.Add(record);
                }

                return records;
            }
        }
    }
}