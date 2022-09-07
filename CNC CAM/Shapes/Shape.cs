using System.Collections.Generic;
using System.Windows.Media;
using WPFShape = System.Windows.Shapes.Shape;
using CNC_CAM.Tools;
using CNC_CAM.Machine.Configs;
using CNC_CAM.Machine.GCode;
using Transform = CNC_CAM.SVG.Subpaths.Transform;

namespace CNC_CAM.Shapes
{
    public abstract class Shape : SVG.Subpaths.Transform
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

        public override SVG.Subpaths.Transform Parent
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