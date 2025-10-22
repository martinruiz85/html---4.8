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
    public partial class frmDiccionary : Form
    {
        public frmDiccionary()
        {
            InitializeComponent();
        }

        private void frmDiccionary_Load(object sender, EventArgs e)
        {

            Dictionary<string, string> EmployeeList = new Dictionary<string, string>();

            EmployeeList.Add("Mahesh Chand", "Programmer");
            EmployeeList.Add("Praveen Kumar", "Project Manager");
            EmployeeList.Add("Raj Kumar", "Architect");
            EmployeeList.Add("Nipun Tomar", "Asst. Project Manager");
            EmployeeList.Add("Dinesh Beniwal", "Manager");

            string value = EmployeeList["Mahesh Chand"];

        }
    }
}
