using NotesKeeper.Models;
using NotesKeeper.ViewModels;
namespace NotesKeeper
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        { 
            base.OnAppearing();

            var vm = (NotesViewModel)BindingContext;
            await vm.RefreshCommand.ExecuteAsync();
        }



    }

}
