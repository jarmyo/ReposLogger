using System;
using System.Diagnostics;
using System.IO;

namespace Repos.Log
{
    public static class Logger
    {
        // Cambie estos valores para su aplicación
        public static readonly string ReposFolder = @"c:\Repos\logs\";
        public static readonly string LogPrefix = "repos";

        private static Stopwatch timer;
        private static bool isStartTrivial = false;
        private static bool isStartDataUpdate = false;
        private static bool isStartConnection = false; 

        public static void Write(string mensaje)
        {
            Debug.WriteLine(mensaje);
            try
            {
                using (StreamWriter w = File.AppendText(ReposFolder + LogPrefix + @".log.txt"))
                {
                    w.WriteLine(DateTime.Now.ToString(@"yy-MM-ddtHH\:mm") + ": " + mensaje);
                }
            }
            catch
            {
                // No se pudo escribir en el log
            }
        }
        public static void WriteDataUpdate(string mensaje)
        {
            WriteDataUpdate(mensaje, true);
        }
        public static void WriteDataUpdate(string mensaje, bool hour)
        {
            if (!isStartDataUpdate)
            {
                isStartDataUpdate = true;
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
            }
            catch
            {
                // No se pudo escribir en el log
            }
        }
        public static void WriteConnection(string mensaje)
        {
            WriteConnection(mensaje, true);
        }
        public static void WriteConnection(string mensaje, bool hour)
        {
            if (!isStartConnection)
            {
                isStartConnection = true;
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
            }
            catch
            {
                // No se pudo escribir en el log
            }
        }
        public static void Trivial(string mensaje)
        {
            Trivial(mensaje, true);
        }
        public static void Trivial(string mensaje, bool hour)
        {
            if (!isStartTrivial)
            {
                var dir = new DirectoryInfo(ReposFolder);
                if (!dir.Exists)
                {
                    dir.Create();
                }

                timer = Stopwatch.StartNew();
                Debug.Indent();
                if (File.Exists(ReposFolder + LogPrefix + @"Trivial.log.txt"))
                {
                    File.Delete(ReposFolder + LogPrefix + @"Trivial.log.txt");
                }

                isStartTrivial = true;
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
            }
            catch
            {
            }
        }
        private static string DateHeader => "=-=-=-=- [ " + DateTime.Now.ToString(@"yy-MM-dd") + " ] =-=-=-=-";
        public static bool IsStartConnection { get => isStartConnection; set => isStartConnection = value; }        
        public static bool IsStartDataUpdate { get => isStartDataUpdate; set => isStartDataUpdate = value; }
        public static bool IsStartTrivial { get => isStartTrivial; set => isStartTrivial = value; }
    }
}