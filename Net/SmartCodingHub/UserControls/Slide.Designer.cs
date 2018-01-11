namespace Cartif.UserControls
{
    partial class Slide
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
            this.components = new System.ComponentModel.Container();
            this.timerSlide = new System.Windows.Forms.Timer(this.components);
            this.button = new SlideButton();
            this.SuspendLayout();
            // 
            // timerSlide
            // 
            this.timerSlide.Interval = 1;
            this.timerSlide.Tick += new System.EventHandler(this.timerSlide_Tick);
            // 
            // button
            // 
            this.button.BackColor = System.Drawing.Color.White;
            this.button.Dock = System.Windows.Forms.DockStyle.Top;
            this.button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button.IsOpen = true;
            this.button.Location = new System.Drawing.Point(0, 0);
            this.button.Margin = new System.Windows.Forms.Padding(0);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(720, 32);
            this.button.TabIndex = 0;
            this.button.Text = "Slide";
            this.button.UseVisualStyleBackColor = false;
            this.button.OnSlideStateChangedEvent += new SlideButton.OnSlideStateChanged(this.button_OnSlideStateChangedEvent);
            this.button.MouseEnter += new System.EventHandler(this.boton_MouseEnter);
            this.button.MouseLeave += new System.EventHandler(this.boton_MouseLeave);
            // 
            // Slide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "Slide";
            this.Size = new System.Drawing.Size(720, 313);
            this.ResumeLayout(false);

        }

        #endregion

        private SlideButton button;
        private System.Windows.Forms.Timer timerSlide;




    }
}
