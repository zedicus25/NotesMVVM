using GalaSoft.MvvmLight.Command;
using Notes.Controller;
using Notes.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Notes.ViewModel
{
    public class MainWindow_ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Note_Model> Notes 
        { 
            get { return _notes; }
            set { _notes = value; OnPropertyChanged("Notes"); }
        }
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
                    _notes.Add(note);
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
                    if(_notes.Contains(SelectedNote) && SelectedNote != null)
                    {
                        _notes.Remove(SelectedNote);
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
        private SortController _sortController;
        private ObservableCollection<Note_Model> _notes;
        private event Action AlphabetSortEvent;
        private event Action DateSortEvent;

        public MainWindow_ViewModel()
        {
            _writeReadController = new WriteReadController();
            _sortController = new SortController();
            _notes = new ObservableCollection<Note_Model>(_writeReadController.ReadFromFile());
        }

        public MainWindow_ViewModel(List<Note_Model> notes)
        {
            _writeReadController = new WriteReadController();
            _sortController = new SortController();
            _notes = new ObservableCollection<Note_Model>(notes);
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void SaveToFile() => _writeReadController.WriteToFile(_notes);

        public void AlphabetSort(int countClick)
        {
            DateSortEvent = null;
            switch (countClick)
            {
                case 0:
                    DateSortEvent += SortByAcendingDate;
                    break;
                case 1:
                    DateSortEvent += SortByDescendingDate;
                    break;
            }
            DateSortEvent?.Invoke();
        }

        public void DateSort(int countClick)
        {
            AlphabetSortEvent = null;
            switch (countClick)
            {
                case 0:
                    AlphabetSortEvent += SortByAlphabetOrder;
                    break;
                case 1:
                    AlphabetSortEvent += SortByInverseAlphabetOrder;
                    break;
            }
            AlphabetSortEvent?.Invoke();
        }
        private void SortByAlphabetOrder() => _sortController.SortByAlphabetOrder(ref _notes);

        private void SortByInverseAlphabetOrder() => _sortController.SortByInverseAlphabetOrder(ref _notes);

        private void SortByAcendingDate() => _sortController.SortByAscendingDate(ref _notes);

        private void SortByDescendingDate() => _sortController.SortByDescendingDate(ref _notes);
    }
}
