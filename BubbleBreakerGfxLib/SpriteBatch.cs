using BubbleBreakerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace BubbleBreakerGfxLib
{
    public class SpriteBatch
    {
        private List<SpriteBatchItem> batch = new List<SpriteBatchItem>();
        private DispatcherTimer timer;

        public SpriteBatch()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, object e)
        {
            Animate();
        }

        public void AddToBatch(Sprite sprite, Position targetTopLeft)
        {
            SpriteBatchItem item = new SpriteBatchItem(ref sprite, targetTopLeft, 15);
            batch.Add(item);
        }

        public void Animate()
        {
            List<int> reachedTargets = new List<int>();
            foreach (var sprite in batch)
            {
                if (sprite.ReachedTarget)
                {
                    reachedTargets.Add(batch.IndexOf(sprite));
                }
                else
                    sprite.Animate();
            }
            for (int i = reachedTargets.Count - 1; i >= 0; i--)
            {
                batch.RemoveAt(reachedTargets[i]);
            }

        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}
