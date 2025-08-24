using System;
using System.Runtime.CompilerServices;

namespace BusinessLogic
{
    internal static class BllResultExtenstions
    {
        /// <summary>
        /// It Logs BLL result errors, successes, and additional information, in addition to exception if exist.
        /// You can add more information to the log message by passing additionalInfo parameter.
        /// If you want to log non-exception cases, you can pass null for the exception parameter.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="additionalInfo"></param>
        /// <param name="memberName"></param>
        /// <param name="filePath"></param>
        /// <param name="lineNumber"></param>

        public static BllResult<T> WithLogging<T>(
            this BllResult<T> result,
            string additionalInfo = "",
            Exception ex = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            BllLogger.Log(ex, result.Error, additionalInfo, memberName, filePath, lineNumber);
            return result;
        }
    }
}
