using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace Cartif.CustomControls
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A button personalizado. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class ButtonPersonalizado : Button
    {
        GraphicsPath border = new GraphicsPath();   /* The border */
        int radius = 3; /* The radius */
        float borderWidth;  /* Width of the border */
        Color borderColor = Color.Orange;   /* The border color */
        bool _checked;  /* true if checked */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the width of the border. </summary>
        /// <value> The width of the border. </value>
        ///--------------------------------------------------------------------------------------------------
        public float BorderWidth
        {
            get { return borderWidth; }
            set
            {
                borderWidth = value;
                Invalidate();
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the color of the border. </summary>
        /// <value> The color of the border. </value>
        ///--------------------------------------------------------------------------------------------------
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public ButtonPersonalizado()
        {
            BorderWidth = 4;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether the checked. </summary>
        /// <value> true if checked, false if not. </value>
        ///--------------------------------------------------------------------------------------------------
        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                Invalidate();
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Genera el evento <see cref="E:System.Windows.Forms.Control.MouseHover" />. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e"> Objeto <see cref="T:System.EventArgs" /> que contiene los datos del evento. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnMouseHover(EventArgs e)
        {
            //base.OnMouseHover(e);
            //"#31793B"
            //"#8CC63E"
            this.BackColor = ColorTranslator.FromHtml("#8CC63E");

        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Raises the mouse enter event. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e"> Proporciona información del evento. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnMouseEnter(EventArgs e)
        {
            //base.OnMouseEnter(e);
            this.BackColor = ColorTranslator.FromHtml("#8CC63E");
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Raises the mouse leave event. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e"> Proporciona la información de evento que falta. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.BackColor = Color.Transparent;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Genera el evento
        ///           <see cref="M:System.Windows.Forms.ButtonBase.OnPaint(System.Windows.Forms.PaintEventArgs)" />. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="pevent"> Objeto <see cref="T:System.Windows.Forms.PaintEventArgs" /> que contiene los
        ///                       datos del evento. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            if (Checked)
            {
                pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                for (float f = BorderWidth; f >= 0.01f; f -= 1f)
                {
                    using (Pen pen = new Pen(Color.FromArgb((int)(100 - 100 * f * f / (BorderWidth * BorderWidth)), borderColor), f))
                    {
                        pen.LineJoin = LineJoin.Round;
                        pen.Alignment = PenAlignment.Center;
                        pevent.Graphics.DrawPath(pen, border);
                    }
                }
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Updates the border. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        private void UpdateBorder()
        {
            border = new GraphicsPath();
            RectangleF rect = new RectangleF { Width = radius * 2, Height = radius * 2, X = BorderWidth / 2, Y = BorderWidth / 2 };
            border.AddArc(rect, 180, 90);
            rect.X = ClientSize.Width - BorderWidth / 2 - radius * 2 - 0.5f;
            border.AddArc(rect, 270, 90);
            rect.Y = ClientSize.Height - BorderWidth / 2 - radius * 2 - 0.5f;
            border.AddArc(rect, 0, 90);
            rect.X = BorderWidth / 2;
            border.AddArc(rect, 90, 90);
            border.CloseAllFigures();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Genera el evento <see cref="E:System.Windows.Forms.Control.SizeChanged" />. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e"> Objeto <see cref="T:System.EventArgs" /> que contiene los datos del evento. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateBorder();
        }
    }
}
