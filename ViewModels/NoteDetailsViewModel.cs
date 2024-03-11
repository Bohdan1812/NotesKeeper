using CommunityToolkit.Mvvm.Input;
using NotesKeeper.Models;
using NotesKeeper.Services;

namespace NotesKeeper.ViewModels
{
    [QueryProperty(nameof(Note), nameof(Note))]
    public partial class NoteDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        Note note;

        [RelayCommand]
        Task Back() => Shell.Current.GoToAsync("..");

        [RelayCommand]
        async Task DeleteNote()
        {
            await NoteService.DeleteNote(note.Id);
            await Back();
        }

        [RelayCommand]
        public async Task Edit()
        {
            await NoteService.UpdateNote(note.Id, note.Title, note.Text);
            await Back();
        }

    }
}
