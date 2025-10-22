using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;

namespace UtilETWeb
{
    public partial class frmPaint : Form
    {
        public const int cols = 3;
        public const int rows = 3;

        System.Windows.Forms.Timer tmr = new System.Windows.Forms.Timer(); 
        BackgroundWorker bgw = new BackgroundWorker();
        public Player Player1;
        public Map Map1;

        public frmPaint()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(frmPaint_Paint);

            bgw.WorkerReportsProgress = true;
            bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            bgw.ProgressChanged += new ProgressChangedEventHandler(bgw_ProgressChanged);

            this.KeyDown += new KeyEventHandler(frmPaint_KeyDown);
            this.KeyPress += new KeyPressEventHandler(frmPaint_KeyPress);
            this.KeyUp += new KeyEventHandler(frmPaint_KeyUp);

            Map1 = new Map();
            Player1 = new Player();

            tmr.Interval = 25;
            tmr.Tick += new EventHandler(tmr_Tick);
        }

        void tmr_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
            this.Update();
        }

        void frmPaint_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                default:
                    break;
            }
        }

        void frmPaint_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.bgw.IsBusy != true)
            {
                this.bgw.RunWorkerAsync();
            }
            Player1.setLocation(e);
            //this.Invalidate();
            //this.Update();
        }

        void frmPaint_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    break;
                case Keys.Right:
                    break;
                case Keys.Down:
                    break;
                case Keys.Left:
                    break;
                default:
                    break;
            }
        }

        void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Text = string.Format("{0} Progress Percentage, UserState: {1}", e.ProgressPercentage, e.UserState);
            this.Player1.Location.Offset((int)((double)e.UserState * 10), 0);
        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            int milliseconds = 1000;
            DateTime oldDate = DateTime.Now;
            DateTime newDate = DateTime.Now.AddMilliseconds(milliseconds);
            TimeSpan totalts = newDate - oldDate;
            while (DateTime.Now <= newDate)
            {
                //darle el tiempo al control para dibujarse de nuevo
                Thread.Sleep(10);
                TimeSpan ts = newDate - DateTime.Now;
                //double porcentage = (ts.TotalMilliseconds / totalts.TotalMilliseconds) * 100;
                double porcentage = Math.Max(0.0, ts.TotalMilliseconds / milliseconds);
                bgw.ReportProgress((int)Math.Round((1.00 - porcentage) * 100), 1.00 - porcentage);
                //back.ReportProgress((int)Math.Round(porcentage), porcentage);

            }
        }

        void frmPaint_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int size = 20;
            Map1.Draw(g, size);
            Player1.Draw(g, size);
        }

        private void frmPaint_Load(object sender, EventArgs e)
        {
            tmr.Start();
        }
    }

    public class GPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public GPoint() : this(0, 0) { }
        public GPoint(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public void Offset(int X, int Y)
        {
            this.X += X;
            this.Y += Y;
        }

        public Point GetPoint()
        {
            return new Point(X, Y);
        }
    }

    public class Map
    {
        char[,] map = new char[10, 10]
        {
            {'#','#','#','#','#','#','#','#','#','#'},
            {'#','#','#','#','#','#','#','#','#','#'},
            {'#','#','#','#','#','#','#','#','#','#'},
            {'#','#','#','#','#','#','#','#','#','#'},
            {'#','#','#','#','#','#','#','#','#','#'},
            {'#','#','#','#','#','#','#','#','#','#'},
            {'#','#','#','#','#','#','#','#','#','#'},
            {'#','#','#','#','#','#','#','#','#','#'},
            {'#','#','#','#','#','#','#','#','#','#'},
            {'#','#','#','#','#','#','#','#','#','#'}
        };

        public void Draw(Graphics g, int size)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    Rectangle rect = new Rectangle(size * j, size * i, size, size);
                    g.DrawRectangle(Pens.LightGray, rect);
                    g.FillRectangle(Brushes.WhiteSmoke, rect);
                }
            }
        }
    }

    public class Player
    {
        GPoint location = new GPoint();

        public GPoint Location
        {
            get { return location; }
            set { location = value; }
        }

        public Player()
        {
        }

        public void Draw(Graphics g, int size)
        {
            g.FillRectangle(Brushes.Green, new Rectangle(this.Location.X * size, this.Location.Y * size, size, size));
        }

        public void setLocation(KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.Up:
                    Location.Offset(0, -1);
                    break;
                case Keys.Right:
                    Location.Offset(1, 0);
                    break;
                case Keys.Down:
                    Location.Offset(0, 1);
                    break;
                case Keys.Left:
                    Location.Offset(-1, 0);
                    break;
                default:
                    break;
            }
        }
    }
}
