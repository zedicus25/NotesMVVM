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
        private int _alphabetClick = 0;
        private int _dateClick = 0;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void SaveToFile(object sender, System.EventArgs e)
        {
            _viewModel.SaveToFile();
        }

        private void AlphabetSortButtonClick(object sender, RoutedEventArgs e)
        {
            _viewModel.AlphabetSort(_alphabetClick);
            _alphabetClick++;
            _alphabetClick = _alphabetClick % 2 == 0 ? 0 : _alphabetClick;
        }

        private void DateSortButtonClick(object sender, RoutedEventArgs e)
        {
            _viewModel.DateSort(_dateClick);
            _dateClick++;
            _dateClick = _dateClick % 2 == 0 ? 0 : _dateClick;
        }

        private void FindTextBoxEvent(object sender, TextChangedEventArgs e)
        {
            if(FindTB.Text == string.Empty)
            {
                DataContext = _viewModel;
            }
            else 
            {
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
    }
}
