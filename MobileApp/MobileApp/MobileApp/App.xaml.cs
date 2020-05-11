using MobileApp.Api;
using MobileApp.View;
using MobileApp.ViewModel;
using Prism;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using Refit;
using System;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    public partial class App
    {
        public App() : this(null)
        {
        }

        public App(IPlatformInitializer platformInitializer) : base(platformInitializer)
        {
        }

        protected override void OnInitialized()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this, "Material.Configuration");
            MainPage = new LoginPage();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<ITdsApi>(RestService.For<ITdsApi>(
                    new System.Net.Http.HttpClient(new ApiMessageHandler(new HttpClientHandler(), () => SessionContext.Token))
                    {
                        BaseAddress = new Uri(Settings.ServerUrl),
                        Timeout = TimeSpan.FromMilliseconds(Settings.TimeoutMs),
                    }));

            containerRegistry.RegisterDialog<RefuelDialog, RefuelDialogViewModel>();

            ViewModelLocationProvider.Register<LoginPage, LoginPageViewModel>();
            ViewModelLocationProvider.Register<OrderListPage, OrderListPageViewModel>();
            ViewModelLocationProvider.Register<OrderPage, OrderPageViewModel>();
            ViewModelLocationProvider.Register<RefuelDialog, RefuelDialogViewModel>();
        }
    }
}
