using CommunityToolkit.Maui.Views;
using NotesKeeper.Models;
using NotesKeeper.ViewModels;

namespace NotesKeeper.Pages;

public partial class EditNotePopup : Popup
{
	public EditNotePopup(Note note)
	{
		InitializeComponent();

		var vm = new EditNoteViewModel(note);
		BindingContext = vm;
		
	}
}