using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace XamarinDemo
{
    public partial class Home : ContentPage
    {
        MediaFile _mediaFile = null;

        public Home()
        {
            InitializeComponent();

            btnPickPhoto.Clicked += BtnPickPhoto_Clicked;

            btnTakePhoto.Clicked += BtnTakePhoto_Clicked;

            btnUploadPhoto.Clicked += BtnUploadPhoto_Clicked;

            btnImagePage.Clicked += BtnImagePage_Clicked;
        }

        private async void BtnImagePage_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ImagePage());
        }

        private async void BtnUploadPhoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                string apiPostUrl = "http://baxamarin.azurewebsites.net/api/Files/Upload";
                var httpClient = new HttpClient();

                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(_mediaFile.GetStream()), "file", _mediaFile.Path);

                var apiResult = await httpClient.PostAsync(apiPostUrl, content);

                lblResult.Text = await apiResult.Content.ReadAsStringAsync();

                _mediaFile = null;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Opps", "Connection Fail " + ex.Message, "OK");
                _mediaFile = null;
            }
        }

        private async void BtnTakePhoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Opps", "Camera not found", "OK");
                return;
            }

            if (_mediaFile == null)
            {
                _mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
                {
                    Name = DateTime.Now.Millisecond + "MyImage.jpg",
                    //DefaultCamera = CameraDevice.Front,
                    PhotoSize = PhotoSize.Small,
                    SaveToAlbum = true
                });

                FileName.Source = ImageSource.FromStream(() =>
                {
                    return _mediaFile.GetStream();
                });
            }

        }

        private async void BtnPickPhoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Opps", "Pick photo not found", "OK");
                return;
            }

            if (_mediaFile == null)
            {
                _mediaFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Small,
                });

                FileName.Source = ImageSource.FromStream(() =>
                {
                    return _mediaFile.GetStream();
                });
            }
        }
    }
}
