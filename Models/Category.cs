using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesKeeper.Models
{
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
