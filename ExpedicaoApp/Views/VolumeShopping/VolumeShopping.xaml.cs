using BarcodeScanner.Mobile;
using ExpedicaoApp.ViewModels;
using Plugin.Maui.Audio;

namespace ExpedicaoApp.Views.VolumeShopping;

public partial class VolumeShopping : ContentPage
{

    private readonly IAudioManager audioManager;

    public VolumeShopping(VolumeShoppingViewModel volumeShoppingViewModel, IAudioManager audioManager)
	{
		InitializeComponent();
        BindingContext = volumeShoppingViewModel;
        this.audioManager = audioManager;

#if ANDROID
        Methods.AskForRequiredPermission();
        Methods.SetSupportBarcodeFormat(BarcodeFormats.QRCode);
#endif
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
		try
		{
            VolumeShoppingViewModel vm = (VolumeShoppingViewModel)BindingContext;
			await vm.SiglasAsync();
            vm.RomaneioModel = await vm.GetRomaneioAsync();
            var tot =  await vm.GetTotItensCarregadosAsync();
            vm.X = tot.Count;
        }
        catch (Exception ex)
		{
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void CameraView_OnDetected(object sender, OnDetectedEventArg e)
    {
        VolumeShoppingViewModel vm = (VolumeShoppingViewModel)BindingContext;
        List<BarcodeResult> obj = e.BarcodeResults;
        string result = string.Empty;
        for (int i = 0; i < obj.Count; i++)
        {
            result += obj[i].DisplayValue;
        }
        this.Dispatcher.Dispatch(async () =>
        {
            try
            {
                var lookup = await vm.GetVolumeAsync(result);
                vm.LookupModel = lookup;
                if (lookup != null)
                {
                    var encontrado = await vm.GetItemsAsync(lookup.Barcode);
                    await vm.SaveVolumeAsync(new Model.VolumeShoppinModel { Barcode = lookup.Barcode, Caminhao = vm.RomaneioModel.PlacaCaminhao, Data = vm.RomaneioModel.DataCarregamento, Resp = vm.RomaneioModel.NomeConferente });
                    var pSucess = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("sucess.mp3"));
                    pSucess.Play();

                    Camera.IsScanning = true;
                    if (!encontrado)
                        vm.X++;
                }
                else
                {
                    var pError = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("error.mp3"));
                    pError.Play();
                    Camera.IsScanning = false;
                    await DisplayAlert("Result", "Volume não Presednte no Lookup", "OK");
                    Camera.IsScanning = true;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                Camera.IsScanning = true;
            }
        });
    }
}