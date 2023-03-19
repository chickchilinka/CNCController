using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CNC_CAM.Tools;

namespace CNC_CAM.UI.CustomWPFElements
{
    public partial class LabeledCheckbox : UserControl
    {
        public LabeledCheckbox()
        {
            InitializeComponent();
            Checkbox.Checked+=CheckboxOnChecked;
            Checkbox.Unchecked+=CheckboxOnUnchecked;
        }

        private void CheckboxOnUnchecked(object sender, RoutedEventArgs e)
        {
            OnChanged?.Invoke(Value);
        }

        private void CheckboxOnChecked(object sender, RoutedEventArgs e)
        {
            OnChanged?.Invoke(Value);
        }

        public event Action<bool> OnChanged;
        private bool _numericOnly = true;
        
        public bool Value
        {
            get => Checkbox.IsChecked ?? false;
            set => Checkbox.IsChecked = value;
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
        

        public object TooltipContent
        {
            get => ToolTip.Content;
            set => ToolTip.Content = value;
        }

    }
}