using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971
{
    class LocalNotifications
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
        //public int IconId { get; set; }
        public DateTime NotifyTime { get; set; }
    }
}
