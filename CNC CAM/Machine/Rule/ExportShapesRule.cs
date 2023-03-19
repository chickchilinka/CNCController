using System.Collections.Generic;
using CNC_CAM.Base;
using CNC_CAM.Configuration;
using CNC_CAM.Machine.Controllers;
using CNC_CAM.Machine.GCode;

namespace CNC_CAM.Machine.Rule;

public class ExportShapesRule : AbstractSignalRule<MachineSignals.ExportShapes>
{
    private SimpleCncSerialController2D _simpleSerialController;
    private DummyCncController2D _dummyCncController2D;
    private CurrentConfiguration _currentConfiguration;

    public ExportShapesRule(SimpleCncSerialController2D serialController, DummyCncController2D dummyCncController2D,
        CurrentConfiguration currentConfiguration, SignalBus signalBus) : base(signalBus)
    {
        _currentConfiguration = currentConfiguration;
        _simpleSerialController = serialController;
        _dummyCncController2D = dummyCncController2D;
    }

    protected override void OnSignalFired(MachineSignals.ExportShapes signal)
    {
        var gcodes = new List<GCodeCommand>();
        gcodes.Add(new GCodeCommand(new List<string> { "G90", "G28" }));
        foreach (var shape in signal.Shapes)
        {
            gcodes.AddRange(shape.GenerateGCodeCommands(_currentConfiguration));
        }

        gcodes.Add(new GCodeCommand(new List<string> { "G90", "G28" }));
        if(signal.TestMode)
            _dummyCncController2D.ExecuteGCodeCommands(gcodes);
        else
            _simpleSerialController.ExecuteGCodeCommands(gcodes);
    }
}