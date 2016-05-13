using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleBreakerLib;

namespace BubbleBreakerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int x, y, r;
            string e;

            Console.WriteLine("BubbleBreaker Console:");
            Console.WriteLine("======================");

            Console.Write("Anzahl Zeilen? ");
            e = Console.ReadLine();
            x = int.Parse(e);

            Console.Write("Anzahl Spalten? ");
            e = Console.ReadLine();
            y = int.Parse(e);

            GameMatrix SpielLogik = new GameMatrix(x, y);
            SpielLogik.ResetMatrix();
            Console.WriteLine(MatrixAusgeben(SpielLogik));

            // Main Loop
            while (SpielLogik.EsGibtGleicheNachbarnUndMatrixIstNichtLeer())
            {
                Console.Write("Element auswählen (#zeile,#spalte): ");
                e = Console.ReadLine();

                // Ich mache keine checks ob die Eingabe korrekt ist, ich gehe davon aus, dass korrekte Eingaben erfolgen
                string[] pStringArray = e.Split(',', ':', '/', '.', ';', ' ');
                if (pStringArray.Length != 2) continue; // Falscheingaben abfangen

                x = int.Parse(pStringArray[0]); // x Adresse
                y = int.Parse(pStringArray[1]); // y Adresse

                if (x < 0 || x >= SpielLogik.Zeilen) continue;   // Falscheingaben abfangen
                if (y < 0 || y >= SpielLogik.Spalten) continue;   // Falscheingaben abfangen

                r = SpielLogik.FindeGleicheNachbarn(x, y);
                Console.WriteLine(string.Format("Gefundene gleichfarbige Bubble: {0}", r));
                SpielLogik.EnferneAusgewaehlteBubbles();

                Console.WriteLine();
                Console.WriteLine(MatrixAusgeben(SpielLogik));
            }

            Console.WriteLine();
            Console.WriteLine("Spiel ist zu ende! Kein weiterer Zug möglich!");
            Console.ReadLine();
        }

        /// <summary>
        /// Ausgabe der Matrix, trennung Präsentationslogic von spiel Logic !!!
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        static string MatrixAusgeben(GameMatrix matrix)
        {
            string result = "";
            string crlf = Environment.NewLine;
            string xAchse = "    |";
            for (int i = 0; i < matrix.Spalten; i++)
            {
                xAchse += string.Format(" {0,2} |", i);
            }
            xAchse += crlf;
            string xDiv = " ";
            foreach (char e in xAchse) xDiv += "-";
            xDiv += crlf;

            string Zeile = "";
            string punkte = string.Format("Punktzahl: {0}", matrix.Score) + crlf + crlf;

            result += punkte;
            result += xAchse;
            result += xDiv;

            for (int i = 0; i < matrix.Zeilen; i++) // Zeilen
            {
                Zeile = string.Format(" {0,2} |", i);
                for (int j = 0; j < matrix.Spalten; j++)
                {
                    Zeile += string.Format("  {0} |", matrix.ZelleDerAdresse(i, j).FarbRepraesentation());
                }
                Zeile += string.Format(" {0,2}", i);
                Zeile += crlf;
                result += Zeile;
                result += xDiv;
            }
            result += xAchse;
            return result;
        }
    }
}
