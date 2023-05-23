using System.Collections.Generic;
using System.Windows.Media;
using CNC_CAM.Configuration;
using WPFShape = System.Windows.Shapes.Shape;
using CNC_CAM.Tools;
using CNC_CAM.Machine.GCode;
using Transform = CNC_CAM.SVG.Subpaths.Transform;

namespace CNC_CAM.Shapes
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
                base.TransformationMatrix = value;
                ApplyTransformationMatrixToWpf();
            }
        }

        public override Transform Parent
        {
            get => _parent;
            set
            {
                base.Parent = value;
                ApplyTransformationMatrixToWpf();
            }
        }
//TODO:создать абстрактную фабрику операций по рисованию, сделать реализацию для станка с ручкой/карандашом, убрать данный метод
        public abstract List<GCodeCommand> GenerateGCodeCommands(CurrentConfiguration config);
        // TODO:так же через фабрику
        public virtual List<WPFShape> GetControlShapes()
        {
            return WpfShapes;
        }
//TODO: сделать для каждого типа shape универсальный View класс, наследюущий WpfShape, фабрику и пул данных вьюшек   
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