using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DurakLibrary.Common
{
    public class Logger : TextWriter
    {
        public static readonly string LogTimeFormat = "HH:mm:ss";
        public static readonly string LogFileTimeFormat = "yyyy-MM-dd_HH_mm";
        public static Logger Singleton { get; private set; }

        static Logger()
        {
            Singleton = new Logger();
        }
        
        public Logger()
        {
            var block = true;
            var index = 0;

            while (block)
            {
                try
                {
                    var fileName = "log_" + DateTime.Now.ToString(LogFileTimeFormat) + (index > 0 ? "(" + index + ")" : "") + ".txt";
                    stream = new StreamWriter(fileName);
                    block = false;
                }
                catch (IOException) { index++; }
            }
        }

        public static void Write(Exception e)
        {
            Write("Encounted {0} at:", e.GetType().Name);
            var trace = new StackTrace(e, true);

            foreach (StackFrame frame in trace.GetFrames())
                Write("\t{0}.{1} - line {2}", frame.GetMethod().DeclaringType, frame.GetMethod().Name, frame.GetFileLineNumber());
            
        }
        
        new public static void Write(string rawText)
        {
            Singleton.WriteLine(rawText);
        }

        new public static void Write(string format, params object[] parameters)
        {
            Singleton.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToString(LogTimeFormat), string.Format(format, parameters)));
        }
        
        public override Encoding Encoding => Encoding.ASCII;

        private StreamWriter stream;

        new private void WriteLine(string line)
        {
            stream.WriteLine(line);
            Singleton.stream.Flush();
        }
    }
}
