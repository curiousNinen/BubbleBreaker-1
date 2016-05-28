using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBreakerLib
{
    public class Zelle
    {
        // Sichtbare Klassenattribute
        public ZellStatus Status { get; private set; }
        public BubbleFarbe Farbe { get; private set; }
        public int Zeile { get { return _position.Zeile; } set { _position.Zeile = value; } } // Jede Zelle kennt seine eigene Position in der Matrix
        public int Spalte { get { return _position.Spalte; } set { _position.Spalte = value; } }
        public int BewegenZeilen { get { return _bewegung.Zeile; } set { _bewegung.Zeile = value; } }
        public int BewegenSpalten { get { return _bewegung.Spalte; } set { _bewegung.Spalte = value; } }
        public bool Ausgewaehlt => Status == ZellStatus.Ausgewaehlt;
        public bool Belegt => Status == ZellStatus.Belegt;
        public bool Leer => Status == ZellStatus.Leer;
        public object Behaelter { get; set; } = null; // Behaelter für Zusatzinfo/Sprite

        private Position _position;
        private Position _bewegung;

        public Zelle ErzeugeKopie()
        {
            Zelle kopie = new Zelle(Zeile, Spalte);
            kopie._bewegung = _bewegung;
            kopie.Status = Status;
            kopie.Farbe = Farbe;
            return kopie;
        }

        /// <summary>
        /// Konstruktor: Initialisiert eine leere Zelle
        /// </summary>
        public Zelle(int zeile = 0, int spalte = 0)
        {
            _position = new Position(zeile, spalte);
            _bewegung = new Position(0, 0);
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
            Behaelter = null;
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
            Behaelter = Behaelter;
            //_position = new Position(zelle._position);
            //_bewegung = new Position(zelle._bewegung);
            //_bewegung = new Position();
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
