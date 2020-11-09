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
        Course currentCourse = new Course();
        Assessment assessment = new Assessment();

        public EditCourse(Course course, Term term)
        {
            currentCourse = course;

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
            string assessmentName = null;
            
            if (PAAssessmentCheckBox.IsChecked && Assessment1.Text != null)
            {
                assessmentName = PAAssessmentPicker.SelectedItem.ToString();
            }
            else if(OAAssessmentCheckBox.IsChecked && Assessment2.Text != null)
            {
                assessmentName = OAAssessmentPicker.SelectedItem.ToString();
            }
            else
            {
                await DisplayAlert("Select Assessment", "Select an assessment to edit.", "OK");
            }

            if(assessmentName != null)
            {
                assessment = await App.Database.GetAssessmentAsync(assessmentName);
                await Navigation.PushAsync(new EditAssessment(assessment));
            }
        }

        private async void DeleteAssessment_Clicked(object sender, EventArgs e)
        {
            int assessmentID = 0;
            string assessmentName = null;

            if (PAAssessmentCheckBox.IsChecked)
            {
                if(Assessment1.Text != null)
                {
                    assessmentName = Assessment1.Text;
                }
                else
                {
                    await DisplayAlert("No Assessment", "No assessment assigned.", "OK");
                }
            }
            else if (OAAssessmentCheckBox.IsChecked)
            {
                if (Assessment2.Text != null)
                {
                    assessmentName = Assessment2.Text;
                }
                else
                {
                    await DisplayAlert("No Assessment", "No assessment assigned.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Select Assessment", "Select an assessment to delete.", "OK");
            }
            if (assessmentName != null)
            {
                var assessments = await App.Database.GetAssessmentsAsync();
                foreach(var a in assessments)
                {
                    if(a.Name == assessmentName)
                    {
                        assessmentID = a.AssessmentID;
                    }
                }

                try
                {
                    if(currentCourse.AssessmentID == assessmentID)
                    {
                        currentCourse.AssessmentID = 0;
                        Assessment1.Text = null;
                    }
                    else if(currentCourse.Assessment2ID == assessmentID)
                    {
                        currentCourse.Assessment2ID = 0;
                        Assessment2.Text = null;
                    }
                }
                catch
                {
                    await DisplayAlert("Error", "Something went wrong.", "OK");
                }
            }
        }

        private async void AddAssessment_Clicked(object sender, EventArgs e)
        {
            string assessmentName = null;
            
            if(currentCourse.AssessmentID == 0 && PACheckbox.IsChecked && PAAssessmentPicker.SelectedIndex != -1)
            {
                assessmentName = PAAssessmentPicker.SelectedItem.ToString();
            }
            else if (currentCourse.Assessment2ID == 0 && OACheckbox.IsChecked && OAAssessmentPicker.SelectedIndex != -1)
            {
                assessmentName = OAAssessmentPicker.SelectedItem.ToString();
            }
            if(assessmentName != null)
            {
                var assessment = await App.Database.GetAssessmentAsync(assessmentName);

                if(assessment.AssessmentType.ToUpper() == "PA")
                {
                    currentCourse.AssessmentID = assessment.AssessmentID;
                    Assessment1.Text = assessment.Name;
                }
                else if(assessment.AssessmentType.ToUpper() == "OA")
                {
                    currentCourse.Assessment2ID = assessment.AssessmentID;
                    Assessment2.Text = assessment.Name;
                }

                await DisplayAlert("Assessment Added", "Assessment will be added to course", "OK");
            }
            else
            {
                if(PACheckbox.IsChecked == false && OACheckbox.IsChecked == false)
                {
                    await DisplayAlert("Select Assessment", "Select an assessment from the provided list and check the box below before adding to course.", "OK");
                }
                else
                {
                    await DisplayAlert("Assessment Not Added", "Assessment could not be added to course. Courses can only contain one PA and one OA.", "OK");
                }
            }

        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            currentCourse.Name = CourseTitleEntry.Text;
            currentCourse.StartDate = StartDatePicker.Date;
            currentCourse.EndDate = EndDatePicker.Date;
            currentCourse.Status = CourseStatusPicker.SelectedItem.ToString();

            var instructorName = InstructorPicker.SelectedItem.ToString();
            Instructor instructor = await App.Database.GetInstructorAsync(instructorName);

            currentCourse.InstructorID = instructor.InstructorID;

            var assessment1 = await App.Database.GetAssessmentAsync(Assessment1.Text);
            var assessment2 = await App.Database.GetAssessmentAsync(Assessment2.Text);

            currentCourse.AssessmentID = assessment1.AssessmentID;
            currentCourse.Assessment2ID = assessment2.AssessmentID;
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
            CourseTitleEntry.Text = currentCourse.Name;
            /////////
            // sets course dates to selected course start and end dates
            StartDatePicker.Date = currentCourse.StartDate;
            EndDatePicker.Date = currentCourse.EndDate;
            //sets selected course status 
            CourseStatusPicker.SelectedItem = currentCourse.Status;
            // loads all course instructors into Picker
            var instructors = await App.Database.GetInstructorsAsync();
            List<string> instructorNames = new List<string>();
            foreach(var i in instructors)
            {
                instructorNames.Add(i.Name);

                if(i.InstructorID == currentCourse.InstructorID)
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

                    if(a.AssessmentID == currentCourse.AssessmentID)
                    {
                        Assessment1.Text = a.Name;
                    }
                }
                else if(a.AssessmentType.ToUpper() == "OA")
                {
                    oaAssessmentNames.Add(a.Name);

                    if(a.AssessmentID == currentCourse.Assessment2ID)
                    {
                        Assessment2.Text = a.Name;

                    }
                }
            }
            PAAssessmentPicker.ItemsSource = paAssessmentNames;
            OAAssessmentPicker.ItemsSource = oaAssessmentNames;
        }

        private void PACheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (PACheckbox.IsChecked)
            {
                OACheckbox.IsChecked = false;
            }
        }

        private void OACheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (OACheckbox.IsChecked)
            {
                PACheckbox.IsChecked = false;
            }
        }

        private void PAAssessmentCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (PAAssessmentCheckBox.IsChecked)
            {
                OAAssessmentCheckBox.IsChecked = false;
            }
        }

        private void OAAssessmentCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (OAAssessmentCheckBox.IsChecked)
            {
                PAAssessmentCheckBox.IsChecked = false;
            }
        }
    }
}