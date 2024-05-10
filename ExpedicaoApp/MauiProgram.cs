using BarcodeScanner.Mobile;
using ExpedicaoApp.ViewModels;
using ExpedicaoApp.Views;
using ExpedicaoApp.Views.Enderecamento;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using Telerik.Maui.Controls.Compatibility;

namespace ExpedicaoApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseTelerik()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddBarcodeScannerHandler();
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.ConfigureSyncfusionCore();

            //builder.Services.AddSingleton<MainPage>();
            //builder.Services.AddSingleton<MainPageViewModel>();

            builder.Services.AddTransient<Enderecamento>();
            builder.Services.AddTransient<EnderecamentoViewModels>();

            builder.Services.AddTransient<QrCodeScanner>();
            builder.Services.AddTransient<LeitorQrCodeViewModel>();

            return builder.Build();
        }
    }
}
