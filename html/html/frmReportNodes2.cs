using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using HtmlAgilityPack;
using System.Xml;

namespace UtilETWeb
{
    public partial class frmReportNodes2 : Form
    {
        public frmReportNodes2()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmReportNodes_Load);
        }

        void frmReportNodes_Load(object sender, EventArgs e)
        {
            string html = File.ReadAllText(@"C:\Users\Consultorin\Documents\Visual Studio 2005\Projects\Report Project1\Report Project1\Resumen360ProgramaEval.rdl");

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(html);

            //Create an XmlNamespaceManager for resolving namespaces.
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("ab", "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition");
            nsmgr.AddNamespace("rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");


            //-- { table body
            string path = "//*[@Name='table1']//ab:Details//ab:TableCells//ab:Value[contains(.,'Col1')]/../ab:Style/ab:Format";
            XmlNode n_TableCellDetail = xdoc.DocumentElement.SelectSingleNode(path, nsmgr);

            for (int i = 0; i < 100; i++)
            {
                XmlNode n_Style = xdoc.DocumentElement.SelectSingleNode(string.Format("//*[@Name='table1']//ab:Details//ab:TableCells//ab:Value[contains(.,'Col{0}')]/../ab:Style", i + 1), nsmgr);

                XmlNode n_TableCellDetail_clone = n_TableCellDetail.Clone();                
                n_Style.PrependChild(n_TableCellDetail_clone);                
            }

            //remove template
            n_TableCellDetail.ParentNode.RemoveChild(n_TableCellDetail);

            //-- } table body

            xdoc.Save("Resumen360ProgramaEval.rdl");

        }
    }
}
