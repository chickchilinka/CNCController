using System;
using System.Windows.Input;
using CNC_CAM.Configuration;
using CNC_CAM.Machine.Configs;
using CNC_CAM.Tools;
using CNC_CAM.UI.CustomWPFElements;

namespace CNC_CAM.Observers;

public class MouseObserver
{
    private Action<string> _callback;
    private CurrentConfiguration _config;
    private Workspace2D _workspace2D;
    private MouseObserver()
    {
        
    }

    public static MouseObserver CreateMouseObserver(CurrentConfiguration config, Workspace2D workspace2D, Action<string> callback)
    {
        var mouseObserver = new MouseObserver();
        mouseObserver._callback = callback;
        mouseObserver._config = config;
        mouseObserver._workspace2D = workspace2D;
        mouseObserver._workspace2D.MouseMove += mouseObserver.Observe;
        return mouseObserver;
    }


    private void Observe(object obj, MouseEventArgs mouseEventArgs)
    {
        var mousePosition = mouseEventArgs.GetPosition(_workspace2D).ToVector();
        var mmPosition = _config.ConvertVectorToPhysical(mousePosition);
        _callback(string.Format(Const.Formatters.MousePositionFormatMM, (int)mmPosition.X, (int)mmPosition.Y));
    }
}