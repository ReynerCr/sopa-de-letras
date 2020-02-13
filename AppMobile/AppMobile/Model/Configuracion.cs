using System;
using System.IO;

namespace AppMobile.Model
{
    public sealed class Configuracion
    {
        private int _estilo;
        private string _mainMenuBkgColor;
        private string _charColorAndSecondaryBkg;
        private string _charColorInSecondaryBkg;
        private string _charColorInAlerts;
        private bool _temporizador;
        private int _estiloPalabras;

        //combinacion de colores
        public int Estilo
        {
            get { return _estilo; }
            private set { _estilo = value; }
        }

        //main menu background color
        public string MainMenuBkgColor
        {
            get { return _mainMenuBkgColor; }
            private set { _mainMenuBkgColor = value; }
        }

        //color for labels enabled and background of the menu buttons in game
        public string CharColorAndSecondaryBkg
        {
            get { return _charColorAndSecondaryBkg; }
            private set { _charColorAndSecondaryBkg = value; }
        }

        //char color in secondary background and color of the alerts
        public string CharColorInSecondaryBkg
        {
            get { return _charColorInSecondaryBkg; }
            private set { _charColorInSecondaryBkg  = value; }
        }

        //char color in alerts
        public string CharColorInAlerts
        {
            get { return _charColorInAlerts; }
            private set { _charColorInAlerts  = value; }
        }

        public bool Temporizador
        {
            get { return _temporizador; }
            private set { _temporizador = value; }
        }

        public int EstiloPalabras
        {
            get { return _estiloPalabras; }
            private set { _estiloPalabras = value; }
        }

        //INSTANCE
        private static Configuracion _instance = null;
        public static Configuracion Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Configuracion();

                return _instance;
            }
        }//public static Configuracion Instance

        private Configuracion()
        {
            //cargar configs.dat
            bool cargado = CargarConfigs();
            if (!cargado)
            {
                //cargar el estilo clásico
                _estilo = 2;
                CambiarColores();
                _temporizador = (JugadorData.Instance.Tiempo == 0) ? false : true;
                _estiloPalabras = 1;

                cargado = GuardarConfigs();
            }//if configs no cargado
        }//private Configuracion()

        public void CambiarColores()
        {
            _estilo = (_estilo == 1) ? 2 : 1;
            switch (_estilo)
            {
                case 1: //clasico
                    _mainMenuBkgColor = "#ffe7b3";
                    _charColorAndSecondaryBkg = "#2a2a2a";
                    _charColorInSecondaryBkg = "#fffffff";
                    _charColorInAlerts = "#000000";
                    break;
                case 2: //oscuro
                    _mainMenuBkgColor = "#2a2a2a";
                    _charColorAndSecondaryBkg = "#ffffff";
                    _charColorInSecondaryBkg = "#2a2a2a";
                    _charColorInAlerts = "#000000";
                    break;
            }
        }//public void CambiarColores()

        public void AlternarTemporizador()
        {
            _temporizador = !_temporizador;
        }

        public void CambiarEstiloPalabras()
        {
            if (_estiloPalabras < 2)
                ++_estiloPalabras;
            else
                _estiloPalabras = 1;
        }

        private bool CargarConfigs()
        {
            try //HAY QUE VERIFICAR SI LOS COLORES SON CORRECTOS ANTES DE HACER NADA
            {
                string l = File.ReadAllText(Constantes._configPath);
                string[] ll = l.Split();
                _estilo = int.Parse(ll[0]);
                _mainMenuBkgColor = ll[1];
                _charColorAndSecondaryBkg = ll[2];
                _charColorInSecondaryBkg = ll[3];
                _charColorInAlerts = ll[4];
                _temporizador = bool.Parse(ll[5]);
                _estiloPalabras = int.Parse(ll[6]);
                
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }//private bool cargarConfigs()

        public bool GuardarConfigs()
        {
            //escribe los colores en un archivo de texto nuevo que va a generar
            try
            {
                File.WriteAllText(
                Constantes._configPath,
                $"{_estilo} {_mainMenuBkgColor} {_charColorAndSecondaryBkg}" +
                $" {_charColorInSecondaryBkg} {_charColorInAlerts} {_temporizador.ToString()} {_estiloPalabras}"
                );
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }//private bool GuardarConfigs()
    }//class Configuracion
}//namespace MobileApp
