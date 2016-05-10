﻿using BubbleBreakerConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace BubbleBreakerUWP
{
    public class GfxInterface
    {
        private Canvas _canvas;
        private GameMatrix _matrix;
        private Dictionary<BubbleFarbe, SolidColorBrush> _farben = new Dictionary<BubbleFarbe, SolidColorBrush>();

        private Ellipse[,] _bubbles;

        private double _zellMass;

        public GfxInterface(Canvas canvas, GameMatrix matrix)
        {
            _canvas = canvas;
            _matrix = matrix;

            _farben[BubbleFarbe.Rot] = new SolidColorBrush(Colors.Red);
            _farben[BubbleFarbe.Gruen] = new SolidColorBrush(Colors.Green);
            _farben[BubbleFarbe.Blau] = new SolidColorBrush(Colors.Blue);
            _farben[BubbleFarbe.Violett] = new SolidColorBrush(Colors.Purple);

            _zellMass = PlatzProZelle();

        }

        private double PlatzProZelle()
        {
            double breite = _canvas.ActualWidth - 10;
            double hoehe = _canvas.ActualHeight - 10;
            double minCanvasBH = Math.Min(breite, hoehe);
            double maxMatrixBH = Math.Max(_matrix.Zeilen, _matrix.Spalten);
            return (double)((uint)(minCanvasBH / maxMatrixBH));
        }

        private Tuple<double, double> ZelleNachZeileSpalte(int z, int s)
        {
            double y = 5 + (_zellMass * z) + 2;
            return new Tuple<double, double>(5 + (_zellMass * z) + 2, 5 + (_zellMass * s) + 2);
        }

        public void BubblesErzeugen()
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
                            Height = _zellMass - 4,
                            Width = _zellMass - 4,
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


        //public void SpielfeldAusgeben()
        //{
        //    for (int i = 0; i < _matrix.Zeilen; i++)
        //    {
        //        for (int j = 0; j < _matrix.Spalten; j++)
        //        {
        //            Zelle zelle = _matrix.ZelleDerAdresse(i, j);
        //            if (zelle.Status == ZellStatus.Belegt)
        //            {
        //                Tupple2 topleft = BerechneZellKoordinate(i, j);

        //            }
        //        }
        //    }
        //}
    }
}
