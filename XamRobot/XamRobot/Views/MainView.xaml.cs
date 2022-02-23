using ReactiveMarbles.ObservableEvents;
using ReactiveUI;
using Xamarin.Forms;

namespace XamRobot.Views
{
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
            DrawGameMap();
            
            // this.WhenActivated(d =>
            // {
            //     this
            //         .command
            //         .Events()
            //         .Completed
            //         .InvokeCommand()
            // })
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