using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleBreakerLib;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BubbleBreakerGfxLib
{
    public class Sprite
    {
        public UIElement Element { get; }
        public Position TopLeft => new Position((int)(Canvas.GetTop(Element)), (int)(Canvas.GetLeft(Element)));
        private bool _inCanvas;

        public Sprite(UIElement sprite, Position topLeft)
        {
            Element = sprite;
            _inCanvas = false;
            SetTopLeft(topLeft);
        }

        public void SetTopLeft(double top, double left)
        {
            Canvas.SetTop(Element, top);
            Canvas.SetLeft(Element, left);
        }

        public void SetTopLeft(Position topLeft)
        {
            Canvas.SetTop(Element, topLeft.Top);
            Canvas.SetLeft(Element, topLeft.Left);
        }

        public void MoveBy(double top, double left)
        {
            Position topLeft = TopLeft;
            topLeft.Top += (int)top;
            topLeft.Left += (int)left;
            SetTopLeft(topLeft);
        }

        public void AddToCanvas(Canvas canvas)
        {
            if (!_inCanvas)
            {
                canvas.Children.Add(Element);
                _inCanvas = true;

            }
        }

        public void RemoveFromCanvas(Canvas canvas)
        {
            if (_inCanvas)
            {
                canvas.Children.Remove(Element);
                _inCanvas = false;

            }
        }
    }
}
