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
                string sql = SqlQueryTemplates.SearchNotes;
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@query", $"%{query}%");
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
                string sql = SqlQueryTemplates.SaveNote;
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@header", header);
                cmd.Parameters.AddWithValue("@body", body);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
