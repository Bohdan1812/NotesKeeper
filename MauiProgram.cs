using Microsoft.Extensions.Logging;
using NotesKeeper.Pages;
using NotesKeeper.ViewModels;
using CommunityToolkit.Maui;

namespace NotesKeeper
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit();
            /*builder.Services.AddSingleton<MainPage>();
            /builder.Services.AddSingleton<NotesViewModel>();
            builder.Services.AddSingleton<NoteDetailsPage>();
            builder.Services.AddSingleton<NoteDetailsViewModel>();*/
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}