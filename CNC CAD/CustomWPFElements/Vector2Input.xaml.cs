using System.Numerics;
using System.Windows.Controls;

namespace CNC_CAD.CustomWPFElements
{
    public partial class Vector2Input : UserControl
    {
        public Vector2Input()
        {
            InitializeComponent();
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

        public void SetOrientation(Orientation orientation)
        {
            Panel.Orientation = orientation;
        }
    }
}