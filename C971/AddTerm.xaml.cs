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
        public AddTerm()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            // add course to new term
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            // Save 
            // then close
            DisplayAlert("Saved", "Term Successfully Created", "Ok");
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopToRootAsync();
        }
    }
}