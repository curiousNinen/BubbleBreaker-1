using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleBreakerLib;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AnimationTest
{
    public class Sprite
    {
        public Position StartPosition { get; private set; }
        public Position CurrentPosition { get; private set; }
        public Position TargetPosition { get; private set; }
        public UIElement Itself { get; private set; }

        public Sprite(UIElement itself, Position topLeftStart)
        {
            Itself = itself;
            SetStartPosition(topLeftStart);
        }

        public void SetStartPosition(Position topLeftStart)
        {
            StartPosition = new Position(topLeftStart);
            CurrentPosition = new Position(StartPosition);
            Canvas.SetTop(Itself, StartPosition.Top);
            Canvas.SetLeft(Itself, StartPosition.Left);
        }

        public void SetTargetPosition(Position topLeftTarget)
        {
            TargetPosition = new Position(topLeftTarget);
        }

        public void ChangeSprite(UIElement sprite)
        {
            Itself = sprite;
            Canvas.SetTop(Itself, CurrentPosition.Top);
            Canvas.SetLeft(Itself, CurrentPosition.Left);
        }

        public void ChangePositionOnTick(int tick, int frames)
        {
            //if (StartPosition.Equals(TargetPosition)) return;
            double coeff = (double)tick / frames;
            double top = (StartPosition.Top + (TargetPosition.Top - StartPosition.Top) * coeff);
            double left = (StartPosition.Left + (TargetPosition.Left - StartPosition.Left) * coeff);
            CurrentPosition.Top = (int)top;
            CurrentPosition.Left = (int)left;
            Canvas.SetTop(Itself, CurrentPosition.Top);
            Canvas.SetLeft(Itself, CurrentPosition.Left);
        }

        public void AddToCanvas(Canvas canvas)
        {
            canvas.Children.Add(Itself);
        }

        public void RemoveFromCanvas(Canvas canvas)
        {
            canvas.Children.Remove(Itself);
        }
    }
}
