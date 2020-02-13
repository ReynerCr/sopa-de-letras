using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JuegoPage : ContentPage
    {
        readonly ViewModels.Container contenedor;
        readonly ViewModels.ManejadorJuego manejadorJuego;
        readonly JPistasPage PistasPage;
        readonly JConfigsPage ConfigsPage;
        private bool cargado = false;

        public JuegoPage()
        {
            contenedor = ViewModels.Container.Instance;
            manejadorJuego = ViewModels.ManejadorJuego.Instance;

            PistasPage = new JPistasPage();
            ConfigsPage = new JConfigsPage();
            manejadorJuego.PistasPage = PistasPage;
            manejadorJuego.ConfigsPage = ConfigsPage;

            InitializeComponent();
            manejadorJuego.Reiniciar(TableroLayout);
        }//public JuegoPage()

        protected override void OnAppearing()
        {
            if (!cargado)
            {
                cargado = true;
                CargarImagenes();
            }

            base.OnAppearing();
            if (contenedor.Temporizador)
                contenedor.IniciarTemporizador();
        }

        protected override void OnDisappearing()
        {
            contenedor.PararTemporizador();
            base.OnDisappearing();
        }

        private void CargarImagenes()
        {
            TimerBox.Source = ImageSource.FromResource("AppMobile.Resources.cuadro_tiempo.png");
            ScoreBox.Source = ImageSource.FromResource("AppMobile.Resources.cuadro_puntaje.png");
            SideConfigMenuButton.Source = ImageSource.FromResource("AppMobile.Resources.boton_configs.png");
            SideHelpMenuButton.Source = ImageSource.FromResource("AppMobile.Resources.boton_ayuda.png");
            switch (contenedor.Estilo)
            {
                case 1: //estilo clasico
                    LimpiarActivos.Source = ImageSource.FromResource("AppMobile.Resources.limpiar1.png");
                    ComprobarActivos.Source = ImageSource.FromResource("AppMobile.Resources.comprobar1.png");
                    break;
                case 2: //estilo oscuro
                    LimpiarActivos.Source = ImageSource.FromResource("AppMobile.Resources.limpiar2.png");
                    ComprobarActivos.Source = ImageSource.FromResource("AppMobile.Resources.comprobar2.png");
                    break;
            }//switch (Estilo)
        }//private void CargarImagenes()

        private async void SideConfigMenuButton_Clicked(object sender, EventArgs e)
        {
            if (!contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                await Navigation.PushAsync(ConfigsPage);
                contenedor.IsBusy = false;
            }
        }//private async void SideConfigMenuButton_Clicked(object sender, EventArgs e)

        private async void SideHelpMenuButton_Clicked(object sender, EventArgs e)
        {
            if (!contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                await Navigation.PushAsync(PistasPage);
                contenedor.IsBusy = false;
            }
        }//private async void SideHelpMenuButton_Clicked(object sender, EventArgs e)

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
        }//private void LimpiarActivos_Clicked(object sender, EventArgs e)

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
        }//private async void ComprobarActivos_Clicked(object sender, EventArgs e)
    }//partial class JuegoPage
}
