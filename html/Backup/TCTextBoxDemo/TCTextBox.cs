using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Trestan
{
    class TCTextBox : Control
    {
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                Modified = true;
                lineHeight = Font.Height;
            }
        }

        private ContextMenuStrip contextMenuStrip1;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem toolStripMenuItemCut;
        private ToolStripMenuItem toolStripMenuItemCopy;
        private ToolStripMenuItem toolStripMenuItemPaste;
        private ToolStripMenuItem toolStripMenuItemDel;

        int startY = 0;
        int lineHeight;
        int maxHeight = 0;
        int selectionStart = -1;
        public int SelectionStart
        {
            get { return selectionStart; }
            set { selectionStart = value; }
        }

        int selectionLength = 0;
        public int SelectionLength
        {
            get { return selectionLength; }
            set { selectionLength = value; }
        }

        string selectText;
        public string SelectText
        {
            get
            {
                selectText = "";
                if (SelectionLength > 0)
                {
                    selectText = Text.Substring(SelectionStart, SelectionLength);
                }
                return selectText;
            }
            set { selectText = value; }
        }


        [DllImport("user32.dll")]
        static extern bool CreateCaret(IntPtr hWnd, IntPtr hBitmap, int nWidth, int nHeight);
        [DllImport("user32.dll")]
        public static extern bool SetCaretPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern bool GetCaretPos(ref System.Drawing.Point lpPoint);
        [DllImport("user32.dll")]
        static extern bool ShowCaret(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool DestroyCaret();

        int charIndex = 0;
        public int CharIndex
        {
            get { return charIndex; }
            set
            {
                charIndex = value;
                if (Selecting)
                {
                    if (SelectionStart == -1)
                    {
                        SelectionStart = charIndex;
                    }
                    SelectionLength = value - SelectionStart;
                }
                else
                {
                    SelectionStart = -1;
                    SelectionLength = 0;
                }
            }
        }

        bool selecting = false;
        public bool Selecting
        {
            get { return selecting; }
            set
            {
                if (selecting == false && value == true)
                {
                    SelectionStart = -1;
                    SelectionLength = 0;
                }
                if (!value)
                {
                    NormalizeStartEnd();
                }
                selecting = value;
            }
        }

        int lineIndex = 0;
        Keys input = Keys.F24;
        bool DialogChar = false;
        Point mousePos = Point.Empty;
        bool Modified = false;
        List<int> lines = new List<int>();
        const int maxLength = 32;

        public TCTextBox()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ImeMode = System.Windows.Forms.ImeMode.On;
        }

        protected override void OnGotFocus(EventArgs e)
        {
            lineHeight = Font.Height;
            CreateCaret(this.Handle, new IntPtr(0), 1, lineHeight);
            ShowCaret(this.Handle);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (input == Keys.Left || input == Keys.Right || input == Keys.Up || input == Keys.Down || Selecting == true)
            {
                this.Focus();
            }
            else
            {
                DestroyCaret();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseDown(e);
            if (Text.Length == 0)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                mousePos = e.Location;
                Selecting = true;
                SelectionLength = 0;
            }
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mousePos = e.Location;
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Selecting = false;
            }
            base.OnMouseUp(e);
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            input = e.KeyCode;
            Selecting = e.Shift;
            if (Selecting && SelectionStart == -1)
            {
                SelectionStart = CharIndex;
            }
            base.OnPreviewKeyDown(e);
            Invalidate();
        }

        void SetCaretAtPosX(Graphics g, int posX)
        {
            if (lineIndex == 1 && (Text[0] == '\r' || posX < 3))
            {
                CharIndex = 0;
                SetCaretPos(1, 0);
            }
            else if (lineIndex != 1 && lines[lineIndex - 1] + 1 < Text.Length && Text[lines[lineIndex - 1] + 1] == '\r')
            {
                CharIndex = lines[lineIndex - 1] + 1;
                SetCaretPos(1, (lineIndex - 1) * lineHeight + startY);
            }
            else
            {
                int i = lines[lineIndex - 1] + 1;
                for (; i <= lines[lineIndex]; i++)
                {
                    if (Text[i] == '\r')
                    {
                        CharIndex = i;
                        SetCaretAtNormalChar(g, false);
                        break;
                    }
                    else
                    {
                        CharacterRange[] charRanges = new CharacterRange[] { new CharacterRange(i, 1) };
                        StringFormat stringFormat1 = new StringFormat();
                        stringFormat1.SetMeasurableCharacterRanges(charRanges);
                        stringFormat1.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                        Region[] charRegion = g.MeasureCharacterRanges(Text, Font, new RectangleF(0, startY, this.Width, maxHeight), stringFormat1);

                        RectangleF theRect = charRegion[0].GetBounds(g);
                        if (posX <= theRect.X + theRect.Width/2)
                        {
                            CharIndex = i;
                            SetCaretPos((int)theRect.Left, (int)theRect.Y);
                            break;
                        }
                    }
                }
                if (i > lines[lineIndex])
                {
                    CharIndex = i;
                    SetCaretAtNormalChar(g, false);
                }
            }
        }

        private void SetCaretPosAtMouse(Graphics g)
        {
            if (Text.Length < 3 || mousePos == Point.Empty)
            {
                return;
            }

            CalculateLines(g);
            lineIndex = (mousePos.Y - startY) / lineHeight + 1;
            if (lineIndex < 1)
            {
                lineIndex = 1;
            }
            if (lineIndex > lines.Count - 1)
            {
                SetCaretAtEnd(g);
            }
            else
            {
                SetCaretAtPosX(g, mousePos.X);
            }
            mousePos = Point.Empty;
        }

        void NormalizeStartEnd()
        {
            if (SelectionLength < 0)
            {
                selectionStart = selectionStart + SelectionLength;
                SelectionLength = 0 - SelectionLength;
            }
        }

        void DrawSelection(Graphics g)
        {
            if (SelectionStart == -1 || SelectionLength == 0)
            {
                return;
            }
            int tempLength = SelectionLength;
            int tempStart = selectionStart;
            if (SelectionLength < 0)
            {
                tempStart = selectionStart + SelectionLength;
                tempLength = 0 - SelectionLength;
            }
            Region[] charRegion = null;
            int count = tempLength / maxLength;
            CharacterRange[] charRanges = new CharacterRange[count + 1];
            charRanges[0] = new CharacterRange(tempStart, tempLength - count * maxLength);
            for (int i = 1; i < count + 1; i++)
            {
                charRanges[i] = new CharacterRange(charRanges[i - 1].First + charRanges[i - 1].Length, maxLength);
            }
            StringFormat stringFormat1 = new StringFormat();
            stringFormat1.SetMeasurableCharacterRanges(charRanges);
            stringFormat1.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
            charRegion = g.MeasureCharacterRanges(Text, Font, new RectangleF(0, startY, this.Width, maxHeight), stringFormat1);
            foreach (Region one in charRegion)
            {
                g.FillRegion(Brushes.LightBlue, one);
            }
        }

        private void CalculateCaretPos(Graphics g, Keys input)
        {
            if (input == Keys.F24)
            {
                return;
            }
            if (Text.Length == 0)
            {
                SetCaretPos(1, 1);
                return;
            }

            bool done = false;
            bool afterReturn = false;
            Point oldPos = Point.Empty;

            switch (input)
            {
                case Keys.Back:
                    Modified = true;
                    AutoScrollDown();
                    if (SelectionLength > 0)
                    {
                        Text = Text.Remove(SelectionStart, SelectionLength);
                        CharIndex = SelectionStart;
                        done = false;
                        Selecting = false;
                        break;
                    }
                    Selecting = false;
                    --CharIndex;
                    if (CharIndex > 0)
                    {
                        if (Text[CharIndex] == '\n')
                        {
                            Text = Text.Remove(--CharIndex, 2);
                            if (CharIndex <= 0)
                            {
                                SetCaretPos(1, 0);
                                CharIndex = 0;
                                done = true;
                            }
                            else if (Text[CharIndex - 1] == '\n')
                            {
                                GetCaretPos(ref oldPos);
                                SetCaretPos(1, oldPos.Y - lineHeight);
                                done = true;
                            }
                            else
                            {
                                done = false;
                            }
                        }
                        else
                        {
                            Text = Text.Remove(CharIndex, 1);
                            if (Text[CharIndex - 1] == '\n')
                            {
                                GetCaretPos(ref oldPos);
                                SetCaretPos(1, oldPos.Y);
                                done = true;
                            }
                            else
                            {
                                done = false;
                            }
                        }
                    }
                    else if (CharIndex == 0)
                    {
                        Text = Text.Remove(CharIndex, 1);
                        SetCaretPos(1, 0);
                        done = true;
                    }
                    else
                    {
                        CharIndex = 0;
                        lineIndex = 1;
                        done = true;
                    }
                    break;

                case Keys.Delete:
                    Modified = true;
                    if (SelectionLength > 0)
                    {
                        Text = Text.Remove(SelectionStart, SelectionLength);
                        CharIndex = SelectionStart;
                        done = false;
                        Selecting = false;
                        break;
                    }
                    if (CharIndex == Text.Length)
                    {
                        done = true;
                        break;
                    }
                    if (Text[CharIndex] == '\r')
                    {
                        Text = Text.Remove(CharIndex, 2);
                    }
                    else
                    {
                        Text = Text.Remove(CharIndex, 1);
                    }
                    done = true;
                    break;

                case Keys.Enter:
                    Modified = true;
                    AutoScrollUp();
                    GetCaretPos(ref oldPos);
                    SetCaretPos(1, oldPos.Y + lineHeight);
                    done = true;
                    break;

                case Keys.Right:
                    Modified = true;
                    if (++CharIndex > Text.Length)
                    {
                        CharIndex = Text.Length;
                        done = true;
                        break;
                    }
                    AutoScrollUp();

                    if (CharIndex < Text.Length && (Text[CharIndex - 1] == '\r' || Text[CharIndex - 1] == '\n'))
                    {
                        if (Text[CharIndex - 1] == '\r')
                        {
                            ++CharIndex;
                        }
                        GetCaretPos(ref oldPos);
                        SetCaretPos(1, oldPos.Y + lineHeight);
                        done = true;
                    }
                    else
                    {
                        done = false; 
                    }
                    break;
                case Keys.Left:
                    Modified = true;
                    AutoScrollDown();
                    if (--CharIndex <= 0)
                    {
                        CharIndex = 0; 
                        lineIndex = 1;
                        SetCaretPos(1,0);
                        done = true;
                        break;
                    }

                    if (Text[CharIndex - 1] != '\n' && Text[CharIndex - 1] != '\r')
                    {
                        done = false;
                        break; 
                    }

                    if (Text[CharIndex - 1] == '\n')
                    {
                        afterReturn = true;
                        done = false;
                        break;
                    }

                    if (Text[CharIndex] == '\n')
                    {
                        --CharIndex;
                        if (CharIndex <= 0)
                        {
                            SetCaretPos(1, 0);
                            CharIndex = 0;
                            done = true;
                            break;
                        }
                        else if (Text[CharIndex - 1] == '\n')
                        {
                            GetCaretPos(ref oldPos);
                            SetCaretPos(1, oldPos.Y - lineHeight);
                            done = true;
                            break;
                        }
                        else
                        {
                            done = false;
                            break;
                        }
                    }
                    break;
               case Keys.Up:
                    CalculateLines(g);
                    AutoScrollDown();
                    if (lineIndex > 1)
                    {
                        lineIndex--;
                        GetCaretPos(ref oldPos);
                        SetCaretAtPosX(g, oldPos.X);
                    }
                    done = true;
                    break;
                case Keys.Down:
                    CalculateLines(g);
                    AutoScrollUp();
                    if (lineIndex < lines.Count)
                    {
                        lineIndex++;
                        if (lineIndex == lines.Count)
                        {
                            lineIndex = lines.Count - 1;
                            SetCaretAtEnd(g);
                        }
                        else
                        {
                            oldPos = Point.Empty;
                            GetCaretPos(ref oldPos);
                            SetCaretAtPosX(g, oldPos.X);
                        }
                    }
                    done = true;
                    break;
                case Keys.Home:
                    CharIndex = 0;
                    startY = 0;
                    lineIndex = 1;
                    SetCaretPos(1, 1);
                    done = true;
                    break;
                case Keys.End:
                    Modified = true;
                    CalculateLines(g);
                    SetCaretAtEnd(g);
                    done = true;
                    break;
                default:
                    if (DialogChar)
                    {
                        DialogChar = false;
                        done = false;
                    }
                    break;
            }
            if (!done)
            {
                SetCaretAtNormalChar(g, afterReturn);
            }
            input = Keys.F24;
        }

        void SetCaretAtEnd(Graphics g)
        {
            CharIndex = Text.Length;
            CalculateLines(g);
            if (Text[Text.Length - 1] == '\n')
            {
                lineIndex = lines.Count;
                SetCaretPos(1, (lineIndex - 1) * lineHeight);
            }
            else
            {
                lineIndex = lines.Count - 1;
                SetCaretAtNormalChar(g, false);
            }
        }

        void SetCaretAtNormalChar(Graphics g, bool afterReturn)
        {
            if (CharIndex == 0)
            {
                SetCaretPos(1,0);
                return;
            }

            if (maxHeight == 0)
            {
                maxHeight = this.Height;
            }
            CharacterRange[] charRanges;
            StringFormat stringFormat1;
            Region[] charRegion;
            RectangleF theRect;

            if (afterReturn)
            {
                charRanges = new CharacterRange[] { new CharacterRange(CharIndex, 1) };
                stringFormat1 = new StringFormat();
                stringFormat1.SetMeasurableCharacterRanges(charRanges);
                stringFormat1.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                charRegion = g.MeasureCharacterRanges(Text, Font, new RectangleF(0, startY, this.Width, maxHeight), stringFormat1);

                theRect = charRegion[0].GetBounds(g);
                SetCaretPos((int)theRect.Left, (int)theRect.Top);
            }
            else
            {
                char temp = Text[CharIndex - 1];
                charRanges = new CharacterRange[] { new CharacterRange(CharIndex - 1, 1) };
                stringFormat1 = new StringFormat();
                stringFormat1.SetMeasurableCharacterRanges(charRanges);
                stringFormat1.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                charRegion = g.MeasureCharacterRanges(Text, Font, new RectangleF(0, startY, this.Width, maxHeight), stringFormat1);

                theRect = charRegion[0].GetBounds(g);
                SetCaretPos((int)theRect.Right, (int)theRect.Top);
            }
        }
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Delete:
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.Focused)
            {
                CalculateCaretPos(e.Graphics, input);
                input = Keys.F24;
                SetCaretPosAtMouse(e.Graphics);

            }
            else
            {
                HideCaret(this.Handle);
                input = Keys.F24;
            }
            DrawSelection(e.Graphics);
            e.Graphics.DrawString(Text, Font, new SolidBrush(this.ForeColor),
                new RectangleF(0, startY, this.Width, this.Height - startY), StringFormat.GenericDefault);
            e.Graphics.DrawRectangle(Pens.Gray, 0, 0, this.Width, this.Height);
        }

        protected override bool ProcessDialogChar(char charCode)
        {
            if ( charCode == '\b' || charCode == (char)26 || charCode == (char)25)
            {
                return false;
            }

            Modified = true;
            AutoScrollUp();
            if (SelectionLength > 0)
            {
                Text = Text.Remove(SelectionStart, SelectionLength);
                CharIndex = SelectionStart;
            }
            Selecting = false;
            if (charCode == '\r')
            {
                Text = Text.Insert(CharIndex, Environment.NewLine);
                CharIndex += Environment.NewLine.Length;
                return true;
            }
            if (CharIndex == Text.Length)
            {
                base.Text += charCode;
                CharIndex++;
            }
            else
            {
                Text = Text.Insert(CharIndex++, charCode.ToString());
            }
            DialogChar = true;
            this.Invalidate();

            return true;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            Modified = true;

            base.OnSizeChanged(e);
        }

        private void CalculateLines(Graphics g)
        {
            if (!Modified)
            {
                return;
            }

            lines.Clear();
            lines.Add(0);
            CharacterRange[] charRanges;
            StringFormat stringFormat1 = new StringFormat(); ;
            stringFormat1.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
            RectangleF rectTextbox = new RectangleF(0, startY, this.Width, this.Height - startY + lineHeight);
            Region[] charRegion;
            RectangleF theRect;

            int i = 0;
            for (; i < Text.Length; i++)
            {
                //carriage return
                if (Text[i] == '\r')
                {
                    lines.Add(++i);
                    continue;
                }

                charRanges = new CharacterRange[] { new CharacterRange(i, 1) };
                stringFormat1.SetMeasurableCharacterRanges(charRanges);
                charRegion = g.MeasureCharacterRanges(Text, Font, rectTextbox, stringFormat1);
                theRect = charRegion[0].GetBounds(g);
                if (input==Keys.End&&theRect.Height == 0 &&Text[i] != ' ')
                {
                    rectTextbox.Height += lineHeight;
                    i--;
                    continue;
                }
                if (theRect.Y > lineHeight * lines.Count + startY - theRect.Height / 2)
                {
                    lines.Add(i - 1);
                }
            }

            if (rectTextbox.Height > maxHeight)
            {
                maxHeight = (int)rectTextbox.Height;
            }
            if (i == Text.Length && lines[lines.Count - 1] != Text.Length - 1)
            {
                lines.Add(Text.Length - 1);
            }

            Point oldPos = Point.Empty;
            GetCaretPos(ref oldPos);
            lineIndex = (int)((oldPos.Y - startY) / (float)lineHeight + 1.5);
            if (lineIndex < 1)
            {
                lineIndex = 1;
                startY = 0;
            }
            if (CharIndex == Text.Length || lineIndex >= lines.Count)
            {
                if (Text[Text.Length - 1] == '\n')
                {
                    lineIndex = lines.Count;
                }
                else
                {
                    lineIndex = lines.Count - 1;
                }
            }
            if (input == Keys.End)
            {
                startY = this.Height - lineHeight * lines.Count;
                if (startY > 0)
                {
                    startY = 0;
                }
            }
            Modified = false;
        }

        void AutoScrollUp()
        {
            Point oldPos = Point.Empty;
            GetCaretPos(ref oldPos);
            if (oldPos.Y + lineHeight > this.Height && maxHeight + startY > 0)
            {
                Modified = true;
                startY -= lineHeight;
            }
        }

        void AutoScrollDown()
        {
            Point oldPos = Point.Empty;
            GetCaretPos(ref oldPos);
            if (oldPos.Y < lineHeight && CharIndex > 0)
            {
                startY += lineHeight;
            }
            if (lineIndex <= 2)
            {
                startY = 0;
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDel = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemCut,
            this.toolStripMenuItemCopy,
            this.toolStripMenuItemPaste,
            this.toolStripMenuItemDel});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(148, 92);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // toolStripMenuItemCut
            // 
            this.toolStripMenuItemCut.Name = "toolStripMenuItemCut";
            this.toolStripMenuItemCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemCut.Size = new System.Drawing.Size(147, 22);
            this.toolStripMenuItemCut.Text = "Cu&t";
            this.toolStripMenuItemCut.Click +=new EventHandler(toolStripMenuItemCut_Click);
            // 
            // toolStripMenuItemCopy
            // 
            this.toolStripMenuItemCopy.Name = "toolStripMenuItemCopy";
            this.toolStripMenuItemCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemCopy.Size = new System.Drawing.Size(147, 22);
            this.toolStripMenuItemCopy.Text = "&Copy";
            this.toolStripMenuItemCopy.Click +=new EventHandler(toolStripMenuItemCopy_Click);
            // 
            // toolStripMenuItemPaste
            // 
            this.toolStripMenuItemPaste.Name = "toolStripMenuItemPaste";
            this.toolStripMenuItemPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.toolStripMenuItemPaste.Size = new System.Drawing.Size(147, 22);
            this.toolStripMenuItemPaste.Text = "&Paste";
            this.toolStripMenuItemPaste.Click +=new EventHandler(toolStripMenuItemPaste_Click);
            // 
            // toolStripMenuItemDel
            // 
            this.toolStripMenuItemDel.Name = "toolStripMenuItemDel";
            this.toolStripMenuItemDel.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolStripMenuItemDel.Size = new System.Drawing.Size(147, 22);
            this.toolStripMenuItemDel.Text = "Delete";
            this.toolStripMenuItemDel.Click +=new EventHandler(toolStripMenuItemDel_Click);
            // 
            // TCTextBox
            // 
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            this.components.Dispose();
            base.Dispose(disposing);
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SelectionLength == 0)
            {
                this.toolStripMenuItemCut.Enabled = false;
                this.toolStripMenuItemCopy.Enabled = false;
                this.toolStripMenuItemDel.Enabled = false;
            }
            else
            {
                this.toolStripMenuItemCut.Enabled = true;
                this.toolStripMenuItemCopy.Enabled = true;
                this.toolStripMenuItemDel.Enabled = true;
            }

            if (Clipboard.ContainsText())
            {
                this.toolStripMenuItemPaste.Enabled = true;
            }
            else
            {
                this.toolStripMenuItemPaste.Enabled = false;
            }
        }

        void toolStripMenuItemDel_Click(object sender, EventArgs e)
        {
            input = Keys.Delete;
            Invalidate();
        }

        void toolStripMenuItemCut_Click(object sender, EventArgs e)
        {
            if (SelectionLength > 0)
            {
                Clipboard.SetText(this.SelectText);
                input = Keys.Delete;
                Invalidate();
            }
        }

        void toolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            if (SelectionLength > 0)
            {
                Clipboard.SetText(this.SelectText);
            }
        }

        void toolStripMenuItemPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                if (SelectionLength > 0)
                {
                    Text = Text.Remove(SelectionStart, SelectionLength);
                    CharIndex = SelectionStart;
                }
                Text = Text.Insert(CharIndex, Clipboard.GetText());
                CharIndex += Clipboard.GetText().Length;
                Modified = true;
                Invalidate();
            }
        }
    }
}
