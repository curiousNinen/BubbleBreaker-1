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
using BubbleBreakerLib;

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
            Punktzahl.Text = $"Punktzahl: {SpielLogik.Score} Highscore: {Highscore}";
        }

        private void GesamtScoreAnzeigen()
        {
            Highscore = Math.Max(Highscore, SpielLogik.GesamtScore);
            Punktzahl.Text = $"Punktzahl: {SpielLogik.GesamtScore}  Highscore: {Highscore}";
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
            MyCanvas.PointerMoved += MyCanvas_PointerMoved;
            MyCanvas.Tapped += MyCanvas_Tapped;
        }

        /// <summary>
        /// Realisierung Touch Eingabe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyCanvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Position position = SpielGfx.ZellAdresseBerechnen(e.GetPosition(MyCanvas));
            SpielGfx.ZeigeZellFokus(position);
        }

        /// <summary>
        /// Realisierung des Mausfokus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Position position = SpielGfx.ZellAdresseBerechnen(e.GetCurrentPoint(MyCanvas).Position);
            SpielGfx.ZeigeZellFokus(position);
        }

        /// <summary>
        /// Mausklick verarbeiten. Erkennen ob das Spiel zu Ende ist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Position position = SpielGfx.ZellAdresseBerechnen(e.GetCurrentPoint(MyCanvas).Position);

            int r = SpielLogik.FindeGleicheNachbarn(position);

            //Debug.Text = SpielGfx.MatrixAusgeben();

            //SpielGfx.Animieren(); // funktioniert nicht richtig

            //Debug.Text += Environment.NewLine + SpielGfx.DebugOut;

            SpielLogik.EnferneAusgewaehlteBubbles();

            //Debug.Text += Environment.NewLine + SpielGfx.MatrixAusgeben();

            SpielGfx.BubblesAnzeigen();

            Debug.Text += SpielGfx.DebugOut;

            SpielGfx.ZeigeZellFokus(position, true);

            PunktzahlAnzeigen();

            if (!SpielLogik.EsGibtGleicheNachbarnUndMatrixIstNichtLeer())
            {
                // Spiel zu Ende
                MyCanvas.PointerMoved -= MyCanvas_PointerMoved;
                MyCanvas.PointerPressed -= MyCanvas_PointerPressed;
                StartMsg.Visibility = Visibility.Visible;
                GesamtScoreAnzeigen();
            }
        }
    }
}
