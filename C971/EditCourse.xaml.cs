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
    public partial class EditCourse : ContentPage
    {
        Course selectedCourse = new Course();

        public EditCourse(Course course, Term term)
        {
            selectedCourse = course;

            InitializeComponent();

            if(course.Status == "Inactive")
            {
                StartDatePicker.MinimumDate = DateTime.Now;
            }
            else
            {
                StartDatePicker.MinimumDate = course.StartDate;
                StartDatePicker.MaximumDate = course.StartDate;
            }

            EndDatePicker.MinimumDate = term.StartDate;
            EndDatePicker.MaximumDate = term.EndDate;

        }

        private async void EditAssessment_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditAssessment());
        }

        private void DeleteAssessment_Clicked(object sender, EventArgs e)
        {

        }

        private async void AddAssessment_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddAssessment());
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            selectedCourse.Name = CourseTitleEntry.Text;
            selectedCourse.StartDate = StartDatePicker.Date;
            selectedCourse.EndDate = EndDatePicker.Date;
            selectedCourse.Status = CourseStatusPicker.SelectedItem.ToString();

            var instructorName = InstructorPicker.SelectedItem.ToString();
            Instructor instructor = await App.Database.GetInstructorAsync(instructorName);

            selectedCourse.InstructorID = instructor.InstructorID;

            var assessment1 = await App.Database.GetAssessmentAsync(Assessment1.Text);
            var assessment2 = await App.Database.GetAssessmentAsync(Assessment2.Text);

            selectedCourse.AssessmentID = assessment1.AssessmentID;
            selectedCourse.Assessment2ID = assessment2.AssessmentID;
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void Home_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //sets selected course name as placeholder for editor
            CourseTitleEntry.Text = selectedCourse.Name;
            /////////
            // sets course dates to selected course start and end dates
            StartDatePicker.Date = selectedCourse.StartDate;
            EndDatePicker.Date = selectedCourse.EndDate;
            //sets selected course status 
            CourseStatusPicker.SelectedItem = selectedCourse.Status;
            // loads all course instructors into Picker
            var instructors = await App.Database.GetInstructorsAsync();
            List<string> instructorNames = new List<string>();
            foreach(var i in instructors)
            {
                instructorNames.Add(i.Name);

                if(i.InstructorID == selectedCourse.InstructorID)
                {
                    InstructorPicker.SelectedItem = i.Name;
                }
            }
            InstructorPicker.ItemsSource = instructorNames;

            // code below throws null exception - AssessmentIDs are 0 - assessment is never found, therefore assessment.Name = null


            var assessments = await App.Database.GetAssessmentsAsync();
            List<string> paAssessmentNames = new List<string>();
            List<string> oaAssessmentNames = new List<string>();
            foreach (var a in assessments)
            {
                if(a.AssessmentType.ToUpper() == "PA")
                {
                    paAssessmentNames.Add(a.Name);
                }
                else if(a.AssessmentType.ToUpper() == "OA")
                {
                    oaAssessmentNames.Add(a.Name);
                }
            }
            PAAssessmentPicker.ItemsSource = paAssessmentNames;
            OAAssessmentPicker.ItemsSource = oaAssessmentNames;
        }

        private void PACheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (PACheckBox.IsChecked)
            {
                OACheckBox.IsChecked = false;
                string assessmentType = "PA";
            }
        }

        private void OACheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (OACheckBox.IsChecked)
            {
                PACheckBox.IsChecked = false;
                string assessmentType = "OA";
            }
        }
    }
}