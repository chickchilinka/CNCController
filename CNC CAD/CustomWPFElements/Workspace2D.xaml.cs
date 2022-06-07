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
        public void AddShapes(params Shape[] shapes)
        {
            foreach (var shape in shapes)
            {
                Canvas.Children.Add(shape);    
            }
            
        }

        public void RemoveShape(Shape shape)
        {
            Canvas.Children.Remove(shape);
        }

        public void ClearShapes()
        {
            Canvas.Children.Clear();
        }
    }
}