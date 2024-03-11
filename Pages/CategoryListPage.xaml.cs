using NotesKeeper.ViewModels;

namespace NotesKeeper.Pages;

public partial class CategoryListPage : ContentPage
{
	public CategoryListPage()
	{
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var vm = (CategoryListViewModel)BindingContext;
        await vm.RefreshCommand.ExecuteAsync();
    }
}