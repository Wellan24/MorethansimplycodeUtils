namespace Cartif.Forms
{
    partial class ParentForm
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
            this.titleBar = new System.Windows.Forms.Panel();
            this.logo = new System.Windows.Forms.PictureBox();
            this.title = new System.Windows.Forms.Label();
            this.minimize = new Cartif.CustomControls.FormIconButton();
            this.close = new Cartif.CustomControls.FormIconButton();
            this.maximize = new Cartif.CustomControls.FormIconButton();
            this.content = new System.Windows.Forms.Panel();
            this.titleBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.SuspendLayout();
            // 
            // titleBar
            // 
            this.titleBar.Controls.Add(this.logo);
            this.titleBar.Controls.Add(this.title);
            this.titleBar.Controls.Add(this.minimize);
            this.titleBar.Controls.Add(this.close);
            this.titleBar.Controls.Add(this.maximize);
            this.titleBar.Location = new System.Drawing.Point(1, 1);
            this.titleBar.Name = "titleBar";
            this.titleBar.Size = new System.Drawing.Size(831, 45);
            this.titleBar.TabIndex = 7;
            // 
            // logo
            // 
            this.logo.Location = new System.Drawing.Point(20, 8);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(25, 25);
            this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logo.TabIndex = 8;
            this.logo.TabStop = false;
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(137)))), ((int)(((byte)(193)))));
            this.title.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.title.Location = new System.Drawing.Point(50, 14);
            this.title.Margin = new System.Windows.Forms.Padding(3);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(211, 17);
            this.title.TabIndex = 7;
            this.title.Text = "Main Window - DELTA Platform";
            // 
            // minimize
            // 
            this.minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minimize.Hover = false;
            this.minimize.HoverColor = System.Drawing.Color.Gainsboro;
            this.minimize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.minimize.Location = new System.Drawing.Point(735, -2);
            this.minimize.Margin = new System.Windows.Forms.Padding(0);
            this.minimize.Name = "minimize";
            this.minimize.Padding = new System.Windows.Forms.Padding(4);
            this.minimize.Pressed = false;
            this.minimize.Size = new System.Drawing.Size(32, 32);
            this.minimize.TabIndex = 6;
            this.minimize.Text = "minimize";
            this.minimize.UseVisualStyleBackColor = true;
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.close.Hover = false;
            this.close.HoverColor = System.Drawing.Color.Red;
            this.close.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.close.Location = new System.Drawing.Point(798, -2);
            this.close.Margin = new System.Windows.Forms.Padding(0);
            this.close.Name = "close";
            this.close.Padding = new System.Windows.Forms.Padding(4);
            this.close.Pressed = false;
            this.close.Size = new System.Drawing.Size(32, 32);
            this.close.TabIndex = 4;
            this.close.Text = "close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // maximize
            // 
            this.maximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maximize.Hover = false;
            this.maximize.HoverColor = System.Drawing.Color.Gainsboro;
            this.maximize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.maximize.Location = new System.Drawing.Point(766, -2);
            this.maximize.Margin = new System.Windows.Forms.Padding(0);
            this.maximize.Name = "maximize";
            this.maximize.Padding = new System.Windows.Forms.Padding(4);
            this.maximize.Pressed = false;
            this.maximize.Size = new System.Drawing.Size(32, 32);
            this.maximize.TabIndex = 5;
            this.maximize.Text = "maximize";
            this.maximize.UseVisualStyleBackColor = true;
            // 
            // content
            // 
            this.content.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.content.Location = new System.Drawing.Point(20, 45);
            this.content.Margin = new System.Windows.Forms.Padding(0);
            this.content.Name = "content";
            this.content.Size = new System.Drawing.Size(790, 430);
            this.content.TabIndex = 8;
            // 
            // ParentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(831, 496);
            this.Controls.Add(this.content);
            this.Controls.Add(this.titleBar);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ParentForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.titleBar.ResumeLayout(false);
            this.titleBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cartif.CustomControls.FormIconButton minimize;
        private Cartif.CustomControls.FormIconButton maximize;
        private Cartif.CustomControls.FormIconButton close;
        private System.Windows.Forms.Panel titleBar;
        private System.Windows.Forms.Label title;
        public System.Windows.Forms.Panel content;
        private System.Windows.Forms.PictureBox logo;
    }
}