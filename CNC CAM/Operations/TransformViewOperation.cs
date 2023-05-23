using System.Windows.Media;
using CNC_CAM.Workspaces.Hierarchy;

namespace CNC_CAM.Operations;

public abstract class TransformViewOperation:Operation, IRevokable
{
    protected Matrix TransformationMatrix = Matrix.Identity;
    private Matrix _sourceMatrix;
    private WorkspaceElement _workspaceElement;

    protected TransformViewOperation(string name) : base(name)
    {
    }

    public TransformViewOperation Initialize(WorkspaceElement workspaceElement)
    {
        _sourceMatrix = workspaceElement.TransformElement.TransformationMatrix;
        _workspaceElement = workspaceElement;
        return this;
    }

    public void Preview()
    {
        _workspaceElement.TransformElement.TransformationMatrix = _sourceMatrix * TransformationMatrix;
    }

    public override void Execute()
    {
        _workspaceElement.TransformElement.TransformationMatrix = _sourceMatrix * TransformationMatrix;
    }

    public void Undo()
    {
        _workspaceElement.TransformElement.TransformationMatrix = _sourceMatrix;
    }

    public void Redo()
    {
        Execute();
    }
}