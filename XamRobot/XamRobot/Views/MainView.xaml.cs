using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;
using Xamarin.Forms;
using XR.Service;

namespace XamRobot.Views
{
    public partial class MainView
    {
        private Label player;
        
        public MainView()
        {
            InitializeComponent();
            DrawGameMap();

            this.WhenActivated(d =>
            {
                var commandObs = 
                    this
                        .command
                        .Events()
                        .Completed
                        .Select(_ => this.command.Text)
                        .Where(t => !string.IsNullOrWhiteSpace(t))
                        .Do(_ => this.command.Text = "")
                        .Publish()
                        .RefCount();

                commandObs
                    .InvokeCommand(this.ViewModel.ProcessRobotCommand)
                    .DisposeWith(d);

                commandObs
                    .InvokeCommand((this.ViewModel.ProcessRecords))
                    .DisposeWith(d);

                this.OneWayBind(
                        this.ViewModel, 
                        vm => vm.CommandRecords, 
                        v => v.records.ItemsSource)
                    .DisposeWith(d);

                this.records
                    .Events()
                    .ItemSelected
                    .Select(e => e.SelectedItem)
                    .Cast<string>()
                    .Subscribe(s => this.command.Text = s)
                    .DisposeWith(d);

                this
                    .ViewModel
                    .Location
                    .DistinctUntilChanged()
                    .Do(this.DrawOnLocation)
                    .Subscribe()
                    .DisposeWith(d);
            });
        }

        private void DrawOnLocation(Position position)
        {
            if(this.player != null)
                this.map.Children.Remove(this.player);
            
            string text;
            switch (position.Direction)
            {
                case Bearing.WEST:
                    text = @"<";
                    break;
                case Bearing.NORTH:
                    text = @"^";
                    break;
                case Bearing.EAST:
                    text = @">";
                    break;
                case Bearing.SOUTH:
                    text = @"v";
                    break;
                case null:
                    text = @"?";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.player = new Label
            {
                Text = text,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 36
            };
            
            this
                .map
                .Children
                .Add(this.player, position.X.Value, 5 - position.Y.Value);
        }
        
        private void DrawGameMap()
        {
            for (var i = 0; i < 6; i++)
            {
                for (var j = 0; j < 6; j++)
                {
                    this.map.Children.Add(new BoxView
                    {
                        HeightRequest = 48,
                        WidthRequest = 48,
                        BackgroundColor = (i + j) % 2 == 1 ? Color.DarkGray : Color.LightPink
                    }, j, i);
                }
            }
        }
    }
}