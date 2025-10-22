using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace UtilETWeb
{
    public partial class ResizeImage : Form
    {

        Stream myStream;
        OpenFileDialog ofdPicture = new OpenFileDialog();
        SaveFileDialog sfdPicture = new SaveFileDialog();


        public ResizeImage()
        {
            InitializeComponent();
            this.Load += new EventHandler(ImageToIcon_Load);
        }

        void ImageToIcon_Load(object sender, EventArgs e)
        {
            this.ofdPicture.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";

            this.sfdPicture.DefaultExt = "png";
            this.sfdPicture.FileName = "test";
            this.sfdPicture.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
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
                Bitmap bitmap = ApplyResizeImage(new Bitmap(pbImage.Image), 25, 25);
                bitmap.Save(fileName);
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

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ApplyResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

    }
}
