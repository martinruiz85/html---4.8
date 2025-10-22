using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UtilETWeb
{
    public partial class frmRadialTree : Form
    {
        public frmRadialTree()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmRadialTree_Load);
        }

        void frmRadialTree_Load(object sender, EventArgs e)
        {
            this.radialTreePanelMySqlObjects1.BackColor = Color.White;
            this.radialTreePanelMySqlObjects1.ItemBind();
            this.radialTreePanelMySqlObjects1.MyTreeNode_MouseDown += new html.RadialTreePanel<frmDependsCustom.MySqlObjects>.DelegateTreeNode_MouseDown(radialTreePanel1_MyTreeNode_MouseDown);
            this.radialTreePanelMySqlObjects1.MyTreeNode_MouseHover += new html.RadialTreePanel<frmDependsCustom.MySqlObjects>.DelegateTreeNode_MouseHover(radialTreePanelMySqlObjects1_MyTreeNode_MouseHover);
            this.radialTreePanelMySqlObjects1.MyTreeNode_MouseOut += new html.RadialTreePanel<frmDependsCustom.MySqlObjects>.DelegateTreeNode_MouseOut(radialTreePanelMySqlObjects1_MyTreeNode_MouseOut);            
        }

        void radialTreePanelMySqlObjects1_MyTreeNode_MouseOut(Node<frmDependsCustom.MySqlObjects> Node, object sender, MouseEventArgs e)
        {
         
        }

        public ToolTip NodeToolTip = new ToolTip();
        int count = 0;

        void radialTreePanelMySqlObjects1_MyTreeNode_MouseHover(Node<frmDependsCustom.MySqlObjects> Node, object sender, MouseEventArgs e)
        {
            //NodeToolTip.Show(string.Format("Node Hover: {0}-{1}", Node.Data, count), this, 100);
            NodeToolTip.SetToolTip(radialTreePanelMySqlObjects1, string.Format("entro a {0}", Node.Item.name));
            //MessageBox.Show(string.Format("entro a {0}", Node.Data));
        }

     

        ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
        void radialTreePanel1_MyTreeNode_MouseDown(Node<frmDependsCustom.MySqlObjects> Node, object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Items.Clear();
                contextMenuStrip1.Items.Add("TreeView");
                contextMenuStrip1.Items.Add("TreeText");
                contextMenuStrip1.Items.Add("ListText");
                contextMenuStrip1.Items.Add("Generate SP Root");
                contextMenuStrip1.Items.Add("Print PostOrden");
                //contextMenuStrip1.Show(this, new Point((int)(Node.Point.X - this.HorizontalScroll.Value), (int)(Node.Point.Y + Node.NodeHeight - this.VerticalScroll.Value)));
                contextMenuStrip1.Show(this, new Point((int)(Node.x - this.radialTreePanelMySqlObjects1.HorizontalScroll.Value), (int)(Node.y + 9 * 2 - this.radialTreePanelMySqlObjects1.VerticalScroll.Value)));
            }
            else
            {
                //MessageBox.Show(string.Format("{0}", Node.Data));
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.radialTreePanelMySqlObjects1._zomm = this.trackBar1.Value;
            this.radialTreePanelMySqlObjects1.ItemBind();
        }



    }
}
