using AppMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuevoJuegoPage : ContentPage
    {
        readonly Contenedor contenedor;
        bool BusyN; //para revisar que no este ocupado el boton de continuar
        public NuevoJuegoPage()
        {
            contenedor = Contenedor.Instance; ;
            InitializeComponent();
            BusyN = false;
        }

        protected override void OnAppearing()
        {
            if (Device.RuntimePlatform == Device.GTK)
            {
                Continuar.Source = "Images/boton_flechaI.png";
            }
            else
            {
                Continuar.Source = "boton_flecha.png";
                Continuar.RotateTo(180);
            }
            base.OnAppearing();
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

        private async void Continuar_Clicked(object sender, EventArgs e)
        {
            if (!BusyN && !contenedor.IsBusy)
            {
                BusyN = true;
                if (string.IsNullOrWhiteSpace(TextEditor.Text))
                {
                    await DisplayAlert("Error", "Campo vacío.", "Aceptar");
                    return;
                }
                string name = TextEditor.Text.Trim();
                if (name.Length >= 3 && name.Length <= 10)
                {
                    contenedor.IsBusy = true;
                    contenedor.Nombre = TextEditor.Text;
                    if (Device.RuntimePlatform == Device.GTK)
                    {
                        await Navigation.PushAsync(new JuegoPage());
                    }
                    else
                    {
                        Navigation.InsertPageBefore(new JuegoPage(), this);
                        await Navigation.PopAsync();
                    }
                    contenedor.IsBusy = false;
                }
                else
                {
                    await DisplayAlert("Error", "Nombre no válido.", "Aceptar");
                }
                BusyN = false;
            }//!BusyN
        }

    }
}
