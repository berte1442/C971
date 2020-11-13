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
    public partial class ViewTerm : ContentPage
    {
        Term currentTerm = new Term();
        public ViewTerm(Term term)
        {
            currentTerm = term;
            InitializeComponent();
        }

        private async void EditTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditTerm(currentTerm));
        }

        private void Home_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            TermNameLabel.Text = "\"" + currentTerm?.Name + "\"";
            TermDatesLabel.Text = currentTerm?.StartDate.ToShortDateString() + " - " + currentTerm?.EndDate.ToShortDateString();

            string courses = null;
            if(currentTerm.CourseID != 0)
            {
                var course1 = await App.Database.GetCourseAsync(currentTerm.CourseID);
                courses = courses + course1.Name + "\n";
            }
            if(currentTerm.Course2ID != 0)
            {
                var course2 = await App.Database.GetCourseAsync(currentTerm.Course2ID);
                courses = courses + course2.Name + "\n";
            }
            if (currentTerm.Course3ID != 0)
            {
                var course3 = await App.Database.GetCourseAsync(currentTerm.Course3ID);
                courses = courses + course3.Name + "\n";
            }
            if (currentTerm.Course4ID != 0)
            {
                var course4 = await App.Database.GetCourseAsync(currentTerm.Course4ID);
                courses = courses + course4.Name + "\n";
            }
            if (currentTerm.Course5ID != 0)
            {
                var course5 = await App.Database.GetCourseAsync(currentTerm.Course5ID);
                courses = courses + course5.Name + "\n";
            }
            if (currentTerm.Course6ID != 0)
            {
                var course6 = await App.Database.GetCourseAsync(currentTerm.Course6ID);
                courses = courses + course6.Name + "\n";
            }

            CoursesLabel.Text = courses;
        }

    }
}