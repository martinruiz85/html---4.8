using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace UtilETWeb
{
    public class Node<T> : IEnumerable<Node<T>>
        where T : class, new()
    {
        private List<Node<T>> nodes;

        public T Item { get; private set; }

        public Node()
            : this(new T())
        {
        }

        public Node(T item)
        {
            nodes = new List<Node<T>>();
            Item = item;
            this.angle = 0;
            this.x = 0;
            this.y = 0;
            this.rightBisector = 0;
            this.leftBisector = TWO_PI;
            this.rightTangent = 0;
            this.leftTangent = TWO_PI;
            this.NodePen = new Pen(Color.LightGray, 3);
            this.NodeBrush = new SolidBrush(Color.Gray);
        }

        public List<Node<T>> Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }

        public string Name;
        public bool IsVisit = false;

        private static float TWO_PI = (float)Math.PI * (float)2.0;
        public bool IsRoot { get; set; }
        public Node<T> Parent { get; set; }
        public Pen NodePen { get; set; }
        public SolidBrush NodeBrush { get; set; }
        public RectangleF NodeRectangleF { get; set; }
        public float angleRange = TWO_PI;
        public int zIndex = 0;


        /// <summary>
        /// propiedad recursiva
        /// </summary>
        public int Depth
        {
            get
            {
                int depth = 1;
                System.Collections.IEnumerator itr = this.nodes.GetEnumerator();
                while (itr.MoveNext())
                {
                    Node<T> node = (Node<T>)itr.Current;
                    int childDepth = node.Depth;
                    if (childDepth >= depth)
                    {
                        depth = childDepth + 1;
                    }
                }
                return depth;
            }
        }

        public IEnumerable<Node<T>> Ancestors
        {
            get
            {
                if (IsRoot)
                {
                    return Enumerable.Empty<Node<T>>();
                }
                return Parent.ToIEnumarable().Concat(this.Parent.Ancestors);
            }
        }

        public int Level
        {
            get
            {
                return Ancestors.Count();
            }
        }

        public float leftLimit()
        {
            return Math.Min(leftBisector, leftTangent);
        }

        public float rightLimit()
        {
            return Math.Max(rightBisector, rightTangent);
        }

        #region IRadialTree Members

        public float angle
        {
            get;
            set;
        }

        public float x
        {
            get;
            set;
        }

        public float y
        {
            get;
            set;
        }

        public float rightBisector
        {
            get;
            set;
        }

        public float leftBisector
        {
            get;
            set;
        }

        public float rightTangent
        {
            get;
            set;
        }

        public float leftTangent
        {
            get;
            set;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<Node<T>> GetEnumerator()
        {
            for (int i = 0; i < this.nodes.Count; i++)
            {
                yield return this.nodes[i];
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion


        public void PrintList(string FileName)
        {
            TextWriter sw = new StreamWriter(FileName, true);
            sw.WriteLine("{0}", this.Name);
            sw.Close();
            foreach (Node<T> n in this.nodes)
                if (this.nodes.IndexOf(n) == this.nodes.Count - 1)
                    n.PrintList(FileName);
                else
                    n.PrintList(FileName);
        }

        public static void PostOrden<T>(Node<T> t, ref List<T> ListT)
            where T : class, new()
        {
            foreach (Node<T> child in t.Nodes)
                // Recorre en postorden el hijo i-esimo
                PostOrden<T>(child, ref ListT);
            ListT.Add(t.Item);

        }

        public static void Preorder<T>(Node<T> t, ref List<T> ListT)
            where T : class, new()
        {
            ListT.Add(t.Item);
            foreach (Node<T> child in t.Nodes)
                //Recorrer en preorden el hijo i-esimo
                Preorder<T>(child, ref ListT);
        }


        public static T DepthFirstSearch<T>(Node<T> node, ref List<T> ListT)
        where T : class, new()
        {

            Stack<Node<T>> stack = new Stack<Node<T>>();
            stack.Push(node);
            while (stack.Count > 0)
            {
                Node<T> thisNode = stack.Pop();
#if DEBUG
                System.Diagnostics.Debug.WriteLine(thisNode.ToString());
#endif
                foreach (Node<T> Item in thisNode.Nodes)
                {
                    stack.Push(Item);
                    ListT.Add(Item.Item);
                }

            }
            return node.Item;
        }


        public void PrintNode(string prefix, string FileName, UtilETWeb.frmDependsCustom.EnumDifference EnumDifference)
        {
            TextWriter sw = new StreamWriter(FileName, true);           
            switch (EnumDifference)
            {
                case frmDependsCustom.EnumDifference.Dev:
                    if (this.Name.EndsWith("[x][-]"))
                        prefix += "✕";               
                    else
                        prefix += " ";//prefix += "✔";
                    break;
                case frmDependsCustom.EnumDifference.Pro:
                    if (this.Name.EndsWith("[-][x]"))
                        prefix += "✕";
                    else
                        prefix += " ";//prefix += "✔";
                    break;
                case frmDependsCustom.EnumDifference.All:
                    if (this.Name.EndsWith("[x][-]") || this.Name.EndsWith("[-][x]"))
                        prefix += "✕";
                    else
                        prefix += " ";//prefix += "✔";
                    break;
                default:
                    break;
            }



            sw.WriteLine("{0} + {1}", prefix, this.Name);
            //sw.WriteLine("{0}", this.Name);
            sw.Close();
            foreach (Node<T> n in this.nodes)
                if (this.nodes.IndexOf(n) == this.nodes.Count - 1)
                    n.PrintNode(prefix + "    ", FileName, EnumDifference);
                else
                    n.PrintNode(prefix + "   |", FileName, EnumDifference);
        }

        public void PrintNodeTreeView(TreeNode node, TreeView TreeView, UtilETWeb.frmDependsCustom.EnumDifference EnumDifference, ref int count)
        {         
            var nodesCollection = node != null
                ? node.Nodes
                : TreeView.Nodes;

            foreach (Node<T> n in this.nodes)
            {
                var newNode = nodesCollection.Add(n.Name);
                switch (EnumDifference)
                {
                    case frmDependsCustom.EnumDifference.Dev:
                        if (n.Name.EndsWith("[x][-]"))
                        {
                            newNode.BackColor = Color.Yellow;
                            count += 1;
                        }                        
                        break;
                    case frmDependsCustom.EnumDifference.Pro:
                        if (n.Name.EndsWith("[-][x]"))
                        {
                            newNode.BackColor = Color.YellowGreen;
                            count += 1;
                        }
                        break;
                    case frmDependsCustom.EnumDifference.All:
                        if (n.Name.EndsWith("[x][-]") || n.Name.EndsWith("[-][x]"))
                        {
                            newNode.BackColor = Color.Orange;
                            count += 1;
                        }
                        break;
                }
                newNode.ImageKey = n.Name.Split("|".ToCharArray())[0];
                newNode.SelectedImageKey = "arrow";
                n.PrintNodeTreeView(newNode, TreeView, EnumDifference,ref count);
            }
        }



        #region IEnumerable<Node<T>> Members

        IEnumerator<Node<T>> IEnumerable<Node<T>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public static class ExtensionsTreeNode
    {
        public static IEnumerable<T> ToIEnumarable<T>(this T item)
        {
            yield return item;
        }
    }
}
