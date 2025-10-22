using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace UtilETWeb.Effects
{
    public class CustomBackgroundWorker : BackgroundWorker
    {
        public object Tag
        {
            get;
            set;
        }
    }

    public class Animate
    {
        private CustomBackgroundWorker _bgw1 = new CustomBackgroundWorker();

        public int Millisecunds { get; set; }

        public object Tag
        {
            get
            {
                return _bgw1.Tag;
            }
            set
            {
                _bgw1.Tag = value;
            }
        }

        public Animate()
        {
            _bgw1.WorkerReportsProgress = true;
            _bgw1.WorkerSupportsCancellation = true;
            _bgw1.DoWork += new DoWorkEventHandler(_bgw1_DoWork);
            _bgw1.ProgressChanged += new ProgressChangedEventHandler(_bgw1_ProgressChanged);
            _bgw1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgw1_RunWorkerCompleted);
        }

        public void Star(object prms)
        {
            if (!_bgw1.IsBusy)
                _bgw1.RunWorkerAsync(prms);
        }

        void _bgw1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (RunWorkerCompleted != null)
                RunWorkerCompleted(sender, e);
        }

        // simple event 
        public event EventHandler RunWorkerCompleted;

        void _bgw1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine(e.ProgressPercentage.ToString());

            if (ProgressChanged != null)
                ProgressChanged(sender, e);

            if (CustomProgressChanged != null)
                CustomProgressChanged(sender, e);

        }

        // simple event 
        public event EventHandler ProgressChanged;

        // custom event
        public delegate void CustomEventHandlerProgressChanged(object sender, ProgressChangedEventArgs e);
        public event CustomEventHandlerProgressChanged CustomProgressChanged;


        void _bgw1_DoWork(object sender, DoWorkEventArgs e)
        {
            object args = e.Argument;
            double diff;
            DateTime future = DateTime.Now.AddMilliseconds(Millisecunds);
            double total = (future - DateTime.Now).TotalMilliseconds;
            do
            {
                diff = Math.Max((future - DateTime.Now).TotalMilliseconds, 0);
                _bgw1.ReportProgress((int)((diff / total) * 100), (diff / total));
                System.Threading.Thread.Sleep(10);
            } while (diff > 0);
        }
    }

    public enum MovesDirection
    {
        top,
        left,
        right,
        bottom
    }

    public class AnimateMovedState
    {
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
        public double porcent { get; set; }
        public MovesDirection direction { get; set; }
        public AnimateMovedState(Rectangle rec, MovesDirection mov) 
        {
            this.x = rec.X;
            this.y = rec.Y;
            this.w = rec.Width;
            this.h = rec.Height;
            this.direction = mov;
 
        }
    }

    public class AnimateMoved
    {
        private CustomBackgroundWorker _bgw1 = new CustomBackgroundWorker();

        public int Millisecunds { get; set; }

        private static Random Rand { get; set; }

        public object Tag
        {
            get
            {
                return _bgw1.Tag;
            }
            set
            {
                _bgw1.Tag = value;
            }
        }

        public AnimateMoved()
        {
            Rand = new Random();
            _bgw1.WorkerReportsProgress = true;
            _bgw1.WorkerSupportsCancellation = true;
            _bgw1.DoWork += new DoWorkEventHandler(_bgw1_DoWork);
            _bgw1.ProgressChanged += new ProgressChangedEventHandler(_bgw1_ProgressChanged);
            _bgw1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgw1_RunWorkerCompleted);
        }

        public void Star(AnimateMovedState prms)
        {
            if (!_bgw1.IsBusy)
                _bgw1.RunWorkerAsync(prms);
        }

        void _bgw1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (RunWorkerCompleted != null)
                RunWorkerCompleted(sender, e);
        }

        // simple event 
        public event EventHandler RunWorkerCompleted;

        void _bgw1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine(e.ProgressPercentage.ToString());

            if (ProgressChanged != null)
                ProgressChanged(sender, e);

            if (CustomProgressChanged != null)
                CustomProgressChanged(sender, e,Rand);

        }

        // simple event 
        public event EventHandler ProgressChanged;

        // custom event
        public delegate void CustomEventHandlerProgressChanged(object sender, ProgressChangedEventArgs e, Random Rand);
        public event CustomEventHandlerProgressChanged CustomProgressChanged;


        void _bgw1_DoWork(object sender, DoWorkEventArgs e)
        {
            AnimateMovedState state = (AnimateMovedState)e.Argument;
            double diff;
            DateTime future = DateTime.Now.AddMilliseconds(Millisecunds);
            double total = (future - DateTime.Now).TotalMilliseconds;
            do
            {
                diff = Math.Max((future - DateTime.Now).TotalMilliseconds, 0);
                state.porcent = (diff / total);
                _bgw1.ReportProgress((int)((diff / total) * 100), state);
                System.Threading.Thread.Sleep(10);
            } while (diff > 0);
        }
    }

    public class ListAnimate
    {
        List<Animate> l = new List<Animate>();

        public ListAnimate()
        {
            l = new List<Animate>();
        }

        public void Add(Animate a)
        {
        }

        public void Start()
        {
            for (int i = 0; i < l.Count; i++)
            {
                Animate item = l[i];

                if (i - 1 >= 0)
                    item.Tag = l[i - 1];

                item.RunWorkerCompleted += new EventHandler(item_RunWorkerCompleted);
            }
        }

        void item_RunWorkerCompleted(object sender, EventArgs e)
        {
            CustomBackgroundWorker _bgw = sender as CustomBackgroundWorker;
            if (_bgw.Tag != null)
                ((CustomBackgroundWorker)(_bgw.Tag)).RunWorkerAsync();

        }

        void ListAnimate_ProgressChanged(object sender, EventArgs e)
        {

        }
    }
}