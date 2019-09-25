using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalApp.Utility
{
    public class ExceptionLogger
    {
        private static string GetPathDir()
        {
            string path = ConfigurationManager.AppSettings["ExceptionLogPath"] as string;
            return path;
        }

        public static void LogToFileAsync(ExceptionBag obj)
        {
            string name = "Log_For_" + DateTime.Now.ToString("dd-MM-yyyy");
            string fileName = GetPathDir() + string.Format("{0}{1}", name, ".txt");

            if (File.Exists(fileName))
            {
                try
                {
                    using (StreamWriter sw = File.AppendText(fileName))
                    {
                        LogWriterAsync(sw, obj);
                    }
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }
            else
            {
                FileStream fs = File.Create(fileName);

                using (StreamWriter sw = new StreamWriter(fs))
                {
                    LogWriterAsync(sw, obj);
                }
            }
        }

        private static void LogWriterAsync(StreamWriter sw, ExceptionBag obj)
        {
            StringBuilder logLine = new StringBuilder();
            logLine.AppendLine("=================================================================================================================");
            logLine.AppendLine(Environment.NewLine);
            logLine.Append(String.Format("Executing Operation : {0}", obj.ExecutingOperation));
            logLine.AppendLine(Environment.NewLine);
            logLine.Append(String.Format("Message : {0}", obj.Message));
            logLine.AppendLine(Environment.NewLine);
            logLine.Append(String.Format("Inner Exception : {0}", obj.InnerException));
            logLine.AppendLine(Environment.NewLine);
            string date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            logLine.Append(String.Format("Time : {0}", date.Substring(10)));
            logLine.AppendLine(Environment.NewLine);
            logLine.AppendLine("=================================================================================================================");
            sw.WriteLineAsync(logLine.ToString());
        }
    }
}
