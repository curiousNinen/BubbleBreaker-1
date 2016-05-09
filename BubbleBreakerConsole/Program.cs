using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleBreakerConsole.Models;

namespace BubbleBreakerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int x, y, r;
            string e;

            GameMatrix spielMatrix = new GameMatrix();
            spielMatrix.ResetMatrix();
            Console.WriteLine(MatrixAusgeben(spielMatrix));

            // Main Loop
            while (spielMatrix.EsGibtGleicheNachbarnUndMatrixIstNichtLeer())
            {
                Console.Write("Element auswählen (#zeile,#spalte): ");
                e = Console.ReadLine();

                // Ich mache keine checks ob die Eingabe korrekt ist, ich gehe davon aus, dass korrekte Eingaben erfolgen
                x = int.Parse(e.Substring(0, 1)); // x Adresse
                y = int.Parse(e.Substring(2, 1)); // y Adresse

                r = spielMatrix.FindeGleicheNachbarn(x, y);
                Console.WriteLine(string.Format("Gefundene gleichfarbige Bubble: {0}", r));
                spielMatrix.EnferneAusgewaehlteBubbles();

                Console.WriteLine();
                Console.WriteLine(MatrixAusgeben(spielMatrix));
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
            string xAchse = "   | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 |  " + crlf;
            string xDiv = " ---------------------------------------------" + crlf;
            string Zeile = "";
            string punkte = string.Format("Punktzahl: {0}", matrix.Score) + crlf + crlf;

            result += punkte;
            result += xAchse;
            result += xDiv;

            for (int i = 0; i < matrix.Zeilen; i++) // Zeilen
            {
                Zeile = string.Format(" {0} |", i);
                for (int j = 0; j < matrix.Spalten; j++)
                {
                    Zeile += string.Format(" {0} |", matrix.ZelleDerAdresse(i,j).FarbRepraesentation());
                }
                Zeile += string.Format(" {0}", i);
                Zeile += crlf;
                result += Zeile;
                result += xDiv;
            }
            result += xAchse;
            return result;
        }
    }
}
