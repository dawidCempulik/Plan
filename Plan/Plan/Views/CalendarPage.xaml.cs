using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SQLite.TableMapping;

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


            for (int row = 0; row < 7; row++)
            {
                StackLayout stack = new StackLayout()
                {
                    WidthRequest = weekGrid.RowDefinitions[0].Height.Value,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                };

                stack.Children.Add(new Label()
                {
                    Text = days[row],
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                });

                weekGrid.Children.Add(stack, 0, row + 1);
            }

            for (int column = 1; column < 19; column++)
            {
                StackLayout stack = new StackLayout()
                {
                    WidthRequest = weekGrid.RowDefinitions[0].Height.Value,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                };

                stack.Children.Add(new Label()
                {
                    Text = (column < 5 ? "0" : "") + (column + 5) + ":00",
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                });

                weekGrid.Children.Add(stack, column, 0);
            }
        }
    }
}