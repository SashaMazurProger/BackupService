using BackupClassLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackupClassLibrary
{
    public class MainProcess
    {
        private bool enabled = true;  //определяет, работает ли процесс
        private ControllerBackupBase controller;
        List<BackupObject> directoryObjects;
        List<BackupObject> filesObjects;
        List<FileSystemWatcher> watchers;
        object objLock = new object();
        public static int MillisecondsOfTimer = 5000;
        public MainProcess(ControllerBackupBase controller)
        {
            this.controller = controller;
            controller.Added += AddNewObject;
            controller.Edited += EditObject;
            controller.Deleted += DeleteObject;
        }
        public void Start()
        {
            controller.StartBackupAll();
            InitializeWatchersForDirec();
            InitializeBackupFiles();

            TimerCallback timerCallback = new TimerCallback(StartBackupFiles);
            Timer backupFilesTimer = new Timer(timerCallback, null, 0, MillisecondsOfTimer);

            while (enabled)
            {

            }
        }
        public  void StartBackupFiles(object defaultObj=null)
        {
            foreach (var item in filesObjects)
            {
                try
                {
                    controller.StartBackup(item);
                }
                catch (Exception ex)
                {
                    Logger.RecordMessageToLog(ex.Message);
                }
            }
        }
        public void Stop()
        {
            foreach (var item in watchers)
            {
                item.EnableRaisingEvents = false;
            }
            enabled = false;
        }
        private void InitializeBackupFiles()
        {

                filesObjects = controller.GetBackupObjects().Where(o => File.Exists(o.FromPath)).ToList();
            
            
        }
        //берем все обьекты для рез.коп. и создаём для каждого
        //FileSystemWatcher для отслеживания изменений
        //FileSystemWatcher нельзя инициализировать путём к файлу
        private void InitializeWatchersForDirec()
        {

            directoryObjects = controller.GetBackupObjects().Where(o => Directory.Exists(o.FromPath)).ToList();
            
            watchers = new List<FileSystemWatcher>();
            foreach (var item in directoryObjects)
            {
                FileSystemWatcher watcher = new FileSystemWatcher(item.FromPath);
                SetWatcherOptions(watcher);
                watchers.Add(watcher);
            }
        }
        //обработчик события - добавления в репозиторий нового обьекта для рез.коп. и добавление его в локальный список
        void AddNewObject(BackupObject obj)
        {
            if (!File.Exists(obj.FromPath))
            {
                directoryObjects.Add(obj);
                FileSystemWatcher watcher = new FileSystemWatcher(obj.FromPath);
                SetWatcherOptions(watcher);
                watchers.Add(watcher);
            }
            else
            {
                filesObjects.Add(obj);
            }
        }
        void SetWatcherOptions(FileSystemWatcher watcher)
        {
            watcher.Changed += watcher_Changed;
            watcher.Deleted += watcher_Changed;
            watcher.Created += watcher_Changed;
            watcher.Renamed += watcher_Changed;
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
        }
        //обработчик события - удаление обьекта из репозитория
        void DeleteObject(BackupObject obj)
        {
            if (!File.Exists(obj.FromPath))
            {
                    directoryObjects.Remove(obj);
                    FileSystemWatcher search = watchers.First(w => w.Path == obj.FromPath);
                    if (search != null)
                    {
                        search.EnableRaisingEvents = false;
                        watchers.Remove(search);
                    }
            }
            else
            {
                filesObjects.Add(obj);
            }
        }
        void EditObject(BackupObject obj)
        {
            if (!File.Exists(obj.FromPath))
            {

                BackupObject search = directoryObjects.First(o => o.FromPath == obj.FromPath);
                if (search != null)
                {
                    search = obj;
                    FileSystemWatcher searchW = watchers.First(w => w.Path == obj.FromPath);
                    if (searchW != null)
                    {
                        try
                        {
                            var newWatcher = new FileSystemWatcher(obj.FromPath);
                            SetWatcherOptions(newWatcher);
                            searchW = newWatcher;
                        }
                        catch (Exception ex)
                        {
                            Logger.RecordMessageToLog(ex.Message);
                        }
                    }
                }
            }
            else
            {
                filesObjects.Add(obj);
            }
        }
        //обработчик события, который ловит любые изменения в обьекте рез.коп.
        void watcher_Changed(object sender, FileSystemEventArgs e)
        {

            //ловим только те изменения, которые произошли на уровне директории или файла в корневой директории
            //if (Directory.Exists(e.FullPath)||
            //    (File.Exists(e.FullPath)&&
            //        objects.Find(o=>o.FromPath==
            //        new FileInfo(e.FullPath).Directory.FullName)!=null))
            
                //ищем нужный обьект, исходный путь которого входит в путь текущего измененного обьекта 
                BackupObject search = directoryObjects.First(o => e.FullPath.Contains(o.FromPath));
                if (search != null)
                {
                        Logger.RecordEventToLog(search.Name, e.ChangeType.ToString(), e.FullPath);
                        try
                        {
                            controller.StartBackup(search);
                        }
                        catch (Exception ex)
                        {
                            Logger.RecordMessageToLog(ex.Message);
                        }
                }
            
        }
    }
}
