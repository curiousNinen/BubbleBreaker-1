using BubbleBreakerConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

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
        private Tuple<double, double> ZelleNachZeileSpalte(int z, int s)
        {
            double y = 5 + (ZellMass * z) + 2;
            return new Tuple<double, double>(5 + (ZellMass * z) + 2, 5 + (ZellMass * s) + 2);
        }

        /// <summary>
        /// Ermitteln anhand einer Koordinate der Bildschirmausgabe die Adresse in der Spiel Logik Matrix
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Point ZellAdresseBerechnen(Point point)
        {
            Point result;
            result.Y = (point.Y - 5) / ZellMass;
            result.X = (point.X - 5) / ZellMass;
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
            for (int i = 0; i < _matrix.Zeilen; i++)
            {
                for (int j = 0; j < _matrix.Spalten; j++)
                {
                    Zelle zelle = _matrix.ZelleDerAdresse(i, j);
                    if (zelle.Status == ZellStatus.Belegt)
                    {
                        Tuple<double, double> ZeileSpalte = ZelleNachZeileSpalte(i, j);
                        _bubbles[i, j] = new Ellipse()
                        {
                            StrokeThickness = 1.0,
                            Height = ZellMass - 4,
                            Width = ZellMass - 4,
                            Stroke = _farben[zelle.Farbe],
                            Fill = _farben[zelle.Farbe]
                        };
                        Canvas.SetTop(_bubbles[i, j], ZeileSpalte.Item1);
                        Canvas.SetLeft(_bubbles[i, j], ZeileSpalte.Item2);
                        _canvas.Children.Add(_bubbles[i, j]);
                    }
                }
            }
        }
    }
}
