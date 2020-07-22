using AppMobile.ViewModels;
using System;
using Xamarin.Forms;

namespace AppMobile.View
{
    public partial class MainPage : ContentPage
    {
        readonly Contenedor contenedor = Contenedor.Instance;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (contenedor.Nombre == string.Empty) //no cargado juego
                Continuar.IsEnabled = false;
            else
                Continuar.IsEnabled = true;

            base.OnAppearing();
        }

        private async void NuevoJuego_Clicked(object sender, EventArgs e)
        {
            if (!contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                bool b = true;
                if (contenedor.Nombre != string.Empty)
                {
                    b = await DisplayAlert("¿Está seguro?",
                        "Se detectó un archivo de guardado, si continúa se sobreescribirá el anterior.", "Aceptar", "Cancelar");
                    if (b)
                        contenedor.EliminarProgreso();
                }
                if (b)
                    await Navigation.PushAsync(new NuevoJuegoPage());
                contenedor.IsBusy = false;
            }
        }

        private async void Continuar_Clicked(object sender, EventArgs e)
        {
            if (!contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                await Navigation.PushAsync(new JuegoPage());
                contenedor.IsBusy = false;
            }
        }

        private async void Opciones_Clicked(object sender, EventArgs e)
        {
            if (!contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                await Navigation.PushAsync(new OpcionesPage());
                contenedor.IsBusy = false;
            }
        }

        private async void Instrucciones_Clicked(object sender, EventArgs e)
        {
            if (!contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                await Navigation.PushAsync(new InstruccionesPage(), true);
                contenedor.IsBusy = false;
            }
        }

        private async void Creditos_Clicked(object sender, EventArgs e)
        {
            if (!contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                await Navigation.PushAsync(new CreditosPage());
                contenedor.IsBusy = false;
            }
        }

    }
}
