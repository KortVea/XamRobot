using ReactiveUI;
using Sextant;

namespace XR.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject, IViewModel
    {
        protected readonly IViewStackService ViewStackService;

        protected ViewModelBase(IViewStackService viewStackService) => ViewStackService = viewStackService;

        public virtual string Id { get; }
    }
}