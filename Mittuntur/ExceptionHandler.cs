using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JDPDO.Mittuntur
{
    internal static class ExeptionHandler
    {
        public static void NewException(Exception exception)
        {
            string output = exception.GetType().ToString() + " in " + exception.Source + ". Message:" + exception.Message;
            Console.Error.WriteLine(output);
        }

        public static void NewArgumentNullExeption(string paramName, string message) => NewException(new ArgumentNullException(paramName, message));
    }
}
