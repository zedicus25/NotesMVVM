using Notes.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace Notes.Controller
{
    public class FindController
    {
        public void FindRightGo(ref ObservableCollection<Note_Model> notes, string findText)
        {
            var tmp = notes.Where(n => n.Name.ToLower().Equals(findText.ToLower())).ToList();
            notes.Clear();
            foreach (var item in tmp)
                notes.Add(item);
        }

        public void Find(ref ObservableCollection<Note_Model> notes, string findText)
        {
            var tmp = notes.Where(n => n.Name.ToLower().Contains(findText.ToLower())).ToList();
            notes.Clear();
            foreach (var item in tmp)
                notes.Add(item);
        }
    }
}
