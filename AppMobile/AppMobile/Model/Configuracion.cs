using System;
using System.IO;

namespace AppMobile.Model
{
    public sealed class Configuracion
    {
        //Combinacion de colores para temas
        public int Estilo { get; private set; }

        //main menu background color
        public string MainMenuBkgColor { get; private set; }

        //color for labels enabled and background of the menu buttons in game
        public string CharColorAndSecondaryBkg { get; private set; }

        //char color in secondary background and color of the alerts
        public string CharColorInSecondaryBkg { get; private set; }

        //char color in alerts
        public string CharColorInAlerts { get; private set; }

        public bool Temporizador { get; private set; }

        public int EstiloPalabras { get; private set; }

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
        }

        private Configuracion()
        {
            //cargar configs.dat
            bool cargado = CargarConfigs();
            if (!cargado)
            {
                //cargar el estilo clásico
                Estilo = 2;
                CambiarColores();
                Temporizador = JugadorData.Instance.Tiempo != 0;
                EstiloPalabras = 1;

                GuardarConfigs();
            }//if configs no cargado
        }

        public void CambiarColores()
        {
            Estilo = (Estilo == 1) ? 2 : 1;
            switch (Estilo)
            {
                case 1: //clasico
                    MainMenuBkgColor = "#ffe7b3";
                    CharColorAndSecondaryBkg = "#2a2a2a";
                    CharColorInSecondaryBkg = "#fffffff";
                    CharColorInAlerts = "#000000";
                    break;
                case 2: //oscuro
                    MainMenuBkgColor = "#2a2a2a";
                    CharColorAndSecondaryBkg = "#ffffff";
                    CharColorInSecondaryBkg = "#2a2a2a";
                    CharColorInAlerts = "#000000";
                    break;
            }
        }

        public void AlternarTemporizador()
        {
            Temporizador = !Temporizador;
        }

        public void CambiarEstiloPalabras()
        {
            if (EstiloPalabras < 2)
                ++EstiloPalabras;
            else
                EstiloPalabras = 1;
        }

        private bool CargarConfigs()
        {
            try //Verificando que los colores sean correctos
            {
                string l = File.ReadAllText(Constantes._configPath);
                string[] ll = l.Split();
                Estilo = int.Parse(ll[0]);
                MainMenuBkgColor = ll[1];
                CharColorAndSecondaryBkg = ll[2];
                CharColorInSecondaryBkg = ll[3];
                CharColorInAlerts = ll[4];
                Temporizador = bool.Parse(ll[5]);
                EstiloPalabras = int.Parse(ll[6]);

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool GuardarConfigs()
        {
            //escribe los colores en un nuevo archivo de texto
            try
            {
                File.WriteAllText(
                Constantes._configPath,
                $"{Estilo} {MainMenuBkgColor} {CharColorAndSecondaryBkg}" +
                $" {CharColorInSecondaryBkg} {CharColorInAlerts} {Temporizador} {EstiloPalabras}"
                );
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}
