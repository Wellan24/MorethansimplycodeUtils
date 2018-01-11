using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Cartif.Extensions;
using System.Drawing.Drawing2D;
using Cartif.CustomControls;

namespace Cartif.Forms
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A toast. </summary>
    /// <remarks> Oscvic, 2016-01-04. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public partial class Toast : Form
    {
        #region Static Properties

        private static ToastStack toastStack = new ToastStack();    /* Stack of toasts */
        private static CartifTableColor defaultTableColor = new CartifTableColor(); /* The default table color */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the default table color. </summary>
        /// <value> The default table color. </value>
        ///--------------------------------------------------------------------------------------------------
        public static CartifTableColor DefaultTableColor
        {
            get { return Toast.defaultTableColor; }
            set { Toast.defaultTableColor = value; }
        }

        private static CartifTableColor defaultToastErrorTableColor = new DefaultErrorTableColor(); /* The default toast error table color */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the default toast error table color. </summary>
        /// <value> The default toast error table color. </value>
        ///--------------------------------------------------------------------------------------------------
        public static CartifTableColor DefaultToastErrorTableColor
        {
            get { return Toast.defaultToastErrorTableColor; }
            set { Toast.defaultToastErrorTableColor = value; }
        }

        #endregion

        #region Properties

        private Padding margin; /* The margin */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the margin. </summary>
        /// <value> The margin. </value>
        ///--------------------------------------------------------------------------------------------------
        new public Padding Margin
        {
            get { return margin; }
            set { margin = value; }
        }
        private CartifTableColor tableColor;    /* The table color */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the size of the check. </summary>
        /// <value> The size of the check. </value>
        ///--------------------------------------------------------------------------------------------------
        public int CheckSize { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the size of the border. </summary>
        /// <value> The size of the border. </value>
        ///--------------------------------------------------------------------------------------------------
        public int BorderSize { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether this Toast is error toast. </summary>
        /// <value> true if this Toast is error toast, false if not. </value>
        ///--------------------------------------------------------------------------------------------------
        private Boolean IsErrorToast { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether the checked. </summary>
        /// <value> true if checked, false if not. </value>
        ///--------------------------------------------------------------------------------------------------
        private Boolean Checked { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether the dispose after show. </summary>
        /// <value> true if dispose after show, false if not. </value>
        ///--------------------------------------------------------------------------------------------------
        private Boolean DisposeAfterShow { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the show time. </summary>
        /// <value> The show time. </value>
        ///--------------------------------------------------------------------------------------------------
        public int ShowTime
        {
            get { return timerWait.Interval; }
            set { this.timerWait.Interval = value; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the form parent. </summary>
        /// <value> The form parent. </value>
        ///--------------------------------------------------------------------------------------------------
        public Form FormParent { get; set; }
        private Point textLocation; /* The text location */

        #endregion

        #region Initializate

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="parent">  The parent. </param>
        /// <param name="message"> The message. </param>
        ///--------------------------------------------------------------------------------------------------
        public Toast(Form parent, string message)
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.DoubleBuffered = true;
            this.tableColor = defaultTableColor;
            InitializeComponent();

            this.BackColor = Color.Transparent;

            this.FormParent = parent;
            parent.LocationChanged += parent_LocationChanged;
            parent.FormClosing += parent_FormClosing;
            parent.Resize += parent_Resize;
            this.Text = message;

            this.FormParent.Focus();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="parent">           The parent. </param>
        /// <param name="message">          The message. </param>
        /// <param name="cartifTableColor"> The cartif table color. </param>
        ///--------------------------------------------------------------------------------------------------
        public Toast(Form parent, string message, CartifTableColor cartifTableColor)
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.DoubleBuffered = true;
            this.tableColor = cartifTableColor;
            InitializeComponent();

            this.BackColor = Color.Transparent;

            this.FormParent = parent;
            parent.LocationChanged += parent_LocationChanged;
            parent.FormClosing += parent_FormClosing;
            parent.Resize += parent_Resize;
            this.Text = message;

            this.FormParent.Focus();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Initialises the status. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        ///--------------------------------------------------------------------------------------------------
        private void InitStatus()
        {
            try
            {
                this.ShowInTaskbar = false;
                this.BorderSize = 3;
                this.Padding = new Padding(20);
                this.Margin = new Padding(20);
                this.CheckSize = 30;

                SizeF textSize = this.CreateGraphics().MeasureString(this.Text, this.Font);

                int checkSize = 0;
                if (Checked)
                    checkSize = CheckSize + BorderSize;

                /* Set Size */
                this.Size = new Size((int)textSize.Width + Padding.Left + Padding.Right + checkSize, (int)textSize.Height + Padding.Top + Padding.Bottom);

                /* Init Text Settings to Center it */
                float width = (Checked) ? checkSize + 15 : (Width / 2 - textSize.Width / 2) + 1;
                float height = (Height / 2 - textSize.Height / 2) + 1;
                width = width <= 1 ? 1 : width;
                height = height <= 1 ? 1 : height;
                textLocation = new Point((int)width, (int)height);

                toastStack.AddToast(this);

                Relocalize();
                this.Opacity = 0.00D;
            }
            catch (Exception)
            {
                /* Empty */
            }
        }
        #endregion

        #region Paint

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Paints this window. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="e"> <see cref="T:System.Windows.Forms.PaintEventArgs" /> que contiene los datos del
        ///                  evento. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle backRect = new Rectangle(BorderSize, BorderSize, this.Width - BorderSize * 2, this.Height - BorderSize * 2);

            /* Paints the background */
            using (SolidBrush brush = new SolidBrush(tableColor.ToastBackColor))
                e.Graphics.FillRoundedRectangle(brush, backRect.X + 1, backRect.Y + 1, backRect.Width - 1, backRect.Height - 1, 10);

            /* Paints the border */
            using (Pen pen = new Pen(tableColor.ToastBorder, BorderSize))
                e.Graphics.DrawRoundedRectangle(pen, backRect, 10);

            /* Draw the string */
            if (this.Text != null)
                e.Graphics.DrawString(this.Text, Font, new SolidBrush(tableColor.ToastForeColor), textLocation);
            /* Check Settings */
            int leftOffset = 16 + BorderSize;
            int topOffset = 12 + BorderSize;
            int tickOffset = 6;
            int thickness = 6;
            int borderThickness = 2;
            int checkBoxSize = 15;

            /* Draw tick or Cross */
            if (Checked)
            {
                if (IsErrorToast)
                {
                    using (Pen pen = new Pen(tableColor.ToastCheck, borderThickness))
                    {
                        e.Graphics.DrawLine(pen, new Point(leftOffset, topOffset + checkBoxSize + tickOffset), new Point(leftOffset + checkBoxSize, topOffset + tickOffset));
                        e.Graphics.DrawLine(pen, new Point(leftOffset, topOffset + tickOffset), new Point(leftOffset + checkBoxSize, topOffset + tickOffset + checkBoxSize));
                    }
                }
                else
                {
                    GraphicsPath path = new GraphicsPath();

                    Point[] points = new Point[]
                                    {
                                        new Point(leftOffset , topOffset + tickOffset + 2), // Left
                                        new Point(leftOffset + (int)(checkBoxSize / 2), topOffset + (checkBoxSize) + tickOffset), // Center bot
                                        new Point(leftOffset + checkBoxSize + 5, topOffset - 5) , // Right
                                        new Point(leftOffset + (int)(checkBoxSize / 2), topOffset + (checkBoxSize) + tickOffset - thickness), //Center top
                                    };
                    path.AddLines(points);

                    using (Brush brush = new SolidBrush(tableColor.ToastCheck))
                        e.Graphics.FillPath(brush, path);
                }
            }
        }
        #endregion

        #region Effects

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Timer para aparecer. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="sender"> . </param>
        /// <param name="e">      . </param>
        ///--------------------------------------------------------------------------------------------------
        private void timerStart_Tick(object sender, EventArgs e)
        {
            if (this.Opacity > 0.9D)
            {
                this.timerStart.Stop();
                this.timerWait.Start();
            }
            else
            {
                this.Opacity += 0.02D;
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> La espera de un par de segundos. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="sender"> . </param>
        /// <param name="e">      . </param>
        ///--------------------------------------------------------------------------------------------------
        private void timerEspera_Tick(object sender, EventArgs e)
        {
            this.timerStop.Start();
            this.timerWait.Stop();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Timer para desvanecer. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="sender"> . </param>
        /// <param name="e">      . </param>
        ///--------------------------------------------------------------------------------------------------
        private void timerStop_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.01D;
            if (this.Opacity < 0.01D)
            {
                this.timerStop.Stop();
                toastStack.RemoveToast(this);
                this.Close();
                this.Dispose();
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by Toast for mouse enter events. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="sender"> . </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void Toast_MouseEnter(object sender, EventArgs e)
        {
            this.timerStart.Stop();
            this.timerStop.Stop();
            this.timerWait.Stop();
            this.Opacity = 1D;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by Toast for mouse leave events. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="sender"> . </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void Toast_MouseLeave(object sender, EventArgs e) { this.timerWait.Start(); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by Toast for shown events. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="sender"> . </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void Toast_Shown(object sender, EventArgs e)
        {
            InitStatus();
            timerStart.Start();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Reshow toast. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public void ReshowToast()
        {
            this.timerStop.Stop();
            this.timerStart.Stop();
            this.timerWait.Stop();
            this.Opacity = 1D;
        }

        #endregion

        #region Methods

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Shows the message. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="parent">  The parent. </param>
        /// <param name="message"> The message. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void ShowMessage(Form parent, String message) { ShowMessageInner(parent, message, false, 5000); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Shows the message. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="parent">    The parent. </param>
        /// <param name="message">   The message. </param>
        /// <param name="isChecked"> true if this Toast is checked. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void ShowMessage(Form parent, String message, Boolean isChecked) { ShowMessageInner(parent, message, isChecked, 5000); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Shows the message. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="parent">   The parent. </param>
        /// <param name="message">  The message. </param>
        /// <param name="showTime"> The show time. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void ShowMessage(Form parent, String message, int showTime) { ShowMessageInner(parent, message, false, showTime); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Shows the message. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="parent">    The parent. </param>
        /// <param name="message">   The message. </param>
        /// <param name="isChecked"> true if this Toast is checked. </param>
        /// <param name="showTime">  The show time. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void ShowMessage(Form parent, String message, Boolean isChecked, int showTime) { ShowMessageInner(parent, message, isChecked, showTime); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Shows the message inner. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="parent">    The parent. </param>
        /// <param name="message">   The message. </param>
        /// <param name="isChecked"> true if this Toast is checked. </param>
        /// <param name="showTime">  The show time. </param>
        ///--------------------------------------------------------------------------------------------------
        private static void ShowMessageInner(Form parent, String message, Boolean isChecked, int showTime)
        {
            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() =>
                {
                    if (!toastStack.CheckIfExistAndReshow(parent, message))
                    {
                        Toast toast = new Toast(parent, message);
                        toast.Checked = isChecked;
                        toast.ShowTime = showTime;
                        toast.Show(parent);
                    }
                }));
            }
            else
            {
                if (!toastStack.CheckIfExistAndReshow(parent, message))
                {
                    Toast toast = new Toast(parent, message);
                    toast.Checked = isChecked;
                    toast.ShowTime = showTime;
                    toast.Show(parent);
                }
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Shows the error message. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="parent">  The parent. </param>
        /// <param name="message"> The message. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void ShowErrorMessage(Form parent, String message) { ShowErrorMessageInner(parent, message, true, 5000); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Shows the error message. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="parent">   The parent. </param>
        /// <param name="message">  The message. </param>
        /// <param name="showTime"> The show time. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void ShowErrorMessage(Form parent, String message, int showTime) { ShowErrorMessageInner(parent, message, true, showTime); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Shows the error message. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="parent">    The parent. </param>
        /// <param name="message">   The message. </param>
        /// <param name="isChecked"> true if this Toast is checked. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void ShowErrorMessage(Form parent, String message, Boolean isChecked) { ShowErrorMessageInner(parent, message, isChecked, 5000); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Shows the error message. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="parent">    The parent. </param>
        /// <param name="message">   The message. </param>
        /// <param name="isChecked"> true if this Toast is checked. </param>
        /// <param name="showTime">  The show time. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void ShowErrorMessage(Form parent, String message, Boolean isChecked, int showTime) { ShowErrorMessageInner(parent, message, isChecked, showTime); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Shows the error message inner. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="parent">    The parent. </param>
        /// <param name="message">   The message. </param>
        /// <param name="isChecked"> true if this Toast is checked. </param>
        /// <param name="showTime">  The show time. </param>
        ///--------------------------------------------------------------------------------------------------
        private static void ShowErrorMessageInner(Form parent, String message, Boolean isChecked, int showTime)
        {
            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() =>
                {
                    if (!toastStack.CheckIfExistAndReshow(parent, message))
                    {
                        Toast toast = new Toast(parent, message);
                        toast.Checked = isChecked;
                        toast.ShowTime = showTime;
                        toast.IsErrorToast = true;
                        toast.tableColor = DefaultErrorTableColor.INSTANCE;
                        toast.Show(parent);
                    }
                }));
            }
            else
            {
                if (!toastStack.CheckIfExistAndReshow(parent, message))
                {
                    Toast toast = new Toast(parent, message);
                    toast.Checked = isChecked;
                    toast.ShowTime = showTime;
                    toast.IsErrorToast = true;
                    toast.tableColor = DefaultErrorTableColor.INSTANCE;
                    toast.Show(parent);
                }
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Relocalizes this Toast. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        ///--------------------------------------------------------------------------------------------------
        private void Relocalize()
        {
            int anchorX;
            int anchorY;

            /* If there is another Toast, you want this over the previous. If not, use the parent as anchor */

            Toast previousToast = toastStack.TakePreviousToast(this);
            if (previousToast != null)
            {
                anchorX = previousToast.Location.X + previousToast.Width + previousToast.Margin.Right;
                anchorY = previousToast.Location.Y;
            }
            else
            {
                anchorX = FormParent.Location.X + FormParent.Width;
                anchorY = FormParent.Location.Y + FormParent.Height;
            }


            Point p = new Point(anchorX - this.Width - this.margin.Right,
                anchorY - this.Height - this.margin.Bottom);

            this.Location = p;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by parent for location changed events. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="sender"> . </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void parent_LocationChanged(object sender, EventArgs e) { Relocalize(); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by parent for resize events. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="sender"> . </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void parent_Resize(object sender, EventArgs e) { Relocalize(); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by parent for form closing events. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="sender"> . </param>
        /// <param name="e">      Form closing event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void parent_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by Toast for click events. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="sender"> . </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void Toast_Click(object sender, EventArgs e)
        {
            toastStack.RemoveToast(this);
            this.Close();
            this.Dispose();
        }

        #endregion

    }
}
