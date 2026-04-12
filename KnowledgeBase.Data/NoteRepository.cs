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
                string sql = string.Format(SqlQueryTemplates.SearchNotes, query);
                
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
                string sql = string.Format(SqlQueryTemplates.SaveNote, header, body);
                var cmd = new SqlCommand(sql, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
