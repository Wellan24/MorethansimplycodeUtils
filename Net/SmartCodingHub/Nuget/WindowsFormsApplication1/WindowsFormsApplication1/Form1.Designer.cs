namespace WindowsFormsApplication1
{
    partial class Form1
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

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.Drawing.StringFormat stringFormat1 = new System.Drawing.StringFormat();
            this.formTextButton1 = new Cartif.CustomControls.FormTextButton();
            this.formIconButton1 = new Cartif.CustomControls.FormIconButton();
            this.SuspendLayout();
            // 
            // formTextButton1
            // 
            this.formTextButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.formTextButton1.ForeColor = System.Drawing.Color.White;
            this.formTextButton1.Hover = false;
            this.formTextButton1.HoverColor = System.Drawing.Color.Gray;
            this.formTextButton1.Location = new System.Drawing.Point(308, 225);
            this.formTextButton1.Name = "formTextButton1";
            this.formTextButton1.Pressed = false;
            this.formTextButton1.Radius = 4;
            this.formTextButton1.Size = new System.Drawing.Size(178, 66);
            this.formTextButton1.StringAlignment = System.Drawing.StringAlignment.Near;
            stringFormat1.Alignment = System.Drawing.StringAlignment.Near;
            stringFormat1.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat1.LineAlignment = System.Drawing.StringAlignment.Near;
            stringFormat1.Trimming = System.Drawing.StringTrimming.Character;
            this.formTextButton1.StringFormat = stringFormat1;
            this.formTextButton1.StringLineAlignment = System.Drawing.StringAlignment.Near;
            this.formTextButton1.TabIndex = 0;
            this.formTextButton1.Text = "formTextButton1";
            this.formTextButton1.UseVisualStyleBackColor = false;
            // 
            // formIconButton1
            // 
            this.formIconButton1.Hover = false;
            this.formIconButton1.HoverColor = System.Drawing.Color.Gray;
            this.formIconButton1.Location = new System.Drawing.Point(360, 128);
            this.formIconButton1.Name = "formIconButton1";
            this.formIconButton1.Pressed = false;
            this.formIconButton1.Size = new System.Drawing.Size(75, 25);
            this.formIconButton1.TabIndex = 1;
            this.formIconButton1.Text = "formIconButton1";
            this.formIconButton1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 528);
            this.Controls.Add(this.formIconButton1);
            this.Controls.Add(this.formTextButton1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Cartif.CustomControls.FormTextButton formTextButton1;
        private Cartif.CustomControls.FormIconButton formIconButton1;
    }
}

