using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AutoFTP
{
    class Program
    {
        static string rootpath = "";
        static Object thisLock = new Object();

        static void Main(string[] args)
        {

            string sPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            rootpath = sPath.Replace(@"\AutoFTP\bin\Debug\AutoFTP.exe", @"\Vida");
            //rootpath = rootpath.Replace("/", @"\");
            CreateFileWatcher(rootpath);
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

        }

        public static void CreateFileWatcher(string path)
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            /* Watch for changes in LastAccess and LastWrite times, and 
               the renaming of files or directories. */

            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*";
            watcher.IncludeSubdirectories = true;
            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;

        }
        public static string[] files(string Fname)
        {
            string[] fs = Fname.Split(new string[] { @"\" }, StringSplitOptions.None);
            return fs;
        }
        // Define the event handlers.
        private static bool allowed(string fpath)
        {
            
            if (fpath.IndexOf("BusinessLogic") > -1) return true;
            if (fpath.IndexOf("API") > -1) return true;
            if (fpath.IndexOf("Controllers") > -1) return true;
            if (fpath.IndexOf("App_Data") > -1) return true;
            if (fpath.IndexOf("App_Start") > -1) return true;
            int count = (fpath.Length - fpath.Replace(@"\", "").Length) - 2;
            if (count > 0) return true;

            return false;
        }
        private static void OnChanged(object source, FileSystemEventArgs e)
        {

            (new Thread(() =>
            {
                try
                {
                    // Specify what is done when a file is changed, created, or deleted.
                    
                    
                    string remotepath = e.FullPath.Replace(rootpath + @"\", @"\wwwroot\");

                    if (allowed(remotepath))
                    {
                        lock (thisLock)
                        {
                            Console.WriteLine("File: " + e.FullPath);
                            Ftp ftpClient = new Ftp(@"ftp://vidapoweressentials.com/", "vida", "power");
                            ftpClient.upload(remotepath, e.FullPath);
                            ftpClient = null;
                        }



                    } 
 

                }
                catch
                {

                }


            })).Start();
            
            // Specify what is done when a file is changed, created, or deleted.
        }
    }

}
