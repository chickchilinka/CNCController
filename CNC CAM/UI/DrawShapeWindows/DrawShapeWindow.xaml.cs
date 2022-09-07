using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CNC_CAM.UI.DrawShapeWindows
{
    public partial class DrawShapeWindow : Window
    {
        private Action _onSubmit;
        private Action _onCancel;
        public DrawShapeWindow(List<Control> fields, Action onSubmit, Action onCancel)
        {
            _onSubmit = onSubmit;
            _onCancel = onCancel;
            InitializeComponent();
            fields.ForEach((field)=>FieldsStack.Children.Add(field));
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
            _onCancel();
        }

        private void DoneButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
            _onSubmit();
        }
    }
}