using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ejercicio1_4.Models;

namespace Ejercicio1_4
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        public ListPage()
        {
            InitializeComponent();
            llenarDatos();
        }

        public async void llenarDatos()
        {
            var photoList = await App.DBase.GetPhotosAsync();
            if (photoList != null)
            {
                lstPhotos.ItemsSource = photoList;
            }
        }

        private async void lstPhotos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var obj = (Photos)e.SelectedItem;
            if (!string.IsNullOrEmpty(obj.id.ToString()))
            {
                var foto = await App.DBase.GetPhotosByIdAsync(obj.id);
                if (foto != null)
                {
                    ViewPhoto photo = new ViewPhoto();
                    photo.BindingContext = foto;
                    await Navigation.PushAsync(photo);
                }
            }
        }
    }
}