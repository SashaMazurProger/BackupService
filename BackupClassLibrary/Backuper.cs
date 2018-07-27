using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupClassLibrary
{
    public class Backuper
    {
        public  void CopyDirectory(object backupObject)
        {
            BackupObject obj = (BackupObject)backupObject;
            DirectoryInfo fromDirectory = new DirectoryInfo(obj.FromPath);
            DirectoryInfo toDirectory = new DirectoryInfo(obj.ToPath);
           //проверяем на наличие исходной директории
           if(!fromDirectory.Exists)
               return;
           else
           {
              //проверяем на наличие директории, в которую будем копировать файлы; если нет-создаем
              if(!toDirectory.Exists)
              {
                  toDirectory.Create();
              }
              else
              {
                  //удаляем файлы и папки, которых уже нет в исходной папке
                  if(toDirectory.GetFiles().Length>0)
                      DeleteFiles(fromDirectory,toDirectory);
                  if(toDirectory.GetDirectories().Length>0)
                      DeleteDirectories(fromDirectory,toDirectory);
              }
              
              FileInfo[] files=fromDirectory.GetFiles();
              if(files.Length>0)
              {
                CopyFiles(files,toDirectory);
              }
              DirectoryInfo[] directories=fromDirectory.GetDirectories();
              if(directories.Length>0)
              {
                  foreach(var item in directories)
                  {
                    DirectoryInfo toChildDirectory=new DirectoryInfo(Path.Combine(toDirectory.FullName,item.Name));
                    BackupObject subObj = new BackupObject
                    {
                        FromPath = item.FullName,
                        ToPath = Path.Combine(toDirectory.FullName, item.Name)
                    };
                    CopyDirectory(subObj);
                  }
              }
           }
        }
        public void CopyFiles(FileInfo[] files,DirectoryInfo toDirectory)
        {
            foreach(var item in files)
            {
                try
                {
                    File.Copy(item.FullName, Path.Combine(toDirectory.FullName, item.Name), true);
                }
                catch (FileNotFoundException ex)
                {
                    throw ex;
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
        }
        public void CopyFile(object backupObject)
        {
            try
            {
                BackupObject fileBackup = (BackupObject)backupObject;
                FileInfo file = new FileInfo(fileBackup.FromPath);
                DirectoryInfo toDirectory = new DirectoryInfo(fileBackup.ToPath);
                File.Copy(file.FullName, Path.Combine(toDirectory.FullName, file.Name), true);
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteDirectories(DirectoryInfo source, DirectoryInfo fromDelete)
        {
            DirectoryInfo[] dir = fromDelete.GetDirectories();
            foreach (var item in dir)
            {
                if (!Directory.Exists(Path.Combine(source.FullName, item.Name)))
                {
                        item.Delete(true);
                    
                }
            }
        }
        public void DeleteFiles(DirectoryInfo source, DirectoryInfo fromDelete)
        {
            FileInfo[] files = fromDelete.GetFiles();
            foreach (var item in files)
            {
                if (!File.Exists(Path.Combine(source.FullName, item.Name)))
                    item.Delete();
            }
        }
    }
}
