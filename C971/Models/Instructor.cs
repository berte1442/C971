using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971
{
    public class Instructor
    {
        [PrimaryKey, AutoIncrement]
        public int InstructorID
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Phone
        {
            get;
            set;
        }
        public string Email
        {
            get;
            set;
        }
    }
}
   
