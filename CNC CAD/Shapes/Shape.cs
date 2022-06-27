using System.Collections.Generic;
using System.Windows.Media;
using CNC_CAD.Configs;
using WPFShape = System.Windows.Shapes.Shape;
using CNC_CAD.GCode;
using CNC_CAD.Tools;
using Transform = CNC_CAD.Curves.Transform;

namespace CNC_CAD.Shapes
{
    public abstract class Shape : Transform
    {
        public double StrokeWidth { get; set; }
        protected List<WPFShape> WpfShapes = new List<WPFShape>();

        public override Matrix TransformationMatrix
        {
            get => _transformationMatrix;
            set
            {
                _transformationMatrix = value;
                ApplyTransformationMatrixToWpf();
            }
        }

        public override Transform Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                ApplyTransformationMatrixToWpf();
            }
        }

        public abstract List<GCodeCommand> GenerateGCodeCommands(CncConfig config);

        public virtual List<WPFShape> GetControlShapes()
        {
            return WpfShapes;
        }

        protected virtual void ApplyTransformationMatrixToWpf()
        {
            foreach (var wpfShape in WpfShapes)
            {
                var scale = FinalMatrix.ExtractScale();
                wpfShape.RenderTransform = new MatrixTransform(FinalMatrix);
                wpfShape.StrokeThickness *= 1 / scale.X;
                wpfShape.InvalidateVisual();
            }
        }
    }
}