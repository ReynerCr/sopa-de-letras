using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AppMobile
{
    public partial class App : Application
    {
        readonly ViewModels.Container contenedor;
        public App()
        {
            contenedor = ViewModels.Container.Instance;
            ViewModels.ManejadorJuego.Instance.LinkToContainer();
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
