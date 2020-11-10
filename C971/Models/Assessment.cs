using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int AssessmentID
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
        public string AssessmentType
        {
            get;
            set;
        }
        public string AssessmentDescription
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
    }
}
