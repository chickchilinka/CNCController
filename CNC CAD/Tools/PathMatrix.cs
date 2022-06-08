using System;
using System.Collections.Generic;
using System.Data.Common;
using CNC_CAD.Curves;

namespace CNC_CAD.Tools
{

    public class Pair<TTransform> where TTransform : Transform
    {
        public TTransform First { get; private init; }
        public TTransform Second { get; private init; }

        public double? Distance => First.GetDistanceTo(Second);

        public Pair(TTransform first, TTransform second)
        {
            First = first;
            Second = second;
        }
    }

    public class PathMatrix<TTransform> where TTransform : Transform
    {
        public List<TTransform> TransformRows { get; private init; }
        public List<TTransform> TransformColumns { get; private init; }
        private readonly List<List<double?>> _distances;
        public int Length => _distances.Count;
        public int FullLength => _distances.Count * _distances.Count;

        public double this[int i, int j]
        {
            get => _distances[i][j] ?? Double.MaxValue;
            set => _distances[i][j] = value;
        }

        public List<int> IgnoreColumns = new List<int>();
        public List<int> IgnoreRows = new List<int>();


        public PathMatrix(List<TTransform> transforms)
        {
            TransformRows = new List<TTransform>(transforms);
            TransformColumns = new List<TTransform>(transforms);
            _distances = new List<List<double?>>();
            for (int i = 0; i < transforms.Count; i++)
            {
                _distances.Add(new List<double?>());
                for (int j = 0; j < transforms.Count; j++)
                {
                    _distances[i].Add(transforms[i].GetDistanceTo(transforms[j]));
                }
            }

            LogMatrix(transforms);
        }

        private void LogMatrix(List<TTransform> points)
        {
            foreach (var point1 in points)
            {
                foreach (var point2 in points)
                {
                    Console.Write($"{point1.GetDistanceTo(point2) ?? 0} ");
                }

                Console.WriteLine();
            }
        }

        private PathMatrix(List<TTransform> transformsRow, List<TTransform> transformsColumn,
            List<List<double?>> distances)
        {
            _distances = distances;
            TransformRows = new List<TTransform>(transformsRow);
            TransformColumns = new List<TTransform>(transformsColumn);
        }

        public PathMatrix<TTransform> Copy()
        {
            return new PathMatrix<TTransform>(TransformRows, TransformColumns, _distances);
        }

        public bool SkipColumn(int column)
        {
            return IgnoreColumns.Contains(column);
        }

        public bool SkipRow(int row)
        {
            return IgnoreRows.Contains(row);
        }

        public void RemoveRow(int row)
        {
            IgnoreRows.Add(row);
        }

        public void RemoveColumn(int column)
        {
            IgnoreColumns.Add(column);
        }

    }
}