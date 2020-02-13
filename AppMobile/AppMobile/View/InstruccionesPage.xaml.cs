using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InstruccionesPage : ContentPage
    {
        public InstruccionesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            Volver.Source = ImageSource.FromResource("AppMobile.Resources.boton_flecha.png");
            base.OnAppearing();
        }

        private async void Volver_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }//partial class InstruccionesPage
}