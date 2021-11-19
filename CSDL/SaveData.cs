using SocketServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketServer.Entities;
namespace SocketServer.CSDL
{
    public class SaveData
    {
        private static SaveData _Instance;
        DB_PBL4Entities1 db = new DB_PBL4Entities1();
        public static SaveData Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SaveData();
                }
                return _Instance;
            }
            private set
            {

            }
        }
        public SaveData()
        {

        }
        public void AddUser(string detail , bool status , string ip)
        {
            User user = new User(detail, status, ip);
            db.Users.Add(user);
            db.SaveChanges();
        }
        public void AddFolder(string time, string type, string id_user)
        {
            Folder folder = new Folder(time, type, id_user);
            if(db.Folders.Where(x => x.Time.Equals(time) && x.Type.Equals(type)).FirstOrDefault() == null)
            {
                db.Folders.Add(folder);
                db.SaveChanges();
            }
            
        }
        public void AddFileDetail(string link , string time, string typeOfFile)
        {
            var folder = db.Folders.Where(x => x.Time.Equals(time) && x.Type.Equals(typeOfFile)).FirstOrDefault();        
            FileDetail file = new FileDetail(link, time, folder.ID_Folder);
            List<FileDetail> listFile = new List<FileDetail>();
            //listFolder.Add()
            //foreach(var item in )
            Console.WriteLine(file.Link);
            try
            {
                db.FileDetails.Add(file);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        public bool CheckUser(string ip)
        {
            if(db.Users.Where(x => x.ID_User.Equals(ip)).FirstOrDefault() != null)
            {
                return true;
            }
            return false;
        }

        public bool CheckFolderExit(string date , string type , string macaddress)
        {
            //string dateTime = db.Folders.Select(x => x.Time).ToString();
            if (db.Folders.Where(x => x.Time.Equals(date)).FirstOrDefault() != null)
            {
                Folder folder = db.Folders.Where(x => x.Time.Equals(date)).FirstOrDefault();
                if (folder.Type.Equals(type)&& folder.ID_User.Equals(macaddress))
                {
                    return true;
                    //if(db.Folders.Where( x => x.ID_User.Equals(macaddress)))
                }
                else
                {
                    return false;
                }
                
            }
            return false;
        }
        
    }
}
