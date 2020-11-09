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
    public partial class EditTerm : ContentPage
    {
        Term currentTerm = new Term();
        bool firstLoad = true;
        string selectedCourse;
        List<int> courseList = new List<int>();
        List<int> removeList = new List<int>();
        List<string> termCourseDisplay = new List<string>();


        public EditTerm(Term term)
        {
            currentTerm = term;

            InitializeComponent();

            if(term.StartDate < DateTime.Now)
            {
                StartDatePicker.MinimumDate = term.StartDate;
                StartDatePicker.MaximumDate = term.StartDate;
                EndDatePicker.MinimumDate = term.EndDate;
                EndDatePicker.MaximumDate = term.EndDate;
            }
            else
            {
                StartDatePicker.MinimumDate = DateTime.Now.AddDays(1);
            }
        }

        private async void EditCourse_Clicked(object sender, EventArgs e)
        {
            selectedCourse = TermCoursesPicker.SelectedItem?.ToString();

            if (selectedCourse == null)
            {
                await DisplayAlert("No Course Selected", "Select course to edit", "Ok");
            }
            else
            {
                Course course = await App.Database?.GetCourseAsync(selectedCourse);
                Term term = await App.Database?.GetTermAsync(currentTerm.TermID);
                //var allCourses = await App.Database.GetCoursesAsync();

                //foreach (var c in allCourses)
                //{
                //    if (c.Name == selectedCourse)
                //    {
                //        course = c;
                //    }
                //}
                await Navigation.PushAsync(new EditCourse(course, term));
            } 
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            
            currentTerm.Name = TermTitleEntry.Text;
            currentTerm.StartDate = StartDatePicker.Date;
            currentTerm.EndDate = EndDatePicker.Date;

            for(int i = 0; i < removeList.Count; i++)
            {
                if (removeList.Contains(currentTerm.CourseID))
                {
                    currentTerm.CourseID = 0;
                }
                else if (removeList.Contains(currentTerm.Course2ID))
                {
                    currentTerm.Course2ID = 0;
                }
                else if (removeList.Contains(currentTerm.Course3ID))
                {
                    currentTerm.Course3ID = 0;
                }
                else if (removeList.Contains(currentTerm.Course4ID))
                {
                    currentTerm.Course4ID = 0;
                }
                else if (removeList.Contains(currentTerm.Course5ID))
                {
                    currentTerm.Course5ID = 0;
                }
                else if (removeList.Contains(currentTerm.Course6ID))
                {
                    currentTerm.Course6ID = 0;
                }
            }

            for(int i = 0; i < courseList.Count; i++)
            {
                if(currentTerm.CourseID == 0)
                {
                    currentTerm.CourseID = courseList[i];
                }
                else if(currentTerm.Course2ID == 0)
                {
                    currentTerm.Course2ID = courseList[i];
                }
                else if(currentTerm.Course3ID == 0)
                {
                    currentTerm.Course3ID = courseList[i];
                }
                else if(currentTerm.Course4ID == 0)
                {
                    currentTerm.Course4ID = courseList[i];
                }
                else if(currentTerm.Course5ID == 0)
                {
                    currentTerm.Course5ID = courseList[i];
                }
                else if(currentTerm.Course6ID == 0)
                {
                    currentTerm.Course6ID = courseList[i];
                }
            }
            await App.Database.SaveTermAsync(currentTerm);

            await DisplayAlert("Saved", "Term Successfully Edited", "Ok");
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void RemoveCourse_Clicked(object sender, EventArgs e)
        {
            var removeCourse = TermCoursesPicker.SelectedItem;
            Course course = await App.Database.GetCourseAsync(removeCourse.ToString());
            removeList.Add(course.CourseID);

            await DisplayAlert("Remove", "Course will be added to remove list", "OK");
            TermCoursesPicker.SelectedIndex = -1;
        }

        private async void AddCourse_Clicked(object sender, EventArgs e)
        {
            var addedCourse = AllCoursesPicker.SelectedItem;
            Course course = await App.Database.GetCourseAsync(addedCourse.ToString());
            courseList.Add(course.CourseID);
            await DisplayAlert("Course Added", "'" + addedCourse.ToString() + "'" + " has been added to " + currentTerm.Name + ".", "OK");
            AllCoursesPicker.SelectedIndex = -1;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //  Sets text to term name
            TermTitleEntry.Text = currentTerm.Name;
            /////////////////////////////////////
            //sets displayed dates to select terms dates
            StartDatePicker.Date = currentTerm.StartDate;
            EndDatePicker.Date = currentTerm.EndDate;
            //////////////////////////////////////

            // Adds terms courses to picker below for editing / deleting
            while (firstLoad)
            {
                List<int> termCourses = new List<int>();

                termCourses.Add(currentTerm.CourseID);
                termCourses.Add(currentTerm.Course2ID);
                termCourses.Add(currentTerm.Course3ID);
                termCourses.Add(currentTerm.Course4ID);
                termCourses.Add(currentTerm.Course5ID);
                termCourses.Add(currentTerm.Course6ID);

                var allCourses = await App.Database.GetCoursesAsync();
                //List<string> termCourseDisplay = new List<string>();

                foreach (var c in allCourses)
                {
                    foreach (var n in termCourses)
                    {
                        if (n == c.CourseID)
                        {
                            termCourseDisplay.Add(c.Name);
                        }
                    }
                }

                TermCoursesPicker.ItemsSource = termCourseDisplay;
                /////////////////////////////////////////////////////
                // provides selection of all courses to add to term - ones already in term
                List<string> allCourseDisplay = new List<string>();

                foreach (var c in allCourses)
                {
                    foreach (var n in termCourses)
                    {
                        if (n != c.CourseID && !allCourseDisplay.Contains(c.Name))
                        {
                            allCourseDisplay.Add(c.Name);
                        }
                    }
                }

                AllCoursesPicker.ItemsSource = allCourseDisplay;

                firstLoad = false;
            }
        }

        private void StartDatePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(currentTerm.StartDate <= DateTime.Now)
            {
                StartDatePicker.MinimumDate = currentTerm.StartDate;
                StartDatePicker.MaximumDate = currentTerm.StartDate;
                EndDatePicker.MinimumDate = currentTerm.EndDate;
                EndDatePicker.MaximumDate = currentTerm.EndDate;

                // display was taken out because it was taking forever to clear out 
                //DisplayAlert("Term in progress", "Term dates cannot be changed once term has started.", "OK");
            }
            else
            {
                EndDatePicker.MaximumDate = StartDatePicker.Date.AddMonths(6);
                EndDatePicker.MinimumDate = StartDatePicker.Date.AddMonths(6);
            }
        }

        private void EndDatePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
    }
}