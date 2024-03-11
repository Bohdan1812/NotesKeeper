using NotesKeeper.Pages;

namespace NotesKeeper
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(NoteDetailsPage), typeof(NoteDetailsPage));
        }
    }
}
