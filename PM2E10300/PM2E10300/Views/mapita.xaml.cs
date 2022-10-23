using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace PM2E10300.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class mapita : ContentPage
    {
        public Byte[] imageByte { get; set; }
        public mapita()
        {
            InitializeComponent();
        }
        private void localizacion_positionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs elemento)
        {
            var pos = new Xamarin.Forms.Maps.Position(elemento.Position.Latitude, elemento.Position.Longitude);
            pMapa.MoveToRegion(new MapSpan(pos, 1, 1));
        }
        protected override async void OnAppearing()
        {
            Pin pinubica = new Pin();
            String latt = lblLat.Text;
            String longg = lblLong.Text;
            String descc = lblDesc.Text;
            base.OnAppearing();
            pinubica.Label = "Mi sitio";
            pinubica.Address = descc;
            pinubica.Position = new Xamarin.Forms.Maps.Position(Convert.ToDouble(latt), Convert.ToDouble(longg));
            pMapa.Pins.Add(pinubica);
            pMapa.MoveToRegion(new MapSpan(new Xamarin.Forms.Maps.Position(Convert.ToDouble(latt), Convert.ToDouble(longg)), 1, 1));

            var locate = CrossGeolocator.Current;
            if (locate != null)
            {
                locate.PositionChanged += localizacion_positionChanged;
                if (!locate.IsListening){await locate.StartListeningAsync(TimeSpan.FromSeconds(10), 100);}
            }
        }

        private async void btnshare_Clicked(object sender, EventArgs e)
        {
            var file = Path.Combine(FileSystem.CacheDirectory, "IM1MAP11.jpg");
            File.WriteAllBytes(file, imageByte);
            await Share.RequestAsync(new ShareFileRequest { Title = Title, File = new ShareFile(file) });
        }
    }
}