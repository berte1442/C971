using System;
using System.Collections.Generic;
using System.Text;

namespace C971
{
    class DataInsert
    {
        public static async void DatabaseCheck()
        {
            var courses = await App.Database.GetCoursesAsync();
            if(courses.Count == 0)
            {
                Populate();
            }
        }
        public static void Populate()
        {
            Instructor instructor = new Instructor();
            instructor.Name = "Stacey Williams";
            instructor.Phone = "(111)111-1111";
            instructor.Email = "staceywilliams99@ymail.com";
            App.Database.SaveInstructorAsync(instructor);

            Instructor instructor2 = new Instructor();
            instructor2.Name = "Kirk Patreece";
            instructor2.Phone = "(222)222-2222";
            instructor2.Email = "kPat18@ymail.com";
            App.Database.SaveInstructorAsync(instructor2);

            Instructor instructor3 = new Instructor();
            instructor3.Name = "Tim Staulings";
            instructor3.Phone = "(333)333-3333";
            instructor3.Email = "TimTeachesYou@ymail.com";
            App.Database.SaveInstructorAsync(instructor3);

            Instructor instructor4 = new Instructor();
            instructor4.Name = "Lane Berksdale";
            instructor4.Phone = "(444)444-4444";
            instructor4.Email = "Lane_Berksdale1989@ymail.com";
            App.Database.SaveInstructorAsync(instructor4);

            Instructor instructor5 = new Instructor();
            instructor5.Name = "Christian Miller";
            instructor5.Phone = "(555)555-5555";
            instructor5.Email = "Christian.Miller11@ymail.com";
            App.Database.SaveInstructorAsync(instructor5);

            Instructor instructor6 = new Instructor();
            instructor6.Name = "Laura Wilks";
            instructor6.Phone = "(666)666-6666";
            instructor6.Email = "LWilks1342@ymail.com";
            App.Database.SaveInstructorAsync(instructor6);

            Assessment assessment = new Assessment();
            assessment.Name = "assessment1";
            assessment.AssessmentType = "PA";
            assessment.AssessmentDescription = "Presentation";
            App.Database.SaveAssessmentAsync(assessment);

            Assessment assessment2 = new Assessment();
            assessment2.Name = "assessment2";
            assessment2.AssessmentType = "PA";
            assessment2.AssessmentDescription = "Coding Challenge";
            App.Database.SaveAssessmentAsync(assessment2);

            Assessment assessment3 = new Assessment();
            assessment3.Name = "assessment3";
            assessment3.AssessmentType = "PA";
            assessment3.AssessmentDescription = "Proposal Paper";
            App.Database.SaveAssessmentAsync(assessment3);

            Assessment assessment4 = new Assessment();
            assessment4.Name = "assessment4";
            assessment4.AssessmentType = "PA";
            assessment4.AssessmentDescription = "Essay";
            App.Database.SaveAssessmentAsync(assessment4);

            Assessment assessment5 = new Assessment();
            assessment5.Name = "assessment5";
            assessment5.AssessmentType = "PA";
            assessment5.AssessmentDescription = "Mobile Application";
            App.Database.SaveAssessmentAsync(assessment5);

            Assessment assessment6 = new Assessment();
            assessment6.Name = "assessment6";
            assessment6.AssessmentType = "OA";
            assessment6.AssessmentDescription = "40 Question Test";
            App.Database.SaveAssessmentAsync(assessment6);

            Assessment assessment7 = new Assessment();
            assessment7.Name = "assessment7";
            assessment7.AssessmentType = "OA";
            assessment7.AssessmentDescription = "50 Question Test";
            App.Database.SaveAssessmentAsync(assessment7);

            Assessment assessment8 = new Assessment();
            assessment8.Name = "assessment8";
            assessment8.AssessmentType = "OA";
            assessment8.AssessmentDescription = "60 Question Test";
            App.Database.SaveAssessmentAsync(assessment8);

            Assessment assessment9 = new Assessment();
            assessment9.Name = "assessment9";
            assessment9.AssessmentType = "OA";
            assessment9.AssessmentDescription = "CompTIA Certification";
            App.Database.SaveAssessmentAsync(assessment9);

            Assessment assessment10 = new Assessment();
            assessment10.Name = "assessment10";
            assessment10.AssessmentType = "OA";
            assessment10.AssessmentDescription = "Microsoft Certification";
            App.Database.SaveAssessmentAsync(assessment10);

            Course course = new Course();
            course.Name = "Software Introduction";
            course.Status = "Inactive";
            course.InstructorID = instructor.InstructorID;
            course.AssessmentID = assessment4.AssessmentID;
            course.Assessment2ID = assessment6.AssessmentID;
            App.Database.SaveCourseAsync(course);

            Course course2 = new Course();
            course2.Name = "Ethics";
            course2.Status = "Inactive";
            course2.InstructorID = instructor2.InstructorID;
            course2.AssessmentID = assessment.AssessmentID;
            course2.Assessment2ID = assessment7.AssessmentID;
            App.Database.SaveCourseAsync(course2);

            Course course3 = new Course();
            course3.Name = "Algebra";
            course3.Status = "Inactive";
            course3.InstructorID = instructor3.InstructorID;
            course3.AssessmentID = assessment3.AssessmentID;
            course3.Assessment2ID = assessment8.AssessmentID;
            App.Database.SaveCourseAsync(course3);

            Course course4 = new Course();
            course4.Name = "Quality Assurance";
            course4.Status = "Inactive";
            course4.InstructorID = instructor4.InstructorID;
            course4.AssessmentID = assessment.AssessmentID;
            course4.Assessment2ID = assessment6.AssessmentID;
            App.Database.SaveCourseAsync(course4);

            Course course5 = new Course();
            course5.Name = "Software Development 1";
            course5.Status = "Inactive";
            course5.InstructorID = instructor5.InstructorID;
            course5.AssessmentID = assessment4.AssessmentID;
            course5.Assessment2ID = assessment10.AssessmentID;
            App.Database.SaveCourseAsync(course5);

            Course course6 = new Course();
            course6.Name = "Software Development 2";
            course6.Status = "Inactive";
            course6.InstructorID = instructor6.InstructorID;
            course6.AssessmentID = assessment2.AssessmentID;
            course6.Assessment2ID = assessment9.AssessmentID;
            App.Database.SaveCourseAsync(course6);

            Course course7 = new Course();
            course7.Name = "Sql Database";
            course7.Status = "Inactive";
            course7.InstructorID = instructor.InstructorID;
            course7.AssessmentID = assessment2.AssessmentID;
            course7.Assessment2ID = assessment10.AssessmentID;
            App.Database.SaveCourseAsync(course7);

            Course course8 = new Course();
            course8.Name = "Technical Communication";
            course8.Status = "Inactive";
            course8.InstructorID = instructor2.InstructorID;
            course8.AssessmentID = assessment3.AssessmentID;
            course8.Assessment2ID = assessment8.AssessmentID;
            App.Database.SaveCourseAsync(course8);

            Course course9 = new Course();
            course9.Name = "UI Design";
            course9.Status = "Inactive";
            course9.InstructorID = instructor3.InstructorID;
            course9.AssessmentID = assessment2.AssessmentID;
            course9.Assessment2ID = assessment7.AssessmentID;
            App.Database.SaveCourseAsync(course9);

            Course course10 = new Course();
            course10.Name = "Capstone";
            course10.Status = "Inactive";
            course10.InstructorID = instructor4.InstructorID;
            course10.AssessmentID = assessment3.AssessmentID;
            course10.Assessment2ID = assessment8.AssessmentID;
            App.Database.SaveCourseAsync(course10);

            Course course11 = new Course();
            course11.Name = "Technical Writing";
            course11.Status = "Inactive";
            course11.InstructorID = instructor5.InstructorID;
            course11.AssessmentID = assessment4.AssessmentID;
            course11.Assessment2ID = assessment6.AssessmentID;
            App.Database.SaveCourseAsync(course11);

            Course course12 = new Course();
            course12.Name = "Mobile Application";
            course12.Status = "Inactive";
            course12.InstructorID = instructor6.InstructorID;
            course12.AssessmentID = assessment5.AssessmentID;
            course12.Assessment2ID = assessment9.AssessmentID;
            App.Database.SaveCourseAsync(course12);

            //Term term = new Term();
            //term.Name = "Term1";
            //term.StartDate = DateTime.Now.Date;
            //term.EndDate = DateTime.Now.AddMonths(6).Date;
            //term.CourseID = course.CourseID;
            //term.Course2ID = course2.CourseID;
            //term.Course3ID = course3.CourseID;
            //term.Course4ID = course4.CourseID;
            //term.Course5ID = course5.CourseID;
            //term.Course6ID = course6.CourseID;
            //App.Database.SaveTermAsync(term);

            //Term term2 = new Term();
            //term2.Name = "Term2";
            //term2.StartDate = DateTime.Now.AddMonths(6).Date;
            //term2.EndDate = DateTime.Now.AddMonths(12).Date;
            //term2.CourseID = course7.CourseID;
            //term2.Course2ID = course8.CourseID;
            //term2.Course3ID = course9.CourseID;
            //term2.Course4ID = course10.CourseID;
            //term2.Course5ID = course11.CourseID;
            //term2.Course6ID = course12.CourseID;
            //App.Database.SaveTermAsync(term2);

        }
        public async void ClearDatabase()
        {
            var courses = await App.Database.GetCoursesAsync();

            foreach (var c in courses)
            {
                await App.Database.DeleteCourseAsync(c);
            }

            var assessments = await App.Database.GetAssessmentsAsync();

            foreach (var a in assessments)
            {
                await App.Database.DeleteAssessmentAsync(a);
            }

            var terms = await App.Database.GetTermsAsync();

            foreach (var t in terms)
            {
                await App.Database.DeleteTermAsync(t);
            }

            var instructors = await App.Database.GetInstructorsAsync();

            foreach (var i in instructors)
            {
                await App.Database.DeleteInstructorAsync(i);
            }
        }
    }
}
