using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int CourseID
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
        public string Status
        {
            get;
            set;
        }
        public string Notes
        {
            get;
            set;
        }
        public int InstructorID
        {
            get;
            set;
        }
        public int AssessmentID
        {
            get;
            set;
        }
        public int Assessment2ID
        {
            get;
            set;
        }
    }
}
