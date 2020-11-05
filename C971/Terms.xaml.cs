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
    public partial class Terms : ContentPage
    {
        public Terms()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditTerm());
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            // delete
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTerm());
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
            // save
        }

        private void Button_Clicked_4(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopToRootAsync();   
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            
            CourseListView.ItemsSource = await App.Database.GetCoursesAsync();  // this code will need to change to getting course info from individual terms
        }
    }
}