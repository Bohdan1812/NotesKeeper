using CommunityToolkit.Maui.Views;
using NotesKeeper.Models;
using NotesKeeper.Services;
using NotesKeeper.ViewModels;

namespace NotesKeeper.Pages;

public partial class EditNotePopup : Popup
{
	List<Category> _categories;
    Note _note;
	public EditNotePopup(Note note, List<Category> categories)
	{
		InitializeComponent();

		_categories = categories;
		_note = note;
		
		Initialize();
		
		
		
	}
    private void Initialize()
    {

        var selectedName = _categories.Find(c => c.Id.Equals(_note.CategoryId)).Name;

		var categoryNames = _categories.Select(category => category.Name).ToList();

        categoryPicker.ItemsSource = categoryNames;
		categoryPicker.SelectedItem = selectedName;


        titleEditor.Text = _note.Title;
		textEditor.Text = _note.Text;

		createdTime.Text = $"Created: {_note.CreatedTime}";
        updatedTime.Text = $"LastUpdated: {_note.UpdatedTime}";
    }

    private void EditNote(object sender, EventArgs e)
    {
		var titleEditorText = titleEditor.Text;

		if (titleEditorText != null)
			_note.Title = titleEditorText;

        var textEditorText = textEditor.Text;

        if (textEditorText != null)
            _note.Text = textEditorText;

		var selectedItem = _categories.Where(c => c.Name == categoryPicker.SelectedItem).First();

		if(selectedItem != null)
		{
			var selectedCategoryId = selectedItem.Id;
			if (selectedCategoryId != Guid.Empty)
				_note.CategoryId = selectedCategoryId;

			Close(_note);
		}	
    }

    private void Cancel(object sender, EventArgs e)
    {
		this.Close();
    }
}