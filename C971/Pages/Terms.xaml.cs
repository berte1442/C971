﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;



// Java.Lang.NullPointerException: 'Attempt to invoke virtual method 'java.lang.String java.lang.Object.toString()' on a null object reference'
// OnAppearing after term has been edited.
//
// program storage has to be cleared after this happens

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Terms : ContentPage
    {
        public string selectedTerm;
        public bool firstLoad = true;

        public Terms()
        {
            InitializeComponent();
            OnAppearing();
        }

        private async void AddTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTerm());
        }

        private async void ViewTerm_Clicked(object sender, EventArgs e)
        {
            if (TermPicker.SelectedIndex == -1)
            {
                await DisplayAlert("No Term Selected", "Select term to view", "Ok");
            }
            else
            {
                Term term = await App.Database.GetTermAsync(TermPicker.SelectedItem.ToString());

                await Navigation.PushAsync(new ViewTerm(term));
            }
        }

        private async void EditTerm_Clicked(object sender, EventArgs e)
        {
            if (TermPicker.SelectedIndex == -1)
            {
                await DisplayAlert("No Term Selected", "Select term to edit", "Ok");
            }
            else
            {           
                Term term = await App.Database.GetTermAsync(TermPicker.SelectedItem.ToString());

                await Navigation.PushAsync(new EditTerm(term));
            }
        }

        private async void DeleteTerm_Clicked(object sender, EventArgs e)
        {
            //// this button will delete an entire term
            if (TermPicker.SelectedIndex == -1)
            {
                await DisplayAlert("No Term Selected", "Select term to delete", "Ok");
            }
            else
            {
                var action = await DisplayAlert("Delete?", "Are you sure to delete this term?", "Yes", "No");
                if (action)
                {
                    List<string> courseDisplay = new List<string>();
                    CourseListView.ItemsSource = null;
                    Term term = new Term();
                    var terms = await App.Database.GetTermsAsync();
                    selectedTerm = TermPicker.SelectedItem.ToString();

                    foreach (var t in terms)
                    {
                        if (selectedTerm == t.Name)
                        {
                            await App.Database.DeleteTermAsync(t);
                        }
                    }
                    TermPicker.SelectedIndex = -1;
                    TermPicker.Items.Remove(selectedTerm);
                    CourseListView.ItemsSource = courseDisplay;
                    selectedTerm = null;
                    await DisplayAlert("Deleted", "Term successfully deleted", "OK");
                }
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var terms = await App.Database.GetTermsAsync();

            var courses = await App.Database.GetCoursesAsync();

            foreach(var c in courses)
            {
                if(c.StartDate < DateTime.Now && c.EndDate > DateTime.Now)
                {
                    c.Status = "Active";
                }
                else if(DateTime.Now > c.EndDate)
                {
                    c.Status = "Completed";
                }
            }
            if(terms.Count > 0)
            {
                TermPicker.Items.Clear(); // clears before every load so not to have duplicates or deleted terms

                for (int i = 0; i < terms.Count; i++)
                {
                    TermPicker.Items.Add(terms[i].Name);
                }
            }
            CourseListView.ItemsSource = null;
        }
        
        private async void TermPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                List<string> courseDisplay = new List<string>();

                selectedTerm = TermPicker.SelectedItem.ToString();
                var terms = await App.Database.GetTermsAsync();
                List<int> courses = new List<int>();

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

                string courseDates = null;
                foreach (var c in allCourses)
                {
                    foreach (var n in courses)
                    {
                        if (n == c.CourseID)
                        {
                            if(c.Status.ToUpper() == "INACTIVE" && c.StartDate < DateTime.Now)
                            {
                                courseDates = "Course dates not set";
                                //await App.Database.SaveCourseAsync(c);
                            }
                            else
                            {
                                courseDates = c.StartDate.ToShortDateString() + "-" + c.EndDate.ToShortDateString();
                            }
                            courseDisplay.Add(c.Name + " \\ " + c.Status + " \\ " + courseDates);
                        }
                    }
                }

                CourseListView.ItemsSource = courseDisplay;
            }
            catch
            {
            }
        }

        private async void EditCourse_Clicked(object sender, EventArgs e)
        {
            if(CourseListView.SelectedItem != null && TermPicker.SelectedIndex != -1)
            {
                Term term = await App.Database.GetTermAsync(selectedTerm);
                var selectedCourse = CourseListView.SelectedItem;
                int position = selectedCourse.ToString().IndexOf("\\");
                var courseName = selectedCourse.ToString().Substring(0, position);
                courseName = courseName.Trim();
                Course course = await App.Database.GetCourseAsync(courseName);
                await Navigation.PushAsync(new EditCourse(course, term));
            }
            else
            {
                await DisplayAlert("No Course Selected", "Select course to edit", "OK");
            }
        }

        private async void CourseInfo_Clicked(object sender, EventArgs e)
        {
            if (CourseListView.SelectedItem != null && TermPicker.SelectedIndex != -1)
            {
                var selectedCourse = CourseListView.SelectedItem;
                int position = selectedCourse.ToString().IndexOf("\\");
                var courseName = selectedCourse.ToString().Substring(0, position);
                courseName = courseName.Trim();
                Course course = await App.Database.GetCourseAsync(courseName);
                Term term = await App.Database.GetTermAsync(selectedTerm);

                await Navigation.PushAsync(new CourseInfo(course, term));
            }
            else
            {
                await DisplayAlert("No Course Selected", "Select course to view course info", "OK");
            }
        }


        //public async void Term_Conflict()
        //{
        //    List<Term> terms = await App.Database.GetTermsAsync();
        //    List<int> courseIDs = new List<int>;

        //    foreach(var t in terms)
        //    {
        //        courseIDs.Add(t.CourseID);
        //        courseIDs.Add(t.Course2ID);
        //        courseIDs.Add(t.Course3ID);
        //        courseIDs.Add(t.Course4ID);
        //        courseIDs.Add(t.Course5ID);
        //        courseIDs.Add(t.Course6ID);
        //    }
        //}
    }
}