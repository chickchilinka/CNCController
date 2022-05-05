using System.Windows;
using CNC_CAD.DrawShapeWindows;

namespace CNC_CAD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonCreateArc3Points_OnClick(object sender, RoutedEventArgs e)
        {
            Window window = MakeShapeWindowBuilder.Create((vectors, fields) =>
                {

                })
                .AddVector2Field("1st point")
                .AddVector2Field("2nd point")
                .AddVector2Field("3rd point")
                .AddWidthHeightField("Demo WH").Build();
            window.Show();
        }
    }
}