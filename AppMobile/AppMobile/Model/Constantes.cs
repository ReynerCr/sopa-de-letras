using System.IO;

namespace AppMobile.Model
{
    public static class Constantes
    {
        public static readonly string _savePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "save.dat");
        public static readonly string _configPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "configs.dat");
    }
}
