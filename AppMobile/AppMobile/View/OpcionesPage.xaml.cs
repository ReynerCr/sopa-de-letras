using AppMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OpcionesPage : ContentPage
    {
        readonly Contenedor contenedor = Contenedor.Instance;

        public OpcionesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (contenedor.Tiempo != 0)
                Temporizador.IsEnabled = false;
            base.OnAppearing();
        }

        private void Temporizador_Clicked(object sender, EventArgs e)
        {
            contenedor.AlternarTemporizador();
        }

        private void Estilo_Clicked(object sender, EventArgs e)
        {
            contenedor.CambiarColores();
        }

        private void EstiloPalabras_Clicked(object sender, EventArgs e)
        {
            contenedor.CambiarEstiloPalabras();
            EstiloPalabras.Text = contenedor.DisplayEstiloPalabras;
        }

        private async void Volver_Clicked(object sender, EventArgs e)
        {
            if (!contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                contenedor.GuardarConfigs();
                await Navigation.PopAsync();
                contenedor.IsBusy = false;
            }
        }

    }
}
