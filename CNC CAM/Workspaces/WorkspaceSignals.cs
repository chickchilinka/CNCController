using CNC_CAM.Workspaces.Hierarchy;

namespace CNC_CAM.Workspaces;

public class WorkspaceSignals
{
    public class SelectElement
    {
        public WorkspaceElement Element { get; }

        public SelectElement(WorkspaceElement element)
        {
            Element = element;
        }
    }
}