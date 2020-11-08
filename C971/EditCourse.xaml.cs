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
        Course selectedCourse;
        public EditCourse(Course course)
        {
            InitializeComponent();

            selectedCourse = course;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditAssessment());
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddAssessment());
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
            //save
        }

        private void Button_Clicked_4(object sender, EventArgs e)
        {
            //cancel
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void Button_Clicked_5(object sender, EventArgs e)
        {
            //Log out
            Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //sets selected course name as placeholder for editor
            CourseTitleEntry.Placeholder = selectedCourse.Name;
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
        }

    }
}