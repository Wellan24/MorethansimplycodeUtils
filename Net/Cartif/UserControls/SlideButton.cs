using Cartif.Util;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Cartif.UserControls
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A slide button. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    class SlideButton : Button
    {
        private Boolean isOpen = true;  /* true if this SlideButton is open */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether this SlideButton is open. </summary>
        /// <value> true if this SlideButton is open, false if not. </value>
        ///--------------------------------------------------------------------------------------------------
        public Boolean IsOpen
        {
            get { return isOpen; }
            set
            {
                isOpen = value;

                /* Refresh the button with the new appearence */
                this.Refresh();

                /* Call the event */
                if (OnSlideStateChangedEvent != null)
                    OnSlideStateChangedEvent(isOpen);
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public SlideButton()
            : base()
        {
            this.BackColor = Color.White;
            if (!DesignMode)
                IsOpen = false;

            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Event for the Slide
        /// </summary>
        [Category("ClickCustomEvents")]
        [Description("Fired when this is clicked")]
        public event OnSlideStateChanged OnSlideStateChangedEvent;  /* Event queue for all listeners interested in onSlideStateChanged events. */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Delegate for the Change of state. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="isOpen"> . </param>
        ///--------------------------------------------------------------------------------------------------
        public delegate void OnSlideStateChanged(Boolean isOpen);

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Override to avoid the default OnClick. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e"> . </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnClick(EventArgs e)
        {
            IsOpen = !isOpen;
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
            /* SETTINGS -> for open/close states of the button */
            Color topColor = Color.LightGray;
            Color otherSidesColor = Color.LightGray;
            Color backgroud = Color.LightGray;
            SingletonImages arrow = SingletonImages.ARROW_DOWN;
            Rectangle arrowPosition = new Rectangle(this.Width - 30, this.Height - 26, 20, 20);

            if (isOpen)
            {
                topColor = Color.LightGray;
                otherSidesColor = Color.Transparent;
                backgroud = Color.White;
                arrow = SingletonImages.ARROW_UP;
            }

            /* PAINT ZONE */
            /* Background */
            using (SolidBrush brush = new SolidBrush(backgroud))
                e.Graphics.FillRectangle(brush, new Rectangle(0, 0, this.Width, this.Height));

            /* Border */
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
            otherSidesColor, 2, ButtonBorderStyle.Solid,
            topColor, 2, ButtonBorderStyle.Solid,
            otherSidesColor, 2, ButtonBorderStyle.Solid,
            otherSidesColor, 2, ButtonBorderStyle.Solid);

            /* Text -> Padding = 5 (This can be changed to use the padding property of the control if need) */
            e.Graphics.DrawString(this.Text, (!isOpen) ? this.Font : new Font(Font, FontStyle.Bold), Brushes.Black, new Point(10, 8));

            /* Arrow */
            e.Graphics.DrawImage(arrow.Image, arrowPosition);
        }
    }
}
