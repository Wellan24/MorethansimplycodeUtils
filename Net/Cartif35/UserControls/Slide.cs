using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel.Design;

namespace Cartif.UserControls
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A slide. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    [Designer("System.Windows.Forms.Design.ParentControlDesigner,System.Design", typeof(IDesigner))]
    public partial class Slide : UserControl
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether this Slide is shown. </summary>
        /// <value> true if this Slide is shown, false if not. </value>
        ///--------------------------------------------------------------------------------------------------
        public Boolean IsShown
        {
            get { return button.IsOpen; }
            set
            {
                button.IsOpen = value;
            }
        }

        private int isShownHeight;  /* Height of the is shown */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the height of the is shown. </summary>
        /// <value> The height of the is shown. </value>
        ///--------------------------------------------------------------------------------------------------
        public int IsShownHeight
        {
            get { return isShownHeight - button.Height; }
            set { isShownHeight = value + button.Height; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the title. </summary>
        /// <value> The title. </value>
        ///--------------------------------------------------------------------------------------------------
        public String Title
        {
            get { return button.Text; }
            set { button.Text = value; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public Slide()
        {
            InitializeComponent();
            this.SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint |
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.Opaque |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                true);
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by boton for mouse enter events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void boton_MouseEnter(object sender, EventArgs e)
        {
            button.BackColor = Color.Gray;
            button.Cursor = Cursors.Hand;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by boton for mouse leave events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void boton_MouseLeave(object sender, EventArgs e)
        {
            button.BackColor = Color.White;
            button.Cursor = Cursors.Arrow;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Button slide state changed event. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="isOpen"> true if this Slide is open. </param>
        ///--------------------------------------------------------------------------------------------------
        private void button_OnSlideStateChangedEvent(bool isOpen)
        {
            CalculateIsShownHeight();
            if (!timerSlide.Enabled)
            {
                timerSlide.Start();
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Calculates the is shown height. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        private void CalculateIsShownHeight()
        {
            if (IsShown)
            {
                int height = 0;
                int lastY = 0;
                ControlCollection controls = this.Controls;

                foreach (Control control in controls)
                {
                    if (lastY < control.Location.Y)
                    {
                        height = control.Height + control.Location.Y + 42;
                        lastY = control.Location.Y;
                    }
                }

                isShownHeight = height;
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Pinta el fondo del control. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e"> Objeto <see cref="T:System.Windows.Forms.PaintEventArgs" /> que contiene los datos
        ///                  del evento. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Genera el evento <see cref="E:System.Windows.Forms.Control.Paint" />. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e"> Objeto <see cref="T:System.Windows.Forms.PaintEventArgs" /> que contiene los datos
        ///                  del evento. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            using (Brush b = new SolidBrush(this.BackColor))
                e.Graphics.FillRectangle(b, 0, 0, this.Width, this.Height);

            int rectWidth = this.Width / 10;
            e.Graphics.FillRectangle(Brushes.LightGray, this.Width / 2 - rectWidth / 2, this.Height - 3, rectWidth, 3);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by timerSlide for tick events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void timerSlide_Tick(object sender, EventArgs e)
        {
            if (IsShown && this.Height + button.Height < isShownHeight)
            {
                int crecimiento = (isShownHeight - button.Height) / 35;
                crecimiento = (crecimiento < 8) ? 8 : crecimiento;
                this.Height += this.Height + crecimiento >= isShownHeight ? isShownHeight - this.Height : crecimiento;
            }
            else if (!IsShown && this.Height > button.Height)
            {
                int decrecimiento = (isShownHeight - button.Height) / 35;
                decrecimiento = (decrecimiento < 8) ? 8 : decrecimiento;
                this.Height -= this.Height - decrecimiento <= button.Height ? this.Height - button.Height : decrecimiento;
            }
            else
            {
                timerSlide.Stop();
            }
        }
    }
}
