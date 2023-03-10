using CNC_CAM.Base;

namespace CNC_CAM.Workspaces.Rule;

public class SelectShapeRule:AbstractSignalRule<WorkspaceSignals.SelectElement>
{
    private WorkspaceFacade _workspaceFacade;
    public SelectShapeRule(SignalBus signalBus, WorkspaceFacade facade) : base(signalBus)
    {
        _workspaceFacade = facade;
    }

    protected override void OnSignalFired(WorkspaceSignals.SelectElement signal)
    {
        _workspaceFacade.Select(signal.Element);
    }
}