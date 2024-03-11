
using SQLite;
using SQLiteNetExtensions.Attributes;


namespace NotesKeeper.Models
{
    public class Note
    {
        [PrimaryKey, AutoIncrement]
        public Guid Id { get; set; }
        [ForeignKey(typeof(Category))]
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
