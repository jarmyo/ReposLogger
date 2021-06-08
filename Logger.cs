using System;
using System.Diagnostics;
using System.IO;

namespace Repos.Log
{
    public static class Logger
    {
        // Cambie estos valores para su aplicación
        public static readonly string ReposFolder = @"c:\Repos\logs\";
        public static string LogPrefix
        {
            get;
            set;
        } = "repos";

        private static Stopwatch timer;
        private static bool isStartTrivial;
        private static bool isStartDataUpdate;
        private static bool isStartConnection;

        private static void Writer(string file, string mensaje, ref bool firstRun, bool destroy)
        {
            string head = string.Empty;
            if (!firstRun)
            {
                var dir = new DirectoryInfo(ReposFolder);
                if (!dir.Exists)
                {
                    dir.Create();
                }

                if (destroy)
                {
                    if (File.Exists(ReposFolder + LogPrefix + @"Trivial.log.txt"))
                    {
                        File.Delete(ReposFolder + LogPrefix + @"Trivial.log.txt");
                    }
                }

                firstRun = true;
                head = "=-=-=-=- [ " + DateTime.Now.ToString(@"yy-MM-dd") + " ] =-=-=-=-" + Environment.NewLine;
            }

            Debug.WriteLine(mensaje);
            try
            {
                using (StreamWriter w = File.AppendText(ReposFolder + LogPrefix + file + @".log.txt"))
                {
                    w.WriteLine(head + mensaje);
                }
            }
            catch
            {
                // No se pudo escribir en el log
            }
        }

        public static void Write(string mensaje)
        {
            bool x = true;
            var mth = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Name;
            Writer("", DateTime.Now.ToString(@"yy-MM-ddtHH\:mm") + " (" + mth + ")\t\t" + mensaje, ref x, false);
        }
        public static void WriteDataUpdate(string mensaje)
        {
            Writer("DataUpdate", DateTime.Now.ToString(@"HH\:mm") + ":" + mensaje, ref isStartDataUpdate, false);
        }
        public static void WriteConnection(string mensaje)
        {
            Writer("Sync", DateTime.Now.ToString(@"HH\:mm") + ":" + mensaje, ref isStartConnection, false);
        }
        public static void Trivial(string mensaje)
        {
            var frame = new StackTrace().GetFrame(1).GetMethod();
            if (!isStartTrivial)
            {
                timer = Stopwatch.StartNew();
                Debug.Indent();                
            }
            var s = timer.Elapsed.ToString() + " (" + frame + ")\t\t" + mensaje;
            Writer("Trivial", s, ref isStartTrivial, true);
        }
    }
}