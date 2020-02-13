using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OpcionesPage : ContentPage
    {
        readonly ViewModels.Container contenedor = ViewModels.Container.Instance;

        public OpcionesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            Volver.Source = ImageSource.FromResource("AppMobile.Resources.boton_flecha.png");
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
            EstiloPalabras.Text = "Aún no implementado.";
            //contenedor.CambiarEstiloPalabras();
        }

        private async void Volver_Clicked(object sender, EventArgs e)
        {
            contenedor.GuardarConfigs(); //debe mostrar un mensaje si no pudo guardar
            await Navigation.PopAsync();
        }
    }//partial class OpcionesPage
}
