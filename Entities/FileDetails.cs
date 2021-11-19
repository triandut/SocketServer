using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.Entities
{
    public class FileDetails
    {

        string Link { get; set; }
        DateTime Time { get; set; }
        int ID_Foleder { get; set; }
        public FileDetails(string link  , DateTime time , int id_folder)
        {
            link = this.Link;
            time = this.Time;
            id_folder = this.ID_Foleder;
        }
    }
}
