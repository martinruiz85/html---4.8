using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(Form3_Paint);
        }

        void Form3_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //int days = 52;
            //float rect_width = 75 / days;

            int days = 15;
            float rect_width = 75 / days;

            float rect_height = 20;

            for (int i = 0; i < days; i++)
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


        }
    }
}
