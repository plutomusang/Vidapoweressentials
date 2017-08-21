using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;

namespace AutoFTP
{
    class Program
    {
        static string rootpath = "";
        static Object thisLock = new Object();
        static HashSet<string> jobs = new HashSet<string>();
        static System.Timers.Timer timer;
        static int ctrclear;
        static void Main(string[] args)
        {
            ctrclear = 0;
            string sPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            rootpath = sPath.Replace(@"\AutoFTP\bin\Debug\AutoFTP.exe", @"\Vida");
            //rootpath = rootpath.Replace("/", @"\");
            CreateFileWatcher(rootpath);
            StartTimer();
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
            watcher.Created += new FileSystemEventHandler(OnChanged);
            // Begin watching.
            watcher.EnableRaisingEvents = true;

        }

        public static string[] files(string Fname)
        {
            string[] fs = Fname.Split(new string[] {@"\"}, StringSplitOptions.None);
            return fs;
        }

        // Define the event handlers.
        private static PathStatus allowed(string fpath)
        {
            PathStatus pathStatus = new PathStatus();
            pathStatus.Allowed = false;
            fpath = fpath + "~";
            
            string[]farray = fpath.Split('~');
            fpath = farray[0];
            pathStatus.Path = fpath;

            bool fpathgood = false;
            if (fpath.IndexOf("BusinessLogic") > -1) fpathgood = true;
            if (fpath.IndexOf("API") > -1) fpathgood = true;
            if (fpath.IndexOf("Controllers") > -1) fpathgood = true;
            if (fpath.IndexOf("App_Data") > -1) fpathgood = true;
            if (fpath.IndexOf("App_Start") > -1) fpathgood = true;
            int count = (fpath.Length - fpath.Replace(@"\", "").Length) - 2;
            if (count > 0) fpathgood = true;

            if (fpathgood)
            {
                string ext = Path.GetExtension(fpath).ToLower();
                string allowedext = ".txt,.cshtml, .html,.txt,.dll,.jpg,.bmp,.png,.js,.css,.map";
                if (ext == "") ext = "empty";
                if (allowedext.IndexOf(ext) > -1)
                {
                    pathStatus.Allowed = true;
                }
            }




            return pathStatus;
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {

            (new Thread(() =>
            {
                try
                {
                    // Specify what is done when a file is changed, created, or deleted.


                    
                    
                    PathStatus pathStatus = allowed(e.FullPath);
                    if (pathStatus.Allowed)
                    {
                        jobs.Add(pathStatus.Path);
                    } 
 

                }
                catch
                {

                }


            })).Start();
            
            // Specify what is done when a file is changed, created, or deleted.
        }

        public static void StartTimer()
        {
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(OnElapsed);
            timer.Start();
        }

        
        private static void OnElapsed(object sender, ElapsedEventArgs e)
        {
            lock (thisLock)
            {
                if (jobs.Count > 0)
                {
                    string[] spath = jobs.ToArray();

                    if (File.Exists(spath[0]))
                    {
                        string remotepath = spath[0].Replace(rootpath + @"\", @"\wwwroot\");
                        Ftp ftpClient = new Ftp(@"ftp://vidapoweressentials.com/", "vida", "power");
                        ftpClient.upload(remotepath, spath[0]);
                        ftpClient = null;
                        try
                        {
                            Console.WriteLine("Uploaded " + jobs.Count.ToString() + " : " + remotepath);

                        }
                        catch (Exception ex)
                        {
                            string msg = ex.Message;

                        }
                        
                    }

                    jobs.Remove(spath[0]);
                    ctrclear = 0;
                }
                else
                {
                    ctrclear++;
                    if (ctrclear > 20)
                    {
                        try
                        {
                            Console.Clear();

                        }
                        catch (Exception ex)
                        {
                            string msg = ex.Message;

                        }

                        
                    }
                    

                }
            }


        }
    }

    class PathStatus
    {
        public string Path;
        public bool Allowed;
    }


}
