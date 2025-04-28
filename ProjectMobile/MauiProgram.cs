using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;


using ProjectMobile.DataServices;
using ProjectMobile.ViewModels;
using ProjectMobile.Views;

namespace ProjectMobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                  .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            builder.Services.AddSingleton<IBookService, BookService>();

            builder.Services.AddTransient<IBookService, BookService>();

            builder.Services.AddTransient<AddOrUpdateBookPageViewModel>();
            builder.Services.AddTransient<AddOrUpdateBookPage>();


            builder.Services.AddTransient<BooklistHomePageViewmodel>();
            builder.Services.AddTransient<BooklistHomePage>();

            builder.Services.AddTransient<BookDetailsPageViewmodel>();
            builder.Services.AddTransient<BookDetailsPage>();

            builder.Services.AddSingleton<IBookService, BookService>();
            builder.Services.AddTransient<BookDetailsPageViewmodel>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
