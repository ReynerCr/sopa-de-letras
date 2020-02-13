using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JConfigsPage : ContentPage
    {
        ViewModels.Container contenedor;

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public JConfigsPage()
        {
            InitializeComponent();
            Volver.Source = ImageSource.FromResource("AppMobile.Resources.boton_flecha.png");
            contenedor = ViewModels.Container.Instance;
        }

        private async void VolverMenu_Clicked(object sender, EventArgs e)
        {
            bool b = await DisplayAlert("¿Desea volver?", "Si vuelve se perderá el progreso"
                + ((contenedor.Nivel == 1) ? ".":" hecho en el nivel."), "Aceptar", "Cancelar");
            if (b)
            {
                contenedor.IsBusy = true;
                if (contenedor.Nivel != 1)
                    contenedor.GuardarProgreso();
                ViewModels.ManejadorJuego.Instance.Limpiar();
                contenedor.ReiniciarTemporizador();
                await Navigation.PopToRootAsync();
                contenedor.IsBusy = false;
            }
        }//private async void Volver_Clicked(object sender, EventArgs e)

        private async void Reiniciar_Clicked(object sender, EventArgs e)
        {
            bool b = await DisplayAlert("¿Está seguro?", "Si acepta perderá el progreso hecho " +
                "en el nivel.", "Aceptar", "Cancelar");
            if (b)
            {
                contenedor.IsBusy = true;
                ViewModels.ManejadorJuego.Instance.Reiniciar();
                contenedor.IsBusy = false;
            }
        }//private async void Reiniciar_Clicked(object sender, EventArgs e)

        private async void Volver_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }//partial JConfigsPage
}