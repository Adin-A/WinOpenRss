using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace OpenRSS_1.Models
{
    public class Subscriptions
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public bool Active { get; set; }
        public bool Error { get; set; }
        public DateTime DateSubscribed { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime LastChecked { get; set; }        
    }
}
