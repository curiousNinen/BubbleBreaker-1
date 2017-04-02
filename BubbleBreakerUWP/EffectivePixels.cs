using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;

namespace BubbleBreakerUWP
{

    internal enum DimensionType { Height, Width }
    public static class EffectivePixels
    {
        public static uint Height
        {
            get { return GetEffectivePixels(DimensionType.Height); }
        }

        public static uint Width
        {
            get { return GetEffectivePixels(DimensionType.Width); }
        }

        /// <summary>
        /// Calculate Effective Pixels based on RawPixelCount and ResolutionScale
        /// </summary>
        /// <param name="t">calculate width or height</param>
        /// <returns>0 if invalid, effecective pixel height/width based on t</returns>
        private static uint GetEffectivePixels(DimensionType t)
        {
            DisplayInformation info = DisplayInformation.GetForCurrentView();
            uint r = 0;
            switch (t)
            {
                case DimensionType.Height:
                    r = info.ScreenHeightInRawPixels;
                    break;
                case DimensionType.Width:
                    r = info.ScreenWidthInRawPixels;
                    break;
            }
            float sf = 0;
            switch (info.ResolutionScale)
            {
                case ResolutionScale.Invalid:
                    sf = 0;
                    break;
                case ResolutionScale.Scale100Percent:
                    sf = 1;
                    break;
                case ResolutionScale.Scale120Percent:
                    sf = 1 / 1.2f;
                    break;
                case ResolutionScale.Scale125Percent:
                    sf = 1 / 1.25f;
                    break;
                case ResolutionScale.Scale140Percent:
                    sf = 1 / 1.4f;
                    break;
                case ResolutionScale.Scale150Percent:
                    sf = 1 / 1.5f;
                    break;
                case ResolutionScale.Scale160Percent:
                    sf = 1 / 1.6f;
                    break;
                case ResolutionScale.Scale175Percent:
                    sf = 1 / 1.75f;
                    break;
                case ResolutionScale.Scale180Percent:
                    sf = 1 / 1.8f;
                    break;
                case ResolutionScale.Scale200Percent:
                    sf = 1 / 2f;
                    break;
                case ResolutionScale.Scale225Percent:
                    sf = 1 / 2.25f;
                    break;
                case ResolutionScale.Scale250Percent:
                    sf = 1 / 2.5f;
                    break;
                case ResolutionScale.Scale300Percent:
                    sf = 1 / 3f;
                    break;
                case ResolutionScale.Scale350Percent:
                    sf = 1 / 3.5f;
                    break;
                case ResolutionScale.Scale400Percent:
                    sf = 1 / 4f;
                    break;
                case ResolutionScale.Scale450Percent:
                    sf = 1 / 4.5f;
                    break;
                case ResolutionScale.Scale500Percent:
                    sf = 1 / 5f;
                    break;
            }

            r = (uint)(r * sf);

            return r;
        }
    }
}
