using Notes.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Notes.Controller
{
    public class SortController
    {
        public void SortByAscendingDate(ref ObservableCollection<Note_Model> notes)
        {
            var coll = notes.OrderBy(n => n.CreationDate).ToList();
            notes.Clear();
            foreach (var item in coll)
            {
                notes.Add(item);
            }
        }

        public void SortByDescendingDate(ref ObservableCollection<Note_Model> notes)
        {
            var coll = notes.OrderByDescending(n => n.CreationDate).ToList();
            notes.Clear();
            foreach (var item in coll)
            {
                notes.Add(item);
            }
        }

        public void SortByAlphabetOrder(ref ObservableCollection<Note_Model> notes)
        {
            var coll = notes.OrderBy(n => n.Name).ToList();
            notes.Clear();
            foreach (var item in coll)
            {
                notes.Add(item);
            }
        }
        public void SortByInverseAlphabetOrder(ref ObservableCollection<Note_Model> notes)
        {
            var coll = notes.OrderByDescending(n => n.Name).ToList();
            notes.Clear();
            foreach (var item in coll)
            {
                notes.Add(item);
            }
        }
    }
}
