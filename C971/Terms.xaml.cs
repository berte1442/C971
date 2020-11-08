﻿using System;
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
        public bool firstLoad = true;
        List<string> courseDisplay = new List<string>();


        public Terms()
        {
            var dbCall = App.Database;

            InitializeComponent();
        }

        private async void EditTerm_Clicked(object sender, EventArgs e)
        {
            if(selectedTerm == null)
            {
                await DisplayAlert("No Term Selected", "Select term to edit", "Ok");
            }
            else
            {
                Term term = new Term();
                var terms = await App.Database.GetTermsAsync();
                selectedTerm = TermPicker.SelectedItem.ToString();

                foreach (var t in terms)
                {
                    if (selectedTerm == t.Name)
                    {
                        term = t;
                    }
                }
                await Navigation.PushAsync(new EditTerm(term));
            }
        }

        private async void DeleteTerm_Clicked(object sender, EventArgs e)
        {
            //// this button will delete an entire term
            if (selectedTerm == null)
            {
                await DisplayAlert("No Term Selected", "Select term to delete", "Ok");
            }
            else
            {
                CourseListView.ItemsSource = null;
                Term term = new Term();
                var terms = await App.Database.GetTermsAsync();
                selectedTerm = TermPicker.SelectedItem.ToString();
                courseDisplay.Clear();

                foreach (var t in terms)
                {
                    if (selectedTerm == t.Name)
                    {
                        await App.Database.DeleteTermAsync(t);
                    }
                }
                CourseListView.ItemsSource = courseDisplay;

            }
        }

        private async void AddTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTerm());
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            TermPicker.Items.Clear(); // clears before every load so not to have duplicates or deleted terms
            var terms = await App.Database.GetTermsAsync();
            
            if(terms.Count > 0)
            {
                for (int i = 0; i < terms.Count; i++)
                {
                    TermPicker.Items.Add(terms[i].Name);
                }
            }
        }

        private async void TermPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                selectedTerm = TermPicker.SelectedItem.ToString();
                var terms = await App.Database.GetTermsAsync();
                List<int> courses = new List<int>();
                //courses.Clear();
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
                foreach (var c in allCourses)
                {
                    foreach (var n in courses)
                    {
                        if (n == c.CourseID)
                        {
                            courseDisplay.Add(c.Name + " \\ " + c.Status + " \\ " + c.StartDate + "-" + c.EndDate);
                        }
                    }
                }

                CourseListView.ItemsSource = courseDisplay;
            }
            catch
            {
                await DisplayAlert("issue","Issues finding terms","ok");
            }
        }
    }
}