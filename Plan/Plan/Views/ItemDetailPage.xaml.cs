using Plan.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Plan.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}