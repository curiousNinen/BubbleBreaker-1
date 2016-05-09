using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBreakerConsole.Models
{
    public class BubbleMatrix
    {
        /// <summary>
        /// Matrix des spiels
        /// </summary>
        private Zelle[,] Matrix = new Zelle[10, 10];

        /// <summary>
        /// Nimmt die möglichen Bubbles auf
        /// </summary>
        private Bubble[] Bubbles = new Bubble[5];

        /// <summary>
        /// Variable für Punkezahl
        /// </summary>
        public int Score { get; set; } = 0;

        /// <summary>
        /// Konstruktor zum Initialisieren der Matrix und der möglichen Bubble
        /// </summary>
        public BubbleMatrix()
        {
            Bubbles[0] = new Bubble(BubbleFarbe.Blau);
            Bubbles[1] = new Bubble(BubbleFarbe.Gruen);
            Bubbles[2] = new Bubble(BubbleFarbe.Rot);
            Bubbles[3] = new Bubble(BubbleFarbe.Violett);
            Bubbles[4] = new Bubble(BubbleFarbe.Transparent);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Matrix[i, j] = new Zelle();
                }
            }
        }

        /// <summary>
        /// Neues Spiel, jede Zelle wird mit einer Farbe belegt und der Highscore wird zurückgesetzt
        /// </summary>
        public void ResetMatrix()
        {
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int bubbleIndex = rnd.Next(0, 4);
                    Matrix[i, j].ZelleVergeben(Bubbles[bubbleIndex]);
                }
            }
            Score = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string AusgebenMatrix()
        {
            string result = "";
            string crlf = Environment.NewLine;
            string xAchse = " |0|1|2|3|4|5|6|7|8|9| " + crlf;
            string xDiv = "-----------------------" + crlf;
            string Zeile = "";
            string punkte = string.Format("Punktzahl: {0}", Score) + crlf + crlf;

            result += punkte;
            result += xAchse;
            result += xDiv;

            for (int i = 0; i < 10; i++) // Zeilen
            {
                Zeile = string.Format("{0}|", i);
                for (int j = 0; j < 10; j++)
                {
                    Zeile += string.Format("{0}|", Matrix[i, j].BubbleZeichnen());
                }
                Zeile += string.Format("{0}", i);
                Zeile += crlf;
                result += Zeile;
                result += xDiv;
            }
            result += xAchse;
            return result;
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
                Matrix[x, y].ZelleAuswaehlen();
                GefundeneGleichfarbigeZellen++;
                minX = Math.Min(minX, x);
                maxX = Math.Max(maxX, x);
                minY = Math.Min(minY, y);
                maxY = Math.Max(maxY, y);

                if (y + 1 <= 9) GleicheNachbarnFindenRekursiv(x, y + 1, farbe);
                if (y - 1 >= 0) GleicheNachbarnFindenRekursiv(x, y - 1, farbe);
                if (x + 1 <= 9) GleicheNachbarnFindenRekursiv(x + 1, y, farbe);
                if (x - 1 >= 0) GleicheNachbarnFindenRekursiv(x - 1, y, farbe);
            }
        }

        /// <summary>
        /// Zähler der gefundenen gleichartigen Zellen
        /// </summary>
        private int GefundeneGleichfarbigeZellen;

        /// <summary>
        /// Bounding Box gleichartiger Zellen
        /// </summary>
        private int minX, minY, maxX, maxY;

        /// <summary>
        /// Starten des Suchmechanismus und initialisieren des Zugscores
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="scoring">zum ausschalten des scoring auf false setzen</param>
        public int FindeGleicheNachbarn(int x, int y, bool scoring = true)
        {
            BubbleFarbe farbe = Matrix[x, y].FarbeDesBubble;
            GefundeneGleichfarbigeZellen = 0;
            minX = x;
            minY = y;
            maxX = x;
            maxY = y;

            GleicheNachbarnFindenRekursiv(x, y, farbe);
            if (scoring) Score += (GefundeneGleichfarbigeZellen * (GefundeneGleichfarbigeZellen - 1));
            return GefundeneGleichfarbigeZellen;
        }

        /// <summary>
        /// Bubble entfernen gemäß der Richtlinien
        /// </summary>
        public void EnferneAusgewaehlteBubbles()
        {
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    // Nur falls Zellstatus ausgewaehlt ist Code ausführen
                    if (Matrix[x, y].Status == ZellStatus.Ausgewaehlt)
                    {
                        for (int i = x; i > 0; i--)
                        {
                            Matrix[i, y].VonZelleUebertragen(Matrix[i - 1, y]);
                        }
                        Matrix[0, y].ZelleLeeren();

                        // Pruefen ob Spalte leer ist und falls ja Zellen der linken Nachbarspalte nach rechts schieben
                        if (Matrix[9, y].Status == ZellStatus.Leer)
                        {
                            for (int i = y; i >= 0; i--)
                            {
                                for (int j = 0; j < 10; j++)
                                {
                                    if (i == 0)
                                        Matrix[j, i].ZelleLeeren();
                                    else
                                        Matrix[j, i].VonZelleUebertragen(Matrix[j, i - 1]);
                                }
                            }
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
            if (Matrix[9, 9].Status == ZellStatus.Leer) return false; // wenn die letzte Zelle leer ist müssen alle anderen auch leer sein!
            for (int i = 9; i >= 0; i--)
            {
                for (int j = 9; j >= 0; j--)
                {
                    if (Matrix[i, j].Status == ZellStatus.Besetzt)
                    {
                        if (j + 1 <= 9) if (Matrix[i, j].FarbVergleich(Matrix[i, j + 1].FarbeDesBubble)) return true;
                        if (j - 1 >= 0) if (Matrix[i, j].FarbVergleich(Matrix[i, j - 1].FarbeDesBubble)) return true;
                        if (i + 1 <= 9) if (Matrix[i, j].FarbVergleich(Matrix[i + 1, j].FarbeDesBubble)) return true;
                        if (i - 1 >= 0) if (Matrix[i, j].FarbVergleich(Matrix[i - 1, j].FarbeDesBubble)) return true;

                    }
                }
            }
            return false;
        }
    }
}

