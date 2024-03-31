using Plan.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plan.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventCreatorPage : ContentPage
    {
        public EventCreatorPage()
        {
            InitializeComponent();
            Title = "Nowe wydarzenie";
        }
    }
}