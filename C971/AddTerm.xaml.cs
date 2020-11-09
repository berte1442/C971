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
    public partial class AddTerm : ContentPage
    {
        bool firstLoad = true;
        List<int> addedCourses = new List<int>();
        string selectedCourse;

        public AddTerm()
        {
            InitializeComponent();

            StartDatePicker.MinimumDate = DateTime.Now.AddDays(1);
            EndDatePicker.MinimumDate = DateTime.Now.AddMonths(6);
        }

        private async void AddCourse_Clicked(object sender, EventArgs e)
        {
            // add course to new term
            selectedCourse = AddCoursesPicker.SelectedItem.ToString();
            var courses = await App.Database.GetCoursesAsync();

            if(addedCourses.Count < 6)
            {
                foreach (var c in courses)
                {
                    if (selectedCourse == c.Name)
                    {
                        addedCourses.Add(c.CourseID);
                    }
                }
            }
            else
            {
                await DisplayAlert("Course Max Reached", "You may only add 6 courses to a term.", "OK");
            }
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            // Save 
            // then close
            Term term = new Term();
            term.Name = TitleEntry.Text;
            term.StartDate = StartDatePicker.Date;
            term.EndDate = EndDatePicker.Date;

            for(int i = 0; i < addedCourses.Count; i++)
            {
                if(term.CourseID == 0)
                {
                    term.CourseID = addedCourses[i];
                }
                else if(term.Course2ID == 0)
                {
                    term.Course2ID = addedCourses[i];
                }
                else if(term.Course3ID == 0)
                {
                    term.Course3ID = addedCourses[i];
                }               
                else if(term.Course4ID == 0)
                {
                    term.Course4ID = addedCourses[i];
                }              
                else if(term.Course5ID == 0)
                {
                    term.Course5ID = addedCourses[i];
                }              
                else if(term.Course6ID == 0)
                {
                    term.Course6ID = addedCourses[i];
                }
            }

            await App.Database.SaveTermAsync(term);
            
            await DisplayAlert("Saved", "Term Successfully Created", "Ok");
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            StartDatePicker.Date = DateTime.Now.AddDays(1);
            EndDatePicker.Date = StartDatePicker.Date.AddMonths(6);

            var courses = await App.Database.GetCoursesAsync();
            //TermPicker.Items.Clear();
            while (firstLoad)
            {
                for (int i = 0; i < courses.Count; i++)
                {
                    AddCoursesPicker.Items.Add(courses[i].Name);
                }
                firstLoad = false;
            }
        }

        private void StartDatePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            EndDatePicker.MaximumDate = StartDatePicker.Date.AddMonths(6);
            EndDatePicker.MinimumDate = StartDatePicker.Date.AddMonths(6);
        }

        private void EndDatePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }
    }
}