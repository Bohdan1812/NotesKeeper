using NotesKeeper.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesKeeper.Models
{
    public class NoteCategoryGroup:List<Note>
    {

        public Category category { get; set; }

        public NoteCategoryGroup(Category category, List<Note> notes): base (notes)
        {
            this.category = category;
        }
    }

    
}
