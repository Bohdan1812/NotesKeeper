using NotesKeeper.Models;
using SQLite;

namespace NotesKeeper.Services
{
    public static class CategoryService
    {
        static SQLiteAsyncConnection db;

        static async Task Init()
        {
            if (db != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<Category>();


        }

        public static async Task<Category> GetCategory(Guid id)
        {
            await Init();

            var category = await db.GetAsync<Category>(id);

            if (category == null)
                throw new Exception("There is no category with this id!");

            return category;
        }

        public static async Task<IEnumerable<Category>> GetCategories()
        {
            await Init();

            var categories = await db.Table<Category>().ToListAsync();

            return categories;
        }

        public static async Task AddCategory(string name)
        {
            await Init();

            var category = new Category
            {
               Name = name
            };

            await db.InsertAsync(category);
        }

        public static async Task UpdateCategory(Guid id, string name)
        {
            var category = await GetCategory(id);

            category.Name = name;

            await db.UpdateAsync(category);

        }

        public static async Task DeleteCategory(Guid id)
        {
            await Init();

            var filterdedNotes = await db.Table<Note>().Where(n => n.CategoryId == id).ToListAsync();
            foreach (var note in filterdedNotes) 
            {
                await NoteService.DeleteNote(note.Id); 
            }

            await db.DeleteAsync<Category>(id);
        }

    }
}

