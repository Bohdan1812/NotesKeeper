using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using NotesKeeper.Models;
using NotesKeeper.Pages;
using NotesKeeper.Services;
using System.Windows.Input;

namespace NotesKeeper.ViewModels
{
    public partial class NotesViewModel:BaseViewModel
    {
        public ObservableRangeCollection<Note> Notes { get; set; } 

        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand<Note> RemoveCommand { get; }
        public AsyncCommand<Note> NavigateToDetailsCommand { get; }

        public NotesViewModel() 
        {
            Title = "MyNotes";

            Notes = new ObservableRangeCollection<Note>();

            RefreshCommand = new AsyncCommand(Refresh);
            AddCommand = new AsyncCommand(Add);
            RemoveCommand = new AsyncCommand<Note>(Remove);
            NavigateToDetailsCommand = new AsyncCommand<Note>(NavigateToDetails);

        }

        async Task Add()
        {
            var title = await App.Current.MainPage.DisplayPromptAsync("Title", "Title");
            var text = await App.Current.MainPage.DisplayPromptAsync("Text", "Text");
            //await NoteService.AddNote(title, text);
            await Refresh();
        }

        public async Task Remove(Note note)
        {
            await NoteService.DeleteNote(note.Id);
            await Refresh();
        }

        public async Task Refresh()
        {
            IsBusy = true;
            await Task.Delay(2000);
            Notes.Clear();

            var notesList = await NoteService.GetNotes();

            Notes.AddRange(notesList);

            IsBusy = false;
        }


      
        public Task NavigateToDetails(Note note) =>  Shell.Current.GoToAsync($"{nameof(NoteDetailsPage)}",
            new Dictionary<string, object> 
            {
                [nameof(Note)] = note
            });
    }
}
