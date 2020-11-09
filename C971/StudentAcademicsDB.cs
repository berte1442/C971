using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace C971
{
    public class StudentAcademicsDB
    {
        readonly SQLiteAsyncConnection database;

        public StudentAcademicsDB(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTablesAsync<Instructor, Assessment, Course, Term>().Wait();
        }

        // Instructor Methods - return list of instructors - return instructor - save/update instructor - delete instructor

        public Task<List<Instructor>> GetInstructorsAsync()
        {
            return database.Table<Instructor>().ToListAsync();
        }
        public Task<Instructor> GetInstructorAsync(int id)
        {
            return database.Table<Instructor>().Where(i => i.InstructorID == id).FirstOrDefaultAsync();
        }
        public Task<Instructor> GetInstructorAsync(string name)
        {
            return database.Table<Instructor>().Where(i => i.Name == name).FirstOrDefaultAsync();
        }
        public Task<int> SaveInstructorAsync(Instructor instructor)
        {
            if(instructor.InstructorID != 0)
            {
                return database.UpdateAsync(instructor);
            }
            else
            {
                return database.InsertAsync(instructor);
            }
        }
        public Task<int> DeleteInstructorAsync(Instructor instructor)
        {
            return database.DeleteAsync(instructor);
        }
        // Assessment Methods - return list of assessments - return assessment - save/update assessment - delete assessment

        public Task<List<Assessment>> GetAssessmentsAsync()
        {
            return database.Table<Assessment>().ToListAsync();
        }
        public Task<Assessment> GetAssessmentAsync(int id)
        {
            return database.Table<Assessment>().Where(i => i.AssessmentID == id).FirstOrDefaultAsync();
        }
        public Task<Assessment> GetAssessmentAsync(string name)
        {
            return database.Table<Assessment>().Where(i => i.Name == name).FirstOrDefaultAsync();
        }
        public Task<int> SaveAssessmentAsync(Assessment assessment)
        {
            if (assessment.AssessmentID != 0)
            {
                return database.UpdateAsync(assessment);
            }
            else
            {
                return database.InsertAsync(assessment);
            }
        }
        public Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            return database.DeleteAsync(assessment);
        }
        // Course Methods - return courses lists of courses - return course - save/update course - delete course

        public Task<List<Course>> GetCoursesAsync()
        {
            return database.Table<Course>().ToListAsync();
        }
        public Task<Course> GetCourseAsync(int id)
        {
            return database.Table<Course>().Where(i => i.CourseID == id).FirstOrDefaultAsync();
        }
        public Task<Course> GetCourseAsync(string name)
        {
            return database.Table<Course>().Where(i => i.Name == name).FirstOrDefaultAsync();
        }
        public Task<int> SaveCourseAsync(Course course)
        {
            if (course.CourseID != 0)
            {
                return database.UpdateAsync(course);
            }
            else
            {
                return database.InsertAsync(course);
            }
        }
        public Task<int> DeleteCourseAsync(Course course)
        {
            return database.DeleteAsync(course);
        }

        // Term Methods - return lists of terms - return term - save/update term - delete term

        public Task<List<Term>> GetTermsAsync()
        {
            return database.Table<Term>().ToListAsync();
        }
        public Task<Term> GetTermAsync(int id)
        {
            return database.Table<Term>().Where(i => i.TermID == id).FirstOrDefaultAsync();
        }
        public Task<int> SaveTermAsync(Term term)
        {
            if (term.TermID != 0)
            {
                return database.UpdateAsync(term);
            }
            else
            {
                return database.InsertAsync(term);
            }
        }
        public Task<int> DeleteTermAsync(Term term)
        {
            return database.DeleteAsync(term);
        }

        // Student Methods - return lists of students - return student - save/update student - delete student

        public Task<List<Student>> GetStudentsAsync()
        {
            return database.Table<Student>().ToListAsync();
        }
        public Task<Student> GetStudentAsync(int id)
        {
            return database.Table<Student>().Where(i => i.StudentID == id).FirstOrDefaultAsync();
        }
        public Task<int> SaveStudentAsync(Student student)
        {
            if (student.StudentID != 0)
            {
                return database.UpdateAsync(student);
            }
            else
            {
                return database.InsertAsync(student);
            }
        }
        public Task<int> DeleteStudentAsync(Student student)
        {
            return database.DeleteAsync(student);
        }
    }
}
