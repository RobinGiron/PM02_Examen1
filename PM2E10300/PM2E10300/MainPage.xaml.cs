using Plugin.Geolocator;
using Plugin.Media;
using PM2E10300.Controllers;
using PM2E10300.Models;
using PM2E10300.Views;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Threading;

namespace PM2E10300
{
    public partial class MainPage : ContentPage
    {
        byte[] imgbytess;
        public MainPage()
        {
            InitializeComponent();
            cargaubi();
            descc.Text = "";
            imgg.Source = null;
        }
        public async void cargaubi()
        {
            try
            {
                var georequest = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                var tokendecancelacion = new CancellationTokenSource();
                var localizacion = await Geolocation.GetLocationAsync(georequest, tokendecancelacion.Token);
                if (localizacion != null)
                {
                    txtlat.Text = localizacion.Latitude.ToString();
                    txtlong.Text = localizacion.Longitude.ToString();
                }
            }
            catch (FeatureNotSupportedException ex1)
            {
                await DisplayAlert("INFO", "Device no support GPS", "Ok");
            }
            catch (FeatureNotEnabledException ex2)
            {
                await DisplayAlert("INFO", "Error de Device", "Ok");
            }
            catch (PermissionException ex3)
            {
                await DisplayAlert("INFO", "Debes tener permisos de localizacion", "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("INFO", "NO LOCATION", "Ok");
            }
        }
        private byte[] GetImgageBytes(Stream stream)
        {
            byte[] ImgBytes;
            using (var memoryStream = new System.IO.MemoryStream())
            {
                stream.CopyTo(memoryStream);
                ImgBytes = memoryStream.ToArray();
            }
            return ImgBytes;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var fototap = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Mispics",
                Name = DateTime.Now.ToString() + "IM0011.jpg",
                SaveToAlbum = true
            });

            if (fototap != null)
            {
                imgbytess = null;
                MemoryStream memoryStream = new MemoryStream();
                fototap.GetStream().CopyTo(memoryStream);
                imgbytess = memoryStream.ToArray();
                imgg.Source = ImageSource.FromStream(() => { return fototap.GetStream(); });
            }
        }

        private async void btnaguardar_Clicked(object sender, EventArgs e)
        {
            Boolean sevalido = false;
            if (String.IsNullOrWhiteSpace(txtlong.Text) || String.IsNullOrWhiteSpace(txtlat.Text)
                || String.IsNullOrWhiteSpace(descc.Text) || imgg.Source == null) 
            {
                await DisplayAlert("Aviso", "Te hacen falta datos!", "Ok");
                sevalido = false; 
            }
            else { sevalido = true; }
            if (sevalido) {
                try
                {
                    var ubicacion = new sitios
                    {
                        img = imgbytess,
                        Longitud = (float)Convert.ToDouble(txtlong.Text),
                        Latitud = (float)Convert.ToDouble(txtlat.Text),
                        desc = descc.Text
                    };

                    var r = await App.dbaa.savelocation(ubicacion);
                    if (r != 0)
                    {
                        await DisplayAlert("Aviso", "Ubicacion Guardada!", "Ok");
                        await Navigation.PushAsync(new Lista());
                        cleanScreen();
                    }
                    else
                    {
                        await DisplayAlert("Aviso", "Ha Ocurrido un Error", "Ok");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message.ToString(), "Ok");
                }
            }
        }

        private void btnsalir_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        private async void cleanScreen()
        {
            cargaubi();
            this.descc.Text = String.Empty;
            imgg.Source = null;
        }
        private async void btnlista_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Lista());
        }
    }
}
