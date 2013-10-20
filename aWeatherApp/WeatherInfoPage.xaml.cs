using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using aWeatherApp.Model;

namespace aWeatherApp
{
    public partial class WeatherInfoPage
    {
        public WeatherInfoPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event for handling data to UI
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (App.CityModel.Id == 0)
            {
                //if tombstoned try to restore from phone memory?
                App.CityModel = (City)State[App.CityKey];
                App.ForeCastModel = (ForecastList) State[App.ForecastKey];

                //there is something really wrong
                if (App.CityModel.Id == 0)
                {
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
            }

            //get main weather
            City currentCityWeather = App.CityModel;
            textBlockLocation.Text = currentCityWeather.Location;
            
            //set temp to ui
            textBlockTemperature.Text = currentCityWeather.CurrentWeather.Temperature.ToString("0.#");
            textBlockTemperature.Text += "°C";

            //set time to ui
            var time = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            time = time.AddSeconds(Convert.ToInt64(currentCityWeather.UnixTime));
            textBlockWeatherTime.Text = time.ToShortDateString() + " : " + time.ToShortTimeString();

            //hide image.. if there is 404 or something weird..
            imageWeather.Visibility = Visibility.Collapsed;

            if (currentCityWeather.WeatherExtraInfos.Any())
            {
                //get image and set it to image
                BitmapImage bitmapFromUri = new BitmapImage();
                bitmapFromUri.ImageOpened += bitmapFromUri_ImageOpened;
                bitmapFromUri.UriSource = new Uri(@"/Images/" + currentCityWeather.WeatherExtraInfos.FirstOrDefault().IconCode + ".png", UriKind.Relative);
                imageWeather.Source = bitmapFromUri;
            }

            var forecastModel = App.ForeCastModel;

            SetForecastsToUI(forecastModel);
        }

        /// <summary>
        /// forecast data to UI
        /// </summary>
        /// <param name="forecastModel"></param>
        private void SetForecastsToUI(ForecastList forecastModel)
        {
            if (forecastModel.Forecasts.Any())
            {
                //first forecast is current day.. so no double information
                var firstCastSkipped = false;

                foreach (var forecast in forecastModel.Forecasts)
                {
                    if (firstCastSkipped)
                    {
                        //create stackpanel
                        var stackPanel = new StackPanel();
                        stackPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;

                        //add image
                        if (forecast.WeatherExtraInfos.Any())
                        {
                            Image forecastIcon = new Image();
                            forecastIcon.Height = 55d;
                            forecastIcon.Width = 55d;
                            
                            //get image from uri
                            BitmapImage bitmapFromUri = new BitmapImage();
                            bitmapFromUri.UriSource = new Uri(@"/Images/" + forecast.WeatherExtraInfos.FirstOrDefault().IconCode + ".png", UriKind.Relative);
                            forecastIcon.Source = bitmapFromUri;
                            
                            //add image to single day stackpanel
                            stackPanel.Children.Add(forecastIcon);
                        }

                        //add timestamp
                        var fcastTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
                        fcastTime = fcastTime.AddSeconds(Convert.ToInt64(forecast.UnixTime));

                        //add temperature and time to textBlock?
                        var tempText = new TextBlock();
                        tempText.Text = forecast.Temperatures.TempDay.ToString();
                        tempText.Text += "°C @ ";
                        tempText.Text += fcastTime.ToShortDateString() + " " + fcastTime.ToShortTimeString();
                        tempText.VerticalAlignment = VerticalAlignment.Center;

                        //insert into stackpanel
                        stackPanel.Children.Add(tempText);

                        ForecastsPanel.Children.Add(stackPanel);
                    }
                    else
                    {
                        firstCastSkipped = true;
                    }
                }
            }
        }
        
        /// <summary>
        /// Event for showing the big image after it is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bitmapFromUri_ImageOpened(object sender, RoutedEventArgs e)
        {
            imageWeather.Visibility = Visibility.Visible;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // If this is a back navigation, the page will be discarded, so there
            // is no need to save state.
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                // Save the ViewModel variable in the page's State dictionary.
                State[App.CityKey] = App.CityModel;
                State[App.ForecastKey] = App.ForeCastModel;
            }
        }

    }
}