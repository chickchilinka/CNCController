using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using CNC_CAM.UI.CustomWPFElements;

namespace CNC_CAM.UI.DrawShapeWindows
{
    public class MakeShapeWindowBuilder
    {
        public static Thickness DefaultMargin = new Thickness(5, 5, 0, 0);
        private readonly List<Control> _controlsToAdd;
        private readonly Action<Dictionary<string, Vector2>, Dictionary<string, float>> _onSubmit;
        private Action _onCancel;
        private Thickness _margin = DefaultMargin;

        private MakeShapeWindowBuilder(Action<Dictionary<string, Vector2>, Dictionary<string, float>> onSubmit, Action onCancel)
        {
            _controlsToAdd = new List<Control>();
            _onSubmit = onSubmit;
            _onCancel = onCancel;
        }

        public static MakeShapeWindowBuilder Create(Action<Dictionary<string, Vector2>, Dictionary<string, float>> onSubmit, Action onCancel)
        {
            return new MakeShapeWindowBuilder(onSubmit, onCancel);
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

        public MakeShapeWindowBuilder AddSimpleFloatField(string fieldName, double width = 100, float defaultValue = 0)
        {
            var control = new GenericField<float>()
            {
                FieldName = fieldName,
                InputWidth = width,
                Margin = _margin,
                HorizontalAlignment = HorizontalAlignment.Left,
                Value = defaultValue.ToString()
            };
            _controlsToAdd.Add(control);
            return this;
        }

        public Window Build()
        {
            return new DrawShapeWindow(_controlsToAdd, () =>
            {
                Dictionary<string, Vector2> vector2s = new Dictionary<string, Vector2>();
                Dictionary<string, float> decimals = new Dictionary<string, float>();
                foreach (var control in _controlsToAdd)
                {
                    if(control is Vector2Input)
                        vector2s.Add(((Vector2Input) control).Header, ((Vector2Input)control).Value);
                    else if(control is LabeledField)
                        decimals.Add(((LabeledField) control).FieldName, (float)((LabeledField)control).NumericValue);
                }

                _onSubmit(vector2s, decimals);
            }, () =>
            {
                _onCancel();
            });
        }
    }
}