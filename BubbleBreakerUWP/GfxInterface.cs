using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using BubbleBreakerLib;

namespace BubbleBreakerUWP
{
    /// <summary>
    /// Klasse welche die Anzeige der Bubble handhabt
    /// </summary>
    public class GfxInterface
    {
        private Canvas _canvas;
        private GameMatrix _matrix;
        private Dictionary<BubbleFarbe, SolidColorBrush> _farben = new Dictionary<BubbleFarbe, SolidColorBrush>();
        private Ellipse[,] _bubbles;
        private UIElement _fokus;
        private Point _letzterFokus;
        private bool _fokusAn;

        public double ZellMass { get; private set; }

        /// <summary>
        /// Konstruktor Initialisiert die Verweise auf benötigte Objekte der Oberfläche und der Spiel Logik
        /// </summary>
        /// <param name="canvas">Objekt für die Zeichenfläche</param>
        /// <param name="matrix">Spiel Logik Objekt</param>
        public GfxInterface(Canvas canvas, GameMatrix matrix)
        {
            _canvas = canvas;
            _matrix = matrix;

            _farben[BubbleFarbe.Rot] = new SolidColorBrush(Colors.Red);
            _farben[BubbleFarbe.Gruen] = new SolidColorBrush(Colors.Green);
            _farben[BubbleFarbe.Blau] = new SolidColorBrush(Colors.Blue);
            _farben[BubbleFarbe.Violett] = new SolidColorBrush(Colors.Purple);

            ZellMass = PlatzProZelle();

            _letzterFokus.X = -1.0;
            _letzterFokus.Y = -1.0;

            _fokus = ErzeugeFokusObjekt();
            _fokusAn = false;

        }

        /// <summary>
        /// Ermittelt die Hoehe und Breite einer Zelle der Matrix
        /// </summary>
        /// <returns></returns>
        private double PlatzProZelle()
        {
            double breite = _canvas.ActualWidth - 10;
            double hoehe = _canvas.ActualHeight - 10;
            double minCanvasBH = Math.Min(breite, hoehe);
            double maxMatrixBH = Math.Max(_matrix.Zeilen, _matrix.Spalten);
            return (double)((uint)(minCanvasBH / maxMatrixBH));
        }

        /// <summary>
        /// Ermittelt die Top, Left für eine Zelle im Ausgabe Koordinatensystem
        /// </summary>
        /// <param name="z"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private Tuple<double, double> ZelleNachZeileSpalte(int z, int s, int offset = 2)
        {
            return new Tuple<double, double>(5 + (ZellMass * z) + offset, 5 + (ZellMass * s) + offset);
        }

        /// <summary>
        /// Ermitteln anhand einer Koordinate der Bildschirmausgabe die Adresse in der Spiel Logik Matrix
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Point ZellAdresseBerechnen(Point point)
        {
            Point result;
            result.Y = Math.Round((point.Y - 5) / ZellMass - 0.5);
            result.X = Math.Round((point.X - 5) / ZellMass - 0.5);
            return result;
        }

        /// <summary>
        /// Gibt die Bubble Matrix ohne Animationanhand des aktuellen Spielstands aus
        /// Die Matrix wird immer komplett neu gezeichnet
        /// </summary>
        public void BubblesAnzeigen()
        {
            _bubbles = new Ellipse[_matrix.Zeilen, _matrix.Spalten];
            _canvas.Children.Clear();
            _fokusAn = false;
            for (int i = 0; i < _matrix.Zeilen; i++)
            {
                for (int j = 0; j < _matrix.Spalten; j++)
                {
                    Zelle zelle = _matrix.ZelleDerAdresse(i, j);
                    if (zelle.Status == ZellStatus.Belegt)
                    {
                        Tuple<double, double> ZeileSpalte = ZelleNachZeileSpalte(i, j);
                        _bubbles[i, j] = ErzeugeBubble(zelle.Farbe);
                        Canvas.SetTop(_bubbles[i, j], ZeileSpalte.Item1);
                        Canvas.SetLeft(_bubbles[i, j], ZeileSpalte.Item2);
                        _canvas.Children.Add(_bubbles[i, j]);
                    }
                }
            }
        }

        /// <summary>
        /// Bubble erzeugen
        /// </summary>
        /// <param name="farbe"></param>
        /// <returns></returns>
        private Ellipse ErzeugeBubble(BubbleFarbe farbe)
        {
            return new Ellipse()
            {
                StrokeThickness = 1.0,
                Height = ZellMass - 4,
                Width = ZellMass - 4,
                Stroke = _farben[farbe],
                Fill = _farben[farbe]
            };
        }

        /// <summary>
        /// Fokus Rectangle erzeugen
        /// </summary>
        /// <returns></returns>
        private UIElement ErzeugeFokusObjekt()
        {
            return new Ellipse()
            {
                StrokeThickness = 5.0,
                Height = ZellMass,
                Width = ZellMass,
                Stroke = new SolidColorBrush(Colors.White)
            };
        }

        /// <summary>
        /// Prüft ob eine Zelladresse gültig ist
        /// </summary>
        /// <param name="zelle"></param>
        /// <returns></returns>
        private bool OutOfBounds(Point zelle)
        {
            return zelle.X < 0 || zelle.X >= _matrix.Spalten || zelle.Y < 0 || zelle.Y >= _matrix.Zeilen;
        }

        /// <summary>
        /// Schaltes das Fokus Grafikobjekt ein und aus und stellt sicher, dass nur ein Objekt zu einer Teit existiert
        /// </summary>
        /// <param name="einschalten"></param>
        private void FokusEinschalten(bool einschalten = true)
        {
            if (!_fokusAn && einschalten)
            {
                _canvas.Children.Add(_fokus);
                _fokusAn = einschalten;
            }
            if (_fokusAn && !einschalten)
            {
                _canvas.Children.Remove(_fokus);
                _fokusAn = einschalten;
            }
        }

        /// <summary>
        /// Schaltet den Fokus ein und aus abhängig von der Position im Koordinatensystem
        /// </summary>
        /// <param name="fokus"></param>
        /// <param name="anzeigeErzwingen"></param>
        public void ZeigeZellFokus(Point fokus, bool anzeigeErzwingen = false)
        {
            if ((_letzterFokus.X == fokus.X) && (_letzterFokus.Y == fokus.Y) && !anzeigeErzwingen) return;

            bool belegt = OutOfBounds(fokus) ? false : _matrix.ZelleDerAdresse((int)fokus.Y, (int)fokus.X).Status == ZellStatus.Belegt;

            Tuple<double, double> ZeileSpalte = ZelleNachZeileSpalte((int)fokus.Y, (int)fokus.X, 0);
            Canvas.SetTop(_fokus, ZeileSpalte.Item1);
            Canvas.SetLeft(_fokus, ZeileSpalte.Item2);

            FokusEinschalten(true);
            _letzterFokus = fokus;
            if (!belegt) FokusEinschalten(false);
        }

    }
}
