using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.ServiceProcess;
using System.Text;
using NLog;
using System.Threading;

namespace FileWatcherService
{
    public partial class FileWatcherService : ServiceBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private string basedir="", filename="";
        public FileWatcherService()
        {
            InitializeComponent();
        }
        
        protected override void OnStart(string[] args)
        {
            try
            {
                if (args.Length != 2)
                {
                    basedir = "D:/";
                    filename = "liza.txt";
                }
                else
                {
                    basedir = args[0];
                    filename = args[1];
                    LogManager.Configuration.Variables["basedir"] = basedir;
                    Logger.Log(LogLevel.Info, "Service started");
                    Run(filename);
                }
            }
            catch(Exception e)
            {
                Logger.Log(LogLevel.Error, e.Message);
            }
        }
        protected override void OnStop()
        {
            Logger.Log(LogLevel.Info, "Service stopped");
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Run(string file)
        {
            FileSystemWatcher fWatcher = new FileSystemWatcher
            {
                Path = "D:/",
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = file
            };
            fWatcher.Changed += OnChanged;
            fWatcher.EnableRaisingEvents = true;
        }
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            using (var file = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(file, Encoding.Default))
                {
                    string text = reader.ReadToEnd();
                    MD5 md = new MD5(); 
                    md.Value = text;
                    Logger.Log(LogLevel.Info, md.FingerPrint);
                }
            }
        }
        //}
        //class Logger
        //{
        //    FileSystemWatcher watcher;
        //    object obj = new object();
        //    bool enabled = true;
        //    public Logger()
        //    {
        //        watcher = new FileSystemWatcher("D:\\Temp");
        //        watcher.Changed += OnChanged;
        //    }

        //    public void Start()
        //    {
        //        watcher.EnableRaisingEvents = true;
        //        while (enabled)
        //        {
        //            Thread.Sleep(1000);
        //        }
        //    }
        //    public void Stop()
        //    {
        //        watcher.EnableRaisingEvents = false;
        //        enabled = false;
        //    }
        //    private void OnChanged(object sender, FileSystemEventArgs e)
        //    {
        //        string fileEvent = "изменен";
        //        string filePath = e.FullPath;
        //        RecordEntry(fileEvent, filePath);
        //    }
        //    private void RecordEntry(string fileEvent, string filePath)
        //    {
        //        lock (obj)
        //        {
        //            using (StreamWriter writer = new StreamWriter("D:\\templog.txt", true))
        //            {
        //                writer.WriteLine(String.Format("{0} файл {1} был {2}",
        //                    DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
        //                writer.Flush();
        //            }
        //        }
        //    }
    }
}
