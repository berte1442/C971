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
            database.CreateTablesAsync<Instructor, Assessment, Course, Term, Student>().Wait();

            Prepopulate();
        }

        //top commentted out code clears database - add async to method
        public void Prepopulate()
        {
            //StudentAcademicsDB studentAcademicsDB = new StudentAcademicsDB(App.Database.ToString());

            //var courses = await App.Database.GetCoursesAsync();

            //for (int i = 0; i < courses.Count; i++)
            //{
            //    await DeleteCourseAsync(courses[i]);
            //}

            //var assessments = await App.Database.GetAssessmentssAsync();

            //for (int i = 0; i < assessments.Count; i++)
            //{
            //    await DeleteAssessmentAsync(assessments[i]);
            //}

            //var terms = await App.Database.GetTermsAsync();

            //for (int i = 0; i < terms.Count; i++)
            //{
            //    await DeleteTermAsync(terms[i]);
            //}

            if (database == null)
            {

                Instructor instructor = new Instructor();
                instructor.Name = "Stacey Williams";
                instructor.Phone = "(111)111-1111";
                instructor.Email = "staceywilliams99@ymail.com";
                SaveInstructorAsync(instructor);

                Instructor instructor2 = new Instructor();
                instructor2.Name = "Kirk Patreece";
                instructor2.Phone = "(222)222-2222";
                instructor2.Email = "kPat18@ymail.com";
                SaveInstructorAsync(instructor2);

                Instructor instructor3 = new Instructor();
                instructor3.Name = "Tim Staulings";
                instructor3.Phone = "(333)333-3333";
                instructor3.Email = "TimTeachesYou@ymail.com";
                SaveInstructorAsync(instructor3);

                Instructor instructor4 = new Instructor();
                instructor4.Name = "Lane Berksdale";
                instructor4.Phone = "(444)444-4444";
                instructor4.Email = "Lane_Berksdale1989@ymail.com";
                SaveInstructorAsync(instructor4);

                Instructor instructor5 = new Instructor();
                instructor5.Name = "Christian Miller";
                instructor5.Phone = "(555)555-5555";
                instructor5.Email = "Christian.Miller11@ymail.com";
                SaveInstructorAsync(instructor5);

                Instructor instructor6 = new Instructor();
                instructor6.Name = "Laura Wilks";
                instructor6.Phone = "(666)666-6666";
                instructor6.Email = "LWilks1342@ymail.com";
                SaveInstructorAsync(instructor6);

                Assessment assessment = new Assessment();
                assessment.AssessmentType = "PA";
                assessment.AssessmentDescription = "Presentation";
                SaveAssessmentAsync(assessment);

                Assessment assessment2 = new Assessment();
                assessment2.AssessmentType = "PA";
                assessment2.AssessmentDescription = "Coding Challenge";
                SaveAssessmentAsync(assessment2);

                Assessment assessment3 = new Assessment();
                assessment3.AssessmentType = "PA";
                assessment3.AssessmentDescription = "Proposal Paper";
                SaveAssessmentAsync(assessment3);

                Assessment assessment4 = new Assessment();
                assessment4.AssessmentType = "PA";
                assessment4.AssessmentDescription = "Essay";
                SaveAssessmentAsync(assessment4);

                Assessment assessment5 = new Assessment();
                assessment5.AssessmentType = "PA";
                assessment5.AssessmentDescription = "Mobile Application";
                SaveAssessmentAsync(assessment5);

                Assessment assessment6 = new Assessment();
                assessment6.AssessmentType = "OA";
                assessment6.AssessmentDescription = "40 Question Test";
                SaveAssessmentAsync(assessment6);

                Assessment assessment7 = new Assessment();
                assessment7.AssessmentType = "OA";
                assessment7.AssessmentDescription = "50 Question Test";
                SaveAssessmentAsync(assessment7);

                Assessment assessment8 = new Assessment();
                assessment8.AssessmentType = "OA";
                assessment8.AssessmentDescription = "60 Question Test";
                SaveAssessmentAsync(assessment8);

                Assessment assessment9 = new Assessment();
                assessment9.AssessmentType = "OA";
                assessment9.AssessmentDescription = "CompTIA Certification";
                SaveAssessmentAsync(assessment9);

                Assessment assessment10 = new Assessment();
                assessment10.AssessmentType = "OA";
                assessment10.AssessmentDescription = "Microsoft Certification";
                SaveAssessmentAsync(assessment10);

                Course course = new Course();
                course.Name = "Software Introduction";
                course.Status = "Inactive";
                course.InstructorID = instructor.InstructorID;
                course.AssessmentID = assessment4.AssessmentID;
                course.Assessment2ID = assessment6.AssessmentID;
                SaveCourseAsync(course);

                Course course2 = new Course();
                course2.Name = "Ethics";
                course2.Status = "Inactive";
                course2.InstructorID = instructor2.InstructorID;
                course2.AssessmentID = assessment.AssessmentID;
                course2.Assessment2ID = assessment7.AssessmentID;
                SaveCourseAsync(course2);

                Course course3 = new Course();
                course3.Name = "Algebra";
                course3.Status = "Inactive";
                course3.InstructorID = instructor3.InstructorID;
                course3.AssessmentID = assessment3.AssessmentID;
                course3.Assessment2ID = assessment8.AssessmentID;
                SaveCourseAsync(course3);

                Course course4 = new Course();
                course4.Name = "Quality Assurance";
                course4.Status = "Inactive";
                course4.InstructorID = instructor.InstructorID;
                course4.AssessmentID = assessment.AssessmentID;
                course4.Assessment2ID = assessment6.AssessmentID;
                SaveCourseAsync(course4);

                Course course5 = new Course();
                course5.Name = "Software Development 1";
                course5.Status = "Inactive";
                course5.InstructorID = instructor.InstructorID;
                course5.AssessmentID = assessment4.AssessmentID;
                course5.Assessment2ID = assessment10.AssessmentID;
                SaveCourseAsync(course5);

                Course course6 = new Course();
                course6.Name = "Software Development 2";
                course6.Status = "Inactive";
                course6.InstructorID = instructor.InstructorID;
                course6.AssessmentID = assessment2.AssessmentID;
                course6.Assessment2ID = assessment9.AssessmentID;
                SaveCourseAsync(course6);

                Course course7 = new Course();
                course7.Name = "Sql Database";
                course7.Status = "Inactive";
                course7.InstructorID = instructor.InstructorID;
                course7.AssessmentID = assessment2.AssessmentID;
                course7.Assessment2ID = assessment10.AssessmentID;
                SaveCourseAsync(course7);

                Course course8 = new Course();
                course8.Name = "Technical Communication";
                course8.Status = "Inactive";
                course8.InstructorID = instructor.InstructorID;
                course8.AssessmentID = assessment3.AssessmentID;
                course8.Assessment2ID = assessment8.AssessmentID;
                SaveCourseAsync(course8);

                Course course9 = new Course();
                course9.Name = "UI Design";
                course9.Status = "Inactive";
                course9.InstructorID = instructor.InstructorID;
                course9.AssessmentID = assessment2.AssessmentID;
                course9.Assessment2ID = assessment7.AssessmentID;
                SaveCourseAsync(course9);

                Course course10 = new Course();
                course10.Name = "Capstone";
                course10.Status = "Inactive";
                course10.InstructorID = instructor.InstructorID;
                course10.AssessmentID = assessment3.AssessmentID;
                course10.Assessment2ID = assessment8.AssessmentID;
                SaveCourseAsync(course10);

                Course course11 = new Course();
                course11.Name = "Technical Writing";
                course11.Status = "Inactive";
                course11.InstructorID = instructor.InstructorID;
                course11.AssessmentID = assessment4.AssessmentID;
                course11.Assessment2ID = assessment6.AssessmentID;
                SaveCourseAsync(course11);

                Course course12 = new Course();
                course12.Name = "Mobile Application";
                course12.Status = "Inactive";
                course12.InstructorID = instructor.InstructorID;
                course12.AssessmentID = assessment5.AssessmentID;
                course12.Assessment2ID = assessment9.AssessmentID;
                SaveCourseAsync(course12);

                Term term = new Term();
                term.Name = "Term1";
                term.StartDate = DateTime.Now.Date;
                term.EndDate = DateTime.Now.AddMonths(6).Date;
                term.CourseID = course.CourseID;
                term.Course2ID = course2.CourseID;
                term.Course3ID = course3.CourseID;
                term.Course4ID = course4.CourseID;
                term.Course5ID = course5.CourseID;
                term.Course6ID = course6.CourseID;
                SaveTermAsync(term);

                Term term2 = new Term();
                term2.Name = "Term2";
                term2.StartDate = DateTime.Now.AddMonths(6).Date;
                term2.EndDate = DateTime.Now.AddMonths(12).Date;
                term2.CourseID = course7.CourseID;
                term2.Course2ID = course8.CourseID;
                term2.Course3ID = course9.CourseID;
                term2.Course4ID = course10.CourseID;
                term2.Course5ID = course11.CourseID;
                term2.Course6ID = course12.CourseID;
                SaveTermAsync(term2);

            }


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

        public Task<List<Assessment>> GetAssessmentssAsync()
        {
            return database.Table<Assessment>().ToListAsync();
        }
        public Task<Assessment> GetAssessmentAsync(int id)
        {
            return database.Table<Assessment>().Where(i => i.AssessmentID == id).FirstOrDefaultAsync();
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
