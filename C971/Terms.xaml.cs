using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Terms : ContentPage
    {
        public string selectedTerm;

        public Terms()
        {
            InitializeComponent();
        }

        private async void EditTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditTerm());
        }

        private void DeleteTerm_Clicked(object sender, EventArgs e)
        {
            //// this button will delete an entire term
            //StudentAcademicsDB studentAcademicsDB = new StudentAcademicsDB(App.Database.ToString());
            //studentAcademicsDB.DeleteTermAsync();
        }

        private async void AddTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTerm());
        }

        //private void Button_Clicked_3(object sender, EventArgs e)
        //{
        //    // save
        //}

        private void Logout_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopToRootAsync();   
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var terms = await App.Database.GetTermsAsync();
            TermPicker.Items.Clear();

            for (int i = 0; i < terms.Count; i++)
            {
                TermPicker.Items.Add(terms[i].Name);                            
            }

            //CourseListView.ItemsSource = await App.Database.GetCoursesAsync();  // this code will need to change to getting course info from individual terms
        }

        private async void TermPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTerm = TermPicker.SelectedItem.ToString();
            var terms = await App.Database.GetTermsAsync();
            List<int> courses = new List<int>();
            courses.Clear();
            for (int i = 0; i < terms.Count; i++)
            {
                if (selectedTerm == terms[i].Name)
                {
                    courses.Add(terms[i].CourseID);
                    courses.Add(terms[i].Course2ID);
                    courses.Add(terms[i].Course3ID);
                    courses.Add(terms[i].Course4ID);
                    courses.Add(terms[i].Course5ID);
                    courses.Add(terms[i].Course6ID);                  
                }
            }

            var allCourses = await App.Database.GetCoursesAsync();
            List<string> courseDisplay = new List<string>();
            foreach(var c in allCourses)
            {
                foreach(var n in courses)
                {
                    if(n == c.CourseID)
                    {
                        courseDisplay.Add(c.Name + " " + c.Status + " " + c.StartDate + "-" + c.EndDate);
                    }
                }
            }

            
            CourseListView.ItemsSource = courseDisplay;
        }
    }
}