using Notes.ViewModel;
using System.Windows;

namespace Notes.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindow_ViewModel viewModel = new MainWindow_ViewModel();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void SaveToFile(object sender, System.EventArgs e)
        {
            viewModel.SaveToFile();
        }
    }
}
