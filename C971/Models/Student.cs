using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971
{
    public class Student
    {
        [PrimaryKey, AutoIncrement]
        public int StudentID
        {
            get;
            set;
        }
        public string Username
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
    }
}
