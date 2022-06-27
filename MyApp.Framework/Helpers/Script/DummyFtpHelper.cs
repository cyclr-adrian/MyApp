using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MyApp.Core;

namespace MyApp.Framework.Helpers.Script
{
    public class DummyFtpHelper
    {
        public static IList<dynamic> DownloadCsv(string fileName, string separatorString, string firstRowIsHeaderString, string encodingName, object[] columnNames)
        {
            try
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                var firstRowIsHeader = bool.Parse(firstRowIsHeaderString);
                var result = DownloadText(filePath, separatorString, firstRowIsHeader, encodingName, columnNames);

                return result;
            }
            catch (Exception ex)
            {
                var (details, _) = CommonHelper.GetExceptionDetails(ex);
                throw new MyAppException($"Failed to download CSV from FTP: {details}", ex);
            }
        }

        private static IList<dynamic> DownloadText(string filePath, string separatorString, bool firstRowIsHeader, string encodingName, object[] columnNames)
        {
            Encoding encoding = null;
            IList<string> columns = null;

            if (!string.IsNullOrWhiteSpace(encodingName))
            {
                encoding = Encoding.GetEncoding(encodingName);
            }

            if (columnNames != null)
            {
                columns = columnNames.Cast<string>().ToList();
            }

            var reader = encoding == null ? new StreamReader(filePath) : new StreamReader(filePath, encoding);
            var records = CsvHelper.GetRecords(reader, separatorString, firstRowIsHeader, columns);

            return records;
        }
    }
}
