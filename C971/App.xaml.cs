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

            MainPage = new NavigationPage(new LoginEntryPage());
        }

        public static StudentAcademicsDB Database
        {
            get
            {
                if(database == null)
                {
                    database = new StudentAcademicsDB(DependencyService.Get<ILocalFileHelper>().GetLocalFilePath("StudentAcademics.db3"));
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
