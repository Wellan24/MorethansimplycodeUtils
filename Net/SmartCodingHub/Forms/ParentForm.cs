using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using System.Windows.Forms;
using Cartif.Extensions;
using Cartif.Util;

namespace Cartif.Forms
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Form for viewing the parent. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public partial class ParentForm : Form
    {
        #region Win32 Constants

        private const int WM_NCLBUTTONDOWN = 0xA1;  /* The windows message nclbuttondown */
        private const int HT_CAPTION = 0x2; /* The height caption */

        private const int wmNcHitTest = 0x84;   /* The windows message non-client hit test */
        private const int htLeft = 10;  /* The height left */
        private const int htRight = 11; /* The height right */
        private const int htTop = 12;   /* The height top */
        private const int htTopLeft = 13;   /* The height top left */
        private const int htTopRight = 14;  /* The height top right */
        private const int htBottom = 15;    /* The height bottom */
        private const int htBottomLeft = 16;    /* The height bottom left */
        private const int htBottomRight = 17;   /* The height bottom right */
        private const int resizeBorderSize = 10;    /* Size of the resize border */

        #endregion

        #region Win32 dll

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Sends a message. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="hWnd">   The window. </param>
        /// <param name="Msg">    The message. </param>
        /// <param name="wParam"> The parameter. </param>
        /// <param name="lParam"> The parameter. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Releases the capture. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///--------------------------------------------------------------------------------------------------
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Creates round rectangle region. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="nLeftRect">      The left rectangle. </param>
        /// <param name="nTopRect">       The top rectangle. </param>
        /// <param name="nRightRect">     The right rectangle. </param>
        /// <param name="nBottomRect">    The bottom rectangle. </param>
        /// <param name="nWidthEllipse">  The width ellipse. </param>
        /// <param name="nHeightEllipse"> The height ellipse. </param>
        /// <returns> The new round rectangle region. </returns>
        ///--------------------------------------------------------------------------------------------------
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
         );

        #endregion

        private Rectangle restoreRectangle; /* The restore rectangle */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the color of the border. </summary>
        /// <value> The color of the border. </value>
        ///--------------------------------------------------------------------------------------------------
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Color BorderColor { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the title. </summary>
        /// <value> The title. </value>
        ///--------------------------------------------------------------------------------------------------
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public String Title { get { return title.Text; } set { title.Text = value; } }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public ParentForm()
        {
            BorderColor = Color.Gray;
            InitializeComponent();
            InitImages();
            InitCustomEvents();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Initialises the images. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        private void InitImages()
        {
            close.Image = SingletonImages.CLOSEFORM.Image;
            maximize.Image = SingletonImages.MAXIMIZEFORM.Image;
            minimize.Image = SingletonImages.MINIMIZEFORM.Image;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Initialises the custom events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        private void InitCustomEvents()
        {
            close.Click += close_Click;
            maximize.Click += maximize_Click;
            minimize.Click += minimize_Click;

            this.MouseDown += MoveForm;
            titleBar.MouseDown += MoveForm;
            title.MouseDown += MoveForm;
            logo.MouseDown += MoveForm;
        }

        #region Form Functionality Events

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by close for click events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by maximize for click events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void maximize_Click(object sender, EventArgs e)
        {
            if (maximize.Image.Equals(SingletonImages.MAXIMIZEFORM.Image))
            {
                restoreRectangle = new Rectangle(this.Location, this.Size);
                Screen screen = Screen.FromPoint(this.Location);
                Rectangle rectangle = screen.WorkingArea;
                this.Location = rectangle.Location;
                this.Size = rectangle.Size;
                maximize.Image = SingletonImages.RESTOREFORM.Image;
            }
            else
            {
                this.Size = restoreRectangle.Size;
                this.Location = restoreRectangle.Location;
                maximize.Image = SingletonImages.MAXIMIZEFORM.Image;
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by minimize for click events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Move form. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Mouse event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void MoveForm(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion

        #region Overriden Methods

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Paints this window. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e"> A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event
        ///                  data. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Rectangle rect = new Rectangle(2, 2, this.Width - 4, this.Height - 4);

            using (Pen pen = new Pen(BorderColor, 2))
                e.Graphics.DrawRectangle(pen, ClientRectangle);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Raises the resize event. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.titleBar.Width = this.Width - 2;
            Refresh();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Window proc. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="m"> [in,out] The Windows <see cref="T:System.Windows.Forms.Message" /> to process. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void WndProc(ref Message m)
        {

            if (m.Msg == wmNcHitTest)
            {
                Point pt = this.PointToClient(Cursor.Position);
                Size clientSize = ClientSize;
                ///allow resize on the lower right corner
                if (pt.X >= clientSize.Width - resizeBorderSize && pt.Y >= clientSize.Height - resizeBorderSize && clientSize.Height >= resizeBorderSize)
                {
                    m.Result = (IntPtr)(IsMirrored ? htBottomLeft : htBottomRight);
                    return;
                }
                ///allow resize on the lower left corner
                if (pt.X <= resizeBorderSize && pt.Y >= clientSize.Height - resizeBorderSize && clientSize.Height >= resizeBorderSize)
                {
                    m.Result = (IntPtr)(IsMirrored ? htBottomRight : htBottomLeft);
                    return;
                }
                ///allow resize on the upper right corner
                if (pt.X <= resizeBorderSize && pt.Y <= resizeBorderSize && clientSize.Height >= resizeBorderSize)
                {
                    m.Result = (IntPtr)(IsMirrored ? htTopRight : htTopLeft);
                    return;
                }
                ///allow resize on the upper left corner
                if (pt.X >= clientSize.Width - resizeBorderSize && pt.Y <= resizeBorderSize && clientSize.Height >= resizeBorderSize)
                {
                    m.Result = (IntPtr)(IsMirrored ? htTopLeft : htTopRight);
                    return;
                }
                ///allow resize on the top border
                if (pt.Y <= resizeBorderSize && clientSize.Height >= resizeBorderSize)
                {
                    m.Result = (IntPtr)(htTop);
                    return;
                }
                ///allow resize on the bottom border
                if (pt.Y >= clientSize.Height - resizeBorderSize && clientSize.Height >= resizeBorderSize)
                {
                    m.Result = (IntPtr)(htBottom);
                    return;
                }
                ///allow resize on the left border
                if (pt.X <= resizeBorderSize && clientSize.Height >= resizeBorderSize)
                {
                    m.Result = (IntPtr)(htLeft);
                    return;
                }
                ///allow resize on the right border
                if (pt.X >= clientSize.Width - resizeBorderSize && clientSize.Height >= resizeBorderSize)
                {
                    m.Result = (IntPtr)(htRight);
                    return;
                }
            }
            base.WndProc(ref m);
        }

        #endregion
    }
}
