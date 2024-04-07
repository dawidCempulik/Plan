using Plan.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plan.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DayCalendarPage : ContentPage
    {
        public DayCalendarPage()
        {
            InitializeComponent();

            Title = "Plan dnia";
        }

        protected override void OnAppearing()
        {
            _ = ((DayCalendarViewModel)BindingContext).LoadEvents();
            base.OnAppearing();
        }
    }
}
