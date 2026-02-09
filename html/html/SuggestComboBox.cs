using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;

namespace UtilETWeb
{
    public class SuggestComboBox : ComboBox
    {
        #region fields and properties

        private readonly ListBox suggestionListBox = new ListBox { Visible = false, TabStop = false };
        private readonly BindingList<string> suggBindingList = new BindingList<string>();

        private Expression<Func<ObjectCollection, IEnumerable<string>>> propertySelector;
        private Func<ObjectCollection, IEnumerable<string>> propertySelectorCompiled;
        private Expression<Func<string, string, bool>> filterRule;
        private Func<string, bool> filterRuleCompiled;
        private Expression<Func<string, string>> suggestListOrderRule;
        private Func<string, string> suggestListOrderRuleCompiled;

        private PropertyDescriptor displayProperty;
        private PropertyDescriptor valueProperty;

        public int SuggestBoxHeight
        {
            get => suggestionListBox.Height;
            set { if (value > 0) suggestionListBox.Height = value; }
        }

        /// <summary>
        /// Permite especificar qué propiedad del objeto mostrar.
        /// </summary>
        public new string DisplayMember
        {
            get => base.DisplayMember;
            set
            {
                base.DisplayMember = value;
                UpdatePropertyDescriptors();
            }
        }

        /// <summary>
        /// Permite especificar qué propiedad usar como valor.
        /// </summary>
        public new string ValueMember
        {
            get => base.ValueMember;
            set
            {
                base.ValueMember = value;
                UpdatePropertyDescriptors();
            }
        }

        private void UpdatePropertyDescriptors()
        {
            if (DataSource is System.Collections.IList list && list.Count > 0)
            {
                var itemType = list[0].GetType();
                var props = TypeDescriptor.GetProperties(itemType);
                displayProperty = !string.IsNullOrEmpty(DisplayMember) ? props.Find(DisplayMember, true) : null;
                valueProperty = !string.IsNullOrEmpty(ValueMember) ? props.Find(ValueMember, true) : null;
            }
        }

        public Expression<Func<ObjectCollection, IEnumerable<string>>> PropertySelector
        {
            get => propertySelector;
            set
            {
                if (value == null) return;
                propertySelector = value;
                propertySelectorCompiled = value.Compile();
            }
        }

        public Expression<Func<string, string, bool>> FilterRule
        {
            get => filterRule;
            set
            {
                if (value == null) return;
                filterRule = value;
                filterRuleCompiled = item => value.Compile()(item, Text);
            }
        }

        public Expression<Func<string, string>> SuggestListOrderRule
        {
            get => suggestListOrderRule;
            set
            {
                if (value == null) return;
                suggestListOrderRule = value;
                suggestListOrderRuleCompiled = value.Compile();
            }
        }

        #endregion

        /// <summary>
        /// ctor
        /// </summary>
        public SuggestComboBox()
        {
            filterRuleCompiled = s => s.ToLower().Contains(Text.Trim().ToLower());
            suggestListOrderRuleCompiled = s => s;
            propertySelectorCompiled = collection => collection.Cast<string>();

            suggestionListBox.DataSource = suggBindingList;
            suggestionListBox.Click += SuggestionListBoxOnClick;

            ParentChanged += OnParentChanged;
        }

        /// <summary>
        /// the magic happens here ;-)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (!Focused) return;

            suggBindingList.Clear();
            suggBindingList.RaiseListChangedEvents = false;

            IEnumerable<string> itemTexts;

            // ✅ Si hay DisplayMember, usamos esa propiedad
            if (DataSource is System.Collections.IList list && list.Count > 0 && displayProperty != null)
            {
                itemTexts = list.Cast<object>()
                                .Select(i => displayProperty.GetValue(i)?.ToString() ?? string.Empty);
            }
            else
            {
                itemTexts = Items.Cast<object>().Select(i => i?.ToString() ?? string.Empty);
            }

            itemTexts
                .Where(filterRuleCompiled)
                .OrderBy(suggestListOrderRuleCompiled)
                .ToList()
                .ForEach(suggBindingList.Add);

            suggBindingList.RaiseListChangedEvents = true;
            suggBindingList.ResetBindings();

            suggestionListBox.Visible = suggBindingList.Any();
            

            if (suggBindingList.Count == 1 &&
                suggBindingList.Single().Length == Text.Trim().Length)
            {
                Text = suggBindingList.Single();
                Select(0, Text.Length);
                suggestionListBox.Visible = false;
            }
        }

        #region size and position of suggest box

        private void OnParentChanged(object sender, EventArgs e)
        {
            Parent.Controls.Add(suggestionListBox);
            Parent.Controls.SetChildIndex(suggestionListBox, 0);
            suggestionListBox.Top = Top + Height;
            suggestionListBox.Left = Left;
            suggestionListBox.Width = Width;
            suggestionListBox.Font = Font;
            

        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            suggestionListBox.Top = Top + Height;
            suggestionListBox.Left = Left;
            
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            suggestionListBox.Width = Width;
            
        }

        #endregion

        #region visibility of suggest box

        protected override void OnLostFocus(EventArgs e)
        {
            if (!suggestionListBox.Focused)
                HideSuggBox();
            base.OnLostFocus(e);
        }

        private void SuggestionListBoxOnClick(object sender, EventArgs eventArgs)
        {
            Text = suggestionListBox.Text;

            if (displayProperty != null && DataSource is System.Collections.IList list)
            {
                var selectedObj = list.Cast<object>()
                    .FirstOrDefault(i => displayProperty.GetValue(i)?.ToString() == suggestionListBox.Text);
                SelectedItem = selectedObj;
            }

            Focus();
        }

        private void HideSuggBox()
        {
            suggestionListBox.Visible = false;
        }

        protected override void OnDropDown(EventArgs e)
        {
            HideSuggBox();
            base.OnDropDown(e);
        }

        #endregion

        #region keystroke events

        private bool ProcessKeyDown(Keys keyData)
        {
            
            if (suggestionListBox.Visible)
            {
                switch (keyData)
                {
                    case Keys.Down:
                        if (suggestionListBox.SelectedIndex < suggBindingList.Count - 1)
                            suggestionListBox.SelectedIndex++;
                        return true;
                    case Keys.Up:
                        if (suggestionListBox.SelectedIndex > 0)
                            suggestionListBox.SelectedIndex--;
                        return true;
                    case Keys.Enter:
                        Text = suggestionListBox.Text;
                        Select(0, Text.Length);
                        suggestionListBox.Visible = false;
                        return true;
                    case Keys.Escape:
                        HideSuggBox();
                        return true;
                }
            }
            return false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (ProcessKeyDown(keyData))
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}
