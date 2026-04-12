using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace KnowledgeBase.Data
{
    public class NoteRepository
    {
        private readonly string _connectionString;

        public NoteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<NoteEntity> SearchNotes(string query)
        {
            var results = new List<NoteEntity>();
            using (var conn = new SqlConnection(_connectionString))
            {
                // VULNERABLE: SQL Injection via string concatenation
                string sql = "SELECT Id, Header, Body FROM RepositoryNotes WHERE Header LIKE '%" + query + "%'";
                
                var cmd = new SqlCommand(sql, conn);
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        results.Add(new NoteEntity {
                            Id = (int)rdr["Id"],
                            Header = rdr["Header"].ToString() ?? "",
                            Body = rdr["Body"].ToString() ?? ""
                        });
                    }
                }
            }
            return results;
        }

        public void SaveNote(string header, string body)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                // VULNERABLE: SQL Injection via interpolation
                string sql = $"INSERT INTO RepositoryNotes (Header, Body) VALUES ('{header}', '{body}')";
                var cmd = new SqlCommand(sql, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
