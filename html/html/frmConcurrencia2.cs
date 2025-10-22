using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using UtilETWeb.Effects;
using System.Runtime.InteropServices;
using System.Collections;

namespace UtilETWeb
{
    public partial class frmConcurrencia2 : Form
    {

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        private Player p1;

        public static ArrayList DrawObjects { get; set; }

        public frmConcurrencia2()
        {


            InitializeComponent();
            this.DoubleBuffered = true;
            this.Load += new EventHandler(frmConcurrencia_Load);
            this.Paint += new PaintEventHandler(frmConcurrencia_Paint);
            this.KeyDown += new KeyEventHandler(frmConcurrencia_KeyDown);
            this.KeyUp += new KeyEventHandler(frmConcurrencia_KeyUp);

        }



        void frmConcurrencia_KeyUp(object sender, KeyEventArgs e)
        {

            switch (e.KeyData)
            {
                case Keys.Up:
                    p1.UpPressed = false;
                    break;
                case Keys.Down:
                    p1.DownPressed = false;
                    break;
                case Keys.Left:
                    p1.LeftPressed = false;
                    break;
                case Keys.Right:
                    p1.RightPressed = false;
                    break;
                case Keys.A:
                    break;
            }

        }

        void frmConcurrencia_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.KeyData);

            if (e.KeyData == Keys.Up)
            {
                p1.Mov = STATE.UP;
                p1.UpPressed = true;
                p1.MOVEDTO(STATE.UP);
            }
            if (e.KeyData == Keys.Down)
            {
                p1.Mov = STATE.DOWN;
                p1.DownPressed = true;
                p1.MOVEDTO(STATE.DOWN);
            }
            if (e.KeyData == Keys.Left)
            {
                p1.Mov = STATE.LEFT;
                p1.LeftPressed = true;
                p1.MOVEDTO(STATE.LEFT);
            }
            if (e.KeyData == Keys.Right)
            {
                p1.Mov = STATE.RIGHT;
                p1.RightPressed = true;
                p1.MOVEDTO(STATE.RIGHT);
            }
            if (e.KeyData == Keys.A)
            {
                SHOOTING();
            }
        }


        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    if (e.Shift)
                    {

                    }
                    else
                    {
                    }
                    break;
            }
        }





        void frmConcurrencia_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //p1.Draw(g);

            //Map
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    //g.DrawRectangle(Pens.Black, i * 20, j * 36, 20, 36);
                    g.FillRectangle(Brushes.White, i * 20, j * 36, 20, 36);
                }
            }


            //Balls Safe
            //https://stackoverflow.com/questions/2024179/collection-was-modified-enumeration-operation-may-not-execute-in-arraylist
            for (int i = DrawObjects.Count - 1; i >= 0; i--)
            {
                ((IDraw)DrawObjects[i]).Draw(g);
                //Balls.RemoveAt(i);
            }

            //foreach (Ball item in Balls)
            //{
            //    item.Draw(g);
            //}



        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        void frmConcurrencia_Load(object sender, EventArgs e)
        {

            ArrayList myAL = new ArrayList();
            DrawObjects = ArrayList.Synchronized(myAL);


            p1 = new Player(0, 0, 20, 36, new Map(0,0,0,0) );

            DrawObjects.Add(p1);

            timer.Interval = 10;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

        }



        private void SHOOTING()
        {
            Ball b = new Ball(p1.Point.X, p1.Point.Y, 10, 10, p1.Mov);
            DrawObjects.Add(b);
            b.SHOOTING();
        }
    }


    


   


}
