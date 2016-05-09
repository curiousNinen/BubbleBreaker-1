using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBreakerConsole.Models
{
    public class Zelle
    {
        /// <summary>
        /// Verweis auf einen Bubble, intern
        /// </summary>
        private Bubble ZellBubble = null;

        /// <summary>
        /// Zellstatus
        /// </summary>
        public ZellStatus Status { get; private set; }

        /// <summary>
        /// Durchreichen der Farbe eines Bubbles einer Zelle mit Standardwert Transparent falls nicht belegt
        /// </summary>
        public BubbleFarbe FarbeDesBubble => ZellBubble != null ? ZellBubble.Farbe : BubbleFarbe.Transparent;

        /// <summary>
        /// Konstruktor: Initialisiert eine Leere Zelle
        /// </summary>
        public Zelle()
        {
            ZelleLeeren();
        }

        /// <summary>
        /// Zelle mit einem Bubble füllen und Status entsprechend setzen
        /// </summary>
        /// <param name="bubble"></param>
        public void ZelleVergeben(Bubble bubble)
        {
            ZellBubble = bubble;
            Status = ZellStatus.Besetzt;
        }

        /// <summary>
        /// Zelle für Rotationsbewegung markieren und Staus entsprechend setzen
        /// </summary>
        public void ZelleAuswaehlen()
        {
            Status = ZellStatus.Ausgewaehlt;
        }

        /// <summary>
        /// Zelleninhalt löschen und Status entsprechend setzen
        /// </summary>
        public void ZelleLeeren()
        {
            ZellBubble = null;
            Status = ZellStatus.Leer;
        }

        /// <summary>
        /// Vergleichen der Farben
        /// </summary>
        /// <param name="farbe"></param>
        /// <returns>True, wenn Übereinstimming</returns>
        public bool FarbVergleich(BubbleFarbe farbe)
        {
            return ZellBubble?.Farbe == farbe;
        }

        /// <summary>
        /// Auswahl bei gleicher Farbe
        /// </summary>
        /// <param name="farbe"></param>
        public void BeiGleicherFarbeAuswahlen(BubbleFarbe farbe)
        {
            if (FarbVergleich(farbe)) ZelleAuswaehlen();
        }

        /// <summary>
        /// Übertragen des Inhalts einer anderen Zelle (für die Rotation) in die eigene Zelle
        /// </summary>
        /// <param name="zelle"></param>
        public void VonZelleUebertragen(Zelle zelle)
        {
            ZellBubble = zelle.ZellBubble;
            Status = zelle.Status;
        }

        /// <summary>
        /// Ausgabe des Buchstaben oder eines Leerzeichen falls zelle leer (Optimierungspotential! in der zelle oder dem Bubble)
        /// </summary>
        /// <returns></returns>
        public string BubbleZeichnen()
        {
            return ZellBubble != null ? ZellBubble.Farbcode() : " ";
        }
    }
}
