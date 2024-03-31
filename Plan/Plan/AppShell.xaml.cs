using Plan.ViewModels;
using Plan.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Plan
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EventCreatorPage), typeof(EventCreatorPage));
        }

    }
}
