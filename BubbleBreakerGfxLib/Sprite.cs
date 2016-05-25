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
        public Position StartPosition { get; private set; }
        public Position CurrentPosition { get; private set; }
        public Position TargetPosition { get; private set; }
        public UIElement Itself { get; private set; }

        public Sprite(UIElement sprite, Position topLeftStart)
        {
            Itself = sprite;
            SetStartPosition(topLeftStart);
        }

        public void SetStartPosition(Position topLeftStart)
        {
            StartPosition = new Position(topLeftStart);
            CurrentPosition = new Position(StartPosition);
            Canvas.SetTop(Itself, StartPosition.Top);
            Canvas.SetLeft(Itself, StartPosition.Left);
        }

        public void MakeCurrentStart()
        {
            SetStartPosition(CurrentPosition);
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

        public void ChangePositionOnTick(double tick, double frames)
        {
            if (StartPosition.Equals(TargetPosition)) return;
            double coeff = tick / frames;
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
