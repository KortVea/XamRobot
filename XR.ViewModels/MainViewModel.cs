using System.Reactive;
using ReactiveUI;
using Sextant;
using Splat;

namespace XR.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public override string Id => nameof(MainViewModel);
        
        public ReactiveCommand<string, Unit> ProcessCommand { get; }

        public MainViewModel() : base(Locator.Current.GetService<IViewStackService>())
        {
            // this.ProcessCommand = ReactiveCommand.Create()
        }
    }
}