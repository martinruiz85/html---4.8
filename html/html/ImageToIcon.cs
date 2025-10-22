using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

//http://convertico.com/
namespace UtilETWeb
{
    public partial class ImageToIcon : Form
    {

        Stream myStream;
        OpenFileDialog ofdPicture = new OpenFileDialog();
        SaveFileDialog sfdPicture = new SaveFileDialog();


        public ImageToIcon()
        {
            InitializeComponent();
            this.Load += new EventHandler(ImageToIcon_Load);
        }

        void ImageToIcon_Load(object sender, EventArgs e)
        {
            this.ofdPicture.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";

            this.sfdPicture.DefaultExt = "ico";
            this.sfdPicture.FileName = "Icon";
            this.sfdPicture.Filter = "Icon File (*.ico)|*.ico";
        }


        private void btnOpenImage_Click(object sender, EventArgs e)
        {

            if (ofdPicture.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = ofdPicture.OpenFile()) != null)
                {
                    Image image = Image.FromFile(ofdPicture.FileName);
                    Image newImage = image.GetThumbnailImage(32, 32, null, new IntPtr());
                    pbImage.Image = newImage;
                }
            }
        }

        private void btnSaveAsIcon_Click(object sender, EventArgs e)
        {
            if (sfdPicture.ShowDialog() == DialogResult.OK)
            {
                String fileName = sfdPicture.FileName;
                Stream IconStream = System.IO.File.OpenWrite(fileName);

                Bitmap bitmap = new Bitmap(pbImage.Image);
                bitmap.SetResolution(72, 72);
                Icon icon = System.Drawing.Icon.FromHandle(bitmap.GetHicon());
                this.Icon = icon;
                icon.Save(IconStream);
                this.Refresh();
            }
        }

        private void pbImage_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

            Pen p = new Pen(Color.Black, 1);
            p.DashPattern = new float[] { 1, 2, 3 };

            if (pbImage.Image == null)
            {
                g.FillRectangle(Brushes.White, 0, 0, pbImage.Width - 1, pbImage.Height - 1);

                // Create a StringFormat object with the each line of text, and the block
                // of text centered on the page.
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                g.DrawString("No Image", this.Font, Brushes.Black, new RectangleF(0, 0, pbImage.Width - 1, pbImage.Height - 1), stringFormat);
            }
           
            g.DrawRectangle(p, 0, 0, pbImage.Width - 1, pbImage.Height - 1);
        }
    }
}
