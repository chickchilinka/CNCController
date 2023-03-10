using System.Windows;
using System.Windows.Controls;
using CNC_CAM.Workspaces.Hierarchy;

namespace CNC_CAM.Workspaces.View;

public class CustomTreeViewItem:TreeViewItem
{
    private SignalBus _signalBus;
    private WorkspaceElement _workspaceElement;

    public CustomTreeViewItem(SignalBus signalBus, WorkspaceElement workspaceElement)
    {
        _workspaceElement = workspaceElement;
        _signalBus = signalBus;
    }
    
    protected override void OnSelected(RoutedEventArgs eventArgs)
    {
        base.OnSelected(eventArgs);
        _signalBus.Fire(new WorkspaceSignals.SelectElement(_workspaceElement));
    }
}