using System;
using System.Linq;
using System.Windows;
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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (App.CityModel.Id == 0)
            {
                //if tombstoned try to restore from phone memory?
                App.CityModel = (City)State["amkCITY"];

                //there is something really wrong
                if (App.CityModel.Id == 0)
                {
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
            }

            City currentCityWeather = App.CityModel;
            textBlockLocation.Text = currentCityWeather.Location;
            
            textBlockTemperature.Text = currentCityWeather.CurrentWeather.Temperature.ToString("0.#");
            textBlockTemperature.Text += "°C";

            var time = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            time = time.AddSeconds(Convert.ToInt64(currentCityWeather.UnixTime));
            textBlockWeatherTime.Text = time.ToShortDateString() + " : " + time.ToShortTimeString();

            imageWeather.Visibility = Visibility.Collapsed;

            if (currentCityWeather.WeatherExtraInfos.Any())
            {
                BitmapImage bitmapFromUri = new BitmapImage();
                bitmapFromUri.ImageOpened += bitmapFromUri_ImageOpened;
                bitmapFromUri.UriSource = new Uri(@"/Images/" + currentCityWeather.WeatherExtraInfos.FirstOrDefault().IconCode + ".png", UriKind.Relative);
                imageWeather.Source = bitmapFromUri;
            }
        }

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
                State["amkCITY"] = App.CityModel;
            }
        }

    }
}