using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Trestan
{
    class TestTextBox:TextBox
    {
        private const int WM_PAINT = 0x000F;
        Image backgroundImage = null;
        public TestTextBox()
        {
            #region Test Case 2:
            //SetStyle(ControlStyles.UserPaint, true);  // Uncomment to trigger OnPaint.
            #endregion 

            #region Test Case 1:Uncomment to see the effect.
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //BackColor = Color.Transparent;  //Once you start to type, the background return to white.
            #endregion
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PAINT)
            {
                if (backgroundImage == null)
                {
                    backgroundImage = this.Parent.BackgroundImage;
                }
                if (backgroundImage != null)
                {
                    #region Test Case 3:
                    //Graphics g = Graphics.FromHwnd(this.Handle);
                    //g.DrawImage(backgroundImage, 0, 0);    //Uncomment to view the effect.
                    //return;
                    #endregion
                }
            }
            base.WndProc(ref m);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (backgroundImage == null)
            {
                backgroundImage = this.Parent.BackgroundImage; //Pretended that you have captured the background as image.
            }
            if (backgroundImage != null)
            {
                #region Test Case 2: 
                // e.Graphics.DrawImage(backgroundImage, 0,0);   //Uncomment to view the effect.
                #endregion
            }
            base.OnPaint(e);
        }
    }

    class Utility
    {
        public delegate bool EnumChildWindowsProc(IntPtr hwnd, uint lParam);
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        #region Dll Import

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "EnumChildWindows")]
        public static extern bool EnumChildWindows(IntPtr hWndParent, EnumChildWindowsProc lpEnumFunc, int lParam);

        [DllImport("user32.dll", EntryPoint = "GetClassName")]
        public static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("User32.dll", EntryPoint = "GetWindowText")]
        public static extern Int32 GetWindowText(IntPtr hWnd, StringBuilder s, int nMaxCount);

        #endregion


        static public void GetChildWindows(IntPtr hWndParent)
        {
            EnumChildWindowsProc myEnumChild = new EnumChildWindowsProc(Utility.EnumChildGetValue);
            try
            {
                bool result = EnumChildWindows(hWndParent, myEnumChild, 0);
            }
            catch (Exception ex)
            {
            }
        }
        static public bool EnumChildGetValue(IntPtr hWnd, uint lParam)
        {
            StringBuilder formDetails = new StringBuilder(256);
            int txtValue;
            string editText = "";
            txtValue = GetWindowText(hWnd, formDetails, 256);
            editText = formDetails.ToString().Trim();
            MessageBox.Show("Contains text of control:" + editText);
            return true;
        }
    }
}
