namespace Cartif.UserControls
{
    partial class Tab
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
            this.bLeft = new System.Windows.Forms.Button();
            this.bRight = new System.Windows.Forms.Button();
            this.tLeft = new System.Windows.Forms.Timer(this.components);
            this.tRight = new System.Windows.Forms.Timer(this.components);
            this.container = new System.Windows.Forms.Panel();
            this.panel = new System.Windows.Forms.FlowLayoutPanel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.container.SuspendLayout();
            this.SuspendLayout();
            // 
            // bLeft
            // 
            this.bLeft.BackColor = System.Drawing.SystemColors.Control;
            this.bLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.bLeft.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.bLeft.ForeColor = System.Drawing.Color.Black;
            this.bLeft.Location = new System.Drawing.Point(0, 0);
            this.bLeft.Name = "bLeft";
            this.bLeft.Size = new System.Drawing.Size(20, 25);
            this.bLeft.TabIndex = 0;
            this.bLeft.Text = "<";
            this.bLeft.UseVisualStyleBackColor = false;
            this.bLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.bLeft_MouseDown);
            this.bLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bLeft_MouseUp);
            // 
            // bRight
            // 
            this.bRight.BackColor = System.Drawing.SystemColors.Control;
            this.bRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.bRight.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.bRight.ForeColor = System.Drawing.Color.Black;
            this.bRight.Location = new System.Drawing.Point(50, 0);
            this.bRight.Name = "bRight";
            this.bRight.Size = new System.Drawing.Size(20, 25);
            this.bRight.TabIndex = 1;
            this.bRight.Text = ">";
            this.bRight.UseVisualStyleBackColor = false;
            this.bRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.bRight_MouseDown);
            this.bRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bRight_MouseUp);
            // 
            // tLeft
            // 
            this.tLeft.Interval = 4;
            this.tLeft.Tick += new System.EventHandler(this.tLeft_Tick);
            // 
            // tRight
            // 
            this.tRight.Interval = 4;
            this.tRight.Tick += new System.EventHandler(this.tRight_Tick);
            // 
            // container
            // 
            this.container.BackColor = System.Drawing.Color.White;
            this.container.Controls.Add(this.panel);
            this.container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.container.Location = new System.Drawing.Point(20, 0);
            this.container.Name = "container";
            this.container.Size = new System.Drawing.Size(30, 25);
            this.container.TabIndex = 2;
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel.AutoSize = true;
            this.panel.BackColor = System.Drawing.Color.White;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Margin = new System.Windows.Forms.Padding(0);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(5);
            this.panel.Size = new System.Drawing.Size(30, 25);
            this.panel.TabIndex = 3;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // Tab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.container);
            this.Controls.Add(this.bRight);
            this.Controls.Add(this.bLeft);
            this.MinimumSize = new System.Drawing.Size(70, 25);
            this.Name = "Tab";
            this.Size = new System.Drawing.Size(70, 25);
            this.container.ResumeLayout(false);
            this.container.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bLeft;
        private System.Windows.Forms.Button bRight;
        private System.Windows.Forms.Timer tLeft;
        private System.Windows.Forms.Timer tRight;
        private System.Windows.Forms.Panel container;
        private System.Windows.Forms.FlowLayoutPanel panel;
        private System.Windows.Forms.Timer timer;
    }
}
