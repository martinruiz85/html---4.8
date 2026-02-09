using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
//using Microsoft.Office.Interop.Excel;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace UtilETWeb
{
    public class ComboBoxIcon : SuggestComboBox //ComboBox
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
            System.Drawing.Font font = cmb.Font;

            //draw back groud of the item
            e.DrawBackground();
            e.DrawFocusRectangle();

            if (e.Index == -1)
                return;

            //Draw the image in combo box using its bound, here size of image is
            // 10, 10 you can increase the size if you want
            e.Graphics.DrawImage(img, e.Bounds.X, e.Bounds.Y, img.Width, img.Height);

            //we need to draw the item as string because we made drawmode to ownervariable            
            string text = GetPropValue(cmb.Items[e.Index], this.DisplayMember).ToString();
            if (text.ToLower().Contains("pro"))
                font = new System.Drawing.Font(font, FontStyle.Bold);

            e.Graphics.DrawString(GetPropValue(cmb.Items[e.Index], this.DisplayMember).ToString(), font,
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
