using System.IO;
using System;

namespace WebApplication.Sample
{
    public class Common
    {
        public static void WriteToFile(string message)
        {
            string assemblyName = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;            
            string filePath = "C:\\QBLogs\\" + "WebService-Logs-File" + ".log";
            Console.WriteLine(message);
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(message);
                sw.Close();
            }
        }

    }
}
