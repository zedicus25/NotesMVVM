using Newtonsoft.Json;
using Notes.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Notes.Controller
{
    public class DatabaseController
    {
        public event Action<string> SendMessage;
        private int _countFromDb;
        public DatabaseController()
        {
        }

       
        public void RemoveNote(Note_Model note)
        {
            using (SqlCommand command = new SqlCommand($"DELETE FROM [Notes] WHERE  [id] = {note.Id};", SqlConnectionSingleton.GetInstance()))
            {
                if (command.ExecuteNonQuery() > 0)
                {
                    SendMessage?.Invoke("Removed");
                    _countFromDb--;
                }
                    
                else
                    SendMessage?.Invoke("not removed");
            }
        }

        public void UpdateNote(Note_Model note)
        {
            if (note == null)
                return;
            using (SqlCommand command = new SqlCommand($"UPDATE [Notes] SET [title]='{note.Name}',[description]='{note.Note}' WHERE  [id] = {note.Id};", SqlConnectionSingleton.GetInstance()))
            {
                if (command.ExecuteNonQuery() > 0)
                    SendMessage?.Invoke("Updated");
                else
                    SendMessage?.Invoke("not updated");
            }
        }

        public void InsertDataToDB(List<Note_Model> notes)
        {
            if (_countFromDb != 0)
                notes.RemoveRange(0, _countFromDb);

            foreach (var item in notes)
            {
                Insert(item.Name, item.CreationDate, item.Note);
            }
        }

        private void Insert(string name, DateTime creationDate, string description)
        {
                string date = creationDate.ToString("yyyy-MM-dd HH:mm:ss");
                using (SqlCommand command = new SqlCommand($"INSERT INTO Notes([title],[creationDate],[description]) VALUES('{name}','{date}','{description}');", SqlConnectionSingleton.GetInstance()))
                {
                    if (command.ExecuteNonQuery() > 0)
                        SendMessage?.Invoke("Added");
                    else
                        SendMessage?.Invoke("not added");
                }
        }


        public List<Note_Model> GetNotes()
        {
            List<Note_Model> notes = new List<Note_Model>();
                string commStr = $"SELECT * FROM [Notes];";
                using (SqlCommand command = GetSqlCommand(SqlConnectionSingleton.GetInstance(), commStr))
                {
                    try
                    {
                        SqlDataReader sqlData = command.ExecuteReader();
                        while (sqlData.Read())
                        {
                            int id = Convert.ToInt32(sqlData.GetValue(0));
                            string name = Convert.ToString(sqlData.GetValue(1));
                            DateTime date = DateTime.Parse(sqlData.GetValue(2).ToString());
                            string note = sqlData.GetValue(3).ToString();
                            notes.Add(new Note_Model(id, name, note, date));
                            _countFromDb++;
                        }
                    }
                    catch (Exception ex)
                    {
                        SendMessage?.Invoke(ex.Message);
                    }
                }
                return notes;
        }

        private SqlCommand GetSqlCommand(SqlConnection connection, string command) => new SqlCommand(command, connection);
    }
}
