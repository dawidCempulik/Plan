using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plan.Views
{
	public partial class KalendarzPage : ContentPage
	{
		public KalendarzPage ()
		{
			InitializeComponent ();

			weekGrid.Children.Add(new Label()
			{
				Text = DateTime.Now.ToString()
			}, 0, 1);
		}
	}
}