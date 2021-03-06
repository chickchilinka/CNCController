using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using CNC_CAD.CustomWPFElements;

namespace CNC_CAD.DrawShapeWindows
{
    public class MakeShapeWindowBuilder
    {
        public static Thickness DefaultMargin = new Thickness(5, 5, 0, 0);
        private readonly List<Control> _controlsToAdd;
        private readonly Action<Dictionary<string, Vector2>, Dictionary<string, float>> _onSubmit;
        private Action _onCancel;
        private Thickness _margin = DefaultMargin;

        private MakeShapeWindowBuilder(Action<Dictionary<string, Vector2>, Dictionary<string, float>> onSubmit)
        {
            _controlsToAdd = new List<Control>();
            _onSubmit = onSubmit;
        }

        public static MakeShapeWindowBuilder Create(Action<Dictionary<string, Vector2>, Dictionary<string, float>> onSubmit)
        {
            return new MakeShapeWindowBuilder(onSubmit);
        }

        public MakeShapeWindowBuilder WithCancelCallback(Action onCancel)
        {
            _onCancel = onCancel;
            return this;
        }

        public MakeShapeWindowBuilder WithDefaultMargin(Thickness margin)
        {
            _margin = margin;
            return this;
        }

        public MakeShapeWindowBuilder AddVector2Field(string fieldName)
        {
            var control = new Vector2Input
            {
                GroupBox = { Header = fieldName }, Margin = _margin, Orientation = Orientation.Horizontal
            };
            _controlsToAdd.Add(control);
            return this;
        }

        public MakeShapeWindowBuilder AddWidthHeightField(string fieldName)
        {
            var control = new Vector2Input
            {
                GroupBox = { Header = fieldName },
                Margin = _margin,
                Orientation = Orientation.Horizontal,
                X = { Content = "Width" },
                Y = { Content = "Height" }
            };
            _controlsToAdd.Add(control);
            return this;
        }

        public MakeShapeWindowBuilder AddSimpleFloatField(string fieldName, double width = 100)
        {
            var control = new GenericField<float>()
            {
                FieldName = fieldName,
                InputWidth = width,
                Margin = _margin,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            _controlsToAdd.Add(control);
            return this;
        }

        public Window Build()
        {
            return new DrawShapeWindow(_controlsToAdd, () =>
            {
                //TODO:Onsubmit
            }, () =>
            {
                _onCancel();
            });
        }
    }
}