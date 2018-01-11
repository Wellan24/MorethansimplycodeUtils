namespace Cartif.Forms
{
    partial class Toast
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerStart = new System.Windows.Forms.Timer(this.components);
            this.timerStop = new System.Windows.Forms.Timer(this.components);
            this.timerWait = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timerStart
            // 
            this.timerStart.Interval = 1;
            this.timerStart.Tick += new System.EventHandler(this.timerStart_Tick);
            // 
            // timerStop
            // 
            this.timerStop.Interval = 2;
            this.timerStop.Tick += new System.EventHandler(this.timerStop_Tick);
            // 
            // timerWait
            // 
            this.timerWait.Interval = 3000;
            this.timerWait.Tick += new System.EventHandler(this.timerEspera_Tick);
            // 
            // Toast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(147, 51);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Toast";
            this.Opacity = 0D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Toast";
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.Shown += new System.EventHandler(this.Toast_Shown);
            this.Click += new System.EventHandler(this.Toast_Click);
            this.MouseEnter += new System.EventHandler(this.Toast_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.Toast_MouseLeave);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerStart;
        private System.Windows.Forms.Timer timerStop;
        private System.Windows.Forms.Timer timerWait;
    }
}