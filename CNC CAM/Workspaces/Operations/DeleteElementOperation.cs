using CNC_CAM.Operations;
using CNC_CAM.Workspaces.Hierarchy;

namespace CNC_CAM.Workspaces.Operations;

public class DeleteElementOperation:Operation, IRevokable
{
    private WorkspaceFacade _workspaceFacade;
    private WorkspaceElement _workspaceElement;
    public DeleteElementOperation(WorkspaceFacade workspaceFacade) : base("Delete")
    {
        _workspaceFacade = workspaceFacade;
    }

    public DeleteElementOperation Initialize(WorkspaceElement element)
    {
        Name = $"Удаление {element.Name}";
        _workspaceElement = element;
        return this;
    }
    public override void Execute()
    {
        Redo();
    }

    public void Undo()
    {
        _workspaceFacade.Add(_workspaceElement);
    }

    public void Redo()
    {
        _workspaceFacade.Remove(_workspaceElement);
    }
}