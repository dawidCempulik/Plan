using Plan.Services;
using Plan.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plan
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent(); 

            MainPage = new AppShell();
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
