using System;

namespace CNC_CAD.Tools
{
    public class Logger
    {
        private Type loggerClass;

        private Logger(Type type)
        {
            loggerClass = type;
        }

        public static Logger CreateFor(object obj)
        {
            return CreateForClass(obj.GetType());
        }
        public static Logger CreateForClass(Type type)
        {
            return new Logger(type);
        }
        
        public void Log(string message)
        {
            string dateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            Console.WriteLine($"[{dateTime}]{loggerClass.Name}:{message}");
        }
    }
}