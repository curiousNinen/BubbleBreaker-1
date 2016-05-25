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
        public bool ReachedTarget => _sprite.TargetPosition.Equals(_sprite.CurrentPosition);
        private int _tick;
        private int _frames;
        private Sprite _sprite;
        public SpriteBatchItem(Sprite sprite, Position targetTopLeft, int ReachInFrames)
        {
            _sprite = sprite;
            _frames = ReachInFrames;
            _sprite.MakeCurrentStart();
            _sprite.SetTargetPosition(new Position(targetTopLeft));
            _tick = 0;
        }

        public void Animate()
        {
            _tick++;
            _sprite.ChangePositionOnTick(_tick, _frames);
        }
    }
}
