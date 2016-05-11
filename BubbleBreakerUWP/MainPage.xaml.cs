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
using Windows.UI.Input;
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
        private GameMatrix SpielLogik;
        private GfxInterface SpielGfx;
        private int Highscore;

        public MainPage()
        {
            this.InitializeComponent();
            Highscore = 0;
        }

        /// <summary>
        /// aktuellen Score anzeigen und Highscore aktualisieren
        /// </summary>
        private void PunktzahlAnzeigen()
        {
            Highscore = Math.Max(Highscore, SpielLogik.Score);
            Punktzahl.Text = $"Punktzahl: {SpielLogik.Score}   Highscore: {Highscore}";
        }

        /// <summary>
        /// Neues Spiel starten (Erzeugen neues Spiel und Gfx Objekt, Initialisieren, Erstmalige Anzeige)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartMsg.Visibility = Visibility.Collapsed;
            SpielLogik = new GameMatrix(10, 10);
            SpielGfx = new GfxInterface(MyCanvas, SpielLogik);
            SpielLogik.ResetMatrix();
            SpielGfx.BubblesAnzeigen();
            PunktzahlAnzeigen();
            MyCanvas.PointerPressed += MyCanvas_PointerPressed;
        }

        /// <summary>
        /// Mausklick / Touch verarbeiten. Erkennen ob das Spiel zu Ende ist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Point zellAdr = SpielGfx.ZellAdresseBerechnen(e.GetCurrentPoint(MyCanvas).Position);
            int r = SpielLogik.FindeGleicheNachbarn((int)zellAdr.Y, (int)zellAdr.X);
            SpielLogik.EnferneAusgewaehlteBubbles();
            SpielGfx.BubblesAnzeigen();
            PunktzahlAnzeigen();

            if (!SpielLogik.EsGibtGleicheNachbarnUndMatrixIstNichtLeer())
            {
                // Spiel zu Ende
                MyCanvas.PointerPressed -= MyCanvas_PointerPressed;
                StartMsg.Visibility = Visibility.Visible;
            }
        }
    }
}
