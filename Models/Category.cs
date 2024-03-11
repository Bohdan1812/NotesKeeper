using SQLite;


namespace NotesKeeper.Models
{
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public Guid Id { get; set; }
        public string Name { get; set; }

    }
}
