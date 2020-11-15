using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.LocalNotifications;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAssessment : ContentPage
    {
        private Assessment currentAssessment = new Assessment();
        private Course currentCourse = new Course();
        private Term currentTerm = new Term();

        public EditAssessment(Assessment assessment, Course course, Term term)
        {
            currentAssessment = assessment;
            currentCourse = course;
            currentTerm = term;
            InitializeComponent();

            StartDatePicker.MinimumDate = currentCourse.StartDate;
            StartDatePicker.MaximumDate = currentCourse.EndDate;
            EndDatePicker.MinimumDate = currentCourse.StartDate;
            EndDatePicker.MaximumDate = currentCourse.EndDate;
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (AssessmentNameEntry.Text != null && AssessmentNameEntry.Text != "")
                {
                    currentAssessment.Name = AssessmentNameEntry.Text;
                }               
                var oType = currentAssessment.AssessmentType;
                string nType = null;
                if (PACheckbox.IsChecked)
                {
                    currentAssessment.AssessmentType = nType = "PA";
                }
                else if (OACheckbox.IsChecked)
                {
                    currentAssessment.AssessmentType = nType = "OA";
                }

                if (oType != nType)
                {
                    if (currentCourse.AssessmentID == currentAssessment.AssessmentID)
                    {
                        DisplayAlert("Assessment Type Changed", "Assessment will be dropped from course. You can reassign this assessment on the Edit Course page", "OK");
                        currentCourse.AssessmentID = 0;
                    }
                    else if (currentCourse.Assessment2ID == currentAssessment.AssessmentID)
                    {
                        DisplayAlert("Assessment Type Changed", "Assessment will be dropped from course. You can reassign this assessment on the Edit Course page", "OK");
                        currentCourse.Assessment2ID = 0;
                    }
                }
                currentAssessment.AssessmentDescription = DescriptionEditor.Text;
                currentAssessment.StartDate = StartDatePicker.Date;
                currentAssessment.EndDate = EndDatePicker.Date;
                currentAssessment.StartNotification = StartSwitch.IsToggled;
                currentAssessment.EndNotification = EndSwitch.IsToggled;
                if (currentAssessment.StartNotification)
                {
                    CrossLocalNotifications.Current.Show("Assessment Start Date", currentAssessment.Name + " is scheduled to start today",
                        104, currentAssessment.StartDate);
                }
                else
                {
                    try
                    {
                        CrossLocalNotifications.Current.Cancel(104);
                    }
                    catch
                    {
                        //ignore
                    }
                }
                if (currentAssessment.EndNotification)
                {
                    CrossLocalNotifications.Current.Show("Assessment End Date", currentAssessment.Name + " is scheduled to end today",
                        105, currentAssessment.EndDate);
                }
                else
                {
                    try
                    {
                        CrossLocalNotifications.Current.Cancel(105);
                    }
                    catch
                    {
                        //ignore
                    }
                }
                if (currentCourse.StartNotification || currentCourse.EndNotification)
                {
                    CrossLocalNotifications.Current.Show("Notifications Set", "Assessment notifications have been turned on",
                        106, DateTime.Now.AddSeconds(1));
                }

                // try updating term course id and course assessment id before saving
                // problem may lie within currentCourse.AssessmentID being set to 0
                await App.Database.SaveAssessmentAsync(currentAssessment);
                //await App.Database.SaveCourseAsync(currentCourse);
                //await App.Database.SaveTermAsync(currentTerm);
                Course course = await App.Database.GetCourseAsync(currentCourse.CourseID);
                Term term = await App.Database.GetTermAsync(currentTerm.TermID);
                await Application.Current.MainPage.Navigation.PopAsync();
                //await Navigation.PushAsync(new EditCourse(course, term));

                // currently does not reload EditCourse page properly
            }
            catch
            {
                DisplayAlert("Error", "Error editing assessment. Check all input fields for accurate data.", "OK");
            }

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

            if(currentAssessment.Name != null)
            {
                AssessmentNameLabel.Text = currentAssessment.Name;
                AssessmentNameEntry.Text = currentAssessment.Name;
            }
            if(currentAssessment.AssessmentDescription != null)
            {
                DescriptionEditor.Text = currentAssessment.AssessmentDescription;
            }
            if (currentAssessment.AssessmentType == "PA")
            {
                PACheckbox.IsChecked = true;
            }
            if(currentAssessment.AssessmentType == "OA")
            {
                OACheckbox.IsChecked = true;
            }
            if(currentAssessment.StartDate != null)
            {
                StartDatePicker.Date = currentAssessment.StartDate;
            }
            if(currentAssessment.EndDate != null)
            {
                EndDatePicker.Date = currentAssessment.EndDate;
            }
            if (currentAssessment.StartNotification)
            {
                StartSwitch.IsToggled = true;
            }
            else
            {
                StartSwitch.IsToggled = false;
            }
            if (currentAssessment.EndNotification)
            {
                EndSwitch.IsToggled = true;
            }
            else
            {
                EndSwitch.IsToggled = false;
            }
        }

        private void OACheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (OACheckbox.IsChecked)
            {
                PACheckbox.IsChecked = false;
            }
        }

        private void PACheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (PACheckbox.IsChecked)
            {
                OACheckbox.IsChecked = false;
            }
        }

        private void EndDatePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }

        private void StartDatePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
    }
}