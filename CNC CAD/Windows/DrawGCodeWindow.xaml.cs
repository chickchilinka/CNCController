using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using CNC_CAD.Configs;
using CNC_CAD.Curves;
using CNC_CAD.CustomWPFElements;

namespace CNC_CAD.Windows
{

    public partial class DrawGCodeWindow : Window
    {
        private Workspace2D _workspace2D;

        public DrawGCodeWindow()
        {
            InitializeComponent();
            _workspace2D = new Workspace2D();
            WorkspaceScrollView.Content = _workspace2D;
        }

        public void Draw(List<ICurve> curvesList, AccuracySettings accuracy)
        {
            Show();
            foreach (var curve in curvesList)
            {
                Vector curPoint = curve.ToGlobalPoint(curve.StartPoint);
                foreach (var lineEnd in curve.Linearize(accuracy))
                {
                    _workspace2D.AddShape(new Line()
                    {
                        X1 = curPoint.X,
                        X2 = lineEnd.X,
                        Y1 = curPoint.Y,
                        Y2 = lineEnd.Y,
                        StrokeThickness = 1,
                        Stroke = Brushes.Black
                    });
                    curPoint = lineEnd;
                }

            }
        }
    }
}