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
        public EditTerm(Term term)
        {
            InitializeComponent();

            currentTerm = term;
        }

        private async void EditCourse_Clicked(object sender, EventArgs e)
        {
            selectedCourse = TermCoursesPicker.SelectedItem.ToString();

            if (selectedCourse == null)
            {
                await DisplayAlert("No Course Selected", "Select course to edit", "Ok");
            }
            else
            {
                Course course = new Course();
                var allCourses = await App.Database.GetCoursesAsync();

                foreach (var c in allCourses)
                {
                    if (c.Name == selectedCourse)
                    {
                        course = c;
                    }
                }
                await Navigation.PushAsync(new EditCourse(course));
            } 
        }

        private void Save_Clicked(object sender, EventArgs e)
        {

        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopToRootAsync();   
        }

        private void RemoveCourse_Clicked(object sender, EventArgs e)
        {

        }

        private void AddCourse_Clicked(object sender, EventArgs e)
        {

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //  Sets placeholder to term name
            TermTitleEntry.Placeholder = currentTerm.Name;
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
                List<string> termCourseDisplay = new List<string>();

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
    }
}