using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using BubbleBreakerLib;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AnimationTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Sprite _sprite;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void AnzeigeKoordinaten()
        {
            Position pos = _sprite.CurrentPosition;
            Koordinaten.Text = $"Koordinaten: ({pos.Top},{pos.Left})";
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            Ellipse item = new Ellipse()
            {
                StrokeThickness = 5.0,
                Height = 60,
                Width = 60,
                Stroke = new SolidColorBrush(Colors.Red),
                Fill = new SolidColorBrush(Colors.Red)
            };

            _sprite = new Sprite(item, new Position(100, 50));
            _sprite.SetTargetPosition(new Position(200, 350));
            _sprite.AddToCanvas(MyCanvas);
            AnzeigeKoordinaten();
        }

        private async void AnimateClick(object sender, RoutedEventArgs e)
        {
            int frames = 30;
            for (int t = 1; t <= frames; t++)
            {
                _sprite.ChangePositionOnTick(t, frames);
                AnzeigeKoordinaten();
                await System.Threading.Tasks.Task.Delay(16);
            }

        }
    }
}
