namespace KnowledgeBase.Data
{
    public static class SqlQueryTemplates
    {
        public const string SearchNotes = "SELECT Id, Header, Body FROM RepositoryNotes WHERE Header LIKE '%{0}%'";

        public const string SaveNote = "INSERT INTO RepositoryNotes (Header, Body) VALUES ('{0}', '{1}')";
    }
}
