﻿using System;
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
        private Course currentCourse = new Course();
        private Term currentTerm = new Term();
        Assessment assessment = new Assessment();

        public EditCourse(Course course, Term term)
        {
            currentCourse = course;
            currentTerm = term;
            InitializeComponent();

            if(course.Status.ToUpper() == "INACTIVE")
            {
                if(DateTime.Now > term.StartDate)
                {
                    StartDatePicker.MinimumDate = DateTime.Now;
                }
                else
                {
                    StartDatePicker.MinimumDate = term.StartDate;
                }
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
                    }
                    else if(currentCourse.Assessment2ID == assessmentID)
                    {
                        currentCourse.Assessment2ID = 0;
                        Assessment2.Text = "No assessment assigned";
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
                    PAAssessmentPicker.SelectedIndex = -1;
                    PACheckbox.IsChecked = false;
                }
                else if(assessment.AssessmentType.ToUpper() == "OA")
                {
                    currentCourse.Assessment2ID = assessment.AssessmentID;
                    Assessment2.Text = assessment.Name;
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
            currentCourse.Name = CourseTitleEntry.Text;
            currentCourse.StartDate = StartDatePicker.Date;
            currentCourse.EndDate = EndDatePicker.Date;

            if (currentCourse.StartDate < DateTime.Now && CompleteCheckbox.IsChecked == false)
            {
                currentCourse.Status = "Active";
            }
            else if(currentCourse.StartDate > DateTime.Now && CompleteCheckbox.IsChecked == false)
            {
                currentCourse.Status = "Inactive";
            }
            else if(currentCourse.Status.ToUpper() == "ACTIVE" && CompleteCheckbox.IsChecked)
            {
                currentCourse.Status = "Completed";
            }
            else if(currentCourse.Status.ToUpper() == "INACTIVE" && CompleteCheckbox.IsChecked)
            {
                currentCourse.Status = "Inactive";
                await DisplayAlert("Error", "Course cannot be marked 'Completed' while inactive.", "OK");
            }
            //currentCourse.Status = CourseStatusPicker.SelectedItem.ToString();

            if(InstructorPicker.SelectedItem != null)
            {
                var instructorName = InstructorPicker.SelectedItem.ToString();
                Instructor instructor = await App.Database.GetInstructorAsync(instructorName);
                instructor.Name = InstructorName.Text;
                instructor.Phone = InstructorPhone.Text;
                instructor.Email = InstructorEmail.Text;

                await App.Database.SaveInstructorAsync(instructor);

                currentCourse.InstructorID = instructor.InstructorID;
            }

            if(Notes.Text != null)
            {
                currentCourse.Notes = Notes.Text;
            }

            if (Assessment1.Text != null && Assessment1.Text != "No assessment assigned")
            {
                var assessment1 = await App.Database.GetAssessmentAsync(Assessment1.Text);
                currentCourse.AssessmentID = assessment1.AssessmentID;
            }
            if(Assessment2.Text != null && Assessment2.Text != "No assessment assigned")
            {
                var assessment2 = await App.Database.GetAssessmentAsync(Assessment2.Text);
                currentCourse.Assessment2ID = assessment2.AssessmentID;
            }
            await App.Database.SaveCourseAsync(currentCourse);
            await Application.Current.MainPage.Navigation.PopAsync();
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
                    StatusLabel.Text = "Course Status: " + currentCourse.Status;
                }
                // loads all course instructors into Picker
                var instructors = await App.Database.GetInstructorsAsync();
                List<string> instructorNames = new List<string>();
                foreach (var i in instructors)
                {
                    if(i.Name != null)
                    {
                        instructorNames.Add(i.Name);
                    }
                }
                InstructorPicker.ItemsSource = instructorNames;
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
                        }
                    }
                    else if (a.AssessmentType.ToUpper() == "OA")
                    {
                        //oaAssessmentNames.Add(a.Name);
                        OAAssessmentPicker.Items.Add(a.Name);

                        if (a.AssessmentID == currentCourse.Assessment2ID)
                        {
                            Assessment2.Text = a.Name;
                        }
                    }
                }
                //PAAssessmentPicker.ItemsSource = paAssessmentNames;
                //OAAssessmentPicker.ItemsSource = oaAssessmentNames;
            }
            catch
            {

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
    }
}