using System;
using System.Runtime.CompilerServices;

namespace BusinessLogic
{
    internal class BllLogger
    {

        /// <summary>
        /// It Logs exceptions and their additional information to console.
        /// You can add more information to the log message by passing additionalInfo parameter.
        /// If you want to log non-exception cases, you can pass null for the exception parameter.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="additionalInfo"></param>
        /// <param name="memberName"></param>
        /// <param name="filePath"></param>
        /// <param name="lineNumber"></param>
        public static void Log(
            Exception ex = null,
            string additionalInfo = "",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            string className = System.IO.Path.GetFileNameWithoutExtension(filePath);

            string logMessage = $"||BLL Error" +
                                $"||{DateTime.Now:yyyy-MM-dd HH:mm:ss} " +
                                $"||Class: {className}, " +
                                $"||Method: {memberName}, " +
                                $"||Line: {lineNumber}, " +
                                $"||Exception: {ex?.ToString() ?? "No exception"}, " +
                                $"||Additional Info: {additionalInfo}";

            // for now we log to console : (will log to file later on)
            Console.WriteLine(logMessage);
        }
    }
}
