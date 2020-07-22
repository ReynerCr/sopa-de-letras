using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

namespace AppMobile.GTK
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Gtk.Application.Init();
            Forms.Init();

            var app = new App();

            var window = new FormsWindow();
            window.LoadApplication(app);
            window.SetApplicationTitle("Sopa de Letras");
            window.SetDefaultSize(600, 840);
            window.SetApplicationIcon("Images/icono.png");
            window.Show();

            Gtk.Application.Run();
        }
    }
}
