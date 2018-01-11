using Cartif.Util;
using Cartif.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace Cartif.CustomControls
{
    class FormTextIconButton : AbstractButton
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        public new Image Image
        {
            get { return base.Image; }
            set
            {
                if (value != null)
                {
                    base.Image = value;
                    imageSize.Width = value.Width;
                    imageSize.Height = value.Height;
                    textLocation.X = ImageLocation.X + TextGap + imageSize.Width;
                    textLocation.Y = 0;
                }
            }
        }

        public int TextGap { get; set; }
        private Point textLocation;
        public Point TextLocation { get { return textLocation; } set { textLocation = value; } }
        public Point ImageLocation { get; set; }

        private Size imageSize;
        public Size ImageSize
        {
            get { return imageSize; }
            set
            {
                if (value != null)
                {
                    imageSize = value;
                    textLocation.X = ImageLocation.X + TextGap + imageSize.Width;
                    textLocation.Y = 0;
                }
            }
        }

        public FormTextIconButton()
            : base()
        {
            ForeColor = Color.White;
            Font = new Font("Arial", 9.75f);
            BackColor = Color.FromArgb(50, 50, 50);
            HoverColor = Color.FromArgb(80, 80, 80);
            StringFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            TextLocation = new Point(Padding.Left, Padding.Top);
            ImageLocation = new Point(Padding.Left, Padding.Top);
            ImageSize = new Size(0, 0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, this.Size.Width, this.Size.Height);

            using (Brush brush = new SolidBrush(this.GetNonTransparentColorFromParent()))
                e.Graphics.FillRectangle(brush, rect);

            rect.Width -= 1;
            rect.Height -= 1;

            if (Pressed)
            {
                Color DarkerColor = ControlUtils.ChangeColorBrightness(HoverColor, -0.15f);
                using (SolidBrush brush = new SolidBrush(DarkerColor))
                    e.Graphics.FillRoundedRectangle(brush, rect, Radius);
            }
            else if (Hover)
            {
                using (SolidBrush brush = new SolidBrush(HoverColor))
                    e.Graphics.FillRoundedRectangle(brush, rect, Radius);
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(BackColor))
                    e.Graphics.FillRoundedRectangle(brush, rect, Radius); 
            }

            if (Image != null)
            {
                rect.X = ImageLocation.X;
                rect.Y = ImageLocation.Y;
                rect.Width = imageSize.Width;
                rect.Height = imageSize.Height;

                e.Graphics.DrawImage(Image, rect);
            }

            if (Text != null)
            {
                rect.X = textLocation.X;
                rect.Y = textLocation.Y;
                rect.Width = Width - textLocation.X;
                rect.Height = Height - textLocation.Y;
                using (SolidBrush brush = new SolidBrush(ForeColor))
                    e.Graphics.DrawString(Text, Font, brush, rect, StringFormat);
            }
        }
    }
}
