using System;
using ReactiveUI;
using Sextant.XamForms;
using Xamarin.Forms;

namespace XamRobot.Views
{
    public class BlueNavigationView : NavigationView, IViewFor
    {
        public BlueNavigationView()
            : base(RxApp.MainThreadScheduler, RxApp.TaskpoolScheduler, ViewLocator.Current)
        {
            BarBackgroundColor = Color.LightSeaGreen;
            BarTextColor = Color.White;
            Title = "Robot Game";
        }

        public object ViewModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}