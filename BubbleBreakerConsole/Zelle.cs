using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBreakerConsole.Models
{
    public class Zelle
    {
        // Sichtbare Klassenattribute
        public ZellStatus Status { get; private set; }
        public BubbleFarbe Farbe { get; private set; }

        /// <summary>
        /// Konstruktor: Initialisiert eine leere Zelle
        /// </summary>
        public Zelle()
        {
            Löschen();
        }

        /// <summary>
        /// Zelle mit einem Bubble füllen und Status entsprechend setzen
        /// </summary>
        /// <param name="bubble"></param>
        public void FarbeFestlegen(BubbleFarbe farbe)
        {
            Farbe = farbe;
            if (Farbe == BubbleFarbe.Leer)
                Status = ZellStatus.Leer;
            else
                Status = ZellStatus.Belegt;
        }

        /// <summary>
        /// Zelle für Rotationsbewegung markieren und Staus entsprechend setzen
        /// </summary>
        public void Auswaehlen()
        {
            Status = ZellStatus.Ausgewaehlt;
        }

        /// <summary>
        /// Zelleninhalt löschen und Status entsprechend setzen
        /// </summary>
        public void Löschen()
        {
            FarbeFestlegen(BubbleFarbe.Leer);
        }

        /// <summary>
        /// Vergleichen der Farben
        /// </summary>
        /// <param name="farbe"></param>
        /// <returns>True, wenn Übereinstimming</returns>
        public bool FarbVergleich(BubbleFarbe farbe)
        {
            return Farbe == farbe;
        }

        /// <summary>
        /// Übertragen des Inhalts einer anderen Zelle (für die Rotation) in die eigene Zelle
        /// </summary>
        /// <param name="zelle"></param>
        public void VonZelleUebertragen(Zelle zelle)
        {
            Farbe = zelle.Farbe;
            Status = zelle.Status;
        }

        public string FarbRepraesentation()
        {
            switch (Farbe)
            {
                case BubbleFarbe.Blau:
                    return "B";
                case BubbleFarbe.Gruen:
                    return "G";
                case BubbleFarbe.Rot:
                    return "R";
                case BubbleFarbe.Violett:
                    return "V";
                default:
                    return " ";
            }
        }
    }
}
