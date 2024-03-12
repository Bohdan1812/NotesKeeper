using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using NotesKeeper.Models;
using NotesKeeper.Services;

namespace NotesKeeper.ViewModels
{
   // [QueryProperty(nameof(Note), nameof(Note))]
    public partial class EditNoteViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Category> Categories { get; set; }
       

        public EditNoteViewModel(Note note)
        {

            Categories = new ObservableRangeCollection<Category>();
            Categories.AddRange(CategoryService.GetCategories().Result);
            this.note = note;


        }

        [ObservableProperty]
        Note note;

        [RelayCommand]
        Task Back() => Shell.Current.GoToAsync("..");

        [RelayCommand]
        async Task RemoveNote()
        {
            await NoteService.DeleteNote(note.Id);
            await Back();
        }

        [RelayCommand]
        public async Task Edit()
        {
            await NoteService.UpdateNote(note.Id, note.Title, note.Text, note.CategoryId);
            await Back();
        }

    }
}
