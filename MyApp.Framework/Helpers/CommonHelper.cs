using System;
using System.Net;
using MyApp.Core;

namespace MyApp.Framework.Helpers
{
    public class CommonHelper
    {
        /// <summary>
        /// Get exception details and stack trace.
        /// </summary>
        /// <param name="ex">Exception.</param>
        /// <returns>Tuple contains exception details and stacktrace.</returns>
        public static Tuple<string, string> GetExceptionDetails(Exception ex)
        {
            var details = ex.Message;
            var stackTrace = ex.StackTrace;

            if (ex is WebException webException)
            {
                var stream = webException.Response?.GetResponseStream();

                if (stream != null)
                {
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        details = reader.ReadToEnd();

                        if (string.IsNullOrWhiteSpace(details))
                        {
                            details = ex.Message;
                        }
                    }
                }
            }
            else if (ex is AggregateException aggregateException)
            {
                aggregateException.Handle(x =>
               {
                   stackTrace = x.StackTrace + Environment.NewLine + stackTrace;
                   details += Environment.NewLine + x.Message;

                   if (x is MyAppException exception)
                   {
                       details += Environment.NewLine + exception.FullMessage;
                   }

                   return true;
               });
            }
            else if (ex is MyAppException appException)
            {
                details += Environment.NewLine + appException.FullMessage;
            }
            else
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    stackTrace = ex.StackTrace + Environment.NewLine + stackTrace;
                    details += ex is MyAppException exception
                        ? Environment.NewLine + exception.FullMessage
                        : Environment.NewLine + ex.Message;
                }
            }

            return new Tuple<string, string>(details, stackTrace);
        }
    }
}
