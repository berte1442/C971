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
        List<string> emailList = new List<string>();
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
            if (emailList.Count > 0)
            {
                ShareNotes message = new ShareNotes();
                //sms.SendSms(currentCourse.Notes, InstructorPhone.Text);
                message.SendEmail("'Student Name' - " + currentCourse.Name + " Notes", currentCourse.Notes, emailList);
                EmailList.Text = "";
            }
            else
            {
                DisplayAlert("No Emails Selected", "Enter email address(es) to send notes to.", "OK");
            }
        }

        private void AddEmailButton_Clicked(object sender, EventArgs e)
        {
            if(AddEmailEntry.Text != null && AddEmailEntry.Text != "")
            {
                var email = AddEmailEntry.Text;
                var validate = EditCourse.Email_Validate(email);
                if (validate)
                {
                    emailList.Add(email);
                    AddEmailEntry.Text = "";

                    if(EmailList.Text == null || EmailList.Text == "")
                    {
                        EmailList.Text = email;
                    }
                    else
                    {
                        EmailList.Text += " / " + email;
                    }
                    currentCourse.NotesPublic = true;
                }
                else
                {
                    DisplayAlert("Invalid", "Invalid email address.", "OK");
                }
            }
            else
            {
                DisplayAlert("No Email", "No email address entered", "OK");
            }
        }

        private void Undo_Clicked(object sender, EventArgs e)
        {
            if (emailList.Count > 0)
            {
                int index = emailList.Count - 1;
                
                if(emailList.Count > 1)
                {
                    int commaIndex = EmailList.Text.LastIndexOf(" / ");

                    string newString = EmailList.Text.Substring(0, commaIndex);

                    EmailList.Text = newString;

                }
                else
                {
                    EmailList.Text = "";
                }
                emailList.RemoveAt(index);

            }
        }
    }
}