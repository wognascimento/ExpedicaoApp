using BarcodeScanner.Mobile;
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

            return builder.Build();
        }
    }
}
