using System;
using ReactiveUI;
using Sextant;
using Sextant.XamForms;
using Splat;
using XamRobot.Views;
using XR.Service;
using XR.ViewModels;

namespace XamRobot
{
    public class CompositeRoot
    {
        private readonly Lazy<MainViewModel> mainViewModel;
        private readonly Lazy<IRobotGame> robotGame;

        protected CompositeRoot()
        {
            ResolveSextant();
            this.mainViewModel = new Lazy<MainViewModel>(this.CreateMainViewModel);
            this.robotGame = new Lazy<IRobotGame>(this.CreateRobotGame);
        }

        private MainViewModel CreateMainViewModel() =>
            new MainViewModel(this.robotGame.Value);

        private IRobotGame CreateRobotGame() =>
            new RobotGame();
        
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