using System.Windows;
using CNC_CAM.Workspaces.Hierarchy;

namespace CNC_CAM.Workspaces.View;


public interface IWorkspaceElementView
{
    public WorkspaceElement Element { get; }
    public void Initialize(WorkspaceElement element);
}
public interface IWorkspaceElementView<in TElement>:IWorkspaceElementView where TElement:WorkspaceElement
{
    public void Initialize(TElement element);
}