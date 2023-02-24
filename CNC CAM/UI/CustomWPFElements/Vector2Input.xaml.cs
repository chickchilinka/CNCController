using System;
using System.Numerics;
using System.Windows.Controls;
using DryIoc.ImTools;

namespace CNC_CAM.UI.CustomWPFElements
{
    public partial class Vector2Input : UserControl
    {
        public Vector2Input()
        {
            InitializeComponent();
            Panel.Orientation = _orientation;
            GroupBox.Header = _header;
            XBox.TextChanged += Changed;
            YBox.TextChanged += Changed;
        }

        private void Changed(object sender, TextChangedEventArgs e)
        {
            OnChangeValue?.Invoke(Value);
        }

        public event Action<Vector2> OnChangeValue;

        private Orientation _orientation;

        public Orientation Orientation
        {
            get => _orientation;
            set
            {
                _orientation = value;
                Panel.Orientation = _orientation;
            }
        }

        private string _header;

        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                GroupBox.Header = _header;
            }
        }
        
        public string XName
        {
            get => XLabel.Content.ToString();
            set => XLabel.Content = value;
        }
        public string YName
        {
            get => YLabel.Content.ToString();
            set => YLabel.Content = value;
        }

        public Vector2 Value
        {
            get
            {
                float x = 0;
                float.TryParse(XBox.Text, out x);
                float y = 0;
                float.TryParse(YBox.Text, out y);
                return new Vector2()
                {
                    X = x, Y = y
                };
            }
            set
            {
                XBox.Text = value.X.ToString();
                YBox.Text = value.Y.ToString();
            }
        }
    }
}