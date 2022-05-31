﻿using GalaSoft.MvvmLight.Command;
using Notes.Controller;
using Notes.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
                    SelectedNote = _notes.Last();
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

        public RelayCommand AlphabetSortCommand
        {
            get
            {
                return _alphabetSortCommand ?? (_alphabetSortCommand = new RelayCommand(() =>
                {
                    AlphabetSort();
                }));
            }
            set { _alphabetSortCommand = value; }
        }

        public RelayCommand DateSortCommand
        {
            get
            {
                return _dateSortCommand ?? (_dateSortCommand = new RelayCommand(() =>
                {
                    DateSort();
                }));
            }
            set { _dateSortCommand = value; }
        }


        private RelayCommand _addCommand;
        private RelayCommand _removeCommand;
        private RelayCommand _alphabetSortCommand;
        private RelayCommand _dateSortCommand;

        private Note_Model _selectedNote;
        private WriteReadController _writeReadController;
        private SortController _sortController;
        private ObservableCollection<Note_Model> _notes;
        private event Action AlphabetSortEvent;
        private event Action DateSortEvent;
        private bool _alphabetAcendingSort = true;
        private bool _dateAcendingSort = true;

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

        private void AlphabetSort()
        {
            AlphabetSortEvent = null;
            if (_alphabetAcendingSort)
                AlphabetSortEvent += SortByAlphabetOrder;
            if (_alphabetAcendingSort == false)
                AlphabetSortEvent += SortByInverseAlphabetOrder;
            AlphabetSortEvent?.Invoke();
            _alphabetAcendingSort = !_alphabetAcendingSort;
        }

        private void DateSort()
        {
            DateSortEvent = null;
            if (_dateAcendingSort)
                DateSortEvent += SortByAcendingDate;
            if (_dateAcendingSort == false)
                DateSortEvent += SortByDescendingDate;
            DateSortEvent?.Invoke();
            _dateAcendingSort = !_dateAcendingSort;
        }
        private void SortByAlphabetOrder() => _sortController.SortByAlphabetOrder(ref _notes);

        private void SortByInverseAlphabetOrder() => _sortController.SortByInverseAlphabetOrder(ref _notes);

        private void SortByAcendingDate() => _sortController.SortByAscendingDate(ref _notes);

        private void SortByDescendingDate() => _sortController.SortByDescendingDate(ref _notes);

        ~MainWindow_ViewModel()
        {
            _writeReadController.WriteToFile(_notes);
        }
    }
}
