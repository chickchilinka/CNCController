
using System.Windows;
using System.Windows.Media;

namespace CNC_CAM.Operations;

public class MoveTransformOperation:TransformViewOperation
{
    public MoveTransformOperation(string name) : base(name)
    {
    }
    
    public virtual void Move(Vector delta)
    {
        TransformationMatrix = Matrix.Multiply(TransformationMatrix, new Matrix(1, 0, 0, 1, delta.X, delta.Y));
    }
}