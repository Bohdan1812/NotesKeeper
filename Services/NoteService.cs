using NotesKeeper.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesKeeper.Services
{
    public static class NoteService
    {
        static SQLiteAsyncConnection db;
        static async Task Init()
        {
            if (db != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<Note>();
        }
        public static async Task<Note> GetNote(Guid id)
        {
            await Init();

            var note = await db.GetAsync<Note>(id);

            if (note == null)
                throw new Exception("There is no note with this id!");

            return note;
        }

        public static async Task<IEnumerable<Note>> GetNotes()
        {
            await Init();

            var notes = await db.Table<Note>().ToListAsync();
            return notes;
        }

        public static async Task AddNote(string title, string text)
        {
            await Init();

            var note = new Note
            {
                Title = title,
                Text = text,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now
            };

            await db.InsertAsync(note);
        }

        public static async Task UpdateNote(Guid id, string title, string text)
        {
            var note = await GetNote(id);

            note.Title = title;
            note.Text = text;
            note.UpdatedTime = DateTime.Now;

            await db.UpdateAsync(note);
            
        }
        public static async Task DeleteNote(Guid id)
        {
            await Init();

            await db.DeleteAsync<Note>(id); 
        }

    }
}
