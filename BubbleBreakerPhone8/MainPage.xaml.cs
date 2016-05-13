using BubbleBreakerLib;
using BubbleBreakerUWP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace BubbleBreakerPhone8
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

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
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
            Point zellAdr = SpielGfx.ZellAdresseBerechnen(e.GetPosition(MyCanvas));
            SpielGfx.ZeigeZellFokus(zellAdr);
        }

        /// <summary>
        /// Realisierung des Mausfokus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Point zellAdr = SpielGfx.ZellAdresseBerechnen(e.GetCurrentPoint(MyCanvas).Position);
            SpielGfx.ZeigeZellFokus(zellAdr);
        }

        /// <summary>
        /// Mausklick verarbeiten. Erkennen ob das Spiel zu Ende ist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Point zellAdr = SpielGfx.ZellAdresseBerechnen(e.GetCurrentPoint(MyCanvas).Position);
            int r = SpielLogik.FindeGleicheNachbarn((int)zellAdr.Y, (int)zellAdr.X);
            SpielLogik.EnferneAusgewaehlteBubbles();
            SpielGfx.BubblesAnzeigen();
            SpielGfx.ZeigeZellFokus(zellAdr, true);
            PunktzahlAnzeigen();

            if (!SpielLogik.EsGibtGleicheNachbarnUndMatrixIstNichtLeer())
            {
                // Spiel zu Ende
                MyCanvas.PointerMoved -= MyCanvas_PointerMoved;
                MyCanvas.PointerPressed -= MyCanvas_PointerPressed;
                StartMsg.Visibility = Visibility.Visible;
            }
        }
    }
}
