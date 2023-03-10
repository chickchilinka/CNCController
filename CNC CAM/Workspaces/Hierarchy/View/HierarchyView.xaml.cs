using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CNC_CAM.Workspaces.View;

namespace CNC_CAM.Workspaces.Hierarchy.View;

public partial class HierarchyView : UserControl
{
    private Dictionary<WorkspaceElement, TreeViewItem> _items = new Dictionary<WorkspaceElement, TreeViewItem>();
    private SignalBus _signalBus;
    public HierarchyView(SignalBus signalBus)
    {
        _signalBus = signalBus;
        InitializeComponent();
    }

    public void Add(WorkspaceElement workspaceElement)
    {
        var itemView = CreateTreeViewItem(workspaceElement);
        itemView.Selected += ItemViewOnSelected; 
        _items.Add(workspaceElement, itemView);
        HierarchyTreeView.Items.Add(itemView);
    }

    private void ItemViewOnSelected(object sender, RoutedEventArgs e)
    {
    }

    public void Remove(WorkspaceElement workspaceElement)
    {
        if(_items.TryGetValue(workspaceElement, out var view))
            HierarchyTreeView.Items.Remove(view);
        _items.Remove(workspaceElement);
    }


    private TreeViewItem CreateTreeViewItem(WorkspaceElement workspaceElement)
    {
        var elementView = new HierarchyElementView(workspaceElement);
        var treeItemView = new CustomTreeViewItem(_signalBus, workspaceElement)
        {
            Header = elementView
        };
        return treeItemView;
    }
}