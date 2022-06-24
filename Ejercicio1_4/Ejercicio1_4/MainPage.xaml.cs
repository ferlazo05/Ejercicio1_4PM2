using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using Xamarin.Forms.Xaml;
using Ejercicio1_4.Models;
using Plugin.Media;
using Xamarin.Essentials;

namespace Ejercicio1_4
{
    public partial class MainPage : ContentPage
    {
        Plugin.Media.Abstractions.MediaFile FileFoto = null;
        public MainPage()
        {
            InitializeComponent();
        }

        private Byte[] ConvertImageToByteArray()
        {
            if (FileFoto != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = FileFoto.GetStream();
                    stream.CopyTo(memory);
                    return memory.ToArray();
                }
            }
            return null;
        }

        private async void btnfoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                FileFoto = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "MyPhotos14",
                    Name = "photo.jpg",
                    SaveToAlbum = true
                });

                await DisplayAlert("Directorio", FileFoto.Path, "Ok");

                if (FileFoto != null)
                {
                    Foto.Source = ImageSource.FromStream(() =>
                    {
                        return FileFoto.GetStream();
                    });
                }
            }
            catch (PermissionException ex)
            {
                await DisplayAlert("Advertencia", "Debe activar el permiso de camara.", "Ok");
                System.Environment.Exit(0);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", "Debe usar la camara desde la aplicacion.", "Ok");
                System.Environment.Exit(0);
            }
        }

        private async void btnagregar_Clicked(object sender, EventArgs e)
        {
            if (FileFoto == null)
            {
                await DisplayAlert("Aviso", "Necesita tomar una fotografia", "Ok");
                return;
            }
            else if (!string.IsNullOrEmpty(txtnombre.Text) && !string.IsNullOrEmpty(txtdescripcion.Text))
            {
                var photo = new Photos
                {
                    id = 0,
                    nombre = txtnombre.Text,
                    descripcion = txtdescripcion.Text,
                    foto = ConvertImageToByteArray()
                };

                try
                {
                    var result = await App.DBase.SavePhotosAsync(photo);
                    if (result > 0)
                    {
                        await DisplayAlert("Registro", "Fotografia registrado con exito!", "OK");
                        LimpiarTxt();
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Aviso", "No se pudo registrar, intente de nuevo.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Aviso", "Debe llenar los campos.", "Ok");
                return;
            }
        }
        private async void btnlista_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListPage());
        }
        private void LimpiarTxt()
        {
            Foto.Source = null;
            txtnombre.Text = "";
            txtdescripcion.Text = "";
        }
    }
}
