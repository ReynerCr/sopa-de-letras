using AppMobile.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JPistasPage : ContentPage
    {
        readonly Contenedor contenedor;
        private List<Label> labelPalabras;
        private bool _DPalabra;
        private bool _DLetra;

        public bool DPalabra
        {
            get { return _DPalabra; }
            private set
            {
                if (_DPalabra != value)
                {
                    _DPalabra = value;
                    DescubrirPalabra.IsEnabled = _DPalabra;
                    if (!_DPalabra)
                        DescubrirPalabra.TextColor = Color.Gray;
                    else
                        DescubrirPalabra.TextColor = Color.Black;
                }//if DPalabra != value
            }//private set
        }

        public bool DLetra
        {
            get { return _DLetra; }
            private set
            {
                if (_DLetra != value)
                {
                    _DLetra = value;
                    DescubrirLetra.IsEnabled = _DLetra;
                    if (!_DLetra)
                        DescubrirLetra.TextColor = Color.Gray;
                    else
                        DescubrirLetra.TextColor = Color.Black;
                }//if _DLetra != value
            }//private set
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public JPistasPage()
        {
            contenedor = Contenedor.Instance;
            _DPalabra = true;
            InitializeComponent();
        }

        public void LimpiarLabels()
        {
            if (ListaPalabrasLayout.Children.Count != 0)
                ListaPalabrasLayout.Children.Clear();
        }

        public void Reiniciar()
        {
            CargarListaPalabrasLayout();

            DPalabra = true;
            DescubrirPalabra.TextColor = Color.White;

            DLetra = true;
            DescubrirLetra.TextColor = Color.White;
        }

        private void CargarListaPalabrasLayout()
        {
            LimpiarLabels();
            if (labelPalabras == null)
                labelPalabras = ManejadorJuego.Instance.GetLabels();
            for (int i = 0; i < labelPalabras.Count; i++)
            {
                labelPalabras[i].HorizontalTextAlignment = TextAlignment.Center;
                ListaPalabrasLayout.Children.Add(labelPalabras[i]);
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

        private async void DescubrirPalabra_Clicked(object sender, EventArgs e)
        {
            if (DPalabra && !contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                ManejadorJuego.Instance.DescubrirPalabra();
                DPalabra = false;
                await Navigation.PopAsync();
                contenedor.IsBusy = false;
            }//if
        }

        private async void DescubrirLetra_Clicked(object sender, EventArgs e)
        {
            if (DLetra && !contenedor.IsBusy)
            {
                contenedor.IsBusy = true;
                ManejadorJuego.Instance.DescubrirLetra();
                DLetra = false;
                await Navigation.PopAsync();
                contenedor.IsBusy = false;
            }//if
        }

    }
}