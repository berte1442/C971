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
    public partial class AssessmentDetails : ContentPage
    {
        string assessmentType;
        public AssessmentDetails(Assessment assessment)
        {
            var currentAssessment = assessment;

            InitializeComponent();
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void Home_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        private void OACheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (OACheckBox.IsChecked)
            {
                PACheckBox.IsChecked = false;
                assessmentType = "OA";
            }
        }

        private void PACheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (PACheckBox.IsChecked)
            {
                OACheckBox.IsChecked = false;
                assessmentType = "PA";
            }
        }

        private void StartDatePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void EndDatePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        //protected async override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    //var assessments = await App.Database.GetAssessmentsAsync();
        //    //List<string> assessmentNames = new List<string>();
        //    //foreach(var a in assessments)
        //    //{
        //    //    assessmentNames.Add(a.Name);
        //    //}
        //    //AssessmentPicker.ItemsSource = assessmentNames;
        //}
    }
}