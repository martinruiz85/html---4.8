using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace UtilETWeb
{
    public class WheelOfColor
    {
        private List<Color> _PrimaryColors = new List<Color>();
        public List<Color> PrimaryColors
        {
            get
            {
                return _PrimaryColors;
            }
        }

        private static WheelOfColor instance;
        public static WheelOfColor Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WheelOfColor();
                }
                return instance;
            }
        }


        public WheelOfColor()
        {
            _PrimaryColors.Add(Color.FromArgb(255, 0, 0));
            _PrimaryColors.Add(Color.FromArgb(0, 255, 0));
            _PrimaryColors.Add(Color.FromArgb(0, 0, 255));
        }


        public List<Color> Generate(int length)
        {
            bool IsPrimaryColors = true;
            List<Color> colors = new List<Color>();
            while (colors.Count <= length)
            {
                if (colors.Count < _PrimaryColors.Count)
                {
                    colors.Add(_PrimaryColors[colors.Count]);
                }
                else
                {
                    Generate(length, ref colors, IsPrimaryColors);
                    IsPrimaryColors = false;
                }
            }
            return colors;
        }

        private static void Generate(int length, ref List<Color> l, bool IsPrimaryColors)
        {
            List<Color> newl = new List<Color>();
            int max = l.Count;
            //combina los colores para obtener la siguiente mezcla
            for (int i = 0; i < max; i++)
            {
                Color c = new Color();
                if (i != l.Count - 1)
                    c = colorMixer(l[i], l[i + 1], IsPrimaryColors);
                else
                    c = colorMixer(l[i], l[0], IsPrimaryColors);
                newl.Add(c);
            }

            //inserta los nuevos colores en en medio de los primeros, 
            //ejemplo: [1]*[2]*[3]*->[1][4][2][5][3][6]
            for (int i = 0; i < newl.Count && l.Count <= length; i++)
            {
                l.Insert((i) * 2 + 1, newl[i]);
            }
        }

        public static Color colorMixer(Color c1, Color c2, bool IsPrimaryColors)
        {
            return Color.FromArgb((c1.A + c2.A) / 2,
                (c1.R + c2.R) / (IsPrimaryColors ? 1 : 2),
                (c1.G + c2.G) / (IsPrimaryColors ? 1 : 2),
                (c1.B + c2.B) / (IsPrimaryColors ? 1 : 2));
        }
    }

}
