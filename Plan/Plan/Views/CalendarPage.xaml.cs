using Plan.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SQLite.TableMapping;

namespace Plan.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        private readonly CalendarViewModel viewModel;

        public CalendarPage()
        {
            InitializeComponent();
            viewModel = (CalendarViewModel)BindingContext;

            Title = "Kalendarz";

            viewModel.WeekGrid = WeekGrid;

            for (int column = 0; column < 24; column++)
            {
                WeekGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = viewModel.GridCellSize });
            }

            for (int row = 0; row < 7; row++)
            {
                WeekGrid.RowDefinitions.Add(new RowDefinition() { Height = viewModel.GridCellSize });
            }


            for (int column = 1; column < 24; column++)
            {
                WeekGrid.Children.Add(new BoxView()
                {
                    WidthRequest = 1,
                    VerticalOptions = LayoutOptions.Fill,
                    HorizontalOptions = LayoutOptions.End,
                    Color = (Color)App.Current.Resources["SecondaryContainer"],
                }, column, column + 1, 1, 8);
            }

            for (int row = 1; row < 7; row++)
            {
                WeekGrid.Children.Add(new BoxView()
                {
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.Fill,
                    Color = (Color)App.Current.Resources["Secondary"],
                }, 1, 25, row, row + 1);
            }

            for (int row = 0; row < 7; row++)
            {
                StackLayout stack = new StackLayout()
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                };

                stack.Children.Add(new Label()
                {
                    TextColor = (Color)App.Current.Resources["OnPrimary"],
                    Text = Constants.daysOfWeek[row],
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                });

                if (row < 6)
                {
                    stack.Children.Add(new BoxView()
                    {
                        HeightRequest = 1,
                        WidthRequest = viewModel.GridCellSize / 3 * 2,
                        HorizontalOptions = LayoutOptions.Center,
                        Color = (Color)App.Current.Resources["OnPrimary"],
                    });
                }
                else
                {
                    stack.Children.Add(new BoxView()
                    {
                        HeightRequest = 1
                    });
                }

                StackVertical.Children.Add(stack);
            }


            for (int column = 0; column < 24; column++)
            {
                StackLayout stack = new StackLayout()
                {
                    WidthRequest = viewModel.GridCellSize,
                    Orientation = StackOrientation.Horizontal,
                };

                stack.Children.Add(new Label()
                {
                    TextColor = (Color)App.Current.Resources["OnPrimary"],
                    Text = (column < 10 ? "0" : "") + column + ":00",
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                });

                if (column < 23)
                {
                    stack.Children.Add(new BoxView()
                    {
                        HeightRequest = viewModel.GridCellSize / 3 * 2,
                        WidthRequest = 1,
                        VerticalOptions = LayoutOptions.Center,
                        Color = (Color)App.Current.Resources["OnPrimary"],
                    });
                }
                else
                {
                    stack.Children.Add(new BoxView()
                    {
                        WidthRequest = 1
                    });
                }

                StackHorizontal.Children.Add(stack);
            }
        }

        protected override void OnAppearing()
        {
            _ = viewModel.LoadEvents();
            base.OnAppearing();
        }
    }
}