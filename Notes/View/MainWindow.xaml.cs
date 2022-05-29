using Notes.ViewModel;
using System.Windows;

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
    }
}
