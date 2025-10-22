using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using System.IO;
using System.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;

namespace UtilETWeb
{
    public partial class frmGenerateInsertsCustom : Form
    {
        public frmGenerateInsertsCustom()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmGenerateInsertsCustom_Load);
        }

        private List<UtilETWeb.MyConfigSection.MyConfigInstanceElement> GetConnexions()
        {

            List<UtilETWeb.MyConfigSection.MyConfigInstanceElement> l = new List<MyConfigSection.MyConfigInstanceElement>();
            MyConfigSection config = ConfigurationManager.GetSection("MyConnection") as MyConfigSection;
            l = config.Instances.OfType<UtilETWeb.MyConfigSection.MyConfigInstanceElement>().ToList();

            UtilETWeb.MyConfigSection.MyConfigInstanceElement item = new UtilETWeb.MyConfigSection.MyConfigInstanceElement();
            item.Name = "(sin especificar)";
            item.Code = "-1";
            l.Insert(0, item);

            return l;
        }

        delegate object GetTextCallback(ComboBox cmb);

        private object GetText(ComboBox cmb)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (cmb.InvokeRequired)
            {
                GetTextCallback d = new GetTextCallback(GetText);
                return this.Invoke(d, new object[] { cmb });
            }
            else
            {
                return cmb.SelectedValue;
            }
        }

        private string ConnectionString
        {
            get
            {
                if (GetText(this.cmbDatabase).ToString().Equals("-1"))
                {
                    return System.Configuration.ConfigurationManager.AppSettings["ConnectionStringPRO"];
                }
                else
                    return GetText(this.cmbDatabase).ToString();
            }
        }



        void frmGenerateInsertsCustom_Load(object sender, EventArgs e)
        {

            this.cmbDatabase.DataSource = GetConnexions();
            this.cmbDatabase.ValueMember = "Code";
            this.cmbDatabase.DisplayMember = "Name";
            this.cmbDatabase.SelectedValue = "-1";



            //string dbName = "DATABASENAME";
            string dbName = this.ConnectionString;
            string outputfile = @"c:\temp\output.sql";

            SqlConnection sqlConn = new SqlConnection(this.ConnectionString);
            sqlConn.Open();
            ServerConnection srvConn = new ServerConnection(sqlConn);

            //Server srv = new Server(@"(local)\INSTANCE");
            Server srv = new Server(srvConn);
            //srv.ConnectionContext.LoginSecure = true;

            Database db = new Database();
            //db = srv.Databases[dbName];
            db = srv.Databases[srvConn.DatabaseName];

            Scripter scr = new Scripter(srv);
            srv.SetDefaultInitFields(typeof(Microsoft.SqlServer.Management.Smo.View), "IsSystemObject");

            ScriptingOptions options = new ScriptingOptions();
            options.DriAll = true;
            options.ClusteredIndexes = true;
            options.Default = true;
            options.DriAll = true;
            options.Indexes = true;
            options.IncludeHeaders = true;
            options.AppendToFile = false;
            options.FileName = outputfile;
            options.ToFileOnly = true;
            scr.Options = options;

            Table[] tbls = new Table[db.Tables.Count];
            db.Tables.CopyTo(tbls, 0);
            scr.Script(tbls);

            options.AppendToFile = true;
            Microsoft.SqlServer.Management.Smo.View[] view = new Microsoft.SqlServer.Management.Smo.View[1];
            for (int idx = 0; idx < db.Views.Count; idx++)
            {
                if (!db.Views[idx].IsSystemObject)
                {
                    view[0] = db.Views[idx];
                    scr.Script(view);
                }
            }

            DependencyTree tree = scr.DiscoverDependencies(tbls, true);
            DependencyWalker depwalker = new Microsoft.SqlServer.Management.Smo.DependencyWalker();
            DependencyCollection depcoll = depwalker.WalkDependencies(tree);

            StreamWriter sw = new StreamWriter(outputfile, true, Encoding.Unicode);

            StringBuilder sb = new StringBuilder();
            foreach (DependencyCollectionNode dep in depcoll)
            {
                if (db.Tables[dep.Urn.GetAttribute("Name")].IsSystemObject)
                    continue;

                sb.AppendFormat("EXEC sp_generate_inserts @table_name='{0}', @owner='dbo'{1}", dep.Urn.GetAttribute("Name"), Environment.NewLine);
            }

            DataSet ds = new DataSet();
            ds = db.ExecuteWithResults(sb.ToString());
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sw.WriteLine(dr[0].ToString());
                }
            }
            sw.Close();
        }
    }
}
