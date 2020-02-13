using System;
using System.ComponentModel;
using AppMobile.Model;
using Xamarin.Forms;
using System.Diagnostics;

namespace AppMobile.ViewModels
{
    public sealed class Container : INotifyPropertyChanged
    {
        private JugadorData jugador;
        private Configuracion configuracion;
        private ManejadorJuego juego;
        private readonly InstruccionesText[] instrucciones;
        private bool _isBusy;

        //INSTANCIA PARA SINGLETON-----------------------------------------------
        private static Container _instance = null;
        public static Container Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Container();

                return _instance;
            }
        }//public static Configuracion Instance

        private Container()
        {
            jugador = JugadorData.Instance;
            configuracion = Configuracion.Instance;
            juego = ViewModels.ManejadorJuego.Instance;
            instrucciones = new InstruccionesText[5];
            for (int i = 0; i < 5; i++)
                instrucciones[i] = new InstruccionesText(i);
        }//private Container()


        //MANEJAR CAMBIO DE PROPIEDADES----------------------------------------
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string nombreVar)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombreVar));
        }//void OnPropertyChanged(string nombreVar)


        //INDICADOR DE ACTIVIDAD
        public bool IsBusy
        {
            get { return _isBusy; }
            set {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }

        //CONFIGURACION--------------------------------------------------------
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
        }//private string EstiloAsText()

        private string EstiloPalabrasAsText()
        {
            string estilo;
            switch (EstiloPalabras)
            {
                case 1:
                    estilo = "clásico";
                    break;
                default:
                    estilo = "ficción";
                    break;
            }
            return estilo;
        }//private string EstiloPalabrasAsText()

        public void AlternarTemporizador()
        {
            configuracion.AlternarTemporizador();
            OnPropertyChanged(nameof(DisplayTemporizador));
        }//public void AlternarTemporizador()

        public string DisplayEstilo => $"Estilo: {EstiloAsText()}";
        public string DisplayEstiloPalabras => $"Estilo de las palabras: {EstiloPalabrasAsText()}";
        public string DisplayTemporizador => "Temporizador: " + ((configuracion.Temporizador) ? "sí" : "no");
        public bool GuardarConfigs() { return configuracion.GuardarConfigs();  }//public bool GuardarConfigs()


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
            set { 
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

        public bool GuardarProgreso() {  return jugador.GuardarProgreso(); }
        public bool EliminarProgreso() {  return jugador.EliminarProgreso(); }


        //INSTRUCCIONES--------------------------------------------------------
        public InstruccionesText[] TextoInstrucciones
        {
            get { return instrucciones; }
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
                Tiempo += getTiempoSegundos();
                ++Nivel;
                return true;
            }
            return false;
        }//public bool AumentarPuntuacion()

        //TEMPORIZADOR---------------------------------------------------------
        private Stopwatch stopwatch = new Stopwatch();
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
        }//public void IniciarTemporizador()
        public void PararTemporizador() { stopwatch.Stop(); }
        public void ReiniciarTemporizador()
        {
            stopwatch.Reset();
            _tiempoMedido = "00:00";
        }

        public int getTiempoSegundos()
        {
            if (stopwatch.IsRunning)
                return -1;
            String []l = _tiempoMedido.Split(':');
            return (int.Parse(l[0]) * 60) + (int.Parse(l[1]));
        }
    }//class Container
}
