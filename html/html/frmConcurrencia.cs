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

//https://dammit.typepad.com/blog/2010/10/recent-hitbox-work.html
//https://developer.mozilla.org/es/docs/Games/Techniques/2D_collision_detection
//http://www.videogamesprites.net/Zelda1/Link/
//https://scottlilly.com/learn-c-by-building-a-simple-rpg-index/
//https://www.piskelapp.com/p/agxzfnBpc2tlbC1hcHByEwsSBlBpc2tlbBiAgKDu6t24Cgw/edit
//http://gaurav.munjal.us/Universal-LPC-Spritesheet-Character-Generator/#
//https://rvros.itch.io/animated-pixel-hero
//https://itch.io/game-assets/free/tag-2d
//https://www.skillshare.com/projects/Turn-Bob-n-Thumbs-Up/75904

namespace UtilETWeb
{
    public partial class frmConcurrencia : Form
    {

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();


        private Player _player;

        public static ArrayList DrawObjects { get; set; }

        public frmConcurrencia()
        {


            InitializeComponent();
            this.DoubleBuffered = true;
            this.Load += new EventHandler(frmConcurrencia_Load);
            this.Paint += new PaintEventHandler(frmConcurrencia_Paint);
            this.KeyDown += new KeyEventHandler(frmConcurrencia_KeyDown);
            this.KeyUp += new KeyEventHandler(frmConcurrencia_KeyUp);
            this.FormClosing += new FormClosingEventHandler(frmConcurrencia_FormClosing);

        }

        void frmConcurrencia_FormClosing(object sender, FormClosingEventArgs e)
        {
            Thread tremove = new Thread(new ThreadStart(Remove));
            tremove.Start();


        }

        void Remove()
        {
            //_player.Mov = MOVED.END;
            for (int i = DrawObjects.OfType<ICharacter>().Count() - 1; i >= 0; i--)
            {
                ((ICharacter)DrawObjects.OfType<ICharacter>().ToArray()[i]).End();
                //Balls.RemoveAt(i);
            }
        }



        void frmConcurrencia_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Up:
                    _player.UpPressed = false;
                    break;
                case Keys.Down:
                    _player.DownPressed = false;
                    break;
                case Keys.Left:
                    _player.LeftPressed = false;
                    break;
                case Keys.Right:
                    _player.RightPressed = false;
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
                _player.Mov = STATE.UP;
                _player.UpPressed = true;
                //p1.MOVEDTO(MOVED.UP);
            }
            if (e.KeyData == Keys.Down)
            {
                _player.Mov = STATE.DOWN;
                _player.DownPressed = true;
                //p1.MOVEDTO(MOVED.DOWN);
            }
            if (e.KeyData == Keys.Left)
            {
                _player.Mov = STATE.LEFT;
                _player.LeftPressed = true;
                //p1.MOVEDTO(MOVED.LEFT);
            }
            if (e.KeyData == Keys.Right)
            {
                _player.Mov = STATE.RIGHT;
                _player.RightPressed = true;
                //p1.MOVEDTO(MOVED.RIGHT);
            }
            if (e.KeyData == Keys.S)
            {
                _player.Mov = STATE.DIE;
            }
            if (e.KeyData == Keys.A)
            {
                _player.Mov = STATE.HIT;
                //SHOOTING();
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

            //Player
            //p1.Draw(g);

            //Map
            //int width = 50;
            //int height = 37;


            //for (int i = 0; i < 10; i++)
            //{
            //    for (int j = 0; j < 10; j++)
            //    {

            //        g.FillRectangle(Brushes.White, i * width, j * height, width, height);
            //        g.DrawRectangle(Pens.WhiteSmoke, i * width, j * height, width, height);
            //    }
            //}


            //Balls Safe
            //https://stackoverflow.com/questions/2024179/collection-was-modified-enumeration-operation-may-not-execute-in-arraylist
            for (int i = DrawObjects.Count - 1; i >= 0; i--)
            {
                if (i < DrawObjects.Count)
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

            Map _map = new Map(50, 37, 10, 10);

            //BloodEffect _blood = new BloodEffect(0, 0, 64, 64);
            //_blood.Effect();
            //DrawObjects.Add(_blood);

            //Effect _effect = new Effect(0, 0, 32, 32);
            //_effect.SHOOTING();
            //DrawObjects.Add(_effect);           

            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Enemy _enemy = new Enemy(50 * (i), 37 * (j), 50, 37, _map);
                    _enemy.MOVE();
                    DrawObjects.Insert(0, _enemy);
                    //DrawObjects.Add(_enemy);
                }
            }

            _player = new Player(50 * (3), 37 * (3), 50, 37, _map);
            _player.MOVEDTO(STATE.BEGIN);
            //DrawObjects.Add(_player);
            DrawObjects.Insert(0, _player);


            DrawObjects.Add(_map);


            //Enemy _enemy = new Enemy(50 * 2, 37 * 2, 50, 37, MOVED.BEGIN);
            //_enemy.MOVE();
            //DrawObjects.Add(_enemy);

            //Enemy _enemy1 = new Enemy(50 * 8, 37 * 2, 50, 37, MOVED.BEGIN);
            //_enemy1.MOVE();
            //DrawObjects.Add(_enemy1);

            //Enemy _enemy2 = new Enemy(50 * 2, 37 * 8, 50, 37, MOVED.BEGIN);
            //_enemy2.MOVE();
            //DrawObjects.Add(_enemy2);

            //Enemy _enemy3 = new Enemy(50 * 8, 37 * 8, 50, 37, MOVED.BEGIN);
            //_enemy3.MOVE();
            //DrawObjects.Add(_enemy3);

            timer.Interval = 10;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

        }



        private void SHOOTING()
        {
            Ball b = new Ball(_player.Point.X, _player.Point.Y, 10, 10, _player.Mov);
            DrawObjects.Add(b);
            b.SHOOTING();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _player.Suspend();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _player.Resume();
        }
    }

    public class xPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point Get()
        {
            return new Point(X, Y);
        }

        public double Distancia(xPoint P2)
        {
            return Math.Pow(Math.Pow((P2.X - this.X), 2.00) + Math.Pow((P2.Y - this.Y), 2.00), 0.5D);
        }

        public double Distancia(Point P2)
        {
            return Math.Pow(Math.Pow((P2.X - this.X), 2.00) + Math.Pow((P2.Y - this.Y), 2.00), 0.5D);
        }

        public double Distancia(int X2, int Y2)
        {
            return Math.Pow(Math.Pow((X2 - this.X), 2.00) + Math.Pow((Y2 - this.Y), 2.00), 0.5D);
        }

        public static double Distancia(xPoint P1, xPoint P2)
        {
            return Math.Pow(Math.Pow((P2.X - P1.X), 2.00) + Math.Pow((P2.Y - P1.Y), 2.00), 0.5D);
        }

        public static double Distancia(Point P1, Point P2)
        {
            return Math.Pow(Math.Pow((P2.X - P1.X), 2.00) + Math.Pow((P2.Y - P1.Y), 2.00), 0.5D);
        }

        public static double Distancia(int X1, int X2, int Y1, int Y2)
        {
            return Math.Pow(Math.Pow((X2 - X1), 2.00) + Math.Pow((Y2 - Y1), 2.00), 0.5D);
        }

        public xPoint(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }

    public class xSize
    {
        public int W { get; set; }
        public int H { get; set; }

        public xSize(int W, int H)
        {
            this.W = W;
            this.H = H;
        }
    }

    public interface IDraw
    {

        void Draw(Graphics g);
    }

    public interface IRectangle
    {
        Rectangle Get();
    }

    public interface ICharacter
    {
        Map Map { get; }
        int Life { get; set; }
        void End();
    }

    public class xRectangle : IDraw, IRectangle
    {
        public xPoint Point { get; set; }
        public xSize Size { get; set; }


        public xRectangle(int X, int Y, int W, int H)
        {
            this.Point = new xPoint(X, Y);
            this.Size = new xSize(W, H);
        }


        public virtual void Draw(Graphics g)
        {
            g.FillEllipse(Brushes.Orange, Get());
        }

        public Rectangle Get()
        {
            return new Rectangle(Point.X, Point.Y, Size.W, Size.H);
        }

    }

    public enum STATE
    {
        UP,
        RIGHT,
        DOWN,
        LEFT,
        SHOTING,
        HIT,
        DIE,
        END,
        BEGIN,
        HURT
    }

    public class Player : xRectangle, ICharacter, INotifyPropertyChanged
    {
        protected Map _map;

        public Map Map { get { return _map; } }

        Thread _t;

        private int _life;

        public int Life
        {
            get
            {
                return _life;
            }
            set
            {
                _life = value;

                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("Life");
            }
        }

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private BarLife _barLife;

        private STATE _state;
        private Color _color = Color.Black;

        public xPoint pImage { get; set; }

        public STATE Mov { get { return _state; } set { _state = value; } }
        public Color C { get { return _color; } set { _color = value; } }

        private bool m_bUpPressed = false;
        private bool m_bDownPressed = false;
        private bool m_bLeftPressed = false;
        private bool m_bRightPressed = false;

        public bool UpPressed { get { return m_bUpPressed; } set { m_bUpPressed = value; } }
        public bool DownPressed { get { return m_bDownPressed; } set { m_bDownPressed = value; } }
        public bool LeftPressed { get { return m_bLeftPressed; } set { m_bLeftPressed = value; } }
        public bool RightPressed { get { return m_bRightPressed; } set { m_bRightPressed = value; } }

        public Player(int X, int Y, int W, int H, Map map)
            : base(X, Y, W, H)
        {
            this.PropertyChanged += new PropertyChangedEventHandler(Player_PropertyChanged);

            _state = STATE.BEGIN;
            m_NewM = STATE.BEGIN;
            _color = Color.Black;
            this.pImage = new xPoint(0, 0);
            this._barLife = new BarLife(X, Y, W, H, this);
            this._barLife.Color = Brushes.Green;
            this.Life = 4;
            this._map = map;
        }

        void Player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _barLife.Porcent = (int)((this._life / 4f) * 100);
        }

        private readonly Object obj = new Object();
        private STATE m_NewM;


        public void Suspend()
        {
            _t.Suspend();
        }
        public void Resume()
        {
            _t.Resume();
        }

        private bool Collision(Enemy item)
        {
            int arriba = Math.Max(Point.Y, item.Point.Y);
            int abajo = Math.Min(Point.Y + Size.H, item.Point.Y + item.Size.H);
            int izquierda = Math.Max(Point.X, item.Point.X);
            int derecha = Math.Max(Point.X + Size.W, item.Point.X + item.Size.W);

            Bitmap _player_bmp = new Bitmap(Size.W, Size.H);
            using (Graphics g = Graphics.FromImage(_player_bmp))
            {
                g.DrawImage(Properties.Resources.adventurer_Sheet,
                    new Rectangle(0, 0, _player_bmp.Width, _player_bmp.Height),
                    new Rectangle(pImage.X, pImage.Y, Size.W, Size.H),
                    GraphicsUnit.Pixel);

            }

            Bitmap _enemy_bmp = new Bitmap(item.Size.W, item.Size.H);
            using (Graphics g = Graphics.FromImage(_enemy_bmp))
            {
                g.DrawImage(Properties.Resources.adventurer_Sheet,
                    new Rectangle(0, 0, _enemy_bmp.Width, _enemy_bmp.Height),
                    new Rectangle(item.pImage.X, item.pImage.Y, Size.W, Size.H),
                    GraphicsUnit.Pixel);

            }

            for (int y = arriba; y < abajo; y++)
            {
                for (int x = izquierda; x < derecha; x++)
                {
                    int _player_x = x - (Point.X + Size.W);
                    int _player_y = y - (Point.Y + Size.H);


                    int _enemy_x = x - (item.Point.X + item.Size.W);
                    int _enemy_y = y - (item.Point.Y + item.Size.H);


                    if (_player_bmp.GetPixel(
                        Math.Min(Math.Abs(Size.W + _player_x), Size.W - 1),
                        Math.Min(Math.Abs(Size.H + _player_y), Size.H - 1)).A > 0 &&
                        _enemy_bmp.GetPixel(
                        Math.Min(Math.Abs(item.Size.W + _enemy_x), item.Size.W - 1),
                        Math.Min(Math.Abs(item.Size.H + _enemy_y), item.Size.H - 1)).A > 0)
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        private void Moved(STATE M)
        {
            // que se esperen los putos
            lock (obj)
            {
                //while (true)
                while (Mov != STATE.END)
                {
                    if (Mov == STATE.DOWN || Mov == STATE.UP || Mov == STATE.RIGHT || Mov == STATE.LEFT)
                    {
                        if ((m_bRightPressed || m_bLeftPressed || m_bDownPressed || m_bUpPressed))
                        {
                            m_NewM = _state;
                            C = Color.Black;
                            xPoint Backup_Point = new xPoint(Point.X, Point.Y);
                            int steps = 6;
                            for (int i = 0; i < steps; i++)
                            {

                                // si detecta un nuevo movimiento
                                if (_state == STATE.DIE)
                                    break;

                                int porcent = (int)(i / (float)(steps - 1) * Size.W);

                                switch (m_NewM)
                                {
                                    case STATE.UP:

                                        pImage.Y = 37;
                                        pImage.X = (steps - (i + 1)) * Size.W;
                                        if (Backup_Point.Y - Size.H < 0)
                                            break;

                                        porcent = (int)(i / (float)(steps - 1) * Size.H);
                                        Point.Y = Backup_Point.Y - porcent;

                                        break;
                                    case STATE.RIGHT:

                                        pImage.Y = 37;
                                        pImage.X = i * Size.W;
                                        if (Backup_Point.X + Size.W > Size.W * (this.Map.Cols - 1))
                                            break;

                                        porcent = (int)(i / (float)(steps - 1) * Size.W);
                                        Point.X = Backup_Point.X + porcent;

                                        break;
                                    case STATE.DOWN:

                                        pImage.Y = 37;
                                        pImage.X = i * Size.W;
                                        if (Backup_Point.Y + Size.H > Size.H * (this.Map.Rows - 1))
                                            break;

                                        porcent = (int)(i / (float)(steps - 1) * Size.H);
                                        Point.Y = Backup_Point.Y + porcent;

                                        break;
                                    case STATE.LEFT:

                                        pImage.Y = 37;
                                        pImage.X = (steps - (i + 1)) * Size.W;
                                        if (Backup_Point.X - Size.W < 0)
                                            break;

                                        porcent = (int)(i / (float)(steps - 1) * Size.W);
                                        Point.X = Backup_Point.X - porcent;

                                        break;
                                    default:
                                        break;
                                }

                                Thread.Sleep(50);
                                C = Color.Red;

                            }
                        }
                        else
                        {
                            Mov = STATE.BEGIN;

                        }

                    }
                    else if (Mov == STATE.DIE)
                    {
                        m_NewM = _state;
                        xPoint Backup_Point = new xPoint(Point.X, Point.Y);
                        int steps = 6;
                        for (int i = 0; i < steps; i++)
                        {
                            int porcent = (int)(i / (float)(steps - 1) * Size.W);
                            pImage.Y = 333;
                            pImage.X = i * Size.W;
                            Thread.Sleep(80);

                        }

                        _state = STATE.END;

                    }
                    else if (Mov == STATE.HIT)
                    {
                        m_NewM = _state;
                        xPoint Backup_Point = new xPoint(Point.X, Point.Y);
                        int steps = 5;
                        List<Enemy> _enemys_hit = new List<Enemy>();
                        for (int i = 0; i < steps; i++)
                        {
                            // si detecta un nuevo movimiento
                            if (m_NewM == STATE.DIE)
                                break;

                            int porcent = (int)(i / (float)(steps - 1) * Size.W);

                            pImage.Y = 222;
                            pImage.X = i * Size.W;

                            Thread.Sleep(80);

                            List<Enemy> _enemys = frmConcurrencia.DrawObjects.ToArray().OfType<Enemy>().Where(e => e.Mov != STATE.END).ToList();
                            foreach (Enemy item in _enemys)
                            {
                                // enemigos golpeados
                                if (!_enemys_hit.Contains(item))
                                {
                                    //if (Get().Contains(item.Get()))
                                    if (Get().IntersectsWith(item.Get()))
                                    {
                                        if (Collision(item))
                                        {
                                            //if (item.Point.X % item.Size.W == 0 && item.Point.Y % item.Size.H == 0)
                                            //{
                                            item.Life -= 1;
                                            //item.Mov = MOVED.HURT;


                                            //agregar a enemigos golpeados
                                            _enemys_hit.Add(item);

                                            if (item.Life <= 0)
                                                item.Mov = STATE.DIE;
                                            else
                                            {
                                                //SmokeEffect _effect = new SmokeEffect(item.Point.X, item.Point.Y, item.Size.W, item.Size.H);
                                                //_effect.Effect();
                                                //frmConcurrencia.DrawObjects.Insert(0, _effect);

                                                BloodEffect _blood = new BloodEffect(item.Point.X, item.Point.Y, item.Size.W, item.Size.H);
                                                _blood.Effect();
                                                frmConcurrencia.DrawObjects.Insert(0, _blood);

                                            }
                                            //}

                                        }

                                    }
                                }
                            }

                            //for (int j = frmConcurrencia.DrawObjects.Count - 1; j >= 0; j--)
                            //{
                            //    if (frmConcurrencia.DrawObjects[j] is Enemy)
                            //    {
                            //        ((Enemy)frmConcurrencia.DrawObjects[j]).Mov = MOVED.DIE;
                            //    }
                            //}

                        }

                        // si detecta un nuevo movimiento
                        if (m_NewM == STATE.DIE)
                            continue;
                        else if (m_bRightPressed)
                            _state = STATE.RIGHT;
                        else if (m_bLeftPressed)
                            _state = STATE.LEFT;
                        else if (m_bDownPressed)
                            _state = STATE.DOWN;
                        else if (m_bUpPressed)
                            _state = STATE.UP;
                        else
                            _state = STATE.BEGIN;

                    }
                    else if (Mov == STATE.BEGIN)
                    {
                        m_NewM = _state;
                        xPoint Backup_Point = new xPoint(Point.X, Point.Y);
                        int steps = 4;
                        for (int i = 0; i < steps; i++)
                        {
                            pImage.Y = 185;
                            pImage.X = i * Size.W;

                            // si detecta un nuevo movimiento
                            if (m_NewM != _state)
                                break;

                            int porcent = (int)(i / (float)(steps - 1) * Size.W);

                            Thread.Sleep(80);
                        }
                    }
                }
            }
        }

        public void MOVEDTO(STATE M)
        {
            if (_t == null || _t.ThreadState == ThreadState.Stopped)
            {
                _t = new Thread(() => Moved(M));
                _t.Start();
            }
        }

        public override void Draw(Graphics g)
        {
            //Player
            SolidBrush sbC = new SolidBrush(C);


            //g.FillRectangle(sbC, Point.X, Point.Y, Size.W, Size.H);
            //g.DrawImage(Properties.Resources.eoe, Get(), Get(), GraphicsUnit.Pixel);
            //g.DrawImage(Properties.Resources.eoe, new Rectangle(0,0,Size.W, Size.H), Get(), GraphicsUnit.Pixel);

            switch (m_NewM)
            {
                case STATE.UP:
                    g.DrawImage(Properties.Resources.adventurer_Sheet, Get(), new Rectangle(50 + pImage.X, 0 + pImage.Y, Size.W, Size.H), GraphicsUnit.Pixel);
                    break;
                case STATE.RIGHT:
                    g.DrawImage(Properties.Resources.adventurer_Sheet, Get(), new Rectangle(50 + pImage.X, 0 + pImage.Y, Size.W, Size.H), GraphicsUnit.Pixel);
                    break;
                case STATE.DOWN:
                    g.DrawImage(Properties.Resources.adventurer_Sheet, Get(), new Rectangle(50 + pImage.X, 0 + pImage.Y, Size.W, Size.H), GraphicsUnit.Pixel);
                    break;
                case STATE.LEFT:
                    g.DrawImage(Properties.Resources.adventurer_Sheet, Get(), new Rectangle(50 + pImage.X, 0 + pImage.Y, Size.W, Size.H), GraphicsUnit.Pixel);
                    break;
                case STATE.HIT:
                    //int width = 192;
                    //int height = 100;

                    int width = Size.W * 1;
                    int height = Size.H * 1;


                    //centrar imagen mas grande
                    g.DrawImage(Properties.Resources.adventurer_Sheet,
                        //new Rectangle(Point.X + (Size.W / 2) - (width / 2), Point.Y + (Size.H / 2) - (height / 2), width, height),
                        //new Rectangle(0 + pImage.X, 222 + pImage.Y, width, height),

                        new Rectangle(Point.X + (Size.W / 2) - (width / 2), Point.Y + (Size.H / 2) - (height / 2), width, height),
                        new Rectangle(0 + pImage.X, 0 + pImage.Y, Size.W, Size.H),

                        GraphicsUnit.Pixel);
                    break;
                case STATE.DIE:
                    g.DrawImage(Properties.Resources.adventurer_Sheet, Get(), new Rectangle(0 + pImage.X, 0 + pImage.Y, Size.W, Size.H), GraphicsUnit.Pixel);
                    break;
                case STATE.BEGIN:
                    g.DrawImage(Properties.Resources.adventurer_Sheet, Get(), new Rectangle(150 + pImage.X, 0 + pImage.Y, Size.W, Size.H), GraphicsUnit.Pixel);
                    break;
                default:
                    break;
            }

            //g.DrawRectangle(Pens.Blue, Get());

            _barLife.Draw(g);


            //g.DrawString(string.Format("{0}", this.Life), frmConcurrencia.DefaultFont, Brushes.Black, 0, 0);

            sbC.Dispose();
        }

        #region ICharacter Members

        public void end()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICharacter Members

        void ICharacter.End()
        {
            _t.Abort();
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    public class Enemy : xRectangle, ICharacter, INotifyPropertyChanged
    {
        protected Map _map;

        public Map Map { get { return _map; } }

        Thread _t;

        private STATE m_Mov;

        public xPoint pImage { get; set; }

        public STATE Mov { get { return m_Mov; } set { m_Mov = value; } }

        private BarLife _barLife;

        private int _life;

        public int Life
        {
            get
            {
                return _life;
            }
            set
            {
                _life = value;

                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("Life");
            }
        }

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public Enemy(int X, int Y, int W, int H, Map map)
            : base(X, Y, W, H)
        {
            this.PropertyChanged += new PropertyChangedEventHandler(Player_PropertyChanged);
            this.m_Mov = STATE.BEGIN;
            this.pImage = new xPoint(0, 0);
            this._barLife = new BarLife(X, Y, W, H, this);
            this.Life = 4;
            this._map = map;

        }

        void Player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _barLife.Porcent = (int)((this._life / 4f) * 100);
        }

        public void MOVE()
        {
            if (_t == null || _t.ThreadState == ThreadState.Stopped)
            {

                _t = new Thread(() => Moved(Mov));
                _t.Start();
            }
        }

        private static readonly Object obj = new Object();
        private STATE m_NewM;
        Random r = new Random();

        private bool Collision(Player item)
        {

            int arriba = Math.Max(Point.Y, item.Point.Y);
            int abajo = Math.Min(Point.Y + Size.H, item.Point.Y + item.Size.H);
            int izquierda = Math.Max(Point.X, item.Point.X);
            int derecha = Math.Max(Point.X + Size.W, item.Point.X + item.Size.W);

            Bitmap _player_bmp = new Bitmap(Size.W, Size.H);
            using (Graphics g = Graphics.FromImage(_player_bmp))
            {
                g.DrawImage(Properties.Resources.adventurer_Sheet,
                    new Rectangle(0, 0, _player_bmp.Width, _player_bmp.Height),
                    new Rectangle(pImage.X, pImage.Y, Size.W, Size.H),
                    GraphicsUnit.Pixel);

            }

            Bitmap _enemy_bmp = new Bitmap(item.Size.W, item.Size.H);
            using (Graphics g = Graphics.FromImage(_enemy_bmp))
            {
                g.DrawImage(Properties.Resources.adventurer_Sheet,
                    new Rectangle(0, 0, _enemy_bmp.Width, _enemy_bmp.Height),
                    new Rectangle(item.pImage.X, item.pImage.Y, Size.W, Size.H),
                    GraphicsUnit.Pixel);

            }

            try
            {
                for (int y = arriba; y < abajo; y++)
                {
                    for (int x = izquierda; x < derecha; x++)
                    {
                        int _player_x = x - (Point.X + Size.W);
                        int _player_y = y - (Point.Y + Size.H);


                        int _enemy_x = x - (item.Point.X + item.Size.W);
                        int _enemy_y = y - (item.Point.Y + item.Size.H);


                        if (_player_bmp.GetPixel(
                            Math.Min(Size.W + _player_x, Size.W - 1),
                            Math.Min(Size.H + _player_y, Size.H - 1)).A > 0 &&
                            _enemy_bmp.GetPixel(
                            Math.Min(item.Size.W + _enemy_x, item.Size.W - 1),
                            Math.Min(item.Size.H + _enemy_y, item.Size.H - 1)).A > 0)
                        {
                            return true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_player_bmp != null)
                    _player_bmp.Dispose();

                if (_enemy_bmp != null)
                    _enemy_bmp.Dispose();
            }
            return false;
        }

        private void Moved(STATE M)
        {

            int value;

            // que se esperen los putos
            //lock (obj)
            //{
            //while (true)
            while (Mov != STATE.END)
            {
                if (Mov == STATE.DOWN || Mov == STATE.UP || Mov == STATE.RIGHT || Mov == STATE.LEFT)
                {
                    m_NewM = m_Mov;
                    xPoint Backup_Point = new xPoint(Point.X, Point.Y);
                    int steps = 6;
                    for (int i = 0; i < steps; i++)
                    {

                        // si detecta un nuevo movimiento
                        if (m_NewM != m_Mov)
                            break;

                        int porcent = (int)(i / (float)(steps - 1) * Size.W);
                        //int porcent = (int)(i / (float)(steps - 1) * r.Next(Size.W, 2 * Size.W));

                        switch (m_NewM)
                        {
                            case STATE.UP:

                                pImage.Y = 37;
                                pImage.X = (steps - (i + 1)) * Size.W;
                                if (Backup_Point.Y - Size.H < 0)
                                    continue;

                                porcent = (int)(i / (float)(steps - 1) * Size.H);
                                Point.Y = Backup_Point.Y - porcent;

                                break;
                            case STATE.RIGHT:

                                pImage.Y = 37;
                                pImage.X = i * Size.W;
                                if (Backup_Point.X + Size.W > Size.W * (this.Map.Cols - 1))
                                    continue;

                                porcent = (int)(i / (float)(steps - 1) * Size.W);
                                Point.X = Backup_Point.X + porcent;

                                break;
                            case STATE.DOWN:

                                pImage.Y = 37;
                                pImage.X = i * Size.W;
                                if (Backup_Point.Y + Size.H > Size.H * (this.Map.Rows - 1))
                                    continue;


                                porcent = (int)(i / (float)(steps - 1) * Size.H);
                                Point.Y = Backup_Point.Y + porcent;

                                break;
                            case STATE.LEFT:

                                pImage.Y = 37;
                                pImage.X = (steps - (i + 1)) * Size.W;
                                if (Backup_Point.X - Size.W < 0)
                                    continue;

                                porcent = (int)(i / (float)(steps - 1) * Size.W);
                                Point.X = Backup_Point.X - porcent;

                                break;
                            default:
                                break;
                        }

                        Thread.Sleep(r.Next(100, 150));
                    }
                }
                else if (Mov == STATE.DIE)
                {
                    m_NewM = m_Mov;
                    xPoint Backup_Point = new xPoint(Point.X, Point.Y);
                    int steps = 6;
                    for (int i = 0; i < steps; i++)
                    {

                        pImage.Y = 333;
                        pImage.X = i * Size.W;
                        int porcent = (int)(i / (float)(steps - 1) * Size.W);

                        Thread.Sleep(r.Next(80, 200));

                    }

                    m_Mov = STATE.END;
                    return;
                }
                else if (Mov == STATE.HURT)
                {
                    m_NewM = m_Mov;

                    xPoint Backup_Point = new xPoint(Point.X, Point.Y);
                    int steps = 3;
                    for (int i = 0; i < steps; i++)
                    {
                        pImage.Y = 296;
                        pImage.X = i * Size.W;
                        int porcent = (int)(i / (float)(steps - 1) * Size.W);

                        Thread.Sleep(r.Next(80, 200));
                    }

                    m_Mov = STATE.BEGIN;


                }
                else if (Mov == STATE.HIT)
                {
                    m_NewM = m_Mov;
                    List<Player> _players_hit = new List<Player>();
                    xPoint Backup_Point = new xPoint(Point.X, Point.Y);
                    int steps = 5;
                    for (int i = 0; i < steps; i++)
                    {
                        pImage.Y = 222;

                        pImage.X = i * Size.W;
                        // si detecta un nuevo movimiento
                        if (m_NewM != m_Mov)
                            break;

                        int porcent = (int)(i / (float)(steps - 1) * Size.W);

                        Thread.Sleep(r.Next(80, 150));

                        lock (obj)
                        {
                            List<Player> _players = frmConcurrencia.DrawObjects.OfType<Player>().Where(e => e.Mov != STATE.END).ToList();
                            foreach (Player item in _players)
                            {
                                // players golpeados
                                if (!_players_hit.Contains(item))
                                {
                                    //if (Get().Contains(item.Get()))
                                    if (Get().IntersectsWith(item.Get()))
                                    {
                                        if (Collision(item))
                                        {
                                            item.Life -= 1;

                                            //agregar a players golpeados
                                            _players_hit.Add(item);

                                            if (item.Life <= 0)
                                                item.Mov = STATE.DIE;
                                            else
                                            {
                                                //SmokeEffect _effect = new SmokeEffect(item.Point.X, item.Point.Y, item.Size.W, item.Size.H);
                                                //_effect.Effect();
                                                //frmConcurrencia.DrawObjects.Insert(0, _effect);

                                                BloodEffect _blood = new BloodEffect(item.Point.X, item.Point.Y, item.Size.W, item.Size.H);
                                                _blood.Effect();
                                                frmConcurrencia.DrawObjects.Insert(0, _blood);
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                if (Mov == STATE.BEGIN)
                {

                    m_NewM = m_Mov;
                    xPoint Backup_Point = new xPoint(Point.X, Point.Y);
                    int steps = 4;
                    for (int i = 0; i < steps; i++)
                    {
                        pImage.Y = 185;
                        pImage.X = i * Size.W;

                        // si detecta un nuevo movimiento
                        if (m_NewM != m_Mov)
                            break;

                        int porcent = (int)(i / (float)(steps - 1) * Size.W);


                        Thread.Sleep(r.Next(80, 150));

                    }

                    bool moverse = false;
                    List<Player> _players = frmConcurrencia.DrawObjects.OfType<Player>().Where(e => e.Mov != STATE.END).ToList();
                    foreach (Player item in _players)
                    {
                        if (xPoint.Distancia(item.Point, this.Point) > 50)
                        {
                            moverse = true;
                            break;
                        }
                    }
                    if (moverse)
                        continue;

                }

                if (Mov == STATE.DIE || Mov == STATE.HURT)
                    continue;

                value = r.Next(0, 6);
                switch (value)
                {
                    case 0:
                        m_Mov = STATE.BEGIN;
                        break;
                    case 1:
                        m_Mov = STATE.DOWN;
                        break;
                    case 2:
                        m_Mov = STATE.RIGHT;
                        break;
                    case 3:
                        m_Mov = STATE.LEFT;
                        break;
                    case 4:
                        m_Mov = STATE.UP;
                        break;
                    case 5:
                        m_Mov = STATE.HIT;
                        break;
                    case 6:
                        m_Mov = STATE.DIE;
                        break;
                    default:

                        break;
                }
            }
            //}
        }

        public override void Draw(Graphics g)
        {

            switch (m_NewM)
            {
                case STATE.UP:
                    g.DrawImage(Properties.Resources.adventurer_Sheet, Get(), new Rectangle(50 + pImage.X, 0 + pImage.Y, Size.W, Size.H), GraphicsUnit.Pixel);
                    break;
                case STATE.RIGHT:
                    g.DrawImage(Properties.Resources.adventurer_Sheet, Get(), new Rectangle(50 + pImage.X, 0 + pImage.Y, Size.W, Size.H), GraphicsUnit.Pixel);
                    break;
                case STATE.DOWN:
                    g.DrawImage(Properties.Resources.adventurer_Sheet, Get(), new Rectangle(50 + pImage.X, 0 + pImage.Y, Size.W, Size.H), GraphicsUnit.Pixel);
                    break;
                case STATE.LEFT:
                    g.DrawImage(Properties.Resources.adventurer_Sheet, Get(), new Rectangle(50 + pImage.X, 0 + pImage.Y, Size.W, Size.H), GraphicsUnit.Pixel);
                    break;
                case STATE.HIT:
                    //int width = 192;
                    //int height = 100;

                    int width = Size.W * 1;
                    int height = Size.H * 1;


                    //centrar imagen mas grande
                    g.DrawImage(Properties.Resources.adventurer_Sheet,
                        //new Rectangle(Point.X + (Size.W / 2) - (width / 2), Point.Y + (Size.H / 2) - (height / 2), width, height),
                        //new Rectangle(0 + pImage.X, 222 + pImage.Y, width, height),

                        new Rectangle(Point.X + (Size.W / 2) - (width / 2), Point.Y + (Size.H / 2) - (height / 2), width, height),
                        new Rectangle(0 + pImage.X, 0 + pImage.Y, Size.W, Size.H),

                        GraphicsUnit.Pixel);
                    break;
                case STATE.DIE:
                    g.DrawImage(Properties.Resources.adventurer_Sheet, Get(), new Rectangle(0 + pImage.X, 0 + pImage.Y, Size.W, Size.H), GraphicsUnit.Pixel);
                    break;
                case STATE.HURT:
                    g.DrawImage(Properties.Resources.adventurer_Sheet, Get(), new Rectangle(150 + pImage.X, 0 + pImage.Y, Size.W, Size.H), GraphicsUnit.Pixel);
                    break;
                case STATE.BEGIN:
                    g.DrawImage(Properties.Resources.adventurer_Sheet, Get(), new Rectangle(150 + pImage.X, 0 + pImage.Y, Size.W, Size.H), GraphicsUnit.Pixel);
                    break;
                default:
                    break;
            }

            bool drawbar = false;
            List<Player> _players = frmConcurrencia.DrawObjects.ToArray().OfType<Player>().Where(e => e.Mov != STATE.END).ToList();
            foreach (Player item in _players)
            {
                if (xPoint.Distancia(item.Point, this.Point) < 50)
                {
                    drawbar = true;
                    break;
                }
            }
            if (drawbar)
                _barLife.Draw(g);

            //g.DrawRectangle(Pens.Red, Get());

        }


        #region ICharacter Members

        public void End()
        {
            _t.Abort();
        }

        #endregion
    }

    public class Map : IDraw
    {
        private int _cols;
        private int _rows;
        private int _width;
        private int _height;

        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        public int Hieght
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        public int Cols
        {
            get
            {
                return _cols;
            }
        }
        public int Rows
        {
            get
            {
                return _rows;
            }
        }

        List<xRectangle> rects = new List<xRectangle>();

        public Map(int width, int height, int Cols, int Rows)
        {
            this.Width = width;
            this.Hieght = height;
            this._cols = Cols;
            this._rows = Rows;
            this.Arrange();

        }

        public void Arrange()
        {
            //Map
            for (int i = 0; i < this.Cols; i++)
            {
                for (int j = 0; j < this.Rows; j++)
                {
                    xRectangle rect = new xRectangle(i * this.Width, j * this.Hieght, this.Width, this.Hieght);
                    rects.Add(rect);
                }
            }
        }


        #region IDraw Members

        public void Draw(Graphics g)
        {

            //Map
            for (int i = 0; i < this.rects.Count; i++)
            {
                g.FillRectangle(Brushes.White, this.rects[i].Get());
                g.DrawRectangle(Pens.WhiteSmoke, this.rects[i].Get());
            }
        }

        #endregion
    }

    public class Ball : xRectangle
    {
        Thread t;

        private STATE m_Mov;

        public Ball(int X, int Y, int W, int H, STATE M)
            : base(X, Y, W, H)
        {
            this.m_Mov = M;
        }

        //private static readonly Object obj_ball = new Object();

        public void SHOOTING()
        {
            if (t == null || t.ThreadState == ThreadState.Stopped)
            {

                t = new Thread(() => Shoting_Ball());
                t.Start();
            }
        }

        private void Shoting_Ball()
        {
            // que se esperen los putos
            //lock (obj_ball)
            //{
            int steps = 80;
            xPoint Backup_Point = new xPoint(Point.X, Point.Y);
            for (int i = 0; i < steps; i++)
            {
                int porcent = (int)(i / (float)(steps - 1) * 100f);


                switch (m_Mov)
                {
                    case STATE.UP:
                        Point.Y = Backup_Point.Y - porcent;
                        break;
                    case STATE.RIGHT:
                        Point.X = Backup_Point.X + porcent;
                        break;
                    case STATE.DOWN:
                        Point.Y = Backup_Point.Y + porcent;
                        break;
                    case STATE.LEFT:
                        Point.X = Backup_Point.X - porcent;
                        break;
                    case STATE.BEGIN:
                        Point.X = Backup_Point.X + porcent;
                        break;
                    default:
                        break;
                }


                Thread.Sleep(10);
            }

            frmConcurrencia.DrawObjects.Remove(this);
            //}
        }
    }

    public class BarLife : xRectangle
    {
        protected int _porcent;
        public int Porcent
        {
            get
            {
                return _porcent;
            }
            set
            {
                _porcent = value;
            }
        }

        private Brush _Color;
        public Brush Color
        {
            get
            {
                return _Color;
            }
            set
            {
                _Color = value;
            }
        }


        public xRectangle _target;

        public BarLife(int X, int Y, int W, int H, xRectangle target)
            : base(X, Y, W, H)
        {
            _Color = Brushes.Red;
            _target = target;
        }

        public override void Draw(Graphics g)
        {
            //g.FillRectangle(Brushes.DimGray, new Rectangle(_target.Point.X, _target.Point.Y, Size.W, 2));
            g.FillRectangle(_Color, new Rectangle(_target.Point.X, _target.Point.Y, (int)(Size.W * (Porcent / 100f)), 2));
        }

    }

    public class BarLifePlayer : BarLife
    {
        public BarLifePlayer(int X, int Y, int W, int H, xRectangle target)
            : base(X, Y, W, H, target)
        {
        }

        public override void Draw(Graphics g)
        {
            //g.FillRectangle(Brushes.DimGray, new Rectangle(_target.Point.X, _target.Point.Y, Size.W, 2));
            g.FillRectangle(this.Color, new Rectangle(0, 0, (int)(Size.W * (Porcent / 100f)), 4));
        }

    }

    public class SmokeEffect : xRectangle
    {
        Thread t;

        xPoint pImage;

        public SmokeEffect(int X, int Y, int W, int H)
            : base(X, Y, W, H)
        {
            pImage = new xPoint(0, 0);
        }

        public void Effect()
        {
            if (t == null || t.ThreadState == ThreadState.Stopped)
            {

                t = new Thread(() => Effect_Aply());
                t.Start();
            }
        }

        private void Effect_Aply()
        {

            int steps = 6;
            xPoint Backup_Point = new xPoint(Point.X, Point.Y);
            for (int i = 0; i < steps; i++)
            {
                pImage.X = i * 32;
                Thread.Sleep(30);
            }
            frmConcurrencia.DrawObjects.Remove(this);
        }

        public override void Draw(Graphics g)
        {
            g.DrawImage(Properties.Resources.smoke,
                new Rectangle(Point.X + (Size.W / 2) - 16, Point.Y + (Size.H / 2) - 16, 32, 32),
                new Rectangle(pImage.X, pImage.Y, 32, 32),
                GraphicsUnit.Pixel);
        }

    }


    public class BloodEffect : xRectangle
    {
        Thread t;

        xPoint pImage;

        public BloodEffect(int X, int Y, int W, int H)
            : base(X, Y, W, H)
        {
            pImage = new xPoint(0, 0);
        }

        public void Effect()
        {
            if (t == null || t.ThreadState == ThreadState.Stopped)
            {

                t = new Thread(() => Effect_Aply());
                t.Start();
            }
        }

        private void Effect_Aply()
        {

            int steps = 6;
            xPoint Backup_Point = new xPoint(Point.X, Point.Y);
            for (int i = 0; i < steps; i++)
            {
                pImage.X = i * 64;
                Thread.Sleep(30);
            }
            frmConcurrencia.DrawObjects.Remove(this);
        }

        public override void Draw(Graphics g)
        {
            g.DrawImage(Properties.Resources.blood,
                new Rectangle(Point.X + (Size.W / 2) - 32, Point.Y + (Size.H / 2) - 32, 64, 64),
                new Rectangle(pImage.X, pImage.Y, 64, 64),
                GraphicsUnit.Pixel);
        }

    }
}
