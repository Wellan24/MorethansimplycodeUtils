using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.ComponentModel;
using Cartif.Extensions;

namespace Cartif.UserControls
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A switch button. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class SwitchButton : Control
    {
        [Category("ClickCustomEvents")]
        [Description("Fired when this is clicked")]
        public event OnSwitchButtonSelected OnSwitchButtonSelectedEvent;    /* Event queue for all listeners interested in onSwitchButtonSelected events. */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Executes the switch button selected action. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="button"> The button. </param>
        ///--------------------------------------------------------------------------------------------------
        public delegate void OnSwitchButtonSelected(SwitchButton button);

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the extreme. </summary>
        /// <value> The extreme. </value>
        ///--------------------------------------------------------------------------------------------------
        public Extreme Extreme { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the radius. </summary>
        /// <value> The radius. </value>
        ///--------------------------------------------------------------------------------------------------
        public float Radius { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the color of the top. </summary>
        /// <value> The color of the top. </value>
        ///--------------------------------------------------------------------------------------------------
        public Color TopColor { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the color of the bottom. </summary>
        /// <value> The color of the bottom. </value>
        ///--------------------------------------------------------------------------------------------------
        public Color BottomColor { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the percentage of dark. </summary>
        /// <value> The percentage of dark. </value>
        ///--------------------------------------------------------------------------------------------------
        public float PercentageOfDark { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the value. </summary>
        /// <value> The value. </value>
        ///--------------------------------------------------------------------------------------------------
        public Object Value { get; set; }

        private Boolean selected;   /* true if selected */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether the selected. </summary>
        /// <value> true if selected, false if not. </value>
        ///--------------------------------------------------------------------------------------------------
        public Boolean Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                this.Refresh();
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public SwitchButton()
        {
            this.DoubleBuffered = true;
            this.Extreme = Extreme.Left;
            this.TopColor = Color.WhiteSmoke;
            this.BottomColor = Color.WhiteSmoke;
            this.PercentageOfDark = 0.05f;
            this.Radius = 2;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="Value"> The value. </param>
        ///--------------------------------------------------------------------------------------------------
        public SwitchButton(Object Value)
            : this()
        {
            this.Value = Value;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="Extreme"> The extreme. </param>
        ///--------------------------------------------------------------------------------------------------
        public SwitchButton(Extreme Extreme)
            : this()
        {
            this.Extreme = Extreme;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="Extreme">     The extreme. </param>
        /// <param name="TopColor">    The top color. </param>
        /// <param name="BottomColor"> The bottom color. </param>
        ///--------------------------------------------------------------------------------------------------
        public SwitchButton(Extreme Extreme, Color TopColor, Color BottomColor)
            : this(Extreme)
        {
            this.TopColor = TopColor;
            this.BottomColor = BottomColor;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="Extreme">     The extreme. </param>
        /// <param name="TopColor">    The top color. </param>
        /// <param name="BottomColor"> The bottom color. </param>
        /// <param name="Value">       The value. </param>
        ///--------------------------------------------------------------------------------------------------
        public SwitchButton(Extreme Extreme, Color TopColor, Color BottomColor, Object Value)
            : this(Extreme, TopColor, BottomColor)
        {
            this.Value = Value;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="Extreme">          The extreme. </param>
        /// <param name="TopColor">         The top color. </param>
        /// <param name="BottomColor">      The bottom color. </param>
        /// <param name="PercentageOfDark"> The percentage of dark. </param>
        /// <param name="Radius">           The radius. </param>
        /// <param name="Value">            The value. </param>
        ///--------------------------------------------------------------------------------------------------
        public SwitchButton(Extreme Extreme, Color TopColor, Color BottomColor, float PercentageOfDark, float Radius, Object Value)
            : this(Extreme, TopColor, BottomColor, Value)
        {
            this.PercentageOfDark = PercentageOfDark;
            this.Radius = Radius;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Pinta el fondo del control. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="pevent"> <see cref="T:System.Windows.Forms.PaintEventArgs" /> que contiene información
        ///                       sobre el control que se va a dibujar. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (this.Parent != null)
            {
                GraphicsContainer cstate = pevent.Graphics.BeginContainer();
                pevent.Graphics.TranslateTransform(-this.Left, -this.Top);
                Rectangle clip = pevent.ClipRectangle;
                clip.Offset(this.Left, this.Top);
                PaintEventArgs pe = new PaintEventArgs(pevent.Graphics, clip);

                //paint the container's bg
                InvokePaintBackground(this.Parent, pe);
                //paints the container fg
                InvokePaint(this.Parent, pe);
                //restores graphics to its original state
                pevent.Graphics.EndContainer(cstate);
            }
            else
                base.OnPaintBackground(pevent);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Genera el evento <see cref="E:System.Windows.Forms.Control.Paint" />. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e"> Objeto <see cref="T:System.Windows.Forms.PaintEventArgs" /> que contiene los datos
        ///                  del evento. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleEdgeFilter filter = RectangleEdgeFilter.None;

            if (Extreme == Extreme.Left)
                filter = RectangleEdgeFilter.TopLeft | RectangleEdgeFilter.BottomLeft;
            else if (Extreme == Extreme.Right)
                filter = RectangleEdgeFilter.TopRight | RectangleEdgeFilter.BottomRight;
            else if (Extreme == Extreme.NoExtreme)
                filter = RectangleEdgeFilter.All;

            PaintArea(e, filter, TopColor, BottomColor, selected);

            if (this.Value != null)
            {
                String text = Value.ToString();
                SizeF size = e.Graphics.MeasureString(text, Font);
                float width = (Width / 2 - size.Width / 2) + 1;
                float height = (Height / 2 - size.Height / 2) + 1;
                width = width <= 1 ? 1 : width;
                height = height <= 1 ? 1 : height;
                e.Graphics.DrawString(text, Font, Brushes.Black, new Point((int)width, (int)height));
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Paints the area. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e">          Paint event information. </param>
        /// <param name="filter">     Specifies the filter. </param>
        /// <param name="StartColor"> The start color. </param>
        /// <param name="EndColor">   The end color. </param>
        /// <param name="invertDark"> true to invert dark. </param>
        ///--------------------------------------------------------------------------------------------------
        private void PaintArea(PaintEventArgs e, RectangleEdgeFilter filter, Color StartColor, Color EndColor, Boolean invertDark)
        {
            StartColor = (invertDark) ? ControlPaint.Dark(StartColor, PercentageOfDark) : StartColor;
            EndColor = (invertDark) ? ControlPaint.Light(StartColor, 1f) : ControlPaint.Dark(EndColor, PercentageOfDark);


            using (LinearGradientBrush brush = new LinearGradientBrush(
                new Point(this.Width / 2, (invertDark) ? -(this.Height / 2) : 0),
                new Point(this.Width / 2, this.Height + ((invertDark) ? 0 : this.Height / 2)),
                StartColor, EndColor))
                e.Graphics.FillRoundedRectangle(brush, 0, 0, this.Width - 1, this.Height, 10, filter);

            using (Pen pen = new Pen((invertDark) ? StartColor : EndColor))
                e.Graphics.DrawRoundedRectangle(pen, 0, 0, this.Width - 1, this.Height - 1, 10, filter);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Genera el evento <see cref="E:System.Windows.Forms.Control.Click" />. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e"> Objeto <see cref="T:System.EventArgs" /> que contiene los datos del evento. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnClick(EventArgs e)
        {
            if (OnSwitchButtonSelectedEvent != null)
                OnSwitchButtonSelectedEvent(this);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Genera el evento <see cref="E:System.Windows.Forms.Control.MouseEnter" />. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e"> Objeto <see cref="T:System.EventArgs" /> que contiene los datos del evento. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnMouseEnter(EventArgs e) { this.Cursor = Cursors.Hand; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Genera el evento <see cref="E:System.Windows.Forms.Control.MouseLeave" />. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e"> Objeto <see cref="T:System.EventArgs" /> que contiene los datos del evento. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnMouseLeave(EventArgs e) { this.Cursor = Cursors.Arrow; }
    }

    ///------------------------------------------------------------------------------------------------------
    /// <summary> Values that represent extremes. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public enum Extreme { Right, Center, Left, NoExtreme }
}
