using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//https://www.capyclo.com/category/informacion-al-usuario/
namespace OrganizationChart
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // get parameter PosID
            int _PosID;
            if (!int.TryParse(this.textBox1.Text, out _PosID))
                _PosID = -1;

            // generate tree
            this.treeNodePanel1.PosID = _PosID;
            this.treeNodePanel1.Generate(_PosID);
            this.treeNodePanel1.Refresh();
        }
    }
}
