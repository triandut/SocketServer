using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.Entities
{
    public class Folders
    {
        DateTime Time { get; set; }
        string Type { get; set; }

        string ID_User { get; set; }

        public Folders(DateTime time , string type , string id_user)
        {
            time = this.Time;
            type = this.Type;
            id_user = this.ID_User;

        }
    }
}
