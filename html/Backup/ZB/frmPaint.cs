using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ZB
{
    public partial class frmPaint : Form
    {
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();

        List<IDraw> Characters = new List<IDraw>();

        Map map = new Map();

        Player p1 = new Player();


        public frmPaint()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
            this.Load += new EventHandler(frmPaint_Load);
            this.Paint += new PaintEventHandler(frmPaint_Paint);
        }

        void frmPaint_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            foreach (IDraw item in this.Characters)
            {
                item.Draw(g);
            }
        }

        Thread rightThread, leftThread, downThread, upThread, shootingThread;


        void frmPaint_Load(object sender, EventArgs e)
        {
            Characters = new List<IDraw>();
            Characters.Add(map);
            Characters.Add(p1);

            rightThread = new Thread(new ThreadStart(Right));
            rightThread.IsBackground = true;
            leftThread = new Thread(new ThreadStart(Left));
            leftThread.IsBackground = true;
            downThread = new Thread(new ThreadStart(Down));
            downThread.IsBackground = true;
            upThread = new Thread(new ThreadStart(Up));
            upThread.IsBackground = true;

            shootingThread = new Thread(new ThreadStart(SootingAnimate));
            shootingThread.IsBackground = true;

            t.Interval = 10;
            t.Tick += new EventHandler(t_Tick);
            t.Start();
        }

        void t_Tick(object sender, EventArgs e)
        {
            this.Update();
            this.Invalidate();
            //this.Refresh();
        }

        private void Right()
        {
            p1.ClearSleep();
            for (int i = 0; i < 6; i++)
            {
                p1.X += 1;
                p1.state = i;
                System.Threading.Thread.Sleep(25);
            }
            p1.Sleep();
        }

        private void Left()
        {
            p1.ClearSleep();
            for (int i = 0; i < 6; i++)
            {
                p1.X -= 1;
                p1.state = i;
                System.Threading.Thread.Sleep(25);
            }
            p1.Sleep();
        }

        private void Up()
        {
            p1.ClearSleep();
            for (int i = 0; i < 6; i++)
            {
                p1.Y -= 1;
                p1.state = i;
                System.Threading.Thread.Sleep(25);
            }
            p1.Sleep();
        }

        private void Down()
        {
            p1.ClearSleep();
            for (int i = 0; i < 6; i++)
            {
                p1.Y += 1;
                p1.state = i;
                System.Threading.Thread.Sleep(25);
            }
            p1.Sleep();
        }

        private void SootingAnimate()
        {
            p1.ClearSleep();
            p1.color = Color.Red;
            for (int i = 0; i < 6; i++)
            {
                p1.state = i;

                System.Threading.Thread.Sleep(25);
            }
            p1.color = Color.Empty;
            p1.Sleep();
        }

        private void frmPaint_KeyDown(object sender, KeyEventArgs e)
        {
            //p1.sleepThread.Abort();

            switch (e.KeyCode)
            {
                case Keys.Left:
                    //p1.X -= 10;
                    //for (int i = 0; i < 10; i++)
                    //{
                    //    p1.X -= 1;
                    //    System.Threading.Thread.Sleep(500);
                    //}
                    if (leftThread.ThreadState == ThreadState.Stopped)
                    {
                        leftThread = new Thread(new ThreadStart(Left));
                    }
                    if (!leftThread.IsAlive)
                    {
                        leftThread.Start();
                    }
                    break;
                case Keys.Down:
                    //p1.Y += 10;
                    //for (int i = 0; i < 10; i++)
                    //{
                    //    p1.Y += 1;
                    //    System.Threading.Thread.Sleep(500);
                    //}
                    if (downThread.ThreadState == ThreadState.Stopped)
                    {
                        downThread = new Thread(new ThreadStart(Down));
                    }
                    if (!downThread.IsAlive)
                    {
                        downThread.Start();
                    }
                    break;
                case Keys.Right:
                    //p1.X += 10;
                    //for (int i = 0; i < 10; i++)
                    //{
                    //    p1.X += 1;
                    //    System.Threading.Thread.Sleep(500);
                    //}
                    if (rightThread.ThreadState == ThreadState.Stopped)
                    {
                        rightThread = new Thread(new ThreadStart(Right));
                    }
                    if (!rightThread.IsAlive)
                    {
                        rightThread.Start();
                    }
                    break;
                case Keys.Up:
                    //p1.Y -= 10;
                    //for (int i = 0; i < 10; i++)
                    //{
                    //    p1.Y -= 1;
                    //    System.Threading.Thread.Sleep(500);
                    //}
                    if (upThread.ThreadState == ThreadState.Stopped)
                    {
                        upThread = new Thread(new ThreadStart(Up));
                    }
                    if (!upThread.IsAlive)
                    {
                        upThread.Start();
                    }
                    break;
                case Keys.A:

                    p1.AddShooting();


                    if (shootingThread.ThreadState == ThreadState.Stopped)
                    {
                        shootingThread = new Thread(new ThreadStart(SootingAnimate));
                    }
                    if (!shootingThread.IsAlive)
                    {
                        shootingThread.Start();
                    }


                    break;
                default:
                    break;
            }


        }
    }

    public interface IDraw
    {
        float X { get; set; }
        float Y { get; set; }
        float Width { get; set; }
        float Height { get; set; }
        void Draw(Graphics g);
    }

    public class Map : IDraw
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public void Draw(Graphics g)
        {
            int width = (int)(g.ClipBounds.Width/10);
            int height = (int)(g.ClipBounds.Height/10);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {

                    g.DrawRectangle(Pens.Black, new Rectangle(i * width, j * height, width, height));
                }
            }
        }
    }

    public class Ball : IDraw
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        Thread rightThread;

        public static List<Ball> balls = new List<Ball>();

        public Ball(float x, float y)
        {
            this.X = x;
            this.Y = y;
            //this.Width = 4;
            //this.Height = 4;

            this.Width = 24;
            this.Height = 20;

            rightThread = new Thread(new ThreadStart(ShootingAnimate));
            rightThread.IsBackground = true;



        }

        private void ShootingAnimate()
        {
            for (int i = 0; i < 500; i++)
            {
                this.X += 1;
                System.Threading.Thread.Sleep(10);
            }
            //Ball.balls.Remove(this);            
        }

        public void Shooting()
        {
            if (rightThread.ThreadState == ThreadState.Stopped)
            {
                rightThread = new Thread(new ThreadStart(ShootingAnimate));
            }
            if (!rightThread.IsAlive)
            {
                rightThread.Start();
            }
        }


        public void Draw(Graphics g)
        {
            RectangleF rect = new RectangleF(this.X, this.Y, this.Width, this.Height);
            //g.FillEllipse(Brushes.Red, rect);

            TextureBrush tbrush = new TextureBrush(Properties.Resources.beams, new RectangleF(237, 5, this.Width, this.Height));
            tbrush.WrapMode = System.Drawing.Drawing2D.WrapMode.Clamp;
            tbrush.TranslateTransform(this.X, this.Y);
            g.FillRectangle(tbrush, rect);
            tbrush.Dispose();
        }

    }

    public class Player : IDraw
    {
        List<Bitmap> Sprite = new List<Bitmap>();

        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public int state { get; set; }
        public int stateSleep { get; set; }
        public Color color { get; set; }


        public Thread sleepThread;

        public Player()
        {
            this.X = 0;
            this.Y = 0;
            this.Width = 18;
            this.Height = 21;
            this.color = Color.Empty;

            Sprite.Add(Properties.Resources.original_1_);
            Sprite.Add(Properties.Resources.original__2_);
            Sprite.Add(Properties.Resources.original__3_);
            Sprite.Add(Properties.Resources.original__4_);
            Sprite.Add(Properties.Resources.original__4_);
            Sprite.Add(Properties.Resources.original__6_);

            sleepThread = new Thread(new ThreadStart(sleepAnimate));
            sleepThread.IsBackground = true;

            this.Sleep();

        }

        private void sleepAnimate()
        {
            for (int i = 0; i < 400; i++)
            {
                this.state = i % 6;
                this.stateSleep = i;
                System.Threading.Thread.Sleep(100);
            }
        }

        public void ClearSleep()
        {
            if (sleepThread != null)
            {
                sleepThread.Abort();
            }
        }

        public void Sleep()
        {
            if (sleepThread != null && !sleepThread.IsAlive)
            {
                sleepThread.Abort();
                sleepThread = null;
                sleepThread = new Thread(new ThreadStart(sleepAnimate));
                sleepThread.Start();
            }
        }



        public void AddShooting()
        {
            Ball ball = new Ball(this.X + 10, this.Y + 4);
            Ball.balls.Add(ball);
            ball.Shooting();
        }


        public void Draw(Graphics g)
        {
            RectangleF rect = new RectangleF(this.X, this.Y, this.Width, this.Height);

            //g.FillRectangle(Brushes.Black, rect);

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            //Font f = new Font(FontFamily.GenericSerif, this.Height);
            Font f = new Font(FontFamily.GenericSerif, 10);
            //g.DrawString(string.Format("{0}", state), f, Brushes.White, rect, sf);


            Bitmap bmp = new Bitmap(Sprite[state]);
            if (Color.Empty != this.color)
            {
                Color color = Color.Red; //Your desired colour

                byte r = color.R; //For Red colour


                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        Color gotColor = bmp.GetPixel(x, y);
                        gotColor = Color.FromArgb(r, gotColor.G, gotColor.B);
                        bmp.SetPixel(x, y, gotColor);
                    }
                }
            }


            TextureBrush tbrush = new TextureBrush(bmp, new RectangleF(0, 0, this.Width, this.Height));
            tbrush.WrapMode = System.Drawing.Drawing2D.WrapMode.Clamp;
            tbrush.TranslateTransform(this.X, this.Y);
            g.FillRectangle(tbrush, rect);

            foreach (Ball item in Ball.balls)
            {
                item.Draw(g);
            }


            //g.DrawString(string.Format("{0}", stateSleep), f, Brushes.Red, rect, sf);


        }
    }
}
