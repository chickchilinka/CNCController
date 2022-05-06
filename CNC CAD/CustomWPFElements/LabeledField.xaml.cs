using System.Windows.Controls;

namespace CNC_CAD.CustomWPFElements
{
    public partial class LabeledField : UserControl
    {
        public LabeledField()
        {
            InitializeComponent();
        }

        public string Value => InputBox.Text;

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
    }
}