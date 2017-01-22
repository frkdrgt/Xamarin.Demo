using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace XamarinDemo
{
    public partial class ImagePage : ContentPage
    {
        public ImagePage()
        {
            InitializeComponent();
            GetImageData();
            lstImages.ItemSelected += LstImages_ItemSelected;
        }

        private async void LstImages_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                CustomImage selectedImage = (CustomImage)e.SelectedItem;

                await Navigation.PushAsync(new ImageDetail(selectedImage.Url));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ooops", ex.Message, "OK");
            }

        }

        public async void GetImageData()
        {
            var httpClient = new HttpClient();
            string apiUrl = "http://baxamarin.azurewebsites.net/api/Files/Get";

            var apiResult = await httpClient.GetStringAsync(apiUrl);
            List<string> data = (List<string>)JsonConvert.DeserializeObject(apiResult, typeof(List<string>));

            ObservableCollection<CustomImage> images = new ObservableCollection<CustomImage>();
            foreach (var item in data) { images.Add(new CustomImage() { Url = item }); }
            lstImages.ItemsSource = images;
        }
        class CustomImage
        {
            public string Url { get; set; }
        }
    }
}
