using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cartif.Extensions;
using System.Drawing;
using System.Windows.Forms;
using Cartif.Util;

namespace Cartif.CustomControls
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A form text button. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class FormTextButton : AbstractButton
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the radius. </summary>
        /// <value> The radius. </value>
        ///--------------------------------------------------------------------------------------------------
        public int Radius { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the string format. </summary>
        /// <value> The string format. </value>
        ///--------------------------------------------------------------------------------------------------
        public StringFormat StringFormat { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the string alignment. </summary>
        /// <value> The string alignment. </value>
        ///--------------------------------------------------------------------------------------------------
        public StringAlignment StringAlignment
        {
            get { return StringFormat.Alignment; }
            set { StringFormat.Alignment = value; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the string line alignment. </summary>
        /// <value> The string line alignment. </value>
        ///--------------------------------------------------------------------------------------------------
        public StringAlignment StringLineAlignment
        {
            get { return StringFormat.LineAlignment; }
            set { StringFormat.LineAlignment = value; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public FormTextButton()
            : base()
        {
            Radius = 4;
            BackColor = Color.FromArgb(200, 200, 200);
            ForeColor = Color.White;
            StringFormat = new StringFormat();
        }

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

            Color ParentColor = Parent.BackColor;
            Control parent = Parent;
            while (ParentColor == Color.Transparent)
            {
                parent = parent.Parent;
                ParentColor = parent.BackColor;
            }

            using (Brush brush = new SolidBrush(ParentColor))
                e.Graphics.FillRectangle(brush, rect);

            rect.Width -= 1;
            rect.Height -= 1;

            if (Pressed)
            {
                Color DarkerColor = ControlUtils.ChangeColorBrightness(HoverColor, -0.15f);
                using (SolidBrush brush = new SolidBrush(DarkerColor))
                    e.Graphics.FillRoundedRectangle(brush, rect, Radius);

            }
            else if (Hover)
            {
                using (SolidBrush brush = new SolidBrush(HoverColor))
                    e.Graphics.FillRoundedRectangle(brush, rect, Radius);
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(BackColor))
                    e.Graphics.FillRoundedRectangle(brush, rect, Radius);
            }

            if (Text != null)
            {
                rect.X += Padding.Left;
                rect.Y += Padding.Top;
                rect.Width -= Padding.Left + Padding.Right;
                rect.Height -= Padding.Top + Padding.Bottom;
                using (SolidBrush brush = new SolidBrush(ForeColor))
                    e.Graphics.DrawString(Text, Font, brush, rect, StringFormat);
            }
        }
    }
}
