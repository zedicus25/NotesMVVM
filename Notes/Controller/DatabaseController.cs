using Newtonsoft.Json;
using Notes.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public  event Action<string> SendMessage;
        public  event Action<bool> ConnectionResult;
        private ConnectionPath _connectionPath;
        private int _countFromDb;
        public DatabaseController(string configFile = "config.json")
        {
            _connectionPath = CreataConncetionPath(configFile);
        }

        public void StartServer()
        {
            TryConnectToServer();
        }

        private ConnectionPath CreataConncetionPath(string fileName)
        {
            if(!File.Exists(fileName))
                throw new FileNotFoundException("File not found");

            string json = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<ConnectionPath>(json);
        }

        private void TryConnectToServer()
        {
            try
            {
                using (SqlConnection connection = GetSqlConnection())
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    ConnectionResult?.Invoke(true);

                }
            }
            catch (Exception ex)
            {
                SendMessage?.Invoke(ex.Message);
                ConnectionResult?.Invoke(false);
            }
        }



        public void InsertDataToDB(List<Note_Model> notes)
        {
            notes.RemoveRange(0, _countFromDb);
            foreach (var item in notes)
            {
                Insert(item.Name, item.CreationDate, item.Note);
            }
        }

        public void Insert(string name, DateTime creationDate, string description)
        {
            using(SqlConnection conn = GetSqlConnection())
            {
                conn.Open();
                string date = creationDate.ToString("yyyy-MM-dd HH:mm:ss");
                using (SqlCommand command = new SqlCommand($"INSERT INTO Notes([title],[creationDate],[description]) VALUES('{name}','{date}','{description}');", conn))
                {
                    if (command.ExecuteNonQuery() > 0)
                        SendMessage?.Invoke("Added");
                    else
                        SendMessage?.Invoke("not added");
                }
            }
        }


        public List<Note_Model> GetNotes()
        {
            List<Note_Model> notes = new List<Note_Model>();
            using (SqlConnection conn = new SqlConnection(_connectionPath.Path))
            {
                conn.Open();
                string commStr = $"SELECT * FROM [Notes];";
                using (SqlCommand command = GetSqlCommand(conn, commStr))
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
        }

        private SqlConnection GetSqlConnection() => new SqlConnection(_connectionPath.Path);
        private SqlCommand GetSqlCommand(SqlConnection connection, string command) => new SqlCommand(command, connection);
    }
}
