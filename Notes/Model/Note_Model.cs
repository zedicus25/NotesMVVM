using Notes.Controller;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Notes.Model
{
    public class Note_Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Note
        {
            get { return _note; }
            set { _note = value;  OnPropertyChanged("Note"); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        public string CreationDate
        {
            get { return _creationDate; }
            private set { _creationDate = value; OnPropertyChanged("CreationDate"); }
        }


        private string _note;
        private string _name;
        private string _creationDate;

        public Note_Model() : this(String.Empty, String.Empty)
        {
        }
        public Note_Model(string name, string content)
        {
            _name = name;
            _note = content;
            _creationDate = DateTime.Now.ToShortDateString();
        }
        public Note_Model(string name, string note, string dateCreation) : this(name, note)
        {
            _creationDate = dateCreation; 
        } 
        public Note_Model(Note_Model note) : this(note.Name, note.Note)
        {
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public override string ToString()
        {
            return String.Format("{0} \n {1} ", _name, _creationDate);
        }

        public string ToStringForFile()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(_name);
            sb.AppendLine(_creationDate);
            sb.AppendLine(_note);
            sb.Append(new String('-',25));
            return sb.ToString();
        }
    }
}
