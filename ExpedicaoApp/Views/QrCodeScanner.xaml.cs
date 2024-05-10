using BarcodeScanner.Mobile;
using ExpedicaoApp.ViewModels;

namespace ExpedicaoApp.Views;

public partial class QrCodeScanner : ContentPage
{
	public QrCodeScanner(LeitorQrCodeViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
#if ANDROID
        BarcodeScanner.Mobile.Methods.AskForRequiredPermission();
        BarcodeScanner.Mobile.Methods.SetSupportBarcodeFormat(BarcodeFormats.QRCode);
#endif
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        LeitorQrCodeViewModel vm = (LeitorQrCodeViewModel)BindingContext;

        Title = $@"CAPTURAR QRCODE {vm.Acao}";
    }

    private void CameraView_OnDetected(object sender, OnDetectedEventArg e)
    {
        List<BarcodeResult> obj = e.BarcodeResults;

        string result = string.Empty;
        for (int i = 0; i < obj.Count; i++)
        {
            //result += $"Type : {obj[i].BarcodeType}, Value : {obj[i].DisplayValue}{Environment.NewLine}";
            result += obj[i].DisplayValue;
        }

        this.Dispatcher.Dispatch(async () =>
        {
            //await DisplayAlert("Result", result, "OK");
            //Camera.IsScanning = true;

            //await Shell.Current.GoToAsync("..");
            await Shell.Current.GoToAsync($"..?qrCode={result}");
        });

        //Canvas.InvalidateSurface();
    }

}