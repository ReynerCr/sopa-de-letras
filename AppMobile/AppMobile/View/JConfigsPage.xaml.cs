using AppMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JConfigsPage : ContentPage
    {
        readonly Contenedor contenedor;

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public JConfigsPage()
        {
            InitializeComponent();
            contenedor = Contenedor.Instance;
        }

        private async void VolverMenu_Clicked(object sender, EventArgs e)
        {
            if (!contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                bool b = await DisplayAlert("¿Desea volver?", "Si vuelve se perderá el progreso"
                + ((contenedor.Nivel == 1) ? "." : " hecho en el nivel."), "Aceptar", "Cancelar");
                if (b)
                {
                    contenedor.GuardarProgreso();
                    ManejadorJuego.Instance.Limpiar();
                    contenedor.ReiniciarTemporizador();
                    await Navigation.PopToRootAsync();
                }
                contenedor.IsBusy = false;
            }
        }

        private async void Reiniciar_Clicked(object sender, EventArgs e)
        {
            if (!contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                bool b = await DisplayAlert("¿Está seguro?", "Si acepta perderá el progreso realizado " +
               "en el nivel.", "Aceptar", "Cancelar");
                if (b)
                {
                    ManejadorJuego.Instance.Reiniciar();
                    await Navigation.PopAsync();
                }
                contenedor.IsBusy = false;
            }
        }

        private async void Volver_Clicked(object sender, EventArgs e)
        {
            if (!contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                await Navigation.PopAsync();
                contenedor.IsBusy = false;
            }
        }

    }
}