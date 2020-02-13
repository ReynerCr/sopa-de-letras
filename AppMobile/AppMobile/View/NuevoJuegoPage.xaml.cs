using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuevoJuegoPage : ContentPage
    {
        bool BusyN; //solo para esta pagina
        public NuevoJuegoPage()
        {
            InitializeComponent();
            BusyN = false;
        }

        protected override void OnAppearing()
        {
            Volver.Source = ImageSource.FromResource("AppMobile.Resources.boton_flecha.png");
            Continuar.Source = ImageSource.FromResource("AppMobile.Resources.boton_flecha.png");
            base.OnAppearing();
        }

        private async void Volver_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Continuar_Clicked(object sender, EventArgs e)
        {
            if (!BusyN)
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
                    ViewModels.Container contenedor = ViewModels.Container.Instance;
                    contenedor.IsBusy = true;
                    contenedor.Nombre = TextEditor.Text;
                    contenedor.Nivel = 1;
                    Navigation.InsertPageBefore(new JuegoPage(), this);
                    await Navigation.PopAsync();
                    contenedor.IsBusy = false;
                }
                else
                {
                    await DisplayAlert("Error", "Nombre no válido.", "Aceptar");
                }
                BusyN = false;
            }//!BusyN
        }//private async void Continuar_Clicked(object sender, EventArgs e)
    }//partial class NuevoJuegoPage
}
