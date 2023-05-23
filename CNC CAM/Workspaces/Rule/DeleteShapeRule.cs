using CNC_CAM.Base;
using CNC_CAM.Operations;
using CNC_CAM.Workspaces.Operations;
using DryIoc;

namespace CNC_CAM.Workspaces.Rule;

public class DeleteShapeRule:AbstractSignalRule<WorkspaceSignals.DeleteElement>
{
    private OperationsController _operationsController;
    private IContainer _container;
    public DeleteShapeRule(SignalBus signalBus, IContainer container,  OperationsController operationsController) : base(signalBus)
    {
        _container = container;
        _operationsController = operationsController;
    }

    protected override void OnSignalFired(WorkspaceSignals.DeleteElement signal)
    {
        var operation = _container.Resolve<DeleteElementOperation>().Initialize(signal.Element);
        _operationsController.LaunchOperation(operation);
    }
}