using System;

namespace ADJ.Common
{
    public class AppException : Exception
    {
        public AppException(Exception ex, string message, params object[] placeholders)
            : base(GetMessage(message, placeholders), ex)
        {
        }

        public AppException(string message, params object[] placeholders)
            : base(GetMessage(message, placeholders))
        {
        }

        private static string GetMessage(string message, object[] placeholders)
        {
            if (placeholders == null || placeholders.Length == 0)
            {
                return message;
            }

            return string.Format(message, placeholders);
        }

        public bool IsDbConcurrencyUpdate { get; set; }

        public static string GetTrueExceptionMessage(Exception ex)
        {
            var message = string.Empty;
            if (ex != null)
            {
                message = ex.Message;
                if (ex.InnerException == null)
                {
                    // No inner exception so try to parse this exception
                    //if (ex is SqlException)
                    //{
                    //    message = GetSqlExceptionMessage(ex);
                    //}
                }
                else if (ex.Message.Contains("See the inner exception for details"))
                {
                    // Get inner exception's message
                    message = GetTrueExceptionMessage(ex.InnerException);
                }
            }

            return message;
        }
    }
}
