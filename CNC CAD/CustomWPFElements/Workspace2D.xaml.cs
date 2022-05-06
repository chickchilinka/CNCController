using System.Windows.Controls;
using System.Windows.Shapes;

namespace CNC_CAD.CustomWPFElements
{
    public partial class Workspace2D : UserControl
    {
        public Workspace2D()
        {
            InitializeComponent();
        }

        public void AddShape(Shape shape)
        {
            Canvas.Children.Add(shape);
        }
    }
}