using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AppMobile
{
    public partial class App : Application
    {
        readonly ViewModels.Contenedor contenedor;
        public App()
        {
            contenedor = ViewModels.Contenedor.Instance;
            ViewModels.ManejadorJuego.Instance.LinkToContenedor();
            BindingContext = contenedor;
            InitializeComponent();
            MainPage = new NavigationPage(new View.MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
