using AppMobile.Model;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace AppMobile.ViewModels
{
    public sealed class Contenedor : INotifyPropertyChanged
    {
        private readonly JugadorData jugador;
        private readonly Configuracion configuracion;
        private readonly ManejadorJuego juego;
        private bool _isBusy; //booleano para verificar si el programa esta ocupado

        //INSTANCIA PARA SINGLETON-----------------------------------------------
        private static Contenedor _instance = null;
        public static Contenedor Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Contenedor();

                return _instance;
            }
        }//public static Configuracion Instance

        private Contenedor()
        {
            jugador = JugadorData.Instance;
            configuracion = Configuracion.Instance;
            SetRutasImagenes();
            ActualizarRutasBotones();
            juego = ManejadorJuego.Instance;
        }


        //MANEJAR CAMBIO DE PROPIEDADES----------------------------------------
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string nombreVar)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombreVar));
        }


        //INDICADOR DE ACTIVIDAD
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }

        //CONFIGURACION--------------------------------------------------------

        //Colores para temas
        public string MainMenuBkgColor { get { return configuracion.MainMenuBkgColor; } }
        public string CharColorAndSecondaryBkg { get { return configuracion.CharColorAndSecondaryBkg; } }
        public string CharColorInSecondaryBkg { get { return configuracion.CharColorInSecondaryBkg; } }
        public string CharColorInAlerts { get { return configuracion.CharColorInAlerts; } }

        public int Estilo { get { return configuracion.Estilo; } }
        public bool Temporizador { get { return configuracion.Temporizador; } }
        public int EstiloPalabras { get { return configuracion.EstiloPalabras; } }

        public void CambiarColores()
        {
            configuracion.CambiarColores();
            ActualizarRutasBotones();
            OnPropertyChanged(nameof(MainMenuBkgColor));
            OnPropertyChanged(nameof(CharColorAndSecondaryBkg));
            OnPropertyChanged(nameof(CharColorInSecondaryBkg));
            OnPropertyChanged(nameof(CharColorInAlerts));
            OnPropertyChanged(nameof(DisplayEstilo));
        }//public void CambiarColores()

        public void CambiarEstiloPalabras()
        {
            configuracion.CambiarEstiloPalabras();
            OnPropertyChanged(nameof(DisplayEstiloPalabras));
        }

        private string EstiloAsText()
        {
            string estilo;
            switch (Estilo)
            {
                case 1:
                    estilo = "clásico";
                    break;
                default:
                    estilo = "oscuro";
                    break;
            }
            return estilo;
        }

        private string EstiloPalabrasAsText()
        {
            string estilo;
            switch (EstiloPalabras)
            {
                case 1:
                    estilo = "clásico";
                    break;
                default:
                    estilo = "empresas";
                    break;
            }
            return estilo;
        }

        public void AlternarTemporizador()
        {
            configuracion.AlternarTemporizador();
            OnPropertyChanged(nameof(DisplayTemporizador));
        }

        public string DisplayEstilo => $"Estilo: {EstiloAsText()}";
        public string DisplayEstiloPalabras => $"Palabras a buscar: {EstiloPalabrasAsText()}";
        public string DisplayTemporizador => "Temporizador: " + ((configuracion.Temporizador) ? "sí" : "no");
        public bool GuardarConfigs() { return configuracion.GuardarConfigs(); }//public bool GuardarConfigs()


        //JUGADOR------------------------------------------------------------
        public string DisplayNombre => $"Jugador: {jugador.Nombre}";
        public string DisplayNivel => $"Nivel {jugador.Nivel}";

        public string Nombre
        {
            get { return jugador.Nombre; }
            set { jugador.Nombre = value; }
        }
        public int Nivel
        {
            get { return jugador.Nivel; }
            set
            {
                if (jugador.Nivel != value)
                {
                    jugador.Nivel = value;
                    OnPropertyChanged(nameof(DisplayNivel));
                }
            }
        }
        public int Tiempo
        {
            get { return jugador.Tiempo; }
            set { jugador.Tiempo = value; }
        }

        public bool GuardarProgreso() { return jugador.GuardarProgreso(); }
        public bool EliminarProgreso() { return jugador.EliminarProgreso(); }


        //IMAGENES-------------------------------------------------------------
        private void SetRutasImagenes()
        {
            BotonAyuda = "boton_ayuda.png";
            BotonConfigs = "boton_configs.png";
            BotonFlecha = "boton_flecha.png";
            CuadroPuntaje = "cuadro_puntaje.png";
            CuadroTiempo = "cuadro_tiempo.png";

            if (Device.RuntimePlatform == Device.GTK)
            {
                BotonAyuda = "Images/" + BotonAyuda;
                BotonConfigs = "Images/" + BotonConfigs;
                BotonFlecha = "Images/" + BotonFlecha;
                CuadroPuntaje = "Images/" + CuadroPuntaje;
                CuadroTiempo = "Images/" + CuadroTiempo;
            }
        }

        public string BotonAyuda { get; private set; }
        public string BotonConfigs { get; private set; }
        public string BotonFlecha { get; private set; }
        public string CuadroPuntaje { get; private set; }
        public string CuadroTiempo { get; private set; }
        public string BotonLimpiar { get; private set; }
        public string BotonComprobar { get; private set; }

        private void ActualizarRutasBotones()
        {
            int estilo = Estilo;
            BotonLimpiar = "limpiar" + estilo + ".png";
            BotonComprobar = "comprobar" + estilo + ".png";

            if (Device.RuntimePlatform == Device.GTK)
            {
                BotonLimpiar = "Images/" + BotonLimpiar;
                BotonComprobar = "Images/" + BotonComprobar;
            }
        }


        //MANEJADORJUEGO-------------------------------------------------------
        public string DisplayCategoriaActual => $"{juego.CategoriaPalabras} {juego.Puntuacion}/{juego.TotalPalabras}";

        public bool AumentarPuntuacion()
        {
            bool cambio = juego.AumentarPuntuacion();
            OnPropertyChanged(nameof(DisplayCategoriaActual));
            if (cambio) //cambio de nivel
            {
                PararTemporizador();
                Tiempo += GetTiempoSegundos();
                ++Nivel;
                return true;
            }
            return false;
        }


        //TEMPORIZADOR---------------------------------------------------------
        private readonly Stopwatch stopwatch = new Stopwatch();
        private string _tiempoMedido = "00:00";
        public string DisplayTiempoActual => $"{_tiempoMedido}";
        public void IniciarTemporizador()
        {
            if (!stopwatch.IsRunning)
            {
                stopwatch.Start();

                Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
                {
                    _tiempoMedido = String.Format("{0}:{1}", stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds);
                    OnPropertyChanged(nameof(DisplayTiempoActual));
                    if (!stopwatch.IsRunning)
                        return false;
                    else
                        return true;
                }//lambda expression
                );//Device.StartTimer
            }//if
        }

        public void PararTemporizador() { stopwatch.Stop(); }
        public void ReiniciarTemporizador()
        {
            stopwatch.Reset();
            _tiempoMedido = "00:00";
        }

        public int GetTiempoSegundos()
        {
            if (stopwatch.IsRunning)
                return -1;
            String[] l = _tiempoMedido.Split(':');
            return (int.Parse(l[0]) * 60) + (int.Parse(l[1]));
        }

    }
}
