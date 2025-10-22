using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Words
{
    public partial class Form1 : Form
    {

        string[,] map;


        List<IDraw> DrawObjects = new List<IDraw>();

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.Load += new EventHandler(Form1_Load);
            this.Paint += new PaintEventHandler(Form1_Paint);
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (IDraw d in DrawObjects)
            {
                d.Draw(g);
            }
        }

        void Form1_Load(object sender, EventArgs e)
        {
            this.timer1.Interval = 10;

            map = new string[,] 
            { 
            { "A", "B", "B", "B", "B", "B", "B", "B", "B", "B" }, 
            { "B", "A", "B", "B", "B", "B", "B", "B", "B", "B" },
            { "B", "A", "B", "B", "C", "K", "B", "B", "B", "B" },
            { "A", "B", "B", "B", "B", "R", "B", "B", "B", "B" }, 
            { "B", "A", "B", "F", "D", "Y", "C", "D", "B", "I" },
            { "Q", "A", "B", "B", "S", "B", "B", "H", "B", "B" },
            { "A", "B", "B", "G", "B", "B", "B", "W", "X", "B" }, 
            { "B", "A", "B", "B", "Z", "C", "B", "B", "N", "O" },
            { "B", "A", "B", "B", "J", "D", "B", "B", "B", "B" },
            { "B", "A", "B", "B", "B", "B", "L", "M", "B", "P" }
            };



            for (int i = 0; i < map.GetLength(1); i++)
            {
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    IDraw _symbol = null;

                    switch (map[j, i])
                    {
                        case "A": _symbol = GenericFactory.CreateGeneric<LetterA>(); break;
                        case "B": _symbol = GenericFactory.CreateGeneric<LetterB>(); break;
                        case "C": _symbol = GenericFactory.CreateGeneric<LetterC>(); break;
                        case "D": _symbol = GenericFactory.CreateGeneric<LetterD>(); break;
                        case "E": _symbol = GenericFactory.CreateGeneric<LetterE>(); break;
                        case "F": _symbol = GenericFactory.CreateGeneric<LetterF>(); break;
                        case "G": _symbol = GenericFactory.CreateGeneric<LetterG>(); break;
                        case "H": _symbol = GenericFactory.CreateGeneric<LetterH>(); break;
                        case "I": _symbol = GenericFactory.CreateGeneric<LetterI>(); break;
                        case "J": _symbol = GenericFactory.CreateGeneric<LetterJ>(); break;
                        case "K": _symbol = GenericFactory.CreateGeneric<LetterK>(); break;
                        case "L": _symbol = GenericFactory.CreateGeneric<LetterL>(); break;
                        case "M": _symbol = GenericFactory.CreateGeneric<LetterM>(); break;
                        case "N": _symbol = GenericFactory.CreateGeneric<LetterN>(); break;
                        case "Ñ": _symbol = GenericFactory.CreateGeneric<LetterÑ>(); break;
                        case "O": _symbol = GenericFactory.CreateGeneric<LetterO>(); break;
                        case "P": _symbol = GenericFactory.CreateGeneric<LetterP>(); break;
                        case "Q": _symbol = GenericFactory.CreateGeneric<LetterQ>(); break;
                        case "R": _symbol = GenericFactory.CreateGeneric<LetterR>(); break;
                        case "S": _symbol = GenericFactory.CreateGeneric<LetterS>(); break;
                        case "T": _symbol = GenericFactory.CreateGeneric<LetterT>(); break;
                        case "U": _symbol = GenericFactory.CreateGeneric<LetterU>(); break;
                        case "V": _symbol = GenericFactory.CreateGeneric<LetterV>(); break;
                        case "W": _symbol = GenericFactory.CreateGeneric<LetterW>(); break;
                        case "X": _symbol = GenericFactory.CreateGeneric<LetterX>(); break;
                        case "Y": _symbol = GenericFactory.CreateGeneric<LetterY>(); break;
                        case "Z": _symbol = GenericFactory.CreateGeneric<LetterZ>(); break;
                        default:
                            _symbol = GenericFactory.CreateGeneric<Letter>();
                            break;
                    }
                    _symbol.X = i * (int)_symbol.W;
                    _symbol.Y = j * (int)_symbol.H;

                    DrawObjects.Add(_symbol);
                }
            }


            //DrawObjects.Add(GenericFactory.CreateGeneric<LeterA>());
            //DrawObjects.Add(GenericFactory.CreateGeneric<LeterB>());

            this.timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void Arrange()
        {
        }
    }

    public static class GenericFactory
    {
        public static T CreateGeneric<T>()
        {
            return ((T)Activator.CreateInstance<T>());
        }
    }

    public interface IRect
    {
        int X { get; set; }
        int Y { get; set; }
        float W { get; set; }
        float H { get; set; }
        RectangleF GetRect();
    }

    public interface IDraw : IRect
    {
        void Draw(Graphics g);
    }

    public abstract class MyDraw : IDraw
    {

        protected int x;
        protected int y;
        protected float w;
        protected float h;

        public MyDraw()
        {
            x = 0;
            y = 0;
            w = 20;
            h = 20;
        }

        #region IDraw Members

        public virtual void Draw(Graphics g)
        {
        }

        #endregion

        #region IRect Members

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public float W
        {
            get
            {
                return w;
            }
            set
            {
                w = value;
            }
        }

        public float H
        {
            get
            {
                return h;
            }
            set
            {
                h = value;
            }
        }

        public RectangleF GetRect()
        {
            return new RectangleF(x, y, w, h);
        }

        #endregion
    }

    public class Letter : MyDraw
    {
        protected string txt { get; set; }
        protected Font f { get; set; }

        public Letter()
        {
            f = new Font("Arial", 12);
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.DrawRectangle(Pens.Black, Rectangle.Round(GetRect()));
            g.DrawString(txt, f, Brushes.Black, GetRect());

        }

    }

    public class LetterA : Letter { public LetterA() : base() { txt = "A"; } }
    public class LetterB : Letter { public LetterB() : base() { txt = "B"; } }
    public class LetterC : Letter { public LetterC() : base() { txt = "C"; } }
    public class LetterD : Letter { public LetterD() : base() { txt = "D"; } }
    public class LetterE : Letter { public LetterE() : base() { txt = "E"; } }
    public class LetterF : Letter { public LetterF() : base() { txt = "F"; } }
    public class LetterG : Letter { public LetterG() : base() { txt = "G"; } }
    public class LetterH : Letter { public LetterH() : base() { txt = "H"; } }
    public class LetterI : Letter { public LetterI() : base() { txt = "I"; } }
    public class LetterJ : Letter { public LetterJ() : base() { txt = "J"; } }
    public class LetterK : Letter { public LetterK() : base() { txt = "K"; } }
    public class LetterL : Letter { public LetterL() : base() { txt = "L"; } }
    public class LetterM : Letter { public LetterM() : base() { txt = "M"; } }
    public class LetterN : Letter { public LetterN() : base() { txt = "N"; } }
    public class LetterÑ : Letter { public LetterÑ() : base() { txt = "Ñ"; } }
    public class LetterO : Letter { public LetterO() : base() { txt = "O"; } }
    public class LetterP : Letter { public LetterP() : base() { txt = "P"; } }
    public class LetterQ : Letter { public LetterQ() : base() { txt = "Q"; } }
    public class LetterR : Letter { public LetterR() : base() { txt = "R"; } }
    public class LetterS : Letter { public LetterS() : base() { txt = "S"; } }
    public class LetterT : Letter { public LetterT() : base() { txt = "T"; } }
    public class LetterU : Letter { public LetterU() : base() { txt = "U"; } }
    public class LetterV : Letter { public LetterV() : base() { txt = "V"; } }
    public class LetterW : Letter { public LetterW() : base() { txt = "W"; } }
    public class LetterX : Letter { public LetterX() : base() { txt = "X"; } }
    public class LetterY : Letter { public LetterY() : base() { txt = "Y"; } }
    public class LetterZ : Letter { public LetterZ() : base() { txt = "Z"; } }





}
