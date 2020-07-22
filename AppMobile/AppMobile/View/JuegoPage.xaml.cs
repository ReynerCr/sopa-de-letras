using AppMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JuegoPage : ContentPage
    {
        readonly Contenedor contenedor;
        readonly ManejadorJuego manejadorJuego;
        readonly JPistasPage PistasPage;
        readonly JConfigsPage ConfigsPage;

        public JuegoPage()
        {
            contenedor = Contenedor.Instance;
            manejadorJuego = ManejadorJuego.Instance;

            PistasPage = new JPistasPage();
            ConfigsPage = new JConfigsPage();
            manejadorJuego.PistasPage = PistasPage;
            manejadorJuego.ConfigsPage = ConfigsPage;

            InitializeComponent();
            manejadorJuego.Reiniciar(TableroLayout);
        }

        protected override void OnAppearing()
        {
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            base.OnAppearing();
            if (contenedor.Temporizador)
                contenedor.IniciarTemporizador();
        }

        protected override void OnDisappearing()
        {
            contenedor.PararTemporizador();
            base.OnDisappearing();
        }

        private async void SideConfigMenuButton_Clicked(object sender, EventArgs e)
        {
            if (!contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                await Navigation.PushAsync(ConfigsPage);
                contenedor.IsBusy = false;
            }
        }

        private async void SideHelpMenuButton_Clicked(object sender, EventArgs e)
        {
            if (!contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                await Navigation.PushAsync(PistasPage);
                contenedor.IsBusy = false;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void LimpiarActivos_Clicked(object sender, EventArgs e)
        {
            if (!contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                manejadorJuego.LimpiarActivos(false);
                contenedor.IsBusy = false;
            }
        }

        private async void ComprobarActivos_Clicked(object sender, EventArgs e)
        {
            if (!contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                if (manejadorJuego.ComprobarActivos())
                {
                    if (contenedor.AumentarPuntuacion()) //si ya se pasa de nivel
                    {
                        if (contenedor.Nivel < 7)
                        {
                            await DisplayAlert("Siguiente nivel",
                                "Enhorabuena, avanzas al siguiente nivel.",
                                "Aceptar");
                            contenedor.GuardarProgreso();
                            manejadorJuego.Reiniciar();
                            contenedor.IniciarTemporizador();
                        }//if (nivel < 6)
                        else
                        {
                            await DisplayAlert("Fin de juego", "Gracias por jugar! " +
                                ((!contenedor.Temporizador) ? "" : "Total de tiempo: " + contenedor.Tiempo + " segundos."), "Aceptar");
                            contenedor.EliminarProgreso();
                            await Navigation.PopAsync();
                        }//sale del juego
                    }//if
                }//if
                contenedor.IsBusy = false;
            }//if !IsBusy
        }

    }
}
