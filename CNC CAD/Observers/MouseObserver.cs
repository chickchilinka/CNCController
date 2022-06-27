using System;
using System.Windows;
using System.Windows.Input;
using CNC_CAD.Configs;
using CNC_CAD.CustomWPFElements;
using CNC_CAD.Tools;
using CNC_CAD.Workspaces;

namespace CNC_CAD.Observers;

public class MouseObserver
{
    private Action<string> _callback;
    private CncConfig _config;
    private Workspace2D _workspace2D;
    private MouseObserver()
    {
        
    }

    public static MouseObserver CreateMouseObserver(CncConfig config, Workspace2D workspace2D, Action<string> callback)
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