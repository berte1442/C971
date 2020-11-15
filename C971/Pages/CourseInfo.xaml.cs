using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseInfo : ContentPage
    {
        Course currentCourse = new Course();
        Term currentTerm = new Term();
        public CourseInfo(Course course, Term term)
        {
            currentCourse = course;
            currentTerm = term;
            InitializeComponent();
        }

        private async void EditCourse_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditCourse(currentCourse, currentTerm));
        }

        private void Home_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            CourseNameLabel.Text = "\"" + currentCourse.Name + "\"";
            CourseDatesLabel.Text = currentCourse.StartDate.ToShortDateString() + " - " + currentCourse.EndDate.ToShortDateString();
            CourseStatusLabel.Text = currentCourse.Status;
            if (currentCourse.NotesPublic)
            {
                PrivacyLabel.Text = "Public";
            }
            else
            {
                PrivacyLabel.Text = "Private";
            }

            var instructor = await App.Database.GetInstructorAsync(currentCourse.InstructorID);
            if(instructor != null)
            {
                CourseInstructorName.Text = instructor.Name;
                CourseInstructorPhone.Text = instructor.Phone;
                CourseInstructorEmail.Text = instructor.Email;
            }
            NotesLabel.Text = currentCourse.Notes;

            var assessment1 = await App.Database.GetAssessmentAsync(currentCourse.AssessmentID);
            var assessment2 = await App.Database.GetAssessmentAsync(currentCourse.Assessment2ID);
            if(assessment1 != null)
            {
                Assessment1Name.Text = assessment1.Name.ToString();
                Assessment1Type.Text = assessment1.AssessmentType.ToString();
                Assessment1Description.Text = assessment1.AssessmentDescription.ToString();
                Assessment1Dates.Text = assessment1.StartDate.ToShortDateString() + " - " + assessment1.EndDate.ToShortDateString();
            }
            if(assessment2 != null)
            {
                Assessment2Name.Text = assessment2.Name.ToString();
                Assessment2Type.Text = assessment2.AssessmentType.ToString();
                Assessment2Description.Text = assessment2.AssessmentDescription.ToString();
                Assessment2Dates.Text = assessment2.StartDate.ToShortDateString() + " - " + assessment2.EndDate.ToShortDateString();
            }
        }

        private void ShareNotes_Clicked(object sender, EventArgs e)
        {
            if (currentCourse.NotesPublic)
            {
                ShareNotes sms = new ShareNotes();
                //sms.SendSms(currentCourse.Notes, InstructorPhone.Text);
                List<string> email = new List<string>();
                email.Add(NotesLabel.Text);
                sms.SendEmail("'Student Name' - " + currentCourse.Name + " Notes", currentCourse.Notes, email);
            }
            else
            {
                DisplayAlert("Private", "Course notes are set to private. You can change settings on the 'Edit Course' page", "OK");
            }
        }
    }
}