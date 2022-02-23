using System;
using ReactiveUI;
using Sextant;
using Sextant.XamForms;
using Splat;
using XamRobot.Views;
using XR.ViewModels;

namespace XamRobot
{
    public class CompositeRoot
    {
        private readonly Lazy<MainViewModel> mainViewModel;

        public CompositeRoot()
        {
            this.mainViewModel = new Lazy<MainViewModel>(CreateMainViewModel);
            ResolveSextant();
        }

        public MainViewModel CreateMainViewModel() =>
            new MainViewModel();
        
        public App CreateApp() =>
            new App(this.mainViewModel.Value);

        private void ResolveSextant()
        {
            RxApp.DefaultExceptionHandler = new SextantDefaultExceptionHandler();
            Sextant.Sextant.Instance.InitializeForms();
            Locator
                .CurrentMutable
                .RegisterViewForNavigation(() => new MainView(), this.CreateMainViewModel)
                .RegisterNavigationView(() => new BlueNavigationView());
        }
    }
}