using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971
{
    public partial class App : Application
    {
        static StudentAcademicsDB database;

        public App()
        {
            InitializeComponent();
            DataInsert.DatabaseCheck();

            MainPage = new NavigationPage(new Terms());  //bypass login page since it was not required
        }

        public static StudentAcademicsDB Database
        {
            get
            {
                if(database == null)
                {
                    database = new StudentAcademicsDB(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "C971Database");
                }
                return database;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
