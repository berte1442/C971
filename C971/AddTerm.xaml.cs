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
        string lastCourse;

        public AddTerm()
        {
            InitializeComponent();

            StartDatePicker.MinimumDate = DateTime.Now.AddDays(1);
            EndDatePicker.MinimumDate = DateTime.Now.AddMonths(6);
        }

       

        private async void Save_Clicked(object sender, EventArgs e)
        {
            if(TitleEntry.Text != null)
            {
                Term term = new Term();
                term.Name = TitleEntry.Text;
                term.StartDate = StartDatePicker.Date;
                term.EndDate = EndDatePicker.Date;

                for (int i = 0; i < addedCourses.Count; i++)
                {
                    if (term.CourseID == 0)
                    {
                        term.CourseID = addedCourses[i];
                    }
                    else if (term.Course2ID == 0)
                    {
                        term.Course2ID = addedCourses[i];
                    }
                    else if (term.Course3ID == 0)
                    {
                        term.Course3ID = addedCourses[i];
                    }
                    else if (term.Course4ID == 0)
                    {
                        term.Course4ID = addedCourses[i];
                    }
                    else if (term.Course5ID == 0)
                    {
                        term.Course5ID = addedCourses[i];
                    }
                    else if (term.Course6ID == 0)
                    {
                        term.Course6ID = addedCourses[i];
                    }
                }

                await App.Database.SaveTermAsync(term);

                await DisplayAlert("Saved", "Term Successfully Created", "Ok");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                DisplayAlert("Name Required", "You must give the term a name in order to save.", "OK");
            }
        }
            

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
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
            EndDatePicker.Date = StartDatePicker.Date.AddMonths(6);
        }

        private void EndDatePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }
        private async void AddCourse_Clicked(object sender, EventArgs e)
        {
            if (AddCoursesPicker.SelectedIndex != -1)
            {
                selectedCourse = AddCoursesPicker.SelectedItem.ToString();
                var courses = await App.Database.GetCoursesAsync();

                if (addedCourses.Count < 6)
                {
                    foreach (var c in courses)
                    {
                        if (selectedCourse == c.Name)
                        {
                            c.StartDate = DateTime.Now.AddMonths(1);
                            c.EndDate = c.StartDate.AddMonths(1);
                            await App.Database.SaveCourseAsync(c);
                            addedCourses.Add(c.CourseID);
                            AddCoursesPicker.Items.Remove(c.Name);
                            lastCourse = c.Name;
                            //DisplayAlert("Course Added", "Course will be added to term.", "OK");
                            AddedCourseLabel.Text = AddedCourseLabel.Text + ", " + c.Name;
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Course Max Reached", "You may only add 6 courses to a term.", "OK");
                }
                AddCoursesPicker.SelectedIndex = -1;
            }
            else
            {
                DisplayAlert("Select Course", "Select a course to add to term", "OK");
            }

        }
        private async void Undo_Clicked(object sender, EventArgs e)
        {
            if (addedCourses.Count > 0)
            {
                var courses = await App.Database.GetCoursesAsync();

                foreach (var c in courses)
                {
                    if (lastCourse == c.Name)
                    {
                        addedCourses.Remove(c.CourseID);
                        AddCoursesPicker.Items.Add(c.Name);

                        int commaIndex = AddedCourseLabel.Text.LastIndexOf(",");
                        string newString = AddedCourseLabel.Text.Substring(0, commaIndex);

                        AddedCourseLabel.Text = newString;

                        int newStringLength = newString.Length;
                        if (newStringLength > 0)
                        {
                            int newCommaIndex = newString.LastIndexOf(",");
                            int length = newStringLength - newCommaIndex;

                            lastCourse = newString.Substring(newCommaIndex + 2, length -2);
                            break;
                        }
                    }
                }
            }         
        }
    }
}