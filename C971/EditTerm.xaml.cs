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

        int[] termCourses = new int[6];
        //List<int> termCourses = new List<int>();

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
            if(TermTitleEntry.Text != "")
            {
                currentTerm.Name = TermTitleEntry.Text;
            }
            currentTerm.StartDate = StartDatePicker.Date;
            currentTerm.EndDate = EndDatePicker.Date;

            currentTerm.CourseID = termCourses[0];
            currentTerm.Course2ID = termCourses[1];
            currentTerm.Course3ID = termCourses[2];
            currentTerm.Course4ID = termCourses[3];
            currentTerm.Course5ID = termCourses[4];
            currentTerm.Course6ID = termCourses[5];

            await App.Database.SaveTermAsync(currentTerm);

            await DisplayAlert("Saved", "Term Successfully Edited", "Ok");
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        //private async void RemoveCourse_Clicked(object sender, EventArgs e)
        //{ 
        //    if(TermCoursesPicker.SelectedIndex != -1)
        //    {
        //        var removeCourse = TermCoursesPicker.SelectedItem;
        //        Course course = await App.Database.GetCourseAsync(removeCourse.ToString());
        //        if (course.Status.ToUpper() == "ACTIVE" || course.Status.ToUpper() == "COMPLETE")
        //        {
        //            await DisplayAlert("Restricted", "Course has already started and cannot be removed", "OK");
        //        }
        //        else
        //        {
        //            //termCourses.Remove(course.CourseID);
        //            for (int i = 0; i < termCourses.Length; i++)
        //            {
        //                if (termCourses[i] == course.CourseID)
        //                {
        //                    termCourses[i] = 0;
        //                }
        //            }
        //            await DisplayAlert("Remove", "Course will be removed from term", "OK");
        //        }

        //        TermCoursesPicker.SelectedIndex = -1;
        //        OnAppearing();
        //    }
        //    else
        //    {
        //        await DisplayAlert("No Course Selected", "Select course to remove", "OK");
        //    }
        //}

        //private async void AddCourse_Clicked(object sender, EventArgs e)
        //{
        //    while(AllCoursesPicker.SelectedIndex != -1)
        //    {
        //        var addedCourse = AllCoursesPicker.SelectedItem;
        //        Course course = await App.Database.GetCourseAsync(addedCourse.ToString());

        //        if (!termCourses.Contains(course.CourseID))
        //        {
        //            bool available = false;
        //            for (int i = 0; i < termCourses.Length; i++)
        //            {
        //                if (termCourses[i] == 0)
        //                {
        //                    course.StartDate = DateTime.Now.AddDays(30);
        //                    termCourses[i] = course.CourseID;
        //                    await DisplayAlert("Course Added", "'" + addedCourse.ToString() + "'" + " has been added to " + currentTerm.Name + ".", "OK");
        //                    //AllCoursesPicker.Items.Remove(addedCourse.ToString());
        //                    //TermCoursesPicker.Items.Add(addedCourse.ToString());}
        //                    available = true;
        //                    break;
        //                }
        //            }
        //            if (available == false)
        //            {
        //                await DisplayAlert("Error", "Only six courses can be assigned to term", "OK");
        //            }
        //        }
        //        else
        //        {
        //            await DisplayAlert("Error", "Course already assigned to term", "OK");
        //        }
        //        AllCoursesPicker.SelectedIndex = -1;
        //    }
        //}

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
                termCourses[0] = currentTerm.CourseID;
                termCourses[1] = currentTerm.Course2ID;
                termCourses[2] = currentTerm.Course3ID;
                termCourses[3] = currentTerm.Course4ID;
                termCourses[4] = currentTerm.Course5ID;
                termCourses[5] = currentTerm.Course6ID;

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

                //AllCoursesPicker.ItemsSource = allCourseDisplay;

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