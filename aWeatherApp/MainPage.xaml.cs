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
        private void HandleJSON(object sender, RoutedEventArgs e)
        {
                // create an instance
                WebClient client = new WebClient();
                // add an event handler
                client.DownloadStringCompleted += JSON_DownloadStringCompleted;
            
                //user input
                var userInput = textBoxLocation.Text;

                // fire the event 
                if (String.IsNullOrEmpty(userInput))
                {
                    MessageBox.Show("Please input a location!");
                }
                else
                {
                    client.DownloadStringAsync(new Uri("http://api.openweathermap.org/data/2.5/find?q=" + userInput + "&type=like&cnt=4&units=metric"));
                }
        }

        //application layer service
        void JSON_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
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
                        else
                        {
                            App.CityModel = obj.Cities.FirstOrDefault();

                            NavigateToWeatherInfoPage();
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

        #endregion JSON

        private void MyListPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListPicker myList = (sender as ListPicker);

            //I want this to react only if user really makes the selection
            if (myList.IsEnabled)
            {
                //real selection made
                App.CityModel = (City)myList.SelectedItem;

                myList.IsEnabled = false;

                NavigateToWeatherInfoPage();
            }
        }

        private void NavigateToWeatherInfoPage()
        {
            NavigationService.Navigate(new Uri("/WeatherInfoPage.xaml", UriKind.Relative));
        }

        private void MyListPicker_OnUnloaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}