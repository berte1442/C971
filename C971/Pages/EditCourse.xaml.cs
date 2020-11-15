using Android.Content;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Android;
using Xamarin.Essentials;
using Plugin.LocalNotifications;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditCourse : ContentPage
    {
        private Course currentCourse = new Course();
        private Term currentTerm = new Term();
        Assessment assessment = new Assessment();
        string barePhone;
        bool publicNotes = false;

        public EditCourse(Course course, Term term)
        {
            currentCourse = course;
            currentTerm = term;
            InitializeComponent();

            if(course.Status.ToUpper() == "INACTIVE")
            {
                StartDatePicker.MinimumDate = term.StartDate;
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
            
            if (PAAssessmentCheckBox.IsChecked && Assessment1.Text != null && Assessment1.Text != "No assessment assigned")
            {
                assessmentName = Assessment1.Text;
            }
            else if(OAAssessmentCheckBox.IsChecked && Assessment2.Text != null && Assessment2.Text != "No assessment assigned")
            {
                assessmentName = Assessment2.Text;
            }
            else
            {
                await DisplayAlert("Select Assessment", "Select an assessment to edit.", "OK");
            }

            if(assessmentName != null)
            {
                assessment = await App.Database.GetAssessmentAsync(assessmentName);
                await Navigation.PushAsync(new EditAssessment(assessment, currentCourse, currentTerm));
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
                        Assessment1.Text = "No assessment assigned";
                        PAAssessmentPicker.Items.Add(assessmentName);
                    }
                    else if(currentCourse.Assessment2ID == assessmentID)
                    {
                        currentCourse.Assessment2ID = 0;
                        Assessment2.Text = "No assessment assigned";
                        OAAssessmentPicker.Items.Add(assessmentName);
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
                    PAAssessmentPicker.Items.Remove(assessment.Name);
                    PAAssessmentPicker.SelectedIndex = -1;
                    PACheckbox.IsChecked = false;
                }
                else if(assessment.AssessmentType.ToUpper() == "OA")
                {
                    currentCourse.Assessment2ID = assessment.AssessmentID;
                    Assessment2.Text = assessment.Name;
                    OAAssessmentPicker.Items.Remove(assessment.Name);
                    OAAssessmentPicker.SelectedIndex = -1;
                    OACheckbox.IsChecked = false;
                }

                await DisplayAlert("Assessment Added", "Assessment will be added to course", "OK");
            }
            else
            {
                if((PACheckbox.IsChecked == false && OACheckbox.IsChecked == false) || (PAAssessmentPicker.SelectedIndex == -1 && OAAssessmentPicker.SelectedIndex == -1) ||
                    (PACheckbox.IsChecked && PAAssessmentPicker.SelectedIndex == -1) || (OACheckbox.IsChecked && OAAssessmentPicker.SelectedIndex == -1))
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
            try 
            { 
                bool emailValidate = true;
                bool phoneValidate = true;

                if (InstructorEmail.Text != null)
                {
                    emailValidate = Email_Validate(InstructorEmail.Text);
                }
                if(InstructorPhone.Text != null)
                {
                    phoneValidate = Phone_Validate(InstructorPhone?.Text);
                }

                if (emailValidate == false)
                {
                    DisplayAlert("Invalid Email", "Invalid email address for course instructor.", "OK");
                }
                else if (phoneValidate == false)
                {
                    DisplayAlert("Invalid Phone", "Invalid phone number for course instructor.", "OK");
                }
                else
                {
                    if (CourseTitleEntry.Text != null && CourseTitleEntry.Text != "")
                    {
                        currentCourse.Name = CourseTitleEntry.Text;
                    }
                    currentCourse.StartDate = StartDatePicker.Date;
                    currentCourse.EndDate = EndDatePicker.Date;
                    currentCourse.StartNotification = StartSwitch.IsToggled;
                    currentCourse.EndNotification = EndSwitch.IsToggled;
                    if (NotesSwitch.IsToggled)
                    {
                        currentCourse.NotesPublic = true;
                        ShareNotes sms = new ShareNotes();
                        //sms.SendSms(currentCourse.Notes, InstructorPhone.Text);
                        List<string> email = new List<string>();
                        email.Add(InstructorEmail.Text);
                        sms.SendEmail("'Student Name' - " + currentCourse.Name + " Notes", currentCourse.Notes, email);
                    }
                    else
                    {
                        currentCourse.NotesPublic = false;
                    }
                    if(currentCourse.StartNotification)
                    {
                        CrossLocalNotifications.Current.Show("Course Started", currentCourse.Name + " starts today",
                            101, currentCourse.StartDate);
                    }
                    else
                    {
                        try
                        {
                            CrossLocalNotifications.Current.Cancel(101);
                        }
                        catch
                        {
                            //ignore
                        }
                    } 
                    if(currentCourse.EndNotification)
                    {
                        CrossLocalNotifications.Current.Show("Course Ended", currentCourse.Name + " ended today",
                            102, currentCourse.StartDate);
                    }
                    else
                    {
                        try
                        {
                            CrossLocalNotifications.Current.Cancel(102);
                        }
                        catch
                        {
                            //ignore
                        }
                    }
                    if(currentCourse.StartNotification || currentCourse.EndNotification)
                    {
                        CrossLocalNotifications.Current.Show("Notifications Set", "Course notifications have been turned on",
                            103, DateTime.Now.AddSeconds(1));
                    }
                    if (CourseStatusPicker.SelectedIndex != -1)
                    {
                        currentCourse.Status = CourseStatusPicker.SelectedItem.ToString();
                    }

                    if (InstructorPicker.SelectedItem != null)
                    {
                        var instructorName = InstructorPicker.SelectedItem.ToString();
                        Instructor instructor = await App.Database.GetInstructorAsync(instructorName);
                        instructor.Name = InstructorName.Text;
                        string phone = barePhone;
                        if (phone.Length == 10)
                        {
                            phone = phone.Insert(0, "(");
                            phone = phone.Insert(4, ")");
                            phone = phone.Insert(8, "-");
                        }
                        else if (phone.Length == 7)
                        {
                            phone.Insert(3, "-");
                        }
                        instructor.Phone = phone;
                        instructor.Email = InstructorEmail.Text;

                        await App.Database.SaveInstructorAsync(instructor);

                        currentCourse.InstructorID = instructor.InstructorID;
                    }

                    if (NotesSwitch.IsToggled)
                    {
                        currentCourse.NotesPublic = true;

                        // add code for sending sms here
                    }
                    else
                    {
                        currentCourse.NotesPublic = false;
                    }

                    if (Notes.Text != null)
                    {
                        currentCourse.Notes = Notes.Text;
                    }

                    if (Assessment1.Text != null && Assessment1.Text != "No assessment assigned")
                    {
                        var assessment1 = await App.Database.GetAssessmentAsync(Assessment1.Text);
                        currentCourse.AssessmentID = assessment1.AssessmentID;
                    }
                    if (Assessment2.Text != null && Assessment2.Text != "No assessment assigned")
                    {
                        var assessment2 = await App.Database.GetAssessmentAsync(Assessment2.Text);
                        currentCourse.Assessment2ID = assessment2.AssessmentID;
                    }
                    await App.Database.SaveCourseAsync(currentCourse);
                    await Application.Current.MainPage.Navigation.PopAsync();

                    DisplayAlert("Saved", "Course edit has been saved.", "OK");
                }
            }
            catch
            {
                await DisplayAlert("Error", "Error editing course. Check all input fields for accurate data.", "OK");
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
            try
            {
                base.OnAppearing();
                //sets noteswitch to correct on/off position
                if (currentCourse.NotesPublic)
                {
                    NotesSwitch.IsToggled = true;
                }
                else
                {
                    NotesSwitch.IsToggled = false;
                }
                //sets startswitch to correct on/off position
                if (currentCourse.StartNotification)
                {
                    StartSwitch.IsToggled = true;
                }
                else
                {
                    StartSwitch.IsToggled = false;
                } 
                //sets endswitch to correct on/off position
                if (currentCourse.EndNotification)
                {
                    EndSwitch.IsToggled = true;
                }
                else
                {
                    EndSwitch.IsToggled = false;
                }
                //sets selected course name as text for editor
                if(currentCourse.Name != null)
                {
                    CourseTitleEntry.Text = currentCourse.Name;
                }
                /////////
                // sets course dates to selected course start and end dates
                if(currentCourse.StartDate != null)
                {
                    StartDatePicker.Date = currentCourse.StartDate;
                }
                if(currentCourse.EndDate != null)
                {
                    EndDatePicker.Date = currentCourse.EndDate;
                }
                //sets selected course status 
                //CourseStatusPicker.SelectedItem = currentCourse.Status;
                if(currentCourse.Status != null)
                {
                    //StatusLabel.Text = "Course Status: " + currentCourse.Status;
                    CourseStatusPicker.SelectedItem = currentCourse.Status;
                }
                // loads all course instructors into Picker
                var instructors = await App.Database.GetInstructorsAsync();
                //List<string> instructorNames = new List<string>();
                foreach (var i in instructors)
                {
                    if(i.Name != null)
                    {
                        InstructorPicker.Items.Add(i.Name);
                        //instructorNames.Add(i.Name);
                    }
                }
                //InstructorPicker.ItemsSource = instructorNames;
                foreach (var i in instructors)
                {
                    if (i.InstructorID == currentCourse.InstructorID)
                    {
                        if(i.Name != null)
                        {
                            InstructorPicker.SelectedItem = i.Name;
                            InstructorName.Text = i.Name;
                        }
                        if(i.Phone != null)
                        {
                            InstructorPhone.Text = i.Phone;
                        }
                        if(i.Email != null)
                        {
                            InstructorEmail.Text = i.Email;
                        }
                    }
                }
                if(currentCourse.Notes != null)
                {
                    Notes.Text = currentCourse.Notes;
                }
                // sets assessment pickers and assessment labels

                var assessments = await App.Database.GetAssessmentsAsync();
                //List<string> paAssessmentNames = new List<string>();
                //List<string> oaAssessmentNames = new List<string>();
                Assessment1.Text = "No assessment assigned";
                Assessment2.Text = "No assessment assigned";
                PAAssessmentPicker.Items.Clear();
                OAAssessmentPicker.Items.Clear();
                foreach (var a in assessments)
                {
                    if (a.AssessmentType.ToUpper() == "PA")
                    {
                        //paAssessmentNames.Add(a.Name);
                        PAAssessmentPicker.Items.Add(a.Name);

                        if (a.AssessmentID == currentCourse.AssessmentID)
                        {
                            Assessment1.Text = a.Name;
                            PAAssessmentPicker.Items.Remove(a.Name);
                        }
                    }
                    else if (a.AssessmentType.ToUpper() == "OA")
                    {
                        //oaAssessmentNames.Add(a.Name);
                        OAAssessmentPicker.Items.Add(a.Name);

                        if (a.AssessmentID == currentCourse.Assessment2ID)
                        {
                            Assessment2.Text = a.Name;
                            OAAssessmentPicker.Items.Remove(a.Name);
                        }
                    }
                }
                if(currentCourse.NotesPublic == false)
                {
                    NotesSwitch.IsToggled = false;
                }
                else
                {
                    NotesSwitch.IsToggled = true;
                }
            }
            catch
            {
                await DisplayAlert("Error", "Error loading course", "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
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

        private async void InstructorPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InstructorPicker.SelectedItem != null)
            {
                Instructor instructor = await App.Database.GetInstructorAsync(InstructorPicker.SelectedItem.ToString());
                if(instructor.Name != null)
                {
                    InstructorName.Text = instructor.Name;
                }
                if(instructor.Phone != null)
                {
                    InstructorPhone.Text = instructor.Phone;
                }
                if(instructor.Email != null)
                {
                    InstructorEmail.Text = instructor.Email;
                }
            }
        }

        private void NotesSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }
        public static bool Email_Validate(string email)
        {
            var emailLength = email.Length;
            var atIndex = email.IndexOf("@");
            var dotIndex = email.LastIndexOf(".");

            var domain = email.Substring(dotIndex + 1);
            var domainValidate = Domain_Check(domain);        

            if(atIndex == -1 || dotIndex == -1 || dotIndex < atIndex || (dotIndex + 3) >= emailLength 
                || atIndex < 3 || (atIndex + 5) >= emailLength || !((dotIndex - atIndex) > 3) || domainValidate == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool Phone_Validate(string phone)
        {
            phone = phone.Trim();
            phone = phone.Replace("(", "");
            phone = phone.Replace(")", "");
            phone = phone.Replace("-", "");
            
            bool result = phone.Any(x => char.IsLetter(x));
            if ((phone.Length != 10 && phone.Length != 7) || result)
            {
                return false;
            }
            else
            {
                barePhone = phone;
                return true;
            }
        }   
        public static bool Domain_Check(string domain)
        {
            if(domain != "com" && domain != "edu" && domain != "gov" && domain != "net" && domain != "org")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void StartSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {


        }

        private void EndSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
    }
}