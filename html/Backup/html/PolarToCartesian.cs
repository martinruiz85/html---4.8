using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace html
{
    public class PolarToCartesian
    {
        public PointF Point { get; set; }

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
        float Radius { get; set; }
        float AngleRadian { get; set; }

        public float AngleDegree
        {
            get
            {
                return (float)(AngleRadian * 360 / (2 * Math.PI));
            }
        }

        public CartesianToPolar(float X, float Y)
        {
            Radius = (float)(Math.Pow((Math.Pow(X, 2) + Math.Pow(Y, 2)), 0.5));
            AngleRadian = (float)(Math.Atan2((double)Y, (double)X));
        }
    }
}
