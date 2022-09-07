using System.Windows;
using System.Windows.Media;

namespace CNC_CAM.SVG.Subpaths
{
    public abstract class Transform
    {
        protected Matrix _transformationMatrix = Matrix.Identity;
        public virtual Matrix TransformationMatrix
        {
            get => _transformationMatrix;
            set => _transformationMatrix = value;
        }
        public virtual Matrix FinalMatrix => TransformationMatrix * (Parent?.FinalMatrix ?? Matrix.Identity);
        
        protected Transform _parent;
        public virtual Transform Parent
        {
            get => _parent;
            set => _parent = value;
        }
        public virtual Vector ToGlobalPoint(Vector point)
        {
            if (Parent == null)
            {
                var newPointNoParent =  TransformationMatrix.Transform(new Point(point.X, point.Y));
                point.X = newPointNoParent.X;
                point.Y = newPointNoParent.Y;
                return point;
            }
            var newPoint =  TransformationMatrix.Transform(new Point(point.X, point.Y));
            point.X = newPoint.X;
            point.Y = newPoint.Y;
            return Parent.ToGlobalPoint(point);
        }
        public virtual void Move(Vector delta)
        {
            TransformationMatrix.Translate(delta.X, delta.Y);
        }

        public virtual void Scale(Vector multiplication, Vector? pivot = null)
        {
            if(pivot!=null)
                TransformationMatrix.ScaleAt(multiplication.X,multiplication.Y, pivot?.X ?? 0, pivot?.Y ?? 0);
            else 
                TransformationMatrix.Scale(multiplication.X, multiplication.Y);
        }

        public virtual void Rotate(float angleInDegrees, Vector? pivot = null)
        {
            if(pivot!=null)
                TransformationMatrix.RotateAt(angleInDegrees, pivot?.X ?? 0, pivot?.Y ?? 0);
            else 
                TransformationMatrix.Rotate(angleInDegrees);
        }

        public virtual void Skew(Vector vector)
        {
            TransformationMatrix.Skew(vector.X, vector.Y);
        }

        public virtual double? GetDistanceTo(Transform transform)
        {
            return null;
        }
    }
}