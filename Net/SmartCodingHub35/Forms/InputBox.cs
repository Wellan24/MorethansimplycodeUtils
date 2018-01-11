using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SimpleDialog
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Values that represent input box result types. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public enum InputBoxResultType
    {
        Any,    /* An enum constant representing any option */
        Boolean,    /* An enum constant representing the boolean option */
        Byte,   /* An enum constant representing the byte option */
        Char,   /* An enum constant representing the character option */
        Date,   /* An enum constant representing the date option */
        Decimal,    /* An enum constant representing the decimal option */
        Double, /* An enum constant representing the double option */
        Float,  /* An enum constant representing the float option */
        Int16,  /* An enum constant representing the int 16 option */
        Int32,  /* An enum constant representing the int 32 option */
        Int64,  /* An enum constant representing the int 64 option */
        UInt16, /* An enum constant representing the int 16 option */
        UInt32, /* An enum constant representing the int 32 option */
        UInt64  /* An enum constant representing the int 64 option */
    }

    ///------------------------------------------------------------------------------------------------------
    /// <summary> Summary description for InputBox. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public partial class InputBox : System.Windows.Forms.Form
    {
        private string defaultValue = string.Empty; /* The default value */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor that prevents a default instance of this class from being created. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        private InputBox()
        {
            InitializeComponent();
            Text = Application.ProductName;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Shows the dialog. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="validationType"> Type of the validation. </param>
        /// <returns> A String. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static String ShowDialog(InputBoxResultType validationType)
        {
            return InputBox.ShowDialog(null, null, null, validationType);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Shows the dialog. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="caption"> The caption. </param>
        /// <returns> A String. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static String ShowDialog(string caption)
        {
            return InputBox.ShowDialog(caption, null, null, InputBoxResultType.Any);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Shows the dialog. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="caption">        The caption. </param>
        /// <param name="validationType"> Type of the validation. </param>
        /// <returns> A String. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static String ShowDialog(string caption, InputBoxResultType validationType)
        {
            return InputBox.ShowDialog(caption, null, null, validationType);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Shows the dialog. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="caption"> The caption. </param>
        /// <param name="prompt">  The prompt. </param>
        /// <returns> A String. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static String ShowDialog(string caption, string prompt)
        {
            return InputBox.ShowDialog(caption, prompt, null, InputBoxResultType.Any);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Shows the dialog. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="caption">        The caption. </param>
        /// <param name="prompt">         The prompt. </param>
        /// <param name="validationType"> Type of the validation. </param>
        /// <returns> A String. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static String ShowDialog(string caption, string prompt, InputBoxResultType validationType)
        {
            return InputBox.ShowDialog(caption, prompt, null, validationType);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Shows the dialog. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="caption">        The caption. </param>
        /// <param name="prompt">         The prompt. </param>
        /// <param name="defaultValue">   The default value. </param>
        /// <param name="validationType"> Type of the validation. </param>
        /// <returns> A String. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static String ShowDialog(string caption, string prompt, string defaultValue, InputBoxResultType validationType)
        {
            // Create an instance of the InputBox class.
            InputBox inputBox = new InputBox();
            String input;
            // Set the members of the new instance
            // according to the value of the parameters
            if (string.IsNullOrEmpty(caption))
                inputBox.Text = Application.ProductName;
            else
                inputBox.Text = caption;

            if (!string.IsNullOrEmpty(prompt))
                inputBox.lblPrompt.Text = prompt;

            if (!string.IsNullOrEmpty(defaultValue))
                inputBox.defaultValue = inputBox.txtInput.Text = defaultValue;

            // Calculate size required for prompt message and adjust
            // Label and dialog size to fit.
            Size promptSize = inputBox.lblPrompt.CreateGraphics().MeasureString(prompt, inputBox.lblPrompt.Font,
               inputBox.ClientRectangle.Width - 20).ToSize();
            // a little wriggle room
            if (promptSize.Height > inputBox.lblPrompt.Height)
            {
                promptSize.Width += 4;
                promptSize.Height += 4;
            }

            inputBox.lblPrompt.Width = inputBox.ClientRectangle.Width - 20;
            inputBox.lblPrompt.Height = Math.Max(inputBox.lblPrompt.Height, promptSize.Height);

            int postLabelMargin = 2;

            if ((inputBox.lblPrompt.Top + inputBox.lblPrompt.Height + postLabelMargin) > inputBox.txtInput.Top)
            {
                inputBox.ClientSize = new Size(inputBox.ClientSize.Width, inputBox.ClientSize.Height +
                   (inputBox.lblPrompt.Top + inputBox.lblPrompt.Height + postLabelMargin - inputBox.txtInput.Top));
            }
            else if ((inputBox.lblPrompt.Top + inputBox.lblPrompt.Height + postLabelMargin) < inputBox.txtInput.Top)
            {
                inputBox.ClientSize = new Size(inputBox.ClientSize.Width, inputBox.ClientSize.Height -
                   (inputBox.lblPrompt.Top + inputBox.lblPrompt.Height + postLabelMargin - inputBox.txtInput.Top));
            }

            // Ensure that the value of input is set
            // There will be a compile error later if not
            input = string.Empty;

            // Declare a variable to hold the result to be
            // returned on exitting the method
            DialogResult result = DialogResult.None;

            // Loop round until the user enters
            // some valid data, or cancels.
            while (result == DialogResult.None)
            {
                result = inputBox.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // if user clicked OK, validate the entry
                    input = inputBox.txtInput.Text;

                    // Only test if specific type is required
                    if (validationType != InputBoxResultType.Any)
                    {
                        // If the test fails - Invalid input.
                        if (!inputBox.Validate(validationType))
                        {
                            // Set variables ready for another loop
                            input = string.Empty;
                            // result to 'None' to ensure while loop
                            // repeats
                            result = DialogResult.None;
                            // Let user know there is a problem
                            MessageBox.Show(inputBox, "The data entered is not a valid " + validationType.ToString() + ".");
                            // Set the focus back to the TextBox
                            inputBox.txtInput.Select();
                        }
                    }
                }
                else
                {
                    // User has cancelled.
                    // Use the defaultValue if there is one, or else
                    // an empty string.
                    if (string.IsNullOrEmpty(inputBox.defaultValue))
                    {
                        input = string.Empty;
                    }
                    else
                    {
                        input = inputBox.defaultValue;
                    }
                }
            }

            // Trash the dialog if it is hanging around.
            if (inputBox != null)
            {
                inputBox.Dispose();
            }

            // Send back the result.
            return input;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Validates the given validation type. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="validationType"> Type of the validation. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///--------------------------------------------------------------------------------------------------
        private bool Validate(InputBoxResultType validationType)
        {
            bool result = false;
            switch (validationType)
            {
                case InputBoxResultType.Boolean:
                    try
                    {
                        Boolean.Parse(this.txtInput.Text);
                        result = true;
                    }
                    catch
                    {
                        // just eat it
                    }
                    break;
                case InputBoxResultType.Byte:
                    try
                    {
                        Byte.Parse(this.txtInput.Text);
                        result = true;
                    }
                    catch
                    {
                        // just eat it
                    }
                    break;
                case InputBoxResultType.Char:
                    try
                    {
                        Char.Parse(this.txtInput.Text);
                        result = true;
                    }
                    catch
                    {
                        // just eat it
                    }
                    break;
                case InputBoxResultType.Date:
                    try
                    {
                        DateTime.Parse(this.txtInput.Text);
                        result = true;
                    }
                    catch
                    {
                        // just eat it
                    }
                    break;
                case InputBoxResultType.Decimal:
                    try
                    {
                        Decimal.Parse(this.txtInput.Text);
                        result = true;
                    }
                    catch
                    {
                        // just eat it
                    }
                    break;
                case InputBoxResultType.Double:
                    try
                    {
                        Double.Parse(this.txtInput.Text);
                        result = true;
                    }
                    catch
                    {
                        // just eat it
                    }
                    break;
                case InputBoxResultType.Float:
                    try
                    {
                        Single.Parse(this.txtInput.Text);
                        result = true;
                    }
                    catch
                    {
                        // just eat it
                    }
                    break;
                case InputBoxResultType.Int16:
                    try
                    {
                        Int16.Parse(this.txtInput.Text);
                        result = true;
                    }
                    catch
                    {
                        // just eat it
                    }
                    break;
                case InputBoxResultType.Int32:
                    try
                    {
                        Int32.Parse(this.txtInput.Text);
                        result = true;
                    }
                    catch
                    {
                        // just eat it
                    }
                    break;
                case InputBoxResultType.Int64:
                    try
                    {
                        Int64.Parse(this.txtInput.Text);
                        result = true;
                    }
                    catch
                    {
                        // just eat it
                    }
                    break;
                case InputBoxResultType.UInt16:
                    try
                    {
                        UInt16.Parse(this.txtInput.Text);
                        result = true;
                    }
                    catch
                    {
                        // just eat it
                    }
                    break;
                case InputBoxResultType.UInt32:
                    try
                    {
                        UInt32.Parse(this.txtInput.Text);
                        result = true;
                    }
                    catch
                    {
                        // just eat it
                    }
                    break;
                case InputBoxResultType.UInt64:
                    try
                    {
                        UInt64.Parse(this.txtInput.Text);
                        result = true;
                    }
                    catch
                    {
                        // just eat it
                    }
                    break;
            }

            return result;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by InputBox for load events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void InputBox_Load(object sender, System.EventArgs e)
        {
            this.txtInput.Focus();
        }
    }
}