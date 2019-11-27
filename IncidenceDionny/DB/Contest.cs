using IncidenceDionny.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace IncidenceDionny.DB
{
    public class Contest
    {
        private string _connectionString = string.Empty;
        private SqlCommand command;
        public Contest(string connectionString)
        {
            _connectionString = connectionString;
        }
        public int Insert(Incidence incedence)
        {
            int rowAffected = 0;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var sql = $"insert into Incidence(Name,Title,Detail) values('{incedence.Name}','{incedence.Title}','{incedence.Detail}')";
                using (command = new SqlCommand(sql, connection))
                {
                command.CommandType = CommandType.Text;
                connection.Open();
                rowAffected =command.ExecuteNonQuery();
                connection.Close();
                }
            }
            return rowAffected;
        }
        public List<Incidence> SelectAll()
        {
            try
            {
                var sql = $"select Id,Name,Title,Detail from  Incidence";
                var data = new List<Incidence>();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            if (dataReader.HasRows) { 
                                while (dataReader.Read())
                                {
                                    
                                    Incidence incidence = new Incidence
                                    {
                                        Id = dataReader.GetInt32(0),
                                        Name = dataReader.GetString(1),
                                        Title = dataReader.GetString(2),
                                        Detail = dataReader.GetString(3)
                                    };
                                    data.Add(incidence);
                                }
                            }
                        }
                        connection.Close();
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public Incidence GetIncidence(int id)
        {
            var sql = $"select Id,Name,Title,Detail from  Incidence where Id ={id}";
            Incidence incidence = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {

                            incidence = new Incidence
                            {
                                Id = dataReader.GetInt32(0),
                                Name = dataReader.GetString(1),
                                Title = dataReader.GetString(2),
                                Detail = dataReader.GetString(3)
                            };
                        }
                    }
                    connection.Close();
                }
            }
            return incidence;
        }
        public void  Delete()
        {
            
        }
    }
}
