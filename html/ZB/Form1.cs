using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZB
{
    public partial class Form1 : Form
    {
        public class Game 
        {
            public static int stage1 = 1;
            public List<Stage> stages { get; set; }

            public void paint() 
            {
                stages[stage1].paint();
            }
        }

        public class Stage 
        {
            public int map { get; set; }
            public int player { get; set; }
            public int enemy { get; set; }

            public void paint()
            {
                
            }
        }

        Timer t = new Timer();

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Load += new EventHandler(Form1_Load);
            this.Paint += new PaintEventHandler(Form1_Paint);
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0, 0, 10, 10));
        }

        void Form1_Load(object sender, EventArgs e)
        {
            t.Tick += new EventHandler(t_Tick);
            t.Start();
        }

        void t_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
