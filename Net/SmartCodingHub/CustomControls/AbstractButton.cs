using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Cartif.Extensions;
using System.Windows.Forms;

namespace Cartif.CustomControls
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> An abstract button. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class AbstractButton : Button
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether the hover. </summary>
        /// <value> true if hover, false if not. </value>
        ///--------------------------------------------------------------------------------------------------
        public bool Hover { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether the pressed. </summary>
        /// <value> true if pressed, false if not. </value>
        ///--------------------------------------------------------------------------------------------------
        public bool Pressed { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the color of the hover. </summary>
        /// <value> The color of the hover. </value>
        ///--------------------------------------------------------------------------------------------------
        public Color HoverColor { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public AbstractButton()
            : base()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.DoubleBuffered = true;

            Hover = false;
            Pressed = false;
            HoverColor = Color.Gray;

            this.MouseEnter += (sender, e) => { Hover = true; Cursor = Cursors.Hand; };
            this.MouseLeave += (sender, e) => {Hover = false; Cursor = Cursors.Arrow;};

            this.MouseDown += (sender, e) => Pressed = true;
            this.MouseUp += (sender, e) => Pressed = false;
        }
    }
}
