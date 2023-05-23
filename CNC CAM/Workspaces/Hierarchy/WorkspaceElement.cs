using System.Windows;
using CNC_CAM.SVG.Subpaths;

namespace CNC_CAM.Workspaces.Hierarchy;


public abstract class WorkspaceElement
{
    public abstract string HierarchyIcon { get; }
    public string Name { get; protected set; }
    public bool IsVisible { get; set; }
    protected Transform _transform;
    public virtual Transform TransformElement => _transform;
    public WorkspaceElement(string name, Transform transform)
    {
        Name = name;
        _transform = transform;
    }   
}
public abstract class WorkspaceElement<TTransform>:WorkspaceElement where TTransform:Transform
{
    public override TTransform TransformElement => _transform as TTransform;
    public WorkspaceElement(string name, TTransform transform) : base(name, transform)
    {
    }
}