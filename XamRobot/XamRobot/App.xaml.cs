using System;
using ReactiveUI;
using Sextant;
using Sextant.XamForms;
using Splat;
using Xamarin.Forms.Xaml;
using XamRobot.Views;
using XR.ViewModels;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace XamRobot
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();

            RxApp.DefaultExceptionHandler = new SextantDefaultExceptionHandler();
            Sextant.Sextant.Instance.InitializeForms();
            Locator
                .CurrentMutable
                .RegisterView<MainView, MainViewModel>()
                .RegisterNavigationView(() => new BlueNavigationView());

            Locator
                .Current
                .GetService<IViewStackService>()
                .PushPage(new MainViewModel(Locator.Current.GetService<IViewStackService>()), null, true, false)
                .Subscribe();

            MainPage = Locator.Current.GetNavigationView();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}