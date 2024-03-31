using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plan.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        public CalendarPage()
        {
            InitializeComponent();

            Title = "Kalendarz";

            string[] days = { "Pon", "Wt", "Śr", "Czw", "Pt", "Sob", "Nie" };

            for (int column = 0; column < 7; column++)
            {

                weekGrid.Children.Add(new Label
                {
                    Text = days[column],
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                }, column + 1, 1);


                weekGrid.Children.Add(new BoxView()
                {
                    WidthRequest = 1,
                    VerticalOptions = LayoutOptions.Fill,
                    HorizontalOptions = LayoutOptions.End,
                    Color = Color.LightGray
                }, column, column + 1, 1, 19);
            }

            for (int row = 2; row < 19; row++)
            {
                weekGrid.Children.Add(new Label()
                {
                    Text = (row < 6 ? "0" : "") + (row + 4) + ":00",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                }, 0, row);

                weekGrid.Children.Add(new BoxView()
                {
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.Fill,
                    Color = Color.LightGray
                }, 0, 8, row - 1, row);
            }
        }
    }
}