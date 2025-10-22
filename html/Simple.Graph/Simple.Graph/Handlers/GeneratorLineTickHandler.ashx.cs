using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Drawing;
using System.IO;

namespace Simple.Graph.Handlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GeneratorLineTickHandler : IHttpHandler
    {

        int _Width = 0;
        int _Height = 0;
        float _Porcent = 0;
        string _HmltColor = "#000000";
        int _Ticks = 10;
        int _LineSize = 1;
        string _TickHmltColor = "#000000";

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

        public string TickHmltColor
        {
            get
            {
                if (HttpContext.Current.Request.Params["tickColor"] != null)
                {
                    try
                    {
                        _TickHmltColor = string.Format("#{0}", HttpContext.Current.Request.Params["tickColor"]);
                    }
                    catch
                    {
                        _TickHmltColor = "#000000";
                    }
                }
                return _TickHmltColor;
            }
        }



        public int Ticks
        {
            get
            {
                if (HttpContext.Current.Request.Params["ticks"] != null)
                {
                    try
                    {
                        _Ticks = int.Parse(HttpContext.Current.Request.Params["ticks"]);
                    }
                    catch
                    {
                        _Ticks = 10;
                    }
                }
                return _Ticks;
            }
        }

        public int LineSize
        {
            get
            {
                if (HttpContext.Current.Request.Params["lineSize"] != null)
                {
                    try
                    {
                        _LineSize = int.Parse(HttpContext.Current.Request.Params["lineSize"]);
                    }
                    catch
                    {
                        _LineSize = 1;
                    }
                }
                return _LineSize;
            }
        }



        public void ProcessRequest(HttpContext context)
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
            Pen pTick = new Pen(System.Drawing.ColorTranslator.FromHtml(this.TickHmltColor), LineSize);



            float rect_width = ((float)Width / (float)Ticks);

            float rect_height = Height;

            for (int i = 0; i < Ticks; i++)
            {
                if (i == 0)
                {
                    g.FillRectangle(Brushes.Green, i * rect_width, 0, rect_width, rect_height);
                    g.DrawRectangle(Pens.Green, i * rect_width, 0, rect_width, rect_height);
                }
                else
                {
                    g.FillRectangle(Brushes.WhiteSmoke, i * rect_width, 0, rect_width, rect_height);
                    g.DrawRectangle(Pens.LightGray, i * rect_width, 0, rect_width, rect_height);
                }
            }


            using (Bitmap image = bmp)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.WriteTo(context.Response.OutputStream);
                }
            }
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

        public class PolarToCartesian
        {
            public PointF Point { get; set; }

            public PolarToCartesian(float Radius, float AngleDegree)
                : this(0, 0, Radius, AngleDegree)
            {

            }

            public PolarToCartesian(float rootx, float rooty, float Radius, float AngleDegree)
            {
                float AngleRadian = (float)(AngleDegree * 2 * Math.PI / 360);
                this.Point = new PointF()
                {
                    X = rootx + (float)(Radius * Math.Cos(AngleRadian)),
                    Y = rooty + (float)(Radius * Math.Sin(AngleRadian))
                };
            }
        }

        public class CartesianToPolar
        {
            public float Radius { get; set; }
            float AngleRadian { get; set; }

            public float AngleDegree
            {
                get
                {
                    return (float)(AngleRadian * 360 / (2 * Math.PI));
                }
            }

            public CartesianToPolar(float Xo, float Yo, float X, float Y)
                : this(X - Xo, Y - Yo)
            {

            }

            public CartesianToPolar(float X, float Y)
            {
                Radius = (float)(Math.Pow((Math.Pow(X, 2) + Math.Pow(Y, 2)), 0.5));
                AngleRadian = (float)(Math.Atan2((double)Y, (double)X));
            }
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
