using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JDPDO.SpaceBreeze
{
    public static class ExceptionHandler
    {
        /// <summary>
        /// Logs a thrown exception to standard error stream.
        /// </summary>
        /// <param name="exception">Exception to log.</param>
        public static void LogException(Exception exception)
        {
            string output = exception.GetType().ToString() + " in " + exception.Source + ". Message:" + exception.Message;
            Console.Error.WriteLine(output);
        }

        /// <summary>
        /// Logs a thrown exception to standard error stream.
        /// </summary>
        /// <param name="message">Additonal message for additional error handling.</param>
        /// <param name="innerException">Exception to log.</param>
        public static void LogException(string message, Exception innerException)
        {
            Exception outerException = new Exception(message, innerException);
            LogException(outerException);
        }
    }
}
