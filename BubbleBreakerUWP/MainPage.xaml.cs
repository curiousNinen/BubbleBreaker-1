using BubbleBreakerConsole.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BubbleBreakerUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private Line line;
        private Ellipse ellipse;
        private Rectangle rectangle;
        private SolidColorBrush brush;
        private List<UIElement> elements = new List<UIElement>();

        private GameMatrix SpielLogik;
        private GfxInterface SpielGfx;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SpielLogik = new GameMatrix(10, 10);
            SpielGfx = new GfxInterface(MyCanvas, SpielLogik);
            SpielLogik.ResetMatrix();
            SpielGfx.BubblesErzeugen();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int zeile = int.Parse(Zeile.Text);
            int spalte = int.Parse(Spalte.Text);
            int r = SpielLogik.FindeGleicheNachbarn(zeile, spalte);
            SpielLogik.EnferneAusgewaehlteBubbles();
            SpielGfx.BubblesErzeugen();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
        }


    }
}
