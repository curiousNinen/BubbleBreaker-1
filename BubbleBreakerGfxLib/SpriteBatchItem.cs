using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleBreakerLib;
using Windows.UI.Xaml;

namespace BubbleBreakerGfxLib
{
    public class SpriteBatchItem
    {
        private int _tick;
        private int _frames;
        private double _movementZ;
        private double _movementS; 
        private Sprite _sprite;
        private Position _moveToTopLeft;

        public bool ReachedTarget => _moveToTopLeft.Equals(_sprite.TopLeft);

        public SpriteBatchItem(ref Sprite sprite, Position moveToTopLeft, int reachInFrames)
        {
            _sprite = sprite;
            _frames = reachInFrames;
            _moveToTopLeft = moveToTopLeft;
           _movementZ = (_moveToTopLeft.Top - _sprite.TopLeft.Top) / _frames;
            _movementS = (_moveToTopLeft.Left - _sprite.TopLeft.Left) / _frames;
            _tick = 0;
        }

        public void Animate()
        {
            _tick++;
            if (ReachedTarget) return;
            _sprite.MoveBy((int)_movementZ, (int)_movementS);
        }
    }
}
