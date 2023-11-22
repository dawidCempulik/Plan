using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Plan.ViewModels
{
    public class KalendarzViewModel : BaseViewModel
    {
        public KalendarzViewModel()
        {
            Title = "Kalendarz";
        }

        public ICommand OpenWebCommand { get; }
    }
}