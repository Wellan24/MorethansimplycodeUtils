using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using Cartif.Extensions;

namespace Cartif.UserControls
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A rich text control. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public partial class RichTextControl : UserControl
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the RTF. </summary>
        /// <value> The RTF. </value>
        ///--------------------------------------------------------------------------------------------------
        public String Rtf
        {
            get
            {
                return richTextComments.Rtf;
            }
            set
            {
                richTextComments.Rtf = value;
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the text. </summary>
        /// <value> The text associated with this control. </value>
        ///--------------------------------------------------------------------------------------------------
        public override String Text
        {
            get
            {
                return richTextComments.Text;
            }
            set
            {
                richTextComments.Text = value;
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public RichTextControl()
        {
            InitializeComponent();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by bBold for click events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void bBold_Click(object sender, EventArgs e)
        {
            ToggleFontKeepSelection(richTextComments, FontStyle.Bold);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by bItalic for click events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void bItalic_Click(object sender, EventArgs e)
        {
            ToggleFontKeepSelection(richTextComments, FontStyle.Italic);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by bUnderline for click events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void bUnderline_Click(object sender, EventArgs e)
        {
            ToggleFontKeepSelection(richTextComments, FontStyle.Underline);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Toggle font keep selection. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="richTextBox">       The rich control. </param>
        /// <param name="fontStyleToToggle"> The font style to toggle. </param>
        ///--------------------------------------------------------------------------------------------------
        private void ToggleFontKeepSelection(RichTextBox richTextBox, FontStyle fontStyleToToggle)
        {
            int selstart = richTextBox.SelectionStart;
            int sellength = richTextBox.SelectionLength;

            if (richTextBox.SelectionFont.Style.HasFlag(fontStyleToToggle))
                richTextBox.SelectionFont = new Font(richTextBox.SelectionFont, richTextBox.SelectionFont.Style ^ fontStyleToToggle);
            else
                richTextBox.SelectionFont = new Font(richTextBox.SelectionFont, richTextBox.SelectionFont.Style | fontStyleToToggle);

            richTextBox.SelectionStart = selstart;
            richTextBox.SelectionLength = sellength;
            richTextBox.Select();
        }
    }
}
