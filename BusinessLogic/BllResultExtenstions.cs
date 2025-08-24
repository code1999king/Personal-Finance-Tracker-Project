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
            // Control the log message based on result status :
            if(result.IsSuccess)
                BllLogger.Log(null, $"Success, {additionalInfo}", memberName, filePath, lineNumber);
            else
                BllLogger.Log(ex, $"Failure = {result.Error.ToString()}, {additionalInfo}", memberName, filePath, lineNumber);
            return result;
        }
    }
}
