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
using BubbleBreakerGfxLib;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AnimationTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Sprite _sprite;
        //private DispatcherTimer timer;
        //private int tick;
        //private int frames;
        private List<Sprite> _sprites = new List<Sprite>();

        private SpriteBatch _batch;
        
        public MainPage()
        {
            this.InitializeComponent();
            _batch = new SpriteBatch();
            //frames = 15;
            //timer = new DispatcherTimer();
            //timer.Tick += Timer_Tick;
            //timer.Interval = new TimeSpan(0, 0, 0, 0, 30);
        }

        private void AnzeigeKoordinaten()
        {
            Position pos = _sprite.CurrentPosition;
            Koordinaten.Text = $"Koordinaten: ({pos.Top},{pos.Left})";
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            _sprites.Clear();
            UIElement item = new Ellipse()
            {
                StrokeThickness = 5.0,
                Height = 60,
                Width = 60,
                Stroke = new SolidColorBrush(Colors.Red),
                Fill = new SolidColorBrush(Colors.Red)
            };

            _sprite = new Sprite(item, new Position(100, 500));
            //_sprite.SetTargetPosition(new Position(200, 350));
            _sprite.AddToCanvas(MyCanvas);
            _sprites.Add(_sprite);

            item = new Rectangle()
            {
                StrokeThickness = 5.0,
                Height = 60,
                Width = 60,
                Stroke = new SolidColorBrush(Colors.Yellow),
                Fill = new SolidColorBrush(Colors.Yellow)
            };
            _sprite = new Sprite(item, new Position(500, 250));
            //_sprite.SetTargetPosition(new Position(500, 250));
            _sprite.AddToCanvas(MyCanvas);
            _sprites.Add(_sprite);
            _batch.Start();
        }

        private /*async*/ void AnimateClick(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            int x = r.Next(-100, +100);
            int y = r.Next(-100, +100);
            Position p0 = new Position(_sprites[0].CurrentPosition);
            Position p1 = new Position(_sprites[1].CurrentPosition);
            _sprites[0].SetStartPosition(p0);
            _sprites[1].SetStartPosition(p1);
            p0.Top += x;
            p0.Left += y;
            p1.Top += y;
            p1.Left += x;
            //tick = 0;
            //timer.Start();
            _batch.AddToBatch(_sprites[0], p0);
            _batch.AddToBatch(_sprites[1], p1);

            //for (int t = 1; t <= frames; t++)
            //{
            //    _sprite.ChangePositionOnTick(t, frames);
            //    AnzeigeKoordinaten();
            //    await System.Threading.Tasks.Task.Delay(16);
            //}

        }

        //private void Timer_Tick(object sender, object e)
        //{
        //    tick++;
        //    foreach (Sprite item in _sprites)
        //    {
        //        item.ChangePositionOnTick(tick, frames);
        //    }
        //    if (tick >= frames) timer.Stop();
        //}
    }
}
