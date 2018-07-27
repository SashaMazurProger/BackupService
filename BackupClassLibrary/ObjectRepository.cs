using BackupClassLibrary.Abstract;
using BackupClassLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupClassLibrary
{
    public class ObjectRepository:IObjectRepository
    {
        private static string path = @"C:\options.dat";
        public static List<BackupObject> objects;
        public ObjectRepository()
        {
            objects = new List<BackupObject>();
            LoadObjects();
        }
        private void LoadObjects()
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    while (reader.PeekChar() > -1)
                    {
                        BackupObject obj = new BackupObject();
                        obj.Name = reader.ReadString();
                        obj.FromPath = reader.ReadString();
                        obj.ToPath = reader.ReadString();
                        objects.Add(obj);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Logger.RecordMessageToLog("File not found: " + path);
            }
            catch (LoadDataException)
            {
                Logger.RecordMessageToLog("Can't load data from file: " + path);
            }
            catch (Exception e)
            {
                Logger.RecordMessageToLog(e.Message);
            }
        }
        private void SaveObjects()
        {
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                {
                    foreach (BackupObject obj in objects)
                    {
                        writer.Write(obj.Name);
                        writer.Write(obj.FromPath);
                        writer.Write(obj.ToPath);
                    }
                }
            }
            catch (SaveDataException)
            {
                Logger.RecordMessageToLog("Can't save data to file: " + path);
            }
            catch (Exception e)
            {
                Logger.RecordMessageToLog(e.Message);
            }
        }
        public IEnumerable<BackupObject> GetList()
        {
            return objects;
        }

        public void DeleteObject(BackupObject obj)
        {
            BackupObject search = objects.First(o => o.FromPath == obj.FromPath);
            if (search!=null)
            {
                objects.Remove(obj);
                SaveObjects();
            }
        }

        public void Edit(BackupObject obj)
        {
            BackupObject searchObj= objects.Find(o=>obj.FromPath==o.FromPath);
            if (searchObj != null)
            {
                searchObj = obj;
                SaveObjects();
            }
        }

        public void AddBackupObject(BackupObject obj)
        {
            if (obj != null&&objects.Find(o=>o.FromPath==obj.FromPath)==null)
            {
                objects.Add(obj);
                SaveObjects();
            }
        }
    }
}
