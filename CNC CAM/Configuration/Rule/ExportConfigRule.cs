using CNC_CAM.Base;
using CNC_CAM.Tools.Serialization;
using Microsoft.Win32;

namespace CNC_CAM.Configuration.Rule;

public class ExportConfigRule:AbstractSignalRule<ConfigurationSignals.ExportConfig>
{
    private SerializationService _serializationService;
    public ExportConfigRule(SignalBus signalBus, SerializationService serializationService) : base(signalBus)
    {
        _serializationService = serializationService;
    }

    protected override void OnSignalFired(ConfigurationSignals.ExportConfig signal)
    {
        var dialog = new SaveFileDialog()
        {
            InitialDirectory = "c:\\",
            Filter = "Файл формата JSON (*.json)|*.json|All files (*.*)|*.*",
            FilterIndex = 1,
            RestoreDirectory = true
        };
        var shown = dialog.ShowDialog();
        if(!shown ?? false)
            return;
        _serializationService.Serialize(dialog.FileName, signal.Config);
    }
}