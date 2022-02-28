using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using Sextant;
using Splat;
using XR.Service;

namespace XR.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public override string Id => nameof(MainViewModel);
        
        public ReactiveCommand<string, ExecResult> ProcessRobotCommand { get; }
        
        public ReactiveCommand<string, Unit> ProcessRecords { get; }

        public IObservable<Position> Location { get; }

        private readonly SourceCache<string, Guid> commandRecordSource = new SourceCache<string, Guid>(_ => Guid.NewGuid());
        
        public readonly ReadOnlyObservableCollection<string> CommandRecords;
        

        public MainViewModel(IRobotGame robotGame) : base(Locator.Current.GetService<IViewStackService>())
        {
            this.ProcessRobotCommand = ReactiveCommand
                .Create<string, ExecResult>(robotGame.Execute);

            this.ProcessRecords = ReactiveCommand
                .Create<string>(cmd => this.commandRecordSource.AddOrUpdate(cmd));

            this
                .commandRecordSource
                .ExpireAfter(_ => TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1))
                .Subscribe();
            
            this
                .commandRecordSource
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out this.CommandRecords)
                .DisposeMany()
                .Subscribe();

            this.Location = robotGame.Location;
        }
    }
}