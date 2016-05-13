﻿using System;
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
        private Ellipse[,] _bubbles;
        //private Position[,] _vectoren;
        private UIElement _fokus;
        private Position _letzterFokus;
        private bool _fokusAn;
        private int _zeilen => _matrix.Zeilen;
        private int _spalten => _matrix.Spalten;

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
            _bubbles = new Ellipse[_zeilen, _spalten];

            //_vectoren = new Position[_zeilen, _spalten];
            //for (int i = 0; i < _zeilen; i++)
            //    for (int j = 0; j < _spalten; j++)
            //        _vectoren[i, j] = new Position();

            _farben[BubbleFarbe.Rot] = new SolidColorBrush(Colors.Red);
            _farben[BubbleFarbe.Gruen] = new SolidColorBrush(Colors.Green);
            _farben[BubbleFarbe.Blau] = new SolidColorBrush(Colors.Blue);
            _farben[BubbleFarbe.Violett] = new SolidColorBrush(Colors.Purple);

            ZellMass = PlatzProZelle();

            _letzterFokus.Zeile = -1;
            _letzterFokus.Spalte = -1;

            _fokus = ErzeugeFokusObjekt();
            _fokusAn = false;

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
            _canvas.Children.Clear();
            _fokusAn = false; // muss ausgeschaltet werden, da alle Grafikobjekte des Canvas mit .Clear gelöscht wurden
            for (int zeile = 0; zeile < _zeilen; zeile++)
            {
                for (int spalte = 0; spalte < _spalten; spalte++)
                {
                    Zelle zelle = _matrix.ZelleDerAdresse(zeile, spalte);
                    if (zelle.Belegt)
                    {
                        Position topLeft = TopLeftZellPosition(zeile, spalte);
                        _bubbles[zeile, spalte] = ErzeugeBubble(zelle.Farbe);
                        Canvas.SetTop(_bubbles[zeile, spalte], topLeft.Top);
                        Canvas.SetLeft(_bubbles[zeile, spalte], topLeft.Left);
                        _canvas.Children.Add(_bubbles[zeile, spalte]);
                    }
                }
            }
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
        public void ZeigeZellFokus(Position fokus, bool anzeigeErzwingen = false)
        {
            if ((_letzterFokus.Zeile == fokus.Zeile) && (_letzterFokus.Spalte == fokus.Spalte) && !anzeigeErzwingen) return;

            bool belegt = OutOfBounds(fokus) ? false : _matrix.ZelleDerAdresse(fokus.Zeile, fokus.Spalte).Belegt;

            Position topLeft = TopLeftZellPosition(fokus.Zeile, fokus.Spalte, 0);
            Canvas.SetTop(_fokus, topLeft.Top);
            Canvas.SetLeft(_fokus, topLeft.Left);

            FokusEinschalten(true);
            _letzterFokus = fokus;
            if (!belegt) FokusEinschalten(false);
        }

        //public async void Animieren()
        //{
        //    for (int spalte = 0; spalte < _spalten; spalte++) // Spalten von Links nach rechts
        //    {
        //        // Feststellen der Spaltenvektoren
        //        int gewaehlt = 0;
        //        int belegt = 0;
        //        for (int zeile = _zeilen - 1; zeile >= 0; zeile--)  // Zeilen von unten nach oben
        //        {
        //            _vectoren[zeile, spalte].Zeile = gewaehlt;
        //            _vectoren[zeile, spalte].Spalte = 0;
        //            Zelle zelle = _matrix.ZelleDerAdresse(zeile, spalte);
        //            if (zelle.Ausgewaehlt) gewaehlt++;
        //            if (zelle.Belegt) belegt++;
        //        }
        //        if (gewaehlt > 0) // Nur ausführen wenn tatsächlich was zu animieren ist
        //        {
        //            if (belegt == 0) // Ausführen, falls die spalte leer wird, --> shift links
        //            {
        //                for (int i = 0; i < spalte; i++)
        //                {
        //                    for (int j = 0; j < _zeilen; j++)
        //                    {
        //                        _vectoren[j, i].Spalte++;
        //                    }
        //                }
        //            }

        //        }

        //    }

        //    // animation
        //    for (int a = 0; a < 10; a++)
        //    {
        //        for (int zeile = 0; zeile < _zeilen; zeile++)
        //        {
        //            for (int spalte = 0; spalte < _spalten; spalte++)
        //            {
        //                //if (_matrix.ZelleDerAdresse(zeile, spalte).Belegt)
        //                //{

        //                    Ellipse bubble = _bubbles[zeile, spalte];
        //                    double z = Canvas.GetTop(bubble) + _vectoren[zeile, spalte].Zeile * 0.2 * ZellMass;
        //                    double s = Canvas.GetLeft(bubble) + _vectoren[zeile, spalte].Spalte * 0.2 * ZellMass;
        //                    Canvas.SetTop(bubble, z);
        //                    Canvas.SetLeft(bubble, s);
        //                //}

        //            }
        //        }
        //        await Task.Delay(300);
        //    }
        //}

    }
}
