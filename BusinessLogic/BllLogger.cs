using System;
using System.Runtime.CompilerServices;

namespace BusinessLogic
{
    internal class BllLogger
    {

        /// <summary>
        /// It Logs exceptions, BLL errors, and their additional information to console.
        /// All parameters are default parameters, so you can pass null when value is not exist.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="additionalInfo"></param>
        /// <param name="memberName"></param>
        /// <param name="filePath"></param>
        /// <param name="lineNumber"></param>
        public static void Log(
            Exception ex = null,
            BllError? error = null,
            string additionalInfo = "",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            string className = System.IO.Path.GetFileNameWithoutExtension(filePath);

            string opStatus = error == null ? "Success" : "Failure";
            string logMessage = $"||BLL||, " + 
                                $"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}, " +
                                $"Operation {opStatus}, " +
                                $"Error: {error?.ToString() ?? "No Error"}, " +
                                $"In {className}.{memberName} at line {lineNumber}, " +
                                $"Additional Info: {additionalInfo}, " +
                                $"Exception: {ex?.Message ?? "No Exception"}";

            // old formatting:
            //string logMessage = $"||BLL Error" +
            //                    $"||{DateTime.Now:yyyy-MM-dd HH:mm:ss} " +
            //                    $"||Class: {className}, " +
            //                    $"||Method: {memberName}, " +
            //                    $"||Line: {lineNumber}, " +
            //                    $"||Exception: {ex?.ToString() ?? "No exception"}, " +
            //                    $"||Additional Info: {additionalInfo}";

            // for now we log to console : (will log to file later on)
            Console.WriteLine(logMessage);
        }
    }
}
