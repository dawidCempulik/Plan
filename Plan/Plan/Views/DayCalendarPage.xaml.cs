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

        private async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            (sender as ListView).SelectedItem = null;
        }
    }
}
