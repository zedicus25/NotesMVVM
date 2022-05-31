using Notes.ViewModel;
using System.Windows;

namespace Notes.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindow_ViewModel();
        }

        private void FindTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FindTB.Text.Equals("Find your note"))
                FindTB.Text = string.Empty;
        }

        private void FindTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindTB.Text.Equals(string.Empty))
                FindTB.Text = "Find your note";
        }
    }
}
