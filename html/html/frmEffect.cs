using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilETWeb.Effects;

namespace UtilETWeb
{
    public partial class frmEffect : Form
    {
        private Timer _tmr = new Timer();

        private List<IDraw> Draws = new List<IDraw>();

        public interface IDraw
        {
            void draw(Graphics g);
        }

        public abstract class Shape : IDraw
        {
            public int x { get; set; }
            public int y { get; set; }
            public int w { get; set; }
            public int h { get; set; }

            protected Animate a = new Animate();

            public int alpha { get; set; }

            private Color _color;
            public Color color
            {
                get
                {
                    if (_color != null)
                        return _color;
                    else
                        return Color.Black;
                }
                set
                {
                    _color = value;
                }
            }

            public Shape()
            {
                a.Millisecunds = 500;
                a.CustomProgressChanged += new Animate.CustomEventHandlerProgressChanged(a_CustomProgressChanged);
                a.RunWorkerCompleted += new EventHandler(a_RunWorkerCompleted);
            }

            protected virtual void a_RunWorkerCompleted(object sender, EventArgs e)
            {
            }

            public abstract void a_CustomProgressChanged(object sender, ProgressChangedEventArgs e);

            public Rectangle get()
            {
                return new Rectangle(x, y, w, h);
            }

            public abstract void draw(Graphics g);
        }

        public class Square : Shape
        {
            protected AnimateMoved b = new AnimateMoved();
            public AnimateMovedState MovedState { get; set; }

            public Square()
                : base()
            {
                this.alpha = 255;
                b.Millisecunds = 200;
                b.CustomProgressChanged += new AnimateMoved.CustomEventHandlerProgressChanged(b_CustomProgressChanged);
            }



            void b_CustomProgressChanged(object sender, ProgressChangedEventArgs e, Random Rand)
            {
                AnimateMovedState state = e.UserState as AnimateMovedState;
                switch (state.direction)
                {
                    case MovesDirection.top:
                        this.y = state.y - (int)((1D - state.porcent) * 10D);
                        break;
                    case MovesDirection.left:
                        this.x = state.x - (int)((1D - state.porcent) * 10D);
                        break;
                    case MovesDirection.right:
                        this.x = state.x + (int)((1D - state.porcent) * 10D);
                        break;
                    case MovesDirection.bottom:
                        this.y = state.y + (int)((1D - state.porcent) * 10D);
                        break;
                    default:
                        break;
                }
            }


            public override void a_CustomProgressChanged(object sender, ProgressChangedEventArgs e)
            {
                x = (int)((1D - (double)e.UserState) * 100D);
                y = (int)((1D - (double)e.UserState) * 100D);
                alpha = (int)((1D - (double)e.UserState) * 255D);
            }

            public override void draw(Graphics g)
            {
                SolidBrush sb = new SolidBrush(Color.FromArgb(alpha, this.color));
                g.FillRectangle(sb, this.get());
            }

            public void Mov(MovesDirection mov)
            {
                this.MovedState = new AnimateMovedState(this.get(), mov);
                b.Star(this.MovedState);
            }

            public void shoot()
            {
                a.Star("derecha");
            }
        }

        public class Cicule : Shape
        {
            protected AnimateMoved b = new AnimateMoved();

            public Cicule()
            {
                this.alpha = 255;
                this.color = Color.Red;
                b.Millisecunds = 5000;
                b.CustomProgressChanged += new AnimateMoved.CustomEventHandlerProgressChanged(b_CustomProgressChanged);
            }

            void b_CustomProgressChanged(object sender, ProgressChangedEventArgs e, Random Rand)
            {
                AnimateMovedState state = e.UserState as AnimateMovedState;
                switch (state.direction)
                {
                    case MovesDirection.top:
                        this.y = state.y - (int)((1D - state.porcent) * 3000);
                        break;
                    case MovesDirection.left:
                        this.x = state.x - (int)((1D - state.porcent) * 3000);
                        break;
                    case MovesDirection.right:
                        this.x = state.x + (int)((1D - state.porcent) * 3000);
                        break;
                    case MovesDirection.bottom:
                        this.y = state.y + (int)((1D - state.porcent) * 3000);
                        break;
                    default:
                        break;
                }
            }

            public override void a_CustomProgressChanged(object sender, ProgressChangedEventArgs e)
            {
                x = (int)((1D - (double)e.UserState) * 100D);
                //y = (int)((1D - (double)e.UserState) * 100D);
                alpha = (int)((1D - (double)e.UserState) * 255D);
            }

            public override void draw(Graphics g)
            {
                SolidBrush sb = new SolidBrush(Color.FromArgb(alpha, this.color));
                g.FillEllipse(sb, this.get());
            }

            public void translate(MovesDirection mov)
            {
                b.Star(new AnimateMovedState(this.get(), mov));
            }
        }

        public frmEffect()
        {
            InitializeComponent();

            // quality grapichs
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;

            // event paint
            this.Paint += new PaintEventHandler(frmEffect_Paint);

            // init timer 
            _tmr.Interval = 10;
            _tmr.Tick += new EventHandler(_tmr_Tick);

            //this.KeyUp += new KeyEventHandler(frmEffect_KeyUp);
            this.KeyDown += new KeyEventHandler(frmEffect_KeyDown);
            //this.KeyPress += new KeyPressEventHandler(frmEffect_KeyPress);
        }

        void frmEffect_KeyPress(object sender, KeyPressEventArgs e)
        {
            
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
                case Keys.Right | Keys.A:
                case Keys.Left | Keys.A:
                case Keys.Down | Keys.A:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        void frmEffect_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {                
                case Keys.Up:
                    _square.Mov(MovesDirection.top);
                    break;
                case Keys.Right:
                    _square.Mov(MovesDirection.right);
                    break;
                case Keys.Left:
                    _square.Mov(MovesDirection.left);
                    break;
                case Keys.Down:
                    _square.Mov(MovesDirection.bottom);
                    break;
            }

            switch (e.KeyCode)
            {
                case Keys.A:
                    Cicule _circle = new Cicule();
                    _circle.x = _square.x;
                    _circle.y = _square.y;
                    _circle.w = _square.w;
                    _circle.h = _square.h;
                    Draws.Add(_circle);
                    if (_square.MovedState != null)
                        _circle.translate(_square.MovedState.direction);
                    else
                        _circle.translate(MovesDirection.bottom);
                    break;               
            }
        }
        
        /*
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {            
            //capture up arrow key
            if (keyData == Keys.Up)
            {
                
                //MessageBox.Show("You pressed Up arrow key");
                return true;
            }
            //capture down arrow key
            if (keyData == Keys.Down)
            {
                
                //MessageBox.Show("You pressed Down arrow key");
                return true;
            }
            //capture left arrow key
            if (keyData == Keys.Left)
            {
                _square.Mov(MovesDirection.left);
                //MessageBox.Show("You pressed Left arrow key");
                return true;
            }
            //capture right arrow key
            if (keyData == Keys.Right)
            {
                _square.Mov(MovesDirection.right);
                //MessageBox.Show("You pressed Right arrow key");
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        */

        void frmEffect_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    Cicule _circle = new Cicule();
                    _circle.x = _square.x;
                    _circle.y = _square.y;
                    _circle.w = _square.w;
                    _circle.h = _square.h;
                    Draws.Add(_circle);
                    if (_square.MovedState != null)
                        _circle.translate(_square.MovedState.direction);
                    else
                        _circle.translate(MovesDirection.bottom);
                    break;
                case Keys.Up:
                    _square.Mov(MovesDirection.top);
                    break;
                case Keys.Right:
                    _square.Mov(MovesDirection.right);
                    break;
                case Keys.Left:
                    _square.Mov(MovesDirection.left);
                    break;
                case Keys.Down:
                    _square.Mov(MovesDirection.bottom);
                    break;
            }
        }



        void _tmr_Tick(object sender, EventArgs e)
        {
            // redraw form
            this.Invalidate();
            //this.Invalidate(new Rectangle(0, 0, _square.x + _square.w, _square.y + _square.h));
            //this.Invalidate(new Rectangle(_square.x, _square.y, _square.w, _square.h));
            this.Update();
        }

        void frmEffect_Paint(object sender, PaintEventArgs e)
        {
            // get graphics
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //g.Clear(this.BackColor);

            // draw objects
            foreach (IDraw item in this.Draws)
            {
                item.draw(g);
            }
        }

        private Square _square = new Square();

        private void frmEffect_Load(object sender, EventArgs e)
        {
            //// sample animate
            //Animate _a = new Animate();
            //_a.Millisecunds = 1000;
            ////a.ProgressChanged += new EventHandler(a_ProgressChanged);
            //_a.CustomProgressChanged += new Animate.CustomEventHandlerProgressChanged(a_CustomProgressChanged);
            //_a.Star();

            // init repaint cicle form
            _tmr.Start();

            // init square
            _square.x = 0;
            _square.x = 0;
            _square.w = 10;
            _square.h = 10;
            _square.color = Color.Salmon;

            Draws.Add(_square);

            Cicule _circle = new Cicule();
            _circle.x = 0;
            _circle.x = 0;
            _circle.w = 10;
            _circle.h = 10;

            //Draws.Add(_circle);

            // move square
            //_square.shoot();
            _circle.translate(MovesDirection.bottom);
        }

        void a_ProgressChanged(object sender, EventArgs e)
        {
            // simple EventArgs, force convert EventArgs -> to -> ProgressChangedEventArgs
            this.Text = string.Format("{0}%", ((ProgressChangedEventArgs)e).ProgressPercentage);
        }

        void a_CustomProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // custom ProgressChangedEventArgs
            this.Text = string.Format("{0}%", e.UserState);
        }

    }
}
