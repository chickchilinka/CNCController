using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using CNC_CAM.Tools;

namespace CNC_CAM.UI.CustomWPFElements
{
    public partial class LabeledField : UserControl
    {
        public LabeledField()
        {
            InitializeComponent();
        }

        private bool _numericOnly = true;

        public bool NumericOnly
        {
            get => _numericOnly;
            set => _numericOnly = value;
        }
        public string Value
        {
            get => InputBox.Text;
            set => InputBox.Text = value;
        }

        public double NumericValue
        {
            get
            {
                var substring = Value;
                if (Value[^1] == '.')
                    substring = Value.Substring(0, Value.Length - 1);
                return double.Parse(substring);
            }
        }

        public string FieldName
        {
            get
            {
                string fieldContent = Label.Content.ToString();
                if (fieldContent != null) 
                    return fieldContent[..^1];
                return "";
            }
            set => Label.Content = value + ":";
        }

        public double InputWidth
        {
            get => InputBox.Width;
            set => InputBox.Width = value;
        }
        
        private void InputBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (_numericOnly)
            {
                var text = InputBox.Text + e.Text;
                e.Handled = !Regex.IsMatch(text, Const.RegexPatterns.DecimalMatcher);
            }
            else e.Handled = false;
        }
    }
}