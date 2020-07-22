using System;
using System.IO;
using Xamarin.Forms;

namespace AppMobile.Model
{
    public static class Constantes
    {
        //Carpeta que guarda los archivos
        public static readonly string folderPath = (Device.RuntimePlatform == Device.GTK) ? "Config/" : Environment.
            GetFolderPath(Environment.SpecialFolder.Personal);

        //Archivo de guardado para la configuracion de juego/temas
        public static readonly string _configPath = Path.
            Combine(folderPath, "configs.dat");

        //Archivo de guardado para ultimo juego del usuario
        public static readonly string _savePath = Path.
            Combine(folderPath, "save.dat");
    }
}
