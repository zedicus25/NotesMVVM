using GalaSoft.MvvmLight.Command;
using Notes.Controller;
using Notes.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Notes.ViewModel
{
    public class MainWindow_ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Note_Model> Notes { get; set; }
        public Note_Model SelectedNote
        {
            get { return _selectedNote; }
            set { _selectedNote = value; OnPropertyChanged("SelectedNote"); }
        }

        public RelayCommand AddCommand
        {
            get
            {
                
                return _addCommand ?? (_addCommand = new RelayCommand(() =>
                {
                    Note_Model note = new Note_Model("New note", String.Empty);
                    Notes.Add(note);
                }));
            }
            set
            {
                _addCommand = value;
            }
        }
        public RelayCommand RemoveCommand
        {
            get
            {
                return _removeCommand ?? (_removeCommand = new RelayCommand(() =>
                {
                    if(Notes.Contains(SelectedNote) && SelectedNote != null)
                    {
                        Notes.Remove(SelectedNote);
                    }
                }));
            }
            set
            {
                _removeCommand = value;
            }
        }

        private Note_Model _selectedNote;
        private RelayCommand _addCommand;
        private RelayCommand _removeCommand;
        private WriteReadController _writeReadController;

        public MainWindow_ViewModel()
        {
            _writeReadController = new WriteReadController();
            Notes = new ObservableCollection<Note_Model>(_writeReadController.ReadFromFile());
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void SaveToFile()
        {
            _writeReadController.WriteToFile(Notes);
        }
    }
}
