using Notes.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Notes.Controller
{
    public class WriteReadController
    {
        public string Path { get; set; }

        public WriteReadController() : this("dataBase.txt")
        {
        }
        public WriteReadController(string path)
        {
           Path = path;
        }

        public void WriteToFile(ObservableCollection<Note_Model> notes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var not in notes)
            {
                if(not.Note != String.Empty)
                    sb.AppendLine(not.ToStringForFile());
            }

            File.WriteAllText(Path, sb.ToString());
        }

        public ObservableCollection<Note_Model> ReadFromFile()
        {
            ObservableCollection<Note_Model> notes = new ObservableCollection<Note_Model>();
            if (File.Exists(Path) == false)
                return notes;

            List<string> lines = File.ReadAllLines(Path).ToList();
            lines.RemoveAll(s => s.Equals(String.Empty));
            lines.RemoveAll( s => s.Equals(new String('-', 25)));
            while(lines.Count > 0)
            {
                notes.Add(new Note_Model(lines[0], lines[2], lines[1]));
                lines.RemoveRange(0, 3);
            }
            return notes;
        }
    }
}
