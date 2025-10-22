using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace UtilETWeb
{
    public partial class frmConcatenate : Form
    {
        public frmConcatenate()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmConcatenate_Load);
        }

        void frmConcatenate_Load(object sender, EventArgs e)
        {            
            Thread t = new Thread(new ThreadStart(count));
            t.Start();
        }

        private void count() 
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(100);    
            }
            
        }
    }
}
