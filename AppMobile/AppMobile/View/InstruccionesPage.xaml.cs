using AppMobile.ViewModels;
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
            base.OnAppearing();
        }

        private async void Volver_Clicked(object sender, EventArgs e)
        {
            Contenedor contenedor = Contenedor.Instance;
            if (!contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                await Navigation.PopAsync();
                contenedor.IsBusy = false;
            }
        }

    }
}