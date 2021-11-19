using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.Entities
{
    public class Users
    {
        public string ID_User { get; set; }
        public string Detail { get; set; }
        public bool Status { get; set; }
        public string MacAdrress { get; set; }
        public Users()
        {

        }
        public Users(string detail , bool status , string ip)
        {
            this.ID_User = ip;
            this.Detail = detail;
            this.Status = status;
            this.MacAdrress = ip;
        }
    }
}
