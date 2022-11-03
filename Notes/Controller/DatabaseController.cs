using Newtonsoft.Json;
using Notes.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Controller
{
    public class DatabaseController
    {
        public  event Action<string> SendMessage;
        public  event Action<bool> ConnectionResult;
        private ConnectionPath _connectionPath;
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

        public List<Note_Model> GetNotes()
        {
            List<Note_Model> notes = new List<Note_Model>();
            using (SqlConnection conn = new SqlConnection(_connectionPath.Path))
            {
                conn.Open();
                string commStr = $"SELECT * FROM [Notes];";
                using (SqlCommand command = new SqlCommand(commStr, conn))
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
