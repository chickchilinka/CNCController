using System.Windows;
using CNC_CAM.Workspaces.Hierarchy;

namespace CNC_CAM.Workspaces.View;

public interface IWorkspaceElementView<in TElement> where TElement:WorkspaceElement
{
    public void Initialize(TElement element);
}