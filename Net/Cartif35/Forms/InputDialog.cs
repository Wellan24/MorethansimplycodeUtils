using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace Cartif.Forms
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> -----------------------------------------------------------------
    ///              Namespace:      Cartif.Forms Class:          InputDialog Description:    Provides
    ///              methos to show a InputDialog Author:         Oscar - Cartif       Date: 03-11-2015
    ///              Notes: Revision History: Name:           Date:        Description:
    ///           -----------------------------------------------------------------. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class InputDialog
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Shows the dialog. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="text">    The text. </param>
        /// <param name="caption"> The caption. </param>
        /// <returns> A string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form();
            prompt.Width = 400;
            prompt.Height = 100;
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.Text = caption;
            prompt.StartPosition = FormStartPosition.CenterScreen;
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 300 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : null;
        }
    }
}
