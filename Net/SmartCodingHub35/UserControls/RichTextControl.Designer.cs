namespace Cartif.UserControls
{
    partial class RichTextControl
    {
        /// <summary> 
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.bUnderline = new System.Windows.Forms.Button();
            this.bItalic = new System.Windows.Forms.Button();
            this.bBold = new System.Windows.Forms.Button();
            this.richTextComments = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // bUnderline
            // 
            this.bUnderline.BackColor = System.Drawing.Color.White;
            this.bUnderline.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bUnderline.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bUnderline.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bUnderline.Location = new System.Drawing.Point(69, 3);
            this.bUnderline.Name = "bUnderline";
            this.bUnderline.Size = new System.Drawing.Size(26, 23);
            this.bUnderline.TabIndex = 6;
            this.bUnderline.Text = "U";
            this.bUnderline.UseVisualStyleBackColor = false;
            this.bUnderline.Click += new System.EventHandler(this.bUnderline_Click);
            // 
            // bItalic
            // 
            this.bItalic.BackColor = System.Drawing.Color.White;
            this.bItalic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bItalic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bItalic.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bItalic.Location = new System.Drawing.Point(37, 3);
            this.bItalic.Name = "bItalic";
            this.bItalic.Size = new System.Drawing.Size(26, 23);
            this.bItalic.TabIndex = 5;
            this.bItalic.Text = "I";
            this.bItalic.UseVisualStyleBackColor = false;
            this.bItalic.Click += new System.EventHandler(this.bItalic_Click);
            // 
            // bBold
            // 
            this.bBold.BackColor = System.Drawing.Color.White;
            this.bBold.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bBold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bBold.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bBold.Location = new System.Drawing.Point(5, 3);
            this.bBold.Name = "bBold";
            this.bBold.Size = new System.Drawing.Size(26, 23);
            this.bBold.TabIndex = 4;
            this.bBold.Text = "B";
            this.bBold.UseVisualStyleBackColor = false;
            this.bBold.Click += new System.EventHandler(this.bBold_Click);
            // 
            // richTextComments
            // 
            this.richTextComments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextComments.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextComments.Location = new System.Drawing.Point(5, 32);
            this.richTextComments.Name = "richTextComments";
            this.richTextComments.Size = new System.Drawing.Size(527, 253);
            this.richTextComments.TabIndex = 7;
            this.richTextComments.Text = "";
            // 
            // RichTextControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.richTextComments);
            this.Controls.Add(this.bUnderline);
            this.Controls.Add(this.bItalic);
            this.Controls.Add(this.bBold);
            this.Name = "RichTextControl";
            this.Size = new System.Drawing.Size(535, 288);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bUnderline;
        private System.Windows.Forms.Button bItalic;
        private System.Windows.Forms.Button bBold;
        private System.Windows.Forms.RichTextBox richTextComments;
    }
}
