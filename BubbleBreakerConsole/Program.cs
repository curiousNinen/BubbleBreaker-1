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
            BubbleMatrix spielMatrix = new BubbleMatrix();
            spielMatrix.ResetMatrix();
            Console.WriteLine(spielMatrix.AusgebenMatrix());

            Console.ReadLine();
        }
    }
}
