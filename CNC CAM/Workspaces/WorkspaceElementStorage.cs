using System.Collections;
using System.Collections.Generic;
using CNC_CAM.Workspaces.Hierarchy;

namespace CNC_CAM.Workspaces;

public class WorkspaceElementStorage:IEnumerable<WorkspaceElement>
{
    private List<WorkspaceElement> _workspaceElements = new List<WorkspaceElement>();

    public void Add(WorkspaceElement workspaceElement)
    {
        _workspaceElements.Add(workspaceElement);
    }

    public void Remove(WorkspaceElement workspaceElement)
    {
        _workspaceElements.Remove(workspaceElement);
    }
    public IEnumerator<WorkspaceElement> GetEnumerator()
    {
        return _workspaceElements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}