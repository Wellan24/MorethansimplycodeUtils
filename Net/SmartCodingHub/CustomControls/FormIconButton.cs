using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using Cartif.Extensions;
using System.Drawing.Drawing2D;
using Cartif.Util;

namespace Cartif.CustomControls
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A form icon button. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class FormIconButton : AbstractButton
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Genera el evento
        ///           <see cref="M:System.Windows.Forms.ButtonBase.OnPaint(System.Windows.Forms.PaintEventArgs)" />. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e"> Objeto <see cref="T:System.Windows.Forms.PaintEventArgs" /> que contiene los datos
        ///                  del evento. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, this.Size.Width, this.Size.Height);

            if (Pressed)
            {
                Color DarkerColor = ControlUtils.ChangeColorBrightness(HoverColor, -0.15f);
                using (SolidBrush brush = new SolidBrush(DarkerColor))
                    e.Graphics.FillRectangle(brush, rect);

            }
            else if (Hover)
            {
                using (SolidBrush brush = new SolidBrush(HoverColor))
                    e.Graphics.FillRectangle(brush, rect);
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(BackColor))
                    e.Graphics.FillRectangle(brush, rect);
            }            

            if (Image != null)
            {
                rect.X += Padding.Left;
                rect.Y += Padding.Top;
                rect.Width -= Padding.Left + Padding.Right;
                rect.Height -= Padding.Top + Padding.Bottom;
                e.Graphics.DrawImage(Image, rect);
            }
        }
    }
}
