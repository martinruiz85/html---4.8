using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;

namespace UtilETWeb
{
    class ButtonFont : Button
    {

        public string Icon { get; set; }

        public ButtonFont()
        {
            this.Icon = "\uf0f6";
            this.Paint += new PaintEventHandler(ButtonFont_Paint);            
        }

        void ButtonFont_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            //PrivateFontCollection collection = new PrivateFontCollection();

            //string result = System.Text.Encoding.UTF8.GetString(Properties.Resources.fontawesome_webfont);

            //collection.AddFontFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Resources\fontawesome-webfont.ttf"));
            //FontFamily fontFamily = new FontFamily("FontAwesome", collection);


            //float h = 12;

            //Font font = new Font(fontFamily, h);


            //define a private font collection
            System.Drawing.Text.PrivateFontCollection pfc = new System.Drawing.Text.PrivateFontCollection();
            //read your resource font into a byte array
            byte[] Bytes = Properties.Resources.fontawesome_webfont;
            //allocate some memory and get a pointer to it
            IntPtr ptr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(Bytes.Length);
            //copy the font data byte array to memory
            System.Runtime.InteropServices.Marshal.Copy(Bytes, 0, ptr, Bytes.Length);
            //Add the font to the private font collection
            pfc.AddMemoryFont(ptr, Bytes.Length);
            //free up the previously allocated memory
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(ptr);
            //define a font from the private font collection
            System.Drawing.Font fnt = new Font(pfc.Families[0], 12, System.Drawing.FontStyle.Regular, GraphicsUnit.Point);
            //dispose of the private font collection
            pfc.Dispose();
            //return the font created from your font resource     

            SizeF size = g.MeasureString(this.Icon, fnt);

            //http://astronautweb.co/snippet/font-awesome/
            g.DrawString(this.Icon, fnt, Brushes.Black, (Bounds.Width - size.Width) / 2f, (Bounds.Height - size.Height) / 2f);
            
        }
    }
}
