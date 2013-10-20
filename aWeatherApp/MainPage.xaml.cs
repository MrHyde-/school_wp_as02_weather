using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using aWeatherApp.Model;

namespace aWeatherApp
{
    public partial class MainPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        #region JSON
        /// <summary>
        /// Event for pressing the get weather button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleJSON(object sender, RoutedEventArgs e)
        {
            //user input
            var userInput = textBoxLocation.Text;

            //check that user has actually inputted some location
            if (String.IsNullOrEmpty(userInput))
            {
                //beware of error from API 8
                MessageBox.Show("Please input a location!");
            }
            else
            {
                //send handler and uri to next method 
                MakeJsonQuery(JSON_FindCityStringCompleted, new Uri("http://api.openweathermap.org/data/2.5/find?q=" + userInput + "&type=like&cnt=4&units=metric"));
            }
        }

        /// <summary>
        /// Event for handling city finding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JSON_FindCityStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            List<City> cityList;
            // make sure everything is working correctly
            if ((e.Result != null) && (e.Error == null))
            {
                string jsonString = e.Result;

                //load into memory stream
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
                {
                    CityList obj = null;

                    //parse into jsonser
                    // note that to using System.Runtime.Serialization.Json
                    // need to add reference System.Servicemodel.Web
                    var ser = new DataContractJsonSerializer(typeof(CityList));

                    try
                    {
                        obj = (CityList)ser.ReadObject(ms);
                        
                        if (obj.CityCount > 1)
                        {
                            //make user to select single city
                            cityList = obj.Cities;

                            myListPicker.Items.Clear();
                            myListPicker.Items.Add(new City { Id = 0, Name = "Please Choose!"});

                            foreach (City c in cityList)
                            {
                                myListPicker.Items.Add(c);
                                System.Diagnostics.Debug.WriteLine(c.ToString());
                            }

                            //myListPicker.SelectedItem = null;
                            myListPicker.IsEnabled = true;
                            myListPicker.Open();
                        }
                        else if (obj.CityCount == 0)
                        {
                            MessageBox.Show("Cannot find any City, specify search term");
                            buttonJSON.IsEnabled = true;
                        }
                        else
                        {
                            //so we have only a single city, use it..
                            var cityToUse = obj.Cities.FirstOrDefault();
                            
                            if (cityToUse != null && cityToUse.Id > 0)
                            {
                                App.CityModel = cityToUse;
                                //start another query for weather forecast
                                MakeJsonQuery(JSON_WeatherForeCastCompleted, new Uri("http://api.openweathermap.org/data/2.5/forecast/daily?id=" + cityToUse.Id.ToString() + "&units=metric&cnt=7"));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Please input more letters");
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
            }
            else if (e.Error != null)
            {
                System.Diagnostics.Debug.WriteLine(e.Error.Message);
            }
        }

        /// <summary>
        /// Event for handling city forecast get..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JSON_WeatherForeCastCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            // make sure everything is working correctly
            if ((e.Result != null) && (e.Error == null))
            {
                string jsonString = e.Result;

                //load into memory stream
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
                {
                    ForecastList obj;

                    //parse into jsonser
                    // note that to using System.Runtime.Serialization.Json
                    // need to add reference System.Servicemodel.Web
                    var ser = new DataContractJsonSerializer(typeof(ForecastList));

                    try
                    {
                        obj = (ForecastList)ser.ReadObject(ms);
                        App.ForeCastModel = obj;
                        NavigateToWeatherInfoPage();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Please input more letters");
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
            }
            else if (e.Error != null)
            {
                System.Diagnostics.Debug.WriteLine(e.Error.Message);
            }

            buttonJSON.IsEnabled = true;
        }

        /// <summary>
        /// Event for making the actual query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MakeJsonQuery(DownloadStringCompletedEventHandler completedEventHandler, Uri downloadFromUri)
        {
            //disable button to prevent multiple query firing..
            buttonJSON.IsEnabled = false;

            // create an instance
            WebClient client = new WebClient();
            
            // add an event handler
            client.DownloadStringCompleted += completedEventHandler;

            //make actual query
            client.DownloadStringAsync(downloadFromUri);
        }
        #endregion JSON

        /// <summary>
        /// Event for handling correct city selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyListPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //sender to local variable
            ListPicker myList = (sender as ListPicker);

            //I want this to react only if user really makes the selection
            if (myList.IsEnabled)
            {
                //real selection has been made
                var selectedCity = (City)myList.SelectedItem;
                App.CityModel = selectedCity;

                //fetch weather forecast using city id..
                MakeJsonQuery(JSON_WeatherForeCastCompleted, new Uri("http://api.openweathermap.org/data/2.5/forecast/daily?id=" + selectedCity.Id.ToString() + "&units=metric&cnt=7"));

                myList.IsEnabled = false;
            }
        }

        /// <summary>
        /// Event for navigating to weather page. This should not be anymore own method because it is used only once, but it is nicer like this..
        /// </summary>
        private void NavigateToWeatherInfoPage()
        {
            NavigationService.Navigate(new Uri("/WeatherInfoPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Event for navigation away from startpage.. storing user inputted string
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            //get search term and save it
            App.UserSearchTerm = textBoxLocation.Text;
            State[App.SearchTermKey] = App.UserSearchTerm;
        }

        /// <summary>
        /// Event for navigating to startpage.. restoring user inputted string
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            //if app level var has not got any terms..
            if (String.IsNullOrEmpty(App.UserSearchTerm))
            {
                //if state does not contain a key term has not been setted yet (first load)
                if (State.ContainsKey(App.SearchTermKey))
                {
                    //restore search term from state to app level
                    App.UserSearchTerm = (String)State[App.SearchTermKey];
                }
            }

            //if there is no active search term but there is one on app level, restore it to the search box..
            if(String.IsNullOrEmpty(textBoxLocation.Text) && String.IsNullOrEmpty(App.UserSearchTerm) == false)
            {
                textBoxLocation.Text = App.UserSearchTerm;
            }
        }
    }
}