using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace Cartif.CustomControls
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A group box color. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    class GroupBoxColor : GroupBox
    {
        private Color borderColor;  /* The border color */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the color of the border. </summary>
        /// <value> The color of the border. </value>
        ///--------------------------------------------------------------------------------------------------
        public Color BorderColor
        {
            get { return this.borderColor; }
            set { this.borderColor = value; }
        }

        private int borderWidth;    /* Width of the border */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the width of the border. </summary>
        /// <value> The width of the border. </value>
        ///--------------------------------------------------------------------------------------------------
        public int BorderWidth
        {
            get { return this.borderWidth; }
            set { this.borderWidth = value; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public GroupBoxColor()
        {
            this.borderColor = Color.Black;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Paints this window. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e"> A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event
        ///                  data. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            Size tSize = TextRenderer.MeasureText(this.Text, this.Font);

            Rectangle borderRect = this.DisplayRectangle;
            borderRect.Y += (tSize.Height / 2) - 12;
            borderRect.Height -= (tSize.Height / 2) - 12;

            ControlPaint.DrawBorder(e.Graphics, borderRect, this.borderColor, borderWidth,
                ButtonBorderStyle.Solid, this.borderColor, borderWidth, ButtonBorderStyle.Solid,
                this.borderColor, borderWidth, ButtonBorderStyle.Solid, this.borderColor, borderWidth,
                ButtonBorderStyle.Solid);

            Rectangle textRect = this.DisplayRectangle;
            textRect.X += 6;
            textRect.Width = tSize.Width;
            textRect.Height = tSize.Height;
            textRect.Y -= 13;

            using (SolidBrush brush = new SolidBrush(this.BackColor))
                e.Graphics.FillRectangle(brush, textRect);

            using (SolidBrush brush = new SolidBrush(this.ForeColor))
                e.Graphics.DrawString(this.Text, this.Font, brush, textRect);
        }
    }
}

