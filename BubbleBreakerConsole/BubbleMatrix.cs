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
        private Zelle[,] Matrix = new Zelle[10,10];

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

        public string AusgebenMatrix()
        {
            string result = "";
            string crlf = Environment.NewLine;
            string xAchse = " |0|1|2|3|4|5|6|7|8|9| " + crlf;
            string xDiv   = "-----------------------" + crlf;
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
    }

}
