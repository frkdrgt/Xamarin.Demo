using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace XamarinDemo
{
    public partial class ImageDetail : ContentPage
    {
        public ImageDetail(string img)
        {
            InitializeComponent();

            imgDetail.Source = ImageSource.FromUri(new Uri(img));
        }
    }
}
