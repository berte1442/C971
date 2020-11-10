using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int TermID
        {
            get;
            set;
        }
        [Unique]
        public string Name
        {
            get;
            set;
        }
        public DateTime StartDate
        {
            get;
            set;
        }
        public DateTime EndDate
        {
            get;
            set;
        }
        public int CourseID
        {
            get;
            set;
        }
        public int Course2ID
        {
            get;
            set;
        }
        public int Course3ID
        {
            get;
            set;
        }        
        public int Course4ID
        {
            get;
            set;
        }
        public int Course5ID
        {
            get;
            set;
        }
        public int Course6ID
        {
            get;
            set;
        }
    }
}

