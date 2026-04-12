namespace KnowledgeBase.Data
{
    /// <summary>
    /// Contains SQL query templates for the NoteRepository.
    /// All the queries in the NoteRepository should be defined here as constants, and then used in the NoteRepository methods. 
    /// This allows for better organization and maintainability of the SQL queries.
    /// </summary>
    public static class SqlQueryTemplates
    {
        //A query template for searching notes by header. 
        public const string SearchNotes = "SELECT Id, Header, Body FROM RepositoryNotes WHERE Header LIKE '%{0}%'";

        //A query template for saving a new note.   
        public const string SaveNote = "INSERT INTO RepositoryNotes (Header, Body) VALUES ('{0}', '{1}')";
    }
}
