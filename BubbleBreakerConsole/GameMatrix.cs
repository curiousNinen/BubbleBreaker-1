using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBreakerConsole.Models
{
    public class GameMatrix
    {
        // Sichtbare Attribute und Zugriffe auf Attribute der Klasse
        public int Score { get; set; } = 0;             // aktueller Gesamt Score
        public int Zeilen { get; private set; }         // Anzahl der Zeilen der Matrix
        public int Spalten { get; private set; }        // Anzahl der Spalten der Matrix
        public Zelle ZelleDerAdresse(int x, int y) 
            => Matrix[Math.Min(Math.Max(x,0),Zeilen-1), Math.Min(Math.Max(y, 0), Spalten - 1)];

        // Private nicht sichtbare schnittstelle des Objekts
        private Zelle[,] Matrix;                        // Spielmatrix
        private int GefundeneGleichfarbigeZellen;       // Anzahl der gefundenen gleiche Nacbarn der letzten Suche
        private int minX, minY, maxX, maxY;             // Bounding box gefundener zellen der letzten Suche

        /// <summary>
        /// Konstruktor zum Initialisieren der Matrix
        /// </summary>
        public GameMatrix(int zeilen = 10, int spalten = 10)
        {
            Zeilen = zeilen;
            Spalten = spalten;
            Matrix = new Zelle[Zeilen, Spalten];
            for (int i = 0; i < Zeilen; i++)
                for (int j = 0; j < Spalten; j++)
                    Matrix[i, j] = new Zelle();
        }

        /// <summary>
        /// Neues Spiel, jede Zelle wird mit einer Farbe belegt und der Highscore wird zurückgesetzt
        /// </summary>
        public void ResetMatrix()
        {
            Score = 0;
            Random rnd = new Random();
            for (int i = 0; i < Zeilen; i++)
                for (int j = 0; j < Spalten; j++)
                {
                    BubbleFarbe farbe = (BubbleFarbe)rnd.Next(1, 5);
                    Matrix[i, j].FarbeFestlegen(farbe);
                }
        }

        /// <summary>
        /// Rekursiver Suchmechanismus
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="farbe"></param>
        protected void GleicheNachbarnFindenRekursiv(int x, int y, BubbleFarbe farbe)
        {
            if (Matrix[x, y].FarbVergleich(farbe) && Matrix[x, y].Status != ZellStatus.Ausgewaehlt)
            {
                Matrix[x, y].Auswaehlen();
                GefundeneGleichfarbigeZellen++;
                minX = Math.Min(minX, x);
                maxX = Math.Max(maxX, x);
                minY = Math.Min(minY, y);
                maxY = Math.Max(maxY, y);

                if (y + 1 < Spalten)
                    GleicheNachbarnFindenRekursiv(x, y + 1, farbe);

                if (y - 1 >= 0)
                    GleicheNachbarnFindenRekursiv(x, y - 1, farbe);

                if (x + 1 < Zeilen)
                    GleicheNachbarnFindenRekursiv(x + 1, y, farbe);

                if (x - 1 >= 0)
                    GleicheNachbarnFindenRekursiv(x - 1, y, farbe);
            }
        }

        /// <summary>
        /// Starten des Suchmechanismus und initialisieren des Zugscores
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public int FindeGleicheNachbarn(int x, int y)
        {
            BubbleFarbe farbe = Matrix[x, y].Farbe;
            GefundeneGleichfarbigeZellen = 0;
            minX = x;
            minY = y;
            maxX = x;
            maxY = y;

            GleicheNachbarnFindenRekursiv(x, y, farbe);
            Score += (GefundeneGleichfarbigeZellen * (GefundeneGleichfarbigeZellen - 1));
            return GefundeneGleichfarbigeZellen;
        }

        /// <summary>
        /// Bubble entfernen gemäß der Richtlinien
        /// </summary>
        public void EnferneAusgewaehlteBubbles()
        {
            for (int x = minX; x <= maxX; x++)
                for (int y = minY; y <= maxY; y++)
                {
                    // Nur falls Zellstatus ausgewaehlt ist Code ausführen
                    if (Matrix[x, y].Status == ZellStatus.Ausgewaehlt)
                    {
                        for (int i = x; i > 0; i--)
                        {
                            Matrix[i, y].VonZelleUebertragen(Matrix[i - 1, y]);
                        }
                        Matrix[0, y].Löschen();

                        // Pruefen ob Spalte leer ist und falls ja Zellen der linken Nachbarspalte nach rechts schieben
                        if (Matrix[Zeilen - 1, y].Status == ZellStatus.Leer)
                        {
                            for (int i = y; i >= 0; i--)
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
        /// <returns></returns>
        public bool EsGibtGleicheNachbarnUndMatrixIstNichtLeer()
        {
            if (Matrix[Zeilen - 1, Spalten - 1].Status == ZellStatus.Leer) return false; // wenn die letzte Zelle leer ist müssen alle anderen auch leer sein!
            for (int i = Zeilen - 1; i >= 0; i--)
                for (int j = Spalten - 1; j >= 0; j--)
                    if (Matrix[i, j].Status == ZellStatus.Belegt)
                    {
                        if (j + 1 < Spalten) if (Matrix[i, j].FarbVergleich(Matrix[i, j + 1].Farbe)) return true;
                        if (j - 1 >= 0) if (Matrix[i, j].FarbVergleich(Matrix[i, j - 1].Farbe)) return true;
                        if (i + 1 < Zeilen) if (Matrix[i, j].FarbVergleich(Matrix[i + 1, j].Farbe)) return true;
                        if (i - 1 >= 0) if (Matrix[i, j].FarbVergleich(Matrix[i - 1, j].Farbe)) return true;
                    }
            return false;
        }
    }
}

