using BarcodeScanner.Mobile;
using ExpedicaoApp.DataBaseLocal;
using ExpedicaoApp.ViewModels;
using ExpedicaoApp.Views;
using ExpedicaoApp.Views.Enderecamento;
using ExpedicaoApp.Views.PreConferencia;
using ExpedicaoApp.Views.Romaneio;
using ExpedicaoApp.Views.VolumeShopping;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
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
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    //fonts.AddFont("Roboto-Medium.ttf", "Roboto-Medium");
                    //fonts.AddFont("Roboto-Regular.ttf", "Roboto-Regular");
                })
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddBarcodeScannerHandler();
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton(AudioManager.Current);

            //builder.ConfigureSyncfusionCore();

            //builder.Services.AddSingleton<MainPage>();
            //builder.Services.AddSingleton<MainPageViewModel>();

            builder.Services.AddTransient<Enderecamento>();
            builder.Services.AddTransient<EnderecamentoViewModels>();

            builder.Services.AddTransient<QrCodeScanner>();
            builder.Services.AddTransient<LeitorQrCodeViewModel>();
            
            builder.Services.AddTransient<Romaneio>();
            builder.Services.AddTransient<RomaneioViewModel>();

            builder.Services.AddTransient<VolumeShopping>();
            builder.Services.AddTransient<VolumeShoppingViewModel>();

            builder.Services.AddTransient<PreConferencia>();
            builder.Services.AddTransient<PreConferenciaViewModel>();


            builder.Services.AddSingleton(new RomaneioRepository());
            builder.Services.AddSingleton(new LookupRepository());
            builder.Services.AddSingleton(new VolumeRepository());





            return builder.Build();
        }
    }
}
