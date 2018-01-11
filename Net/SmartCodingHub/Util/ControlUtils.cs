using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace Cartif.Util
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Control class utilities. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class ControlUtils
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Change color brightness. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="color">            The color. </param>
        /// <param name="correctionFactor"> The correction factor. </param>
        /// <returns> A Color. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the parent of this item. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="c"> The Control to process. </param>
        /// <returns> The non transparent color from parent. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static Color GetNonTransparentColorFromParent(Control c)
        {
            Color ParentColor = c.Parent.BackColor;
            Control parent = c.Parent;
            while (ParentColor == Color.Transparent)
            {
                parent = parent.Parent;
                ParentColor = parent.BackColor;
            }

            return ParentColor;
        }
    }
}
