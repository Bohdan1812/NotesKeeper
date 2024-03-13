using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using NotesKeeper.Models;
using NotesKeeper.Pages;
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
            Title = "Note Keeper";

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


               
            if (name != null && !Categories.Any(c => c.Name == name))
            {
                await CategoryService.AddCategory(name);
                await Refresh();

            }
            else
                Shell.Current.DisplayAlert("New category name error", $"There is already category with name {name}!", "OK");
        }

        [RelayCommand]
        public async Task RemoveCategory(NoteCategoryGroup categoryGroup)
        {
            var deleteAgreemant = await Shell.Current.DisplayAlert("Category delete alert", $"Delete category{categoryGroup.category.Name} ?\n" +
                $" (All notes with this category also will be deleted!)", "Ok", "Cancel");
            if (deleteAgreemant)
            {
                await CategoryService.DeleteCategory(categoryGroup.category.Id);
                NoteCategoryGroups.Remove(categoryGroup);

                await Refresh();
            }
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

        [RelayCommand]
        public async Task RemoveNote(Note note)
        {
            await NoteService.DeleteNote(note.Id);
            await Refresh();
        }

        [RelayCommand]
        public async Task ShowNoteEditPopup(Note note)
        {
            var editNotePopup = new EditNotePopup(note, Categories.ToList());
            var editPopupResult = await Shell.Current.CurrentPage.ShowPopupAsync(editNotePopup);
            if (editPopupResult != null)
            {
                var editedNote = (Note)editPopupResult;
                await NoteService.UpdateNote(editedNote.Id, editedNote.Title, editedNote.Text, editedNote.CategoryId);

                await Refresh();
            }
        }
    }
}
