using PM2E10300.Models;
using PM2E10300.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E10300.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Lista : ContentPage
    {
        public Lista()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var listaubicaciones = await App.dbaa.listadire();
            ObservableCollection<sitios> observableCollectionPhotos = new ObservableCollection<sitios>();
            listubi.ItemsSource = observableCollectionPhotos;
            foreach (sitios img in listaubicaciones)
            {
                observableCollectionPhotos.Add(img);
            }
        }
        private async void listubi_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            sitios item = (sitios)e.Item;
            string accion = await DisplayActionSheet("Desea ir a la ubicacion seleccionada?", "Cancel", "Borrar", "IR");

            if (accion.Equals("IR"))
            {
                var newpage2 = new mapita();
                newpage2.imageByte = item.img;
                newpage2.BindingContext = item;
                await Navigation.PushAsync(newpage2);
            }
            else if(accion.Equals("Borrar"))
            {
                var r = await App.dbaa.EliminarUbicacion(item);
                if (r != 0){await DisplayAlert("Aviso", "Sitio eliminado!", "Ok");}
                else{await DisplayAlert("Aviso", "Algo fallo!", "Ok");}
                await Navigation.PopAsync();
            }
        }
    }
}