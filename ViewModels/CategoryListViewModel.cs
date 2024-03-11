using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using NotesKeeper.Models;
using NotesKeeper.Services;

namespace NotesKeeper.ViewModels
{
    public class Grouping<TKey, TItem> : List<TItem>, IGrouping<TKey, TItem>
    {
        public TKey Key { get; private set; }

        public Grouping(TKey key, IEnumerable<TItem> items)
        {
            Key = key;
            foreach (var item in items)
                this.Add(item);
        }
    }

    public partial class CategoryListViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Category> Categories { get; set; }
        public ObservableRangeCollection<Note> Notes { get; set; }
        public List<Grouping<Category, Note>> GroupedNotes { get; set; }
        public AsyncCommand RefreshCommand { get; }

        public CategoryListViewModel()
        {
            Title = "My Notes";

            Categories = new ObservableRangeCollection<Category>();
            Notes = new ObservableRangeCollection<Note>();
            RefreshCommand = new AsyncCommand(Refresh);

        }

        public async Task Refresh()
        {
            IsBusy = true;
            await Task.Delay(2000);
            Categories.Clear();
            Notes.Clear();

            var categoriesList = await CategoryService.GetCategories();
            var notesList = await NoteService.GetNotes();
            

            Categories.AddRange(categoriesList);
            Notes.AddRange(notesList);

            GroupedNotes = Notes
                .GroupBy(n => Categories.First(c => c.Id == n.CategoryId))
                .Select(g => new Grouping<Category, Note>(g.Key, g))
            .ToList();


            IsBusy = false;
        }

        [RelayCommand]
        public async Task AddCategory()
        {
            var name = await App.Current.MainPage.DisplayPromptAsync("Add category", "Name");

            await CategoryService.AddCategory(name);
            await Refresh();
        }

        [RelayCommand]
        public async Task RemoveCategory(Category category)
        {
            await CategoryService.DeleteCategory(category.Id);

            await Refresh();
        }
        [RelayCommand]
        public async Task UpdateCategory(Category category)
        { 
            var newName = await App.Current.MainPage.DisplayPromptAsync("Update category", "Name", initialValue: category.Name);
            if (newName != null)
            {
                category.Name = newName;
             
                await CategoryService.UpdateCategory(category.Id, category.Name);
                await Refresh();
            }
        }
        public string GetCategoryName(Guid id)
        {
            return CategoryService.GetCategory(id).Result.Name;
        }
        
    }
}
