using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBreakerConsole.Models
{
    public class Bubble
    {
        /// <summary>
        /// Farbe des Bubble (Schnittstelle)
        /// </summary>
        public BubbleFarbe Farbe { get; set; }

        /// <summary>
        /// Bitmap des Bubbles, nur interne Verwendung
        /// </summary>
        //private BitmapImage Bitmap;
        private String Bitmap; // Für die Kommandozeilenversion
        /// <summary>
        /// Konstruktor für einen Bubble. 
        /// </summary>
        /// <param name="farbe">Farbe des Bubbles</param>
        public Bubble(BubbleFarbe farbe)
        {
            //string ImageDatei = "TransparenterBubble.jpg";
            String imageMarker = " ";
                
            Farbe = farbe;
            switch (Farbe)
            {
                case BubbleFarbe.Blau:
                    //ImageDatei = "BlauerBubble.jpg";
                    imageMarker = "B";
                    break;
                case BubbleFarbe.Gruen:
                    //ImageDatei = "GruenerBubble.jpg";
                    imageMarker = "G";
                    break;
                case BubbleFarbe.Rot:
                    //ImageDatei = "RoterBubble.jpg";
                    imageMarker = "R";
                    break;
                case BubbleFarbe.Violett:
                    //ImageDatei = "VioletterBubble.jpg";
                    imageMarker = "V";
                    break;
            }
            //Bitmap = new BitmapImage(new Uri(ImageDatei));
            Bitmap = imageMarker;
        }

        /// <summary>
        /// Zeichnen im Canvas
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="hoehe"></param>
        /// <param name="breite"></param>
        public void Zeichnen(int x, int y, int hoehe, int breite)
        {
            // Zeichnen des Bubbles an den Koordinaten x,y in den Ausmaßen hoehe, breite
        }

        /// <summary>
        /// Löschen im Canva
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="hoehe"></param>
        /// <param name="breite"></param>
        public void Löschen(int x, int y, int hoehe, int breite)
        {
            // Leeres Bitmap zeichnen, Bubble überschreiben an den Koordinaten x,y in den Ausmaßen hoehe, breite
        }

        /// <summary>
        /// Für Prototyp zur Ausgabe der Tabelle (Bubble zeichnet sich nicht selbst)
        /// </summary>
        /// <returns></returns>
        public string Farbcode()
        {
            return Bitmap;
        }


    }
}
