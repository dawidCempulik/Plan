using Plan.ViewModels;
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

            ((CalendarViewModel)BindingContext).WeekGrid = WeekGrid;

            WeekGrid.Children.Add(new BoxView()
            {
                WidthRequest = 1,
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.End,
                Color = Color.LightGray
            }, 0, 1, 0, 8);


            for (int column = 1; column < 24; column++)
            {
                WeekGrid.Children.Add(new BoxView()
                {
                    WidthRequest = 1,
                    VerticalOptions = LayoutOptions.Fill,
                    HorizontalOptions = LayoutOptions.End,
                    Color = Color.FromHex("#555")
                }, column, column + 1, 0, 8);
            }

            for (int row = 0; row < 8; row++)
            {
                WeekGrid.RowDefinitions.Add(new RowDefinition() { Height = 88 });
            }

            for (int row = 0; row < 7; row++)
            {

                StackLayout stack = new StackLayout()
                {
                    WidthRequest = WeekGrid.RowDefinitions[0].Height.Value,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                };

                stack.Children.Add(new Label()
                {
                    Text = Constants.daysOfWeek[row],
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                });

                WeekGrid.Children.Add(stack, 0, row + 1);

                WeekGrid.Children.Add(new BoxView()
                {
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.Fill,
                    Color = Color.LightGray
                }, 0, 25, row, row + 1);
            }

            for (int column = 1; column < 25; column++)
            {
                WeekGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = 88 });

                StackLayout stack = new StackLayout()
                {
                    WidthRequest = WeekGrid.RowDefinitions[0].Height.Value,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                };

                stack.Children.Add(new Label()
                {
                    Text = (column < 11 ? "0" : "") + (column - 1) + ":00",
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                });

                WeekGrid.Children.Add(stack, column, 0);
            }
        }

        protected override void OnAppearing()
        {
            _ = ((CalendarViewModel)BindingContext).LoadEvents();
            base.OnAppearing();
        }
    }
}