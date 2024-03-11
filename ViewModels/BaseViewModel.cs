
namespace NotesKeeper.ViewModels
{
    
    public partial class BaseViewModel : ObservableObject
    {
        public BaseViewModel()
        { 
            
        }
        
        [ObservableProperty] 
        //[AlsoNotifyChangeFor(nameof(IsNotBusy))] застарілий атрибут
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title;

        public bool IsNotBusy => !IsBusy;
    }
}
