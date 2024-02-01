// WunderVision 2024
using FunKeyViewer.KeyboardUtils;
using FunKeyViewer.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace FunKeyViewer
{
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }
        public MainWindow()
        {
            ViewModel = new MainWindowViewModel(this);
            DataContext = ViewModel;
            InitializeComponent();

            KeyboardHookMananger.AddHook();
        }

        private void BorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
