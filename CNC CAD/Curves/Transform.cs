using System.Windows;
using System.Windows.Media;

namespace CNC_CAD.Curves
{
    public abstract class Transform
    {
        public Matrix TransformationMatrix { get; set; } = Matrix.Identity;
        public Transform Parent { get; set; }
        public virtual Vector ToGlobalPoint(Vector point)
        {
            if (Parent == null)
                return point * TransformationMatrix;
            return Parent.ToGlobalPoint(point * TransformationMatrix);
        }
        public virtual void Move(Vector delta)
        {
            TransformationMatrix.Transform(delta);
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