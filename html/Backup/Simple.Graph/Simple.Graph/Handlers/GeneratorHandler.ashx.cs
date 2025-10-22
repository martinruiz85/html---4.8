using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Simple.Graph.Handlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GeneratorHandler : IHttpHandler
    {

        int _Width = 0;
        int _Height = 0;
        float _Porcent = 0;
        string _HmltColor = "#000000";

        public int Width
        {
            get
            {
                if (HttpContext.Current.Request.Params["width"] != null)
                {
                    try
                    {
                        _Width = int.Parse(HttpContext.Current.Request.Params["width"]);
                    }
                    catch
                    {
                        _Width = 0;
                    }
                }

                return _Width;
            }
        }
        public int Height
        {
            get
            {
                if (HttpContext.Current.Request.Params["height"] != null)
                {
                    try
                    {
                        _Height = int.Parse(HttpContext.Current.Request.Params["height"]);
                    }
                    catch
                    {
                        _Height = 0;
                    }
                }

                return _Height;
            }
        }
        public float Porcent
        {
            get
            {
                if (HttpContext.Current.Request.Params["porcent"] != null)
                {
                    try
                    {
                        _Porcent = float.Parse(HttpContext.Current.Request.Params["porcent"]);
                    }
                    catch
                    {
                        _Porcent = 0f;
                    }
                }
                return _Porcent;
            }
        }
        public string HmltColor
        {
            get
            {
                if (HttpContext.Current.Request.Params["color"] != null)
                {
                    try
                    {
                        _HmltColor = string.Format("#{0}", HttpContext.Current.Request.Params["color"]);
                    }
                    catch
                    {
                        _HmltColor = "#000000";
                    }
                }
                return _HmltColor;
            }
        }

        public void ProcessRequest(System.Web.HttpContext context)
        {

            context.Response.ContentType = "image/png";


            Bitmap bmp = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            Brush br = Brushes.Orange;

            RectangleF rect = CenterRectangle(Width / 2, Height / 2, Height / 3);

            Pen pGray = new Pen(Brushes.LightGray, 5);
            Pen pBlack = new Pen(System.Drawing.ColorTranslator.FromHtml(this.HmltColor), 5);

            //g.FillEllipse(Brushes.WhiteSmoke, rect);
            g.DrawEllipse(pGray, rect);
            g.DrawArc(pBlack, Rectangle.Round(rect), 0F, this.Porcent * 360);


            Font f = new Font(FontFamily.GenericSansSerif.Name, Height / 6, FontStyle.Regular, GraphicsUnit.Point);

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            RectangleF recttext = CenterRectangle((bmp.Width + 1F) / 2F, (bmp.Height + 1F) / 2F, Height / 3);
            g.DrawString(string.Format("{0:0%}", this.Porcent), f, Brushes.Black, recttext, sf);



            using (Bitmap image = bmp)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.WriteTo(context.Response.OutputStream);
                }
            }



            //if (context.Request.Params["width"] != null)
            //{
            //    try
            //    {
            //        Width = int.Parse(context.Request.Params["width"]);
            //    }
            //    catch
            //    {
            //        Width = 0;
            //    }
            //}
            //if (Width <= 0 && Height <= 0)
            //{
            //    context.Response.Clear();
            //    context.Response.ContentType = getContentType(context.Request.PhysicalPath);
            //    context.Response.WriteFile(context.Request.PhysicalPath);
            //    context.Response.End();
            //}
            //else
            //{
            //    context.Response.Clear();
            //    context.Response.ContentType = getContentType(context.Request.PhysicalPath);
            //    byte[] buffer = getResizedImage(context.Request.PhysicalPath, Width, Height);
            //    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            //    context.Response.End();
            //}
        }

        byte[] getResizedImage(String path, int width, int height)
        {
            Bitmap imgIn = new Bitmap(path);
            double y = imgIn.Height;
            double x = imgIn.Width;
            double factor = 1;
            if (width > 0)
            {
                factor = width / x;
            }
            else if (height > 0)
            {
                factor = height / y;
            }
            System.IO.MemoryStream outStream = new System.IO.MemoryStream();
            Bitmap imgOut = new Bitmap((int)(x * factor), (int)(y * factor));
            Graphics g = Graphics.FromImage(imgOut);
            g.Clear(Color.White);
            g.DrawImage(imgIn, new Rectangle(0, 0, (int)(factor * x), (int)(factor * y)), new Rectangle(0, 0, (int)x, (int)y), GraphicsUnit.Pixel);
            imgOut.Save(outStream, getImageFormat(path));
            return outStream.ToArray();
        }

        string getContentType(String path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp": return "Image/bmp";
                case ".gif": return "Image/gif";
                case ".jpg": return "Image/jpeg";
                case ".png": return "Image/png";
                default: break;
            }
            return "";
        }

        ImageFormat getImageFormat(String path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp": return ImageFormat.Bmp;
                case ".gif": return ImageFormat.Gif;
                case ".jpg": return ImageFormat.Jpeg;
                case ".png": return ImageFormat.Png;
                default: break;
            }
            return ImageFormat.Jpeg;
        }

        public RectangleF CenterRectangle(float xCenter, float yCenter, float radius)
        {
            return new RectangleF()
            {
                X = xCenter - radius,
                Y = yCenter - radius,
                Width = radius * 2F,
                Height = radius * 2F
            };
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
