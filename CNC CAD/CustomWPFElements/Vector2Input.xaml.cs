using System;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;

namespace CNC_CAD.CustomWPFElements
{
    public partial class Vector2Input : UserControl
    {
        public Vector2Input()
        {
            InitializeComponent();
            Panel.Orientation = _orientation;
            GroupBox.Header = _header;
        }
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
        }
    }
}