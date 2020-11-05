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
        public EditCourse()
        {
            InitializeComponent();
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
    }
}