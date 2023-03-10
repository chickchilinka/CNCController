using System.Windows;
using CNC_CAM.SVG.Subpaths;

namespace CNC_CAM.Workspaces.Hierarchy;


public abstract class WorkspaceElement
{
    public abstract string HierarchyIcon { get; }
    public string Name { get; protected set; }
    public bool IsVisible { get; set; }
    protected Transform _element;
    public virtual Transform Element => _element;
    public WorkspaceElement(string name, Transform element)
    {
        Name = name;
        _element = element;
    }   
}
public abstract class WorkspaceElement<TTransform>:WorkspaceElement where TTransform:Transform
{
    public override TTransform Element => _element as TTransform;
    public WorkspaceElement(string name, TTransform element) : base(name, element)
    {
    }
}