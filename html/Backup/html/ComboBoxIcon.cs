using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace UtilETWeb
{
    public class ComboBoxIcon : ComboBox
    {
        private Bitmap img = Properties.Resources.arrow;
        public Bitmap Img
        {
            get
            {
                return img;
            }
            set
            {
                img = value;
            }
        }

        public static bool IsInDesignMode()
        {
            if (Application.ExecutablePath.IndexOf("devenv.exe", StringComparison.OrdinalIgnoreCase) > -1)
            {
                return true;
            }
            return false;
        }

        public ComboBoxIcon()
            : base()
        {
            if (!IsInDesignMode())
            {
                this.DoubleBuffered = true;
                this.ResizeRedraw = true;
                this.DrawMode = DrawMode.OwnerDrawVariable;
                this.DropDownStyle = ComboBoxStyle.DropDownList;
                this.SelectedIndex = -1;
                this.DrawItem += new DrawItemEventHandler(ComboBoxIcon_DrawItem);
            }
        }

        void ComboBoxIcon_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBoxIcon cmb = sender as ComboBoxIcon;

            //draw back groud of the item
            e.DrawBackground();
            e.DrawFocusRectangle();

            //Draw the image in combo box using its bound, here size of image is
            // 10, 10 you can increase the size if you want
            e.Graphics.DrawImage(img, e.Bounds.X, e.Bounds.Y, img.Width, img.Height);

            //we need to draw the item as string because we made drawmode to ownervariable
            e.Graphics.DrawString(GetPropValue(cmb.Items[e.Index], this.DisplayMember).ToString(), cmb.Font,
                new SolidBrush(cmb.ForeColor),
                new RectangleF(e.Bounds.X + 16, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            //draw rectangle over the item selected
            e.DrawFocusRectangle();
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
