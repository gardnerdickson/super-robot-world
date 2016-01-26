using System;
using System.IO;

namespace RobotGame.Engine
{
    public static class Logging
    {
        private enum Severity
        {
            DEBUG,
            WARNING,
            ERROR
        }

        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private static string file;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public static void Init(string errorLog)
        {
            file = errorLog;
        }

        public static void LogDebug(string message, Type clazz)
        {
            Write(Severity.DEBUG, message, clazz);
        }

        public static void LogDebug(string message, Type clazz, string stackTrace)
        {
            Write(Severity.DEBUG, message, clazz, stackTrace);
        }

        public static void LogWarning(string message, Type clazz)
        {
            Write(Severity.WARNING, message, clazz);
        }

        public static void LogWarning(string message, Type clazz, string stackTrace)
        {
            Write(Severity.WARNING, message, clazz, stackTrace);
        }
        
        public static void LogError(string message, Type clazz)
        {
            Write(Severity.ERROR, message, clazz);
        }

        public static void LogError(string message, Type clazz, string stackTrace)
        {
            Write(Severity.ERROR, message, clazz, stackTrace);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private static StreamWriter GetStreamWriter()
        {
            return new StreamWriter(file, true);
        }

        private static void Write(Severity severity, string message, Type clazz)
        {
            StreamWriter writer = GetStreamWriter();
            LogMessage(writer, severity, message, clazz);
            writer.WriteLine();
            writer.Close();
        }

        private static void Write(Severity severity, string message, Type clazz, string stackTrace)
        {
            StreamWriter writer = GetStreamWriter();
            LogMessage(writer, severity, message, clazz);
            LogStackTrace(writer, stackTrace);
            writer.WriteLine();
            writer.Close();
        }

        private static void LogMessage(StreamWriter writer, Severity severity, string message, Type clazz)
        {
            writer.WriteLine(String.Format("[{0}] [{1}] [{2}.{3}] - {4}", System.DateTime.Now.ToString(), severity, clazz.Namespace, clazz.Name, message));
        }

        private static void LogStackTrace(StreamWriter writer, string stackTrace)
        {
            writer.WriteLine(stackTrace);
        }
    }
}
