using Sextant;

namespace XR.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public override string Id => nameof(MainViewModel);

        public MainViewModel(IViewStackService viewStackService) : base(viewStackService)
        {
            
        }
    }
}