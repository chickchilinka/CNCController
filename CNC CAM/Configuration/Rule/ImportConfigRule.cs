using CNC_CAM.Base;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Tools.Serialization;
using Microsoft.Win32;

namespace CNC_CAM.Configuration.Rule;

public class ImportConfigRule:AbstractSignalRule<ConfigurationSignals.ImportConfig>
{
    private SerializationService _serializationService;
    private ConfigurationStorage _configurationStorage;
    public ImportConfigRule(SignalBus signalBus, ConfigurationStorage configurationStorage, SerializationService serializationService) : base(signalBus)
    {
        _serializationService = serializationService;
        _configurationStorage = configurationStorage;
    }

    protected override void OnSignalFired(ConfigurationSignals.ImportConfig signal)
    {
        var dialog = new OpenFileDialog()
        {
            InitialDirectory = "c:\\",
            Filter = "Файл формата JSON (*.json)|*.json|All files (*.*)|*.*",
            FilterIndex = 1,
            RestoreDirectory = true
        };
        var shown = dialog.ShowDialog();
        if(!shown ?? false)
            return;
        var config = _serializationService.Deserialize<BaseConfig>(dialog.FileName, null);
        _configurationStorage.RegisterConfig(config);
    }
}