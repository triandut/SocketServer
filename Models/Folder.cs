//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SocketServer.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Folder
    {
        public Folder()
        {
            this.FileDetails = new HashSet<FileDetail>();
        }
    
        public string ID_Folder { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public string Type { get; set; }
        public string ID_User { get; set; }
    
        public virtual ICollection<FileDetail> FileDetails { get; set; }
        public virtual User User { get; set; }
    }
}
