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
            Console.WriteLine(spielMatrix.AusgebenMatrix());

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
                Console.WriteLine(spielMatrix.AusgebenMatrix());
            }

            Console.WriteLine();
            Console.WriteLine("Spiel ist zu ende! Kein weiterer Zug möglich!");
            Console.ReadLine();
        }
    }
}
