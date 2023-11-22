using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf.Logging;

namespace FileConverterService
{
    internal class ConverterService
    {
        private FileSystemWatcher _watcher;
        private static readonly LogWriter _log = HostLogger.Get<ConverterService>();

        public bool Start()
        {
            _watcher = new FileSystemWatcher();
            _watcher.Path = @"C:\temp\ServiceToUpper";
            _watcher.Filter = "*.txt";
            _watcher.Created += FileCreated;
            _watcher.IncludeSubdirectories = false;
            _watcher.EnableRaisingEvents = true;
            return true;
        }

        public bool Pause()
        {
            _watcher.EnableRaisingEvents = false;
            return true;
        }

        public bool Continue()
        {
            _watcher.EnableRaisingEvents = true;
            return true;
        }

        public void CustomCommand(int commandNumber)
        {
            _log.InfoFormat("Received command number {0}", commandNumber);
        }

        private void FileCreated(object sender, FileSystemEventArgs e)
        {
            _log.InfoFormat("Started conversion of : {0}", e.FullPath);
            if (e.FullPath.Contains("bad_in"))
            {
                throw new NotSupportedException("Cannot convert");
            }
            string content = File.ReadAllText(e.FullPath);
            string upperContent = content.ToUpper();
            var dir = Path.GetDirectoryName(e.FullPath);
            var convertedFileName = Path.GetFileNameWithoutExtension(e.FullPath) + ".converted";
            var convertedPath = Path.Combine(dir, convertedFileName);
            File.WriteAllText(convertedPath, upperContent);
        }

        public bool Stop()
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
            return true;
        }
    }
}
