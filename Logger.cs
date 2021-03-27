using System;
using System.Diagnostics;
using System.IO;

namespace Repos.Log
{
    public class Logger
    {
        // Cambie estos valores para su aplicación
        public const string ReposFolder = @"c:\Repos\logs\";
        public static string LogPrefix = "repos";
        
        
        public static Stopwatch timer;
        public static bool IsStartConnection = false;
        public static bool IsStartTrivial = false;
        public static bool IsStartDataUpdate = false;

        public static void Write(string mensaje)
        {
            Debug.WriteLine(mensaje);
            try
            {
                using (StreamWriter w = File.AppendText(ReposFolder + LogPrefix + @".log.txt"))
                {
                    w.WriteLine(DateTime.Now.ToString(@"yy-MM-ddtHH\:mm") + ": " + mensaje);
                }
            } catch
            {
            }
        }

        public static void WriteDataUpdate(string mensaje, bool hour = true)
        {
            if (!IsStartDataUpdate)
            {
                IsStartDataUpdate = true;
                WriteDataUpdate(DateHeader, false);
            }

            Debug.WriteLine(mensaje);
            try
            {
                using (StreamWriter w = File.AppendText(ReposFolder + LogPrefix + @"DataUpdate.log.txt"))
                {
                    string h = hour ? DateTime.Now.ToString(@"HH\:mm") + ":" : string.Empty;
                    w.WriteLine(h + mensaje);
                }
            } catch
            {
            }
        }

        public static void WriteConnection(string mensaje, bool hour = true)
        {
            if (!IsStartConnection)
            {
                IsStartConnection = true;
                WriteConnection(DateHeader, false);
            }

            Debug.WriteLine(mensaje);
            try
            {
                using (StreamWriter w = File.AppendText(ReposFolder + LogPrefix + @"Sync.log.txt"))
                {
                    string h = hour ? DateTime.Now.ToString(@"HH\:mm") + ":" : string.Empty;
                    w.WriteLine(h + mensaje);
                }
            } catch
            {
            }
        }

        public static void Trivial(string mensaje, bool hour = true)
        {
            if (!IsStartTrivial)
            {
                var dir = new DirectoryInfo(ReposFolder);
                if (!dir.Exists)
                    dir.Create();

                timer = Stopwatch.StartNew();
                Debug.Indent();
                if (File.Exists(ReposFolder + LogPrefix + @"Trivial.log.txt"))
                    File.Delete(ReposFolder + LogPrefix + @"Trivial.log.txt");
                IsStartTrivial = true;
                Trivial(DateHeader, false);
            }

            var mth = new StackTrace().GetFrame(1).GetMethod();
            var _prefix = hour ? timer.Elapsed.ToString() + " (" + mth.ReflectedType.Name + ")\t\t" : string.Empty;
            var s = _prefix + mensaje;

            Debug.WriteLine(s);
            try
            {
                using (StreamWriter w = File.AppendText(Logger.ReposFolder + LogPrefix + @"Trivial.log.txt"))
                {
                    w.WriteLine(s);
                }
            } catch
            {
            }
        }

        private static string DateHeader
        {
            get
            {
                return "=-=-=-=- [ " + DateTime.Now.ToString(@"yy-MM-dd") + " ] =-=-=-=-";
            }
        }
    }
}
