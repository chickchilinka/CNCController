using System;
using System.Collections.Generic;
using CNC_CAM.Data.Attributes;
using Microsoft.VisualBasic.ApplicationServices;

namespace CNC_CAM.Configuration.Data;

public class Config
{
    [DBField, DBPrimaryKey] private string _id;
    [DBField] private string _drawingHeadConfigId;
    [DBField] private string _machineConfigId;
    [DBField] private string _userSettingsId;
    [DBField] private string _worksheetConfigId;

    public Config(){}
    public Config(string id)
    {
        _id = id;
    }

    public Dictionary<Type, string> GetLastIds()
    {
        return new Dictionary<Type, string>()
        {
            { typeof(DrawingHeadConfig), _drawingHeadConfigId },
            { typeof(MachineConfig), _machineConfigId },
            { typeof(UserSettings), _userSettingsId },
            { typeof(WorksheetConfig), _worksheetConfigId }
        };
    }

    public Config SetLastIds(Dictionary<Type, string> ids)
    {
        _drawingHeadConfigId = ids[typeof(DrawingHeadConfig)];
        _machineConfigId = ids[typeof(MachineConfig)];
        _userSettingsId = ids[typeof(UserSettings)];
        _worksheetConfigId = ids[typeof(WorksheetConfig)];
        return this;
    }
}