using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBreakerLib
{
    public class GameMatrix
    {
        // Sichtbare Attribute und Zugriffe auf Attribute der Klasse
        public int Score { get; set; } = 0;             // aktueller Gesamt Score
        public int Zeilen { get; private set; }         // Anzahl der Zeilen der Matrix
        public int Spalten { get; private set; }        // Anzahl der Spalten der Matrix
        public Zelle ZelleDerAdresse(int zeile, int spalte) 
            => Matrix[Math.Min(Math.Max(zeile,0),Zeilen-1), Math.Min(Math.Max(spalte, 0), Spalten - 1)];

        // Private nicht sichtbare schnittstelle des Objekts
        private Zelle[,] Matrix;                        // Spielmatrix
        private int GefundeneGleichfarbigeNachbarn;       // Anzahl der gefundenen gleiche Nacbarn der letzten Suche
        private int minZeile, minSpalte, maxZeile, maxSpalte;             // Bounding box gefundener zellen der letzten Suche

        /// <summary>
        /// Konstruktor zum Initialisieren der Matrix
        /// </summary>
        public GameMatrix(int zeilen = 10, int spalten = 10)
        {
            Zeilen = zeilen;
            Spalten = spalten;
            Matrix = new Zelle[Zeilen, Spalten];
            for (int zeile = 0; zeile < Zeilen; zeile++)
                for (int spalte = 0; spalte < Spalten; spalte++)
                    Matrix[zeile, spalte] = new Zelle();
        }

        /// <summary>
        /// Neues Spiel, jede Zelle wird mit einer Farbe belegt und der Highscore wird zurückgesetzt
        /// </summary>
        public void ResetMatrix()
        {
            Score = 0;
            Random rnd = new Random();
            for (int zeile = 0; zeile < Zeilen; zeile++)
                for (int spalte = 0; spalte < Spalten; spalte++)
                {
                    BubbleFarbe farbe = (BubbleFarbe)rnd.Next(1, 5);
                    Matrix[zeile, spalte].FarbeFestlegen(farbe);
                }
        }

        /// <summary>
        /// Rekursiver Suchmechanismus
        /// </summary>
        /// <param name="zeile"></param>
        /// <param name="spalte"></param>
        /// <param name="farbe"></param>
        protected void GleicheNachbarnFindenRekursiv(int zeile, int spalte, BubbleFarbe farbe)
        {
            if (Matrix[zeile, spalte].FarbVergleich(farbe) && Matrix[zeile, spalte].Status != ZellStatus.Ausgewaehlt)
            {
                Matrix[zeile, spalte].Auswaehlen();
                GefundeneGleichfarbigeNachbarn++;
                minZeile = Math.Min(minZeile, zeile);
                maxZeile = Math.Max(maxZeile, zeile);
                minSpalte = Math.Min(minSpalte, spalte);
                maxSpalte = Math.Max(maxSpalte, spalte);

                if (spalte + 1 < Spalten)
                    GleicheNachbarnFindenRekursiv(zeile, spalte + 1, farbe);

                if (spalte - 1 >= 0)
                    GleicheNachbarnFindenRekursiv(zeile, spalte - 1, farbe);

                if (zeile + 1 < Zeilen)
                    GleicheNachbarnFindenRekursiv(zeile + 1, spalte, farbe);

                if (zeile - 1 >= 0)
                    GleicheNachbarnFindenRekursiv(zeile - 1, spalte, farbe);

                if (GefundeneGleichfarbigeNachbarn == 1) Matrix[zeile, spalte].FarbeFestlegen(farbe);
            }
        }

        /// <summary>
        /// Starten des Suchmechanismus und initialisieren des Zugscores
        /// </summary>
        /// <param name="zeile"></param>
        /// <param name="spalte"></param>
        public int FindeGleicheNachbarn(int zeile, int spalte)
        {
            BubbleFarbe farbe = Matrix[zeile, spalte].Farbe;
            GefundeneGleichfarbigeNachbarn = 0;
            minZeile = zeile;
            minSpalte = spalte;
            maxZeile = zeile;
            maxSpalte = spalte;

            GleicheNachbarnFindenRekursiv(zeile, spalte, farbe);
            Score += (GefundeneGleichfarbigeNachbarn * (GefundeneGleichfarbigeNachbarn - 1));
            return GefundeneGleichfarbigeNachbarn;
        }

        public int FindeGleicheNachbarn(Position position)
        {
            return FindeGleicheNachbarn(position.Zeile, position.Spalte);
        }

        /// <summary>
        /// Bubble entfernen gemäß der Richtlinien
        /// </summary>
        public void EnferneAusgewaehlteBubbles()
        {
            for (int zeile = minZeile; zeile <= maxZeile; zeile++)
                for (int spalte = minSpalte; spalte <= maxSpalte; spalte++)
                {
                    // Nur falls Zellstatus ausgewaehlt ist Code ausführen
                    if (Matrix[zeile, spalte].Status == ZellStatus.Ausgewaehlt)
                    {
                        for (int i = zeile; i > 0; i--)
                        {
                            Matrix[i, spalte].VonZelleUebertragen(Matrix[i - 1, spalte]);
                        }
                        Matrix[0, spalte].Löschen();

                        // Pruefen ob Spalte leer ist und falls ja Zellen der linken Nachbarspalte nach rechts schieben
                        if (Matrix[Zeilen - 1, spalte].Status == ZellStatus.Leer)
                        {
                            for (int i = spalte; i >= 0; i--)
                                for (int j = 0; j < Zeilen; j++)
                                {
                                    if (i == 0)
                                        Matrix[j, i].Löschen();
                                    else
                                        Matrix[j, i].VonZelleUebertragen(Matrix[j, i - 1]);
                                }
                        }
                    }
                }
        }

        /// <summary>
        /// Suchen nach verbliebenen gleichen Nachbarn und Prüfung ob die Tabelle leer ist
        /// </summary>
        /// <returns>True, wenn noch ein Zug möglich ist</returns>
        public bool EsGibtGleicheNachbarnUndMatrixIstNichtLeer()
        {
            if (Matrix[Zeilen - 1, Spalten - 1].Status == ZellStatus.Leer) return false; // wenn die letzte Zelle leer ist müssen alle anderen auch leer sein!
            for (int zeile = Zeilen - 1; zeile >= 0; zeile--)
                for (int spalte = Spalten - 1; spalte >= 0; spalte--)
                    if (Matrix[zeile, spalte].Status == ZellStatus.Belegt)
                    {
                        if (spalte + 1 < Spalten) if (Matrix[zeile, spalte].FarbVergleich(Matrix[zeile, spalte + 1].Farbe)) return true;
                        if (spalte - 1 >= 0) if (Matrix[zeile, spalte].FarbVergleich(Matrix[zeile, spalte - 1].Farbe)) return true;
                        if (zeile + 1 < Zeilen) if (Matrix[zeile, spalte].FarbVergleich(Matrix[zeile + 1, spalte].Farbe)) return true;
                        if (zeile - 1 >= 0) if (Matrix[zeile, spalte].FarbVergleich(Matrix[zeile - 1, spalte].Farbe)) return true;
                    }
            return false;
        }
    }
}

