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
    public partial class frmCalendarP2P : Form
    {
        public frmCalendarP2P()
        {
            InitializeComponent();
        }

        //https://stackoverflow.com/questions/1847580/how-do-i-loop-through-a-date-range
        private void frmCalendarP2P_Load(object sender, EventArgs e)
        {

            DateTime myVacation1 = new DateTime(2019, 1, 10);
            DateTime myVacation2 = new DateTime(2019, 1, 17);

            monthCalendar1.AddBoldedDate(myVacation1);
            monthCalendar1.AddBoldedDate(myVacation2);
            monthCalendar1.UpdateBoldedDates();

            monthCalendar1.ShowWeekNumbers = true;
        }
    }

    public class MyCalendar : MonthCalendar 
    {
        public MyCalendar() 
        {
        }
    }
}
