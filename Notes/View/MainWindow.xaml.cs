using Notes.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Notes.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindow_ViewModel _viewModel = new MainWindow_ViewModel();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void FindTextBoxEvent(object sender, TextChangedEventArgs e)
        {
            if (rightCB == null)
                return;
            if(FindTB.Text.Equals("Find your note"))
            {
                DataContext = _viewModel;
                AddBtn.IsEnabled = true;
                DeleteBtn.IsEnabled = true;
            }
            else 
            {
                AddBtn.IsEnabled = false;
                DeleteBtn.IsEnabled = false;
                if (rightCB.IsPressed)
                {
                    DataContext = new MainWindow_ViewModel(_viewModel.Notes.Where(
                    n => n.Name.ToLower().Equals(FindTB.Text.ToLower())).ToList());
                }
                else
                {
                    DataContext = new MainWindow_ViewModel(_viewModel.Notes.Where(
                    n => n.Name.ToLower().Contains(FindTB.Text.ToLower())).ToList());
                }
            }
        }

        private void FindTB_GotFocus(object sender, RoutedEventArgs e)
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
