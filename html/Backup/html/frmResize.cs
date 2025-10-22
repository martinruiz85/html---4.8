using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace UtilETWeb
{
    public partial class frmResize : Form
    {
        public frmResize()
        {
            InitializeComponent();
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtPath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        public float imageHeight
        {
            get
            {
                float value;
                if (float.TryParse(this.txtHeight.Text, out value))
                    return value;
                else
                    return 500f;
            }
        }

        public float imageWidth
        {
            get
            {
                float value;
                if (float.TryParse(this.txtHeight.Text, out value))
                    return value;
                else
                    return 600f;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string folder = Path.Combine(Environment.CurrentDirectory, "Images");
            string[] images = Directory.GetFiles(this.txtPath.Text, "*.jpg");
            foreach (var item in images)
            {
                Bitmap bmp = new Bitmap(item, false);
                float newHeightPorcent = imageHeight / bmp.Height;
                float newWidthPorcent = imageWidth / bmp.Width;
                int marginPorcent = 40;


                int newheight = (int)(newWidthPorcent * bmp.Height);
                int newwidth = (int)(newWidthPorcent * bmp.Width);

                //newheight = newheight + marginPorcent;
                //newwidth = newwidth + marginPorcent;

                Bitmap bmpResult = new Bitmap((int)(newwidth * 1.1f), (int)(newheight * 1.1f));
                Graphics g = Graphics.FromImage(bmpResult);
                //escale
                g.DrawImage(bmp, new Rectangle(0, 0, (int)(newwidth * 1.1f), (int)(newheight * 1.1f)));
                g.Save();

                string name = Path.GetFileNameWithoutExtension(item);
                string path = Path.Combine(Environment.CurrentDirectory, string.Format(@"Images\{0}_change.jpg", name));
                string newpath = Path.Combine(Environment.CurrentDirectory, string.Format(@"Images\{0}_newchange.jpg", name));

                bmpResult.Save(path);

                //secund escale
                Bitmap newbmp = new Bitmap(path, false);
                Bitmap newbmpResult = new Bitmap((int)imageWidth, (int)imageHeight);
                Graphics newg = Graphics.FromImage(newbmpResult);

                newg.DrawImage(newbmp, (imageWidth - newbmp.Width) / 2, (imageHeight - newbmp.Height) / 2);
                /*
                newg.DrawImage(newbmp,
                new Rectangle(
                        0,
                        0,
                    newwidth,
                    newheight),
                new Rectangle(
                        (newwidth - newbmpResult.Width) / 2,
                        0,
                        newbmpResult.Width,
                        newbmpResult.Height),
                GraphicsUnit.Pixel);
                 */

                newbmpResult.Save(newpath);

            }

            Process.Start(folder);
        }
    }
}
