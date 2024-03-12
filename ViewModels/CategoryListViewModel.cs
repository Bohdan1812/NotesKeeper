using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using NotesKeeper.Models;
using NotesKeeper.Services;

namespace NotesKeeper.ViewModels
{
    

    public partial class CategoryListViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Category> Categories { get; set; }
        public ObservableRangeCollection<Note> Notes { get; set; }
        public ObservableRangeCollection<NoteCategoryGroup> NoteCategoryGroups { get; set; }
        public AsyncCommand RefreshCommand { get; }

        public CategoryListViewModel()
        {
            Title = "My Notes";

            Categories = new ObservableRangeCollection<Category>();
            Notes = new ObservableRangeCollection<Note>();
            NoteCategoryGroups = new ObservableRangeCollection<NoteCategoryGroup>();
            RefreshCommand = new AsyncCommand(Refresh);

        }

        public async Task Refresh()
        {
            IsBusy = true;

            await Task.Delay(2000);

            Categories.Clear();
            Notes.Clear();
            NoteCategoryGroups.Clear();

            var categoriesList = await CategoryService.GetCategories();
            var notesList = await NoteService.GetNotes();
            var noteCategoryGroupList = new List<NoteCategoryGroup>();
            
            
            foreach (var category in categoriesList)
            {
                var filteredNotes = notesList.Where(n => n.CategoryId == category.Id).ToList();
                noteCategoryGroupList.Add(new NoteCategoryGroup(category, filteredNotes));
            }

            Categories.AddRange(categoriesList);
            Notes.AddRange(notesList);
            NoteCategoryGroups.AddRange(noteCategoryGroupList);

            


            IsBusy = false;
        }

        [RelayCommand]
        public async Task AddNote(Category category)
        {
            var title = await App.Current.MainPage.DisplayPromptAsync("Add note", "Title");
            var text = await App.Current.MainPage.DisplayPromptAsync("Add note", "Text");
            if (title != null && text != null & category != null)
            {
                await NoteService.AddNote(title, text, category.Id);
                await Refresh(); 
            }
        }

        [RelayCommand]
        public async Task AddCategory()
        {
            var name = await App.Current.MainPage.DisplayPromptAsync("Add category", "Name");
            if (name != null)
            {
                await CategoryService.AddCategory(name);
                await Refresh();
            }
        }

        [RelayCommand]
        public async Task RemoveCategory(NoteCategoryGroup categoryGroup)
        {
            await CategoryService.DeleteCategory(categoryGroup.category.Id);
            NoteCategoryGroups.Remove(categoryGroup);

            await Refresh();
        }
        [RelayCommand]
        public async Task UpdateCategory(NoteCategoryGroup categoryGroup)
        { 
            var newName = await App.Current.MainPage.DisplayPromptAsync("Update category", "Name", initialValue: categoryGroup.category.Name);
            if (newName != null)
            {

                categoryGroup.category.Name = newName;
                                
                await CategoryService.UpdateCategory(categoryGroup.category.Id, newName);
                await Refresh();
            }
        }
        public string GetCategoryName(Guid id)
        {
            return CategoryService.GetCategory(id).Result.Name;
        }
        
    }
}
