using BubbleBreakerGfxLib;
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

namespace BubbleBreakerLib
{
    /// <summary>
    /// Klasse welche die Anzeige der Bubble handhabt
    /// </summary>
    public class GfxInterface
    {
        private Canvas _canvas;
        private GameMatrix _matrix;
        private Dictionary<BubbleFarbe, SolidColorBrush> _farben = new Dictionary<BubbleFarbe, SolidColorBrush>();
        private Sprite _fokus;
        private Position _letzterFokus;
        private bool _fokusAn;
        private int _zeilen => _matrix.Zeilen;
        private int _spalten => _matrix.Spalten;
        private SpriteBatch _batch;

        private string _debugOut;
        public string DebugOut => _debugOut;

        public void ClearDebugInfo()
        {
#if DEBUG
            _debugOut = "";
#endif
        }

        private void DebugWrite(string s = "", bool newLine = true)
        {
#if DEBUG
            _debugOut += s;
            if (newLine) _debugOut += Environment.NewLine;
#endif
        }


        public int ZellMass { get; private set; }

        /// <summary>
        /// Konstruktor Initialisiert die Verweise auf benötigte Objekte der Oberfläche und der Spiel Logik
        /// </summary>
        /// <param name="canvas">Objekt für die Zeichenfläche</param>
        /// <param name="matrix">Spiel Logik Objekt</param>
        public GfxInterface(Canvas canvas, GameMatrix matrix)
        {
            _canvas = canvas;
            _matrix = matrix;

            ZellMass = PlatzProZelle();

            _batch = new SpriteBatch();
            _batch.Start();

            _farben[BubbleFarbe.Rot] = new SolidColorBrush(Colors.Red);
            _farben[BubbleFarbe.Gruen] = new SolidColorBrush(Colors.Green);
            _farben[BubbleFarbe.Blau] = new SolidColorBrush(Colors.Blue);
            _farben[BubbleFarbe.Violett] = new SolidColorBrush(Colors.Purple);


            _letzterFokus = new Position(-1, -1);

            _fokus = new Sprite(ErzeugeFokusObjekt(), new Position());
            _fokusAn = false;

            _debugOut = "";

        }

        public string MatrixAusgeben()
        {
            string result = "";
            string crlf = Environment.NewLine;

            string Zeile = "";

            for (int i = 0; i < _matrix.Zeilen; i++) // Zeilen
            {
                Zeile = $"Z{i}: ";
                for (int j = 0; j < _matrix.Spalten; j++)
                {
                    Zelle zelle = _matrix.ZelleDerAdresse(i, j);
                    Sprite sprite = zelle.Behaelter as Sprite;
                    Position pos;
                    if (sprite != null)
                    {
                        Ellipse item = sprite.Element as Ellipse;
                        double z = Canvas.GetTop(item);//Zeile
                        double s = Canvas.GetLeft(item);//Spalte
                        pos = new Position((int)z, (int)s);
                    }
                    else pos = new Position();
                    //Zeile += $"|{zelle.FarbRepraesentation()}, {zelle.Zeile},{zelle.Spalte}, {pos.ToString()}|";
                    Zeile += $"|{zelle.FarbRepraesentation()}, {pos.ToString()}|";

                }
                Zeile += crlf;
                result += Zeile;
            }

            return result;
        }

        /// <summary>
        /// Ermittelt die Hoehe und Breite einer Zelle der Matrix
        /// </summary>
        /// <returns></returns>
        private int PlatzProZelle()
        {
            double breite = _canvas.ActualWidth - 10;
            double hoehe = _canvas.ActualHeight - 10;
            double minCanvasBH = Math.Min(breite, hoehe);
            double maxMatrixBH = Math.Max(_zeilen, _spalten);
            return (int)(minCanvasBH / maxMatrixBH);
        }

        /// <summary>
        /// Ermittelt die Top, Left für eine Zelle im Ausgabe Koordinatensystem
        /// </summary>
        /// <param name="zeile"></param>
        /// <param name="spalte"></param>
        /// <returns></returns>
        private Position TopLeftZellPosition(int zeile, int spalte, int offset = 2)
        {
            Position result = new Position();
            result.Top = (5 + (ZellMass * zeile) + offset);
            result.Left = (5 + (ZellMass * spalte) + offset);
            return result;
        }

        /// <summary>
        /// Ermitteln anhand einer Koordinate der Bildschirmausgabe die Adresse in der Spiel Logik Matrix
        /// </summary>
        /// <param name="point">Bildschirmkoordinate</param>
        /// <returns></returns>
        public Position ZellAdresseBerechnen(Point point)
        {
            Position result = new Position();
            result.Zeile = (int)Math.Round((point.Y - 5) / ZellMass - 0.5);
            result.Spalte = (int)Math.Round((point.X - 5) / ZellMass - 0.5);
            return result;
        }

        /// <summary>
        /// Gibt die Bubble Matrix ohne Animationanhand des aktuellen Spielstands aus
        /// Die Matrix wird immer komplett neu gezeichnet
        /// </summary>
        public void BubblesAnzeigen()
        {
            ClearDebugInfo();
            _canvas.Children.Clear();
            _fokusAn = false; // muss ausgeschaltet werden, da alle Grafikobjekte des Canvas mit .Clear gelöscht wurden

            //for (int zeile = 0; zeile < _zeilen; zeile++)
            //{
            //    for (int spalte = 0; spalte < _spalten; spalte++)
            //    {
            //        Zelle zelle = _matrix.ZelleDerAdresse(zeile, spalte);
            //        if (zelle.Belegt)
            //        {
            //            Position topLeft = TopLeftZellPosition(zeile, spalte);
            //            zelle.Behaelter = new Sprite(ErzeugeBubble(zelle.Farbe), topLeft);
            //            (zelle.Behaelter as Sprite).AddToCanvas(_canvas);
            //        }
            //    }
            //}

            for (int spalte = _spalten - 1; spalte >= 0; spalte--)
            {
                Zelle zelle = _matrix.ZelleDerAdresse(_zeilen - 1, spalte);
                int zeile = _zeilen - 1;
                if (zelle.Leer)
                {
                    DebugWrite($"Break at spalte={spalte}");
                    break;
                }
                else
                {
                    bool fortfahren = true;
                    while (zeile >= 0 && fortfahren)
                    {
                        zelle = _matrix.ZelleDerAdresse(zeile, spalte);
                        if (zelle.Belegt)
                        {
                            Position topLeft = TopLeftZellPosition(zeile, spalte);
                            zelle.Behaelter = new Sprite(ErzeugeBubble(zelle.Farbe), topLeft);
                            (zelle.Behaelter as Sprite).AddToCanvas(_canvas);
                        }
                        else fortfahren = false;
                        zeile--;
                    }
                }
            }
            //DebugWrite("BubblesAnzeigen:");
            //DebugWrite(MatrixAusgeben());
        }

        /// <summary>
        /// Bubble erzeugen
        /// </summary>
        /// <param name="farbe"></param>
        /// <returns>Grafische Repräsentation eines Bubble</returns>
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
        /// <returns>Grafische Repräsentation eines Fokus</returns>
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
        private bool OutOfBounds(Position zelle)
        {
            return zelle.Spalte < 0 || zelle.Spalte >= _spalten || zelle.Zeile < 0 || zelle.Zeile >= _zeilen;
        }

        /// <summary>
        /// Schaltes das Fokus Grafikobjekt ein und aus und stellt sicher, dass nur ein Objekt zu einer Teit existiert
        /// </summary>
        /// <param name="einschalten"></param>
        private void FokusEinschalten(bool einschalten = true)
        {
            if (!_fokusAn && einschalten)
            {
                _fokus.AddToCanvas(_canvas);
                _fokusAn = einschalten;
            }
            if (_fokusAn && !einschalten)
            {
                _fokus.RemoveFromCanvas(_canvas);
                _fokusAn = einschalten;
            }
        }

        /// <summary>
        /// Schaltet den Fokus ein und aus abhängig von der Position im Koordinatensystem
        /// </summary>
        /// <param name="fokus"></param>
        /// <param name="anzeigeErzwingen"></param>
        public void ZeigeZellFokus(Position fokus, bool anzeigeErzwingen = false)
        {
            if ((_letzterFokus.Zeile == fokus.Zeile) && (_letzterFokus.Spalte == fokus.Spalte) && !anzeigeErzwingen) return;

            bool belegt = OutOfBounds(fokus) ? false : _matrix.ZelleDerAdresse(fokus.Zeile, fokus.Spalte).Belegt;

            Position topLeft = TopLeftZellPosition(fokus.Zeile, fokus.Spalte, 0);
            _fokus.SetTopLeft(topLeft);

            FokusEinschalten(true);
            _letzterFokus = fokus;
            if (!belegt) FokusEinschalten(false);
        }

        public void Animieren()
        {
            List<Zelle> werdenBewegt = new List<Zelle>();

            for (int spalte = 0; spalte < _spalten; spalte++) // Spalten von Links nach rechts
            {
                // Feststellen der Spaltenvektoren
                int gewaehlt = 0; // wieviele bubble sind ausgweählt?
                int belegt = 0; // wieviele bubble sind belegt?
                for (int zeile = _zeilen - 1; zeile >= 0; zeile--)  // Zeilen von unten nach oben durchgehen
                {
                    Zelle zelle = _matrix.ZelleDerAdresse(zeile, spalte);
                    if (!zelle.Leer)
                    {
                        zelle.BewegenZeilen = gewaehlt;
                        zelle.BewegenSpalten = 0;
                        if (zelle.Ausgewaehlt)
                        {
                            (zelle.Behaelter as Sprite).RemoveFromCanvas(_canvas);
                            gewaehlt++;
                        }
                        if (zelle.Belegt)
                        {
                            belegt++;
                            werdenBewegt.Add(zelle);
                        }
                    }
                }
                if (gewaehlt > 0 && belegt == 0) // Nur ausführen wenn tatsächlich Spalten verschoben werden müssen
                    foreach (Zelle zelle in werdenBewegt)
                    {
                        zelle.BewegenSpalten++;
                    }
            }

            ClearDebugInfo();
            DebugWrite("bewegung!:");

            foreach (Zelle zelle in werdenBewegt)
            {
                if (zelle.BewegenZeilen != 0 || zelle.BewegenSpalten != 0)
                {
                    Sprite sprite = zelle.Behaelter as Sprite;
                    Position pos = sprite.TopLeft;
                    Position ziel = TopLeftZellPosition(zelle.Zeile + zelle.BewegenZeilen, zelle.Spalte + zelle.BewegenSpalten);

                    DebugWrite($"Z{zelle.Zeile},S{zelle.Spalte} --> {zelle.BewegenZeilen},{zelle.BewegenSpalten} == ", false);
                    DebugWrite($"Start: Current: {pos.ToString()}  Ziel: {ziel.ToString()}");
                    DebugWrite();

                    _batch.AddToBatch(sprite, ziel);
                }

            }

        }

    }
}
