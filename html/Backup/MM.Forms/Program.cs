using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace MM.Forms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /*
            string names = "";
            string[] directorys = Directory.GetDirectories(@"Y:\Trabajo\2020\062 STX 4N");
            for (int i = 0; i < directorys.Length; i++)
            {
                names = names + directorys[i] + "\n";
            }



            var x = System.Drawing.Image.FromStream(new System.IO.MemoryStream(new System.Net.WebClient().DownloadData("http://vxmtymxintsch/ETWeb114/DataPages/OrgChartImgExporter.ashx?PosId=9783")));
            var y = System.Drawing.Image.FromStream(new System.IO.MemoryStream(new System.Net.WebClient().DownloadData("http://vxmtymxintsch/ETWeb114/DataPages/OrgChartImgExporter.ashx?PosId=9783"))).Width;
             */

            //Application.Run(new Form1());
            Application.Run(new frmGifMM());
            //Application.Run(new frmGifMaster());
        }
    }
}
