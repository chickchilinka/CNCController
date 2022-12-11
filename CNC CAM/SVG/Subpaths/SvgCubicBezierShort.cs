using System;
using System.Windows;

namespace CNC_CAM.SVG.Subpaths;

public class SvgCubicBezierShort : SvgCubicBezier
{
    public SvgCubicBezierShort(double[] args, SvgCubicBezier lastCubic, Vector start, bool relative = false) 
    {
        P0 = start;
        P1 = start - (lastCubic.GetLast().P2 - start);
        P2 = new Vector(args[0], args[1]);
        P3 = new Vector(args[2], args[3]);
        if (relative)
        {
            P2 += start;
            P3 += start;
        }

        DetectContinuation(args, relative);
    }
    public override void DetectContinuation(double[] arguments, bool relative)
    {
        if (arguments.Length > 4)
        {
            if (arguments.Length % 4 != 0)
            {
                throw new Exception("Invalid arguments count");
            }

            double[] continArgs = new double[arguments.Length - 4];
            Array.Copy(arguments, 4, continArgs, 0, arguments.Length - 4);
            continuation = new SvgCubicBezierShort(continArgs, this, P3, relative);
        }
    }
}