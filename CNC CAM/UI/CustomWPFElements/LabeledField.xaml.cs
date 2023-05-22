using System;
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
            InputBox.TextChanged+=InputBoxOnTextChanged;
        }

        private void InputBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            OnChanged?.Invoke(Value);
            OnChangedNumeric?.Invoke(NumericValue);
        }

        public event Action<string> OnChanged;
        public event Action<double> OnChangedNumeric;
        private bool _numericOnly = true;
        private string _inputMatcher;


        public string InputMatcher
        {
            get => _inputMatcher;
            set => _inputMatcher = value;
        }
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
                try
                {
                    var substring = Value;
                    if (Value[^1] == '.')
                        substring = Value.Substring(0, Value.Length - 1);
                    return double.Parse(substring);
                }
                catch
                {
                    return 0;
                }
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

        public object TooltipContent
        {
            get => ToolTip.Content;
            set => ToolTip.Content = value;
        }
        
        private void InputBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (_numericOnly)
            {
                var text = InputBox.Text.Insert(InputBox.CaretIndex, e.Text);
                e.Handled = !Regex.IsMatch(text, InputMatcher);
            }
            else e.Handled = false;
        }
        
        
    }
}