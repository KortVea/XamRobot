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
        public App(MainViewModel mainViewModel)
        {
            InitializeComponent();
            
            Locator
                .Current
                .GetService<IViewStackService>()
                .PushPage(mainViewModel, null, true, false)
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