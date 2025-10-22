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
    public partial class frmReportNodes : Form
    {
        public frmReportNodes()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmReportNodes_Load);
        }

        void frmReportNodes_Load(object sender, EventArgs e)
        {
            string html = File.ReadAllText(@"C:\Users\Consultorin\Documents\Visual Studio 2005\Projects\Report Project1\Report Project1\Resumen360ProgramaEval.back1.rdl");

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(html);

            //Create an XmlNamespaceManager for resolving namespaces.
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("ab", "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition");
            nsmgr.AddNamespace("rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");

            //--- { Col1
            XmlNode n_Fields = xdoc.DocumentElement.SelectSingleNode("//*[@Name='Data']//ab:Fields[1]", nsmgr);
            XmlNode n_FielCol = xdoc.DocumentElement.SelectSingleNode("//*[@Name='Data']//ab:Fields//ab:Field[@Name='Col1'][1]", nsmgr);
            for (int i = 0; i < 100; i++)
            {
                XmlNode n_FielCol_clon = n_FielCol.Clone();
                n_FielCol_clon.Attributes["Name"].Value = string.Format("Col{0}", i + 1);

                XmlNode n_DatafieldCol = n_FielCol_clon.SelectSingleNode("./ab:DataField", nsmgr);
                n_DatafieldCol.InnerXml = n_DatafieldCol.InnerXml.Replace("Col1", string.Format("Col{0}", i + 1));

                n_Fields.AppendChild(n_FielCol_clon);
            }
            //--- } Col1

            //remove template
            n_FielCol.ParentNode.RemoveChild(n_FielCol);

            //-- { ColName2
            XmlNode n_FielColName = xdoc.DocumentElement.SelectSingleNode("//*[@Name='Data']//ab:Fields//ab:Field[@Name='ColName1'][1]", nsmgr);
            for (int i = 0; i < 100; i++)
            {
                XmlNode n_FielColName_clon = n_FielColName.Clone();
                n_FielColName_clon.Attributes["Name"].Value = string.Format("ColName{0}", i + 1);

                XmlNode n_DatafieldCol = n_FielColName_clon.SelectSingleNode("./ab:DataField", nsmgr);
                n_DatafieldCol.InnerXml = n_DatafieldCol.InnerXml.Replace("ColName1", string.Format("ColName{0}", i + 1));

                n_Fields.AppendChild(n_FielColName_clon);

            }

            //remove template
            n_FielColName.ParentNode.RemoveChild(n_FielColName);

            //-- } ColName2

            //-- { table head
            XmlNode n_Header = xdoc.DocumentElement.SelectSingleNode("//*[@Name='table1']//ab:Header//ab:TableRows//ab:TableRow//ab:TableCells", nsmgr);
            XmlNode n_TableCellHead = xdoc.DocumentElement.SelectSingleNode("//*[@Name='table1']//ab:Header//ab:TableCells//ab:Value[contains(.,'ColName1')]/../../..", nsmgr);
            for (int i = 0; i < 100; i++)
            {
                XmlNode n_TableCellHead_clon = n_TableCellHead.Clone();

                XmlNode n_Textbox = n_TableCellHead_clon.SelectSingleNode("./ab:ReportItems/ab:Textbox", nsmgr);
                n_Textbox.Attributes["Name"].Value = string.Format("txtColName{0}", i + 1);

                XmlNode n_Hidden = n_TableCellHead_clon.SelectSingleNode("./ab:ReportItems/ab:Textbox/ab:Visibility/ab:Hidden", nsmgr);
                n_Hidden.InnerXml = n_Hidden.InnerXml.Replace("ColName1", string.Format("ColName{0}", i + 1));

                XmlNode n_Value = n_TableCellHead_clon.SelectSingleNode("./ab:ReportItems/ab:Textbox/ab:Value", nsmgr);
                n_Value.InnerXml = n_Value.InnerXml.Replace("ColName1", string.Format("ColName{0}", i + 1));

                n_Header.AppendChild(n_TableCellHead_clon);
            }

            //remove template
            n_TableCellHead.ParentNode.RemoveChild(n_TableCellHead);
            //-- } table head

            //-- { table body
            XmlNode n_Details = xdoc.DocumentElement.SelectSingleNode("//*[@Name='table1']//ab:Details//ab:TableRows//ab:TableRow//ab:TableCells", nsmgr);
            XmlNode n_TableCellDetail = xdoc.DocumentElement.SelectSingleNode("//*[@Name='table1']//ab:Details//ab:TableCells//ab:Value[contains(.,'Col1')]/../../..", nsmgr);
            for (int i = 0; i < 100; i++)
            {
                XmlNode n_TableCellDetail_clon = n_TableCellDetail.Clone();

                XmlNode n_Textbox = n_TableCellDetail_clon.SelectSingleNode("./ab:ReportItems/ab:Textbox", nsmgr);
                n_Textbox.Attributes["Name"].Value = string.Format("txtCol{0}", i + 1);

                XmlNode n_Hidden = n_TableCellDetail_clon.SelectSingleNode("./ab:ReportItems/ab:Textbox/ab:Visibility/ab:Hidden", nsmgr);
                n_Hidden.InnerXml = n_Hidden.InnerXml.Replace("Col1", string.Format("Col{0}", i + 1));

                XmlNode n_Value = n_TableCellDetail_clon.SelectSingleNode("./ab:ReportItems/ab:Textbox/ab:Value", nsmgr);
                n_Value.InnerXml = n_Value.InnerXml.Replace("Col1", string.Format("Col{0}", i + 1));

                n_Details.AppendChild(n_TableCellDetail_clon);
            }

            //remove template
            n_TableCellDetail.ParentNode.RemoveChild(n_TableCellDetail);

            //-- } table body

            //-- { columns
            XmlNode n_TableColumns = xdoc.DocumentElement.SelectSingleNode("//*[@Name='table1']//ab:TableColumns",nsmgr);
            XmlNode n_TableColumn = xdoc.DocumentElement.SelectNodes("//*[@Name='table1']//ab:TableColumns//ab:TableColumn", nsmgr).Cast<XmlNode>().Last();
            for (int i = 0; i < 100; i++)
            {
                XmlNode n_TableColumn_clon = n_TableColumn.Clone();
                n_TableColumns.AppendChild(n_TableColumn_clon);
            }

            //remove template
            n_TableColumn.ParentNode.RemoveChild(n_TableColumn);
            //-- } columns


            xdoc.Save("Resumen360ProgramaEval.rdl");


            /*
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.OptionOutputOriginalCase = true;
            doc.LoadHtml(html);

            HtmlNode n_Fields = doc.DocumentNode.SelectSingleNode("//*[@name='Data']//fields");
            HtmlNode n_FielCol = doc.DocumentNode.SelectSingleNode("//*[@name='Data']//fields//field[@name='Col1']");            
            for (int i = 0; i < 100; i++)
            {                
                HtmlNode n_FielCol_clon = n_FielCol.Clone();                                
                n_FielCol_clon.SetAttributeValue("name", string.Format("Col{0}", i + 1));

                HtmlNode n_DatafieldCol = n_FielCol_clon.SelectSingleNode("./datafield");
                n_DatafieldCol.InnerHtml = n_DatafieldCol.InnerHtml.Replace("Col1", string.Format("Col{0}", i + 1));

                n_Fields.AppendChild(n_FielCol_clon);                
            }
            
            //remove template
            n_FielCol.Remove();


            HtmlNode n_FielColName = doc.DocumentNode.SelectSingleNode("//*[@name='Data']//fields//field[@name='ColName1']");
            for (int i = 0; i < 100; i++)
            {                
                HtmlNode n_FielColName_clon = n_FielColName.Clone();
                n_FielColName_clon.SetAttributeValue("name", string.Format("ColName{0}", i + 1));
                              
                HtmlNode n_DatafieldColName = n_FielColName_clon.SelectSingleNode("./datafield");
                n_DatafieldColName.InnerHtml = n_DatafieldColName.InnerHtml.Replace("ColName1", string.Format("ColName{0}", i + 1));
                
                n_Fields.AppendChild(n_FielColName_clon);
            }

            //remove template
            n_FielColName.Remove();

            HtmlNode n_Header = doc.DocumentNode.SelectSingleNode("//*[@name='table1']//header");
            HtmlNode n_TableCellHead = doc.DocumentNode.SelectSingleNode("//*[@name='table1']//header//tablecells//value[contains(.,'ColName1')]/../../..");
            for (int i = 0; i < 100; i++)
            {
                HtmlNode n_TableCellHead_clon = n_TableCellHead.Clone();

                HtmlNode n_Textbox = n_TableCellHead_clon.SelectSingleNode("./reportitems/textbox");
                n_Textbox.SetAttributeValue("name", string.Format("txtColName{0}", i + 1));

                HtmlNode n_Hidden = n_TableCellHead_clon.SelectSingleNode("./reportitems/textbox/visibility/hidden");
                n_Hidden.InnerHtml = n_Hidden.InnerHtml.Replace("ColName1", string.Format("Col{0}", i + 1));

                HtmlNode n_Value = n_TableCellHead_clon.SelectSingleNode("./reportitems/textbox/value");
                n_Value.InnerHtml = n_Value.InnerHtml.Replace("ColName1", string.Format("Col{0}", i + 1));

                n_Header.AppendChild(n_TableCellHead_clon);
            }

            //remove template
            n_TableCellHead.Remove();


            HtmlNode n_Details = doc.DocumentNode.SelectSingleNode("//*[@name='table1']//details");
            HtmlNode n_TableCell = doc.DocumentNode.SelectSingleNode("//*[@name='table1']//details//tablecells//value[contains(.,'Col1')]/../../..");
            for (int i = 0; i < 100; i++)
            {
                HtmlNode n_TableCell_clon = n_TableCell.Clone();

                HtmlNode n_Textbox = n_TableCell_clon.SelectSingleNode("./reportitems/textbox");
                n_Textbox.SetAttributeValue("name", string.Format("txtCol{0}", i + 1));

                HtmlNode n_Hidden = n_TableCell_clon.SelectSingleNode("./reportitems/textbox/visibility/hidden");
                n_Hidden.InnerHtml = n_Hidden.InnerHtml.Replace("Col1", string.Format("Col{0}", i + 1));

                HtmlNode n_Value = n_TableCell_clon.SelectSingleNode("./reportitems/textbox/value");
                n_Value.InnerHtml = n_Value.InnerHtml.Replace("Col1", string.Format("Col{0}", i + 1));

                n_Details.AppendChild(n_TableCell_clon);
            }

            //remove template
            n_TableCell.Remove();


            HtmlNode n_TableColumns = doc.DocumentNode.SelectSingleNode("//*[@name='table1']//tablecolumns");
            HtmlNode n_TableColumn = doc.DocumentNode.SelectNodes("//*[@name='table1']//tablecolumns//tablecolumn").Last();
            for (int i = 0; i < 100; i++)
            {
                HtmlNode n_TableColumn_clon = n_TableColumn.Clone();
                n_TableColumns.AppendChild(n_TableColumn_clon);
            }

            //remove template
            n_TableColumn.Remove();


            doc.Save("Resumen360ProgramaEval.rdl");

            MessageBox.Show("termino");
            */

        }
    }
}
