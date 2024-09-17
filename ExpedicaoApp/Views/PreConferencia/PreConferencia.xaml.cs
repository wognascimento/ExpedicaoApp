using BarcodeScanner.Mobile;
using ExpedicaoApp.Model;
using ExpedicaoApp.ViewModels;
using Plugin.Maui.Audio;
using Syncfusion.Maui.Inputs;

namespace ExpedicaoApp.Views.PreConferencia;

public partial class PreConferencia : ContentPage
{

    private readonly IAudioManager audioManager;

    public PreConferencia(VolumeShoppingViewModel volumeShoppingViewModel, IAudioManager audioManager)
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
            await vm.GetAprovadosTaskAsync();
            vm.RomaneioModel = await vm.GetRomaneioAsync();
            var tot = await vm.GetTotItensCarregadosAsync();
            vm.X = tot.Count;

            //foreach(var ap in vm.Aprovados.Where(s => vm.Siglas.Contains(s.SiglaServ)).ToList())
            //{
                //vm.AprovadosSelectedItems.Add(ap);
            //}
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void OnSelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        VolumeShoppingViewModel vm = (VolumeShoppingViewModel)BindingContext;
        var combo = (SfComboBox)sender;
        //vm.AprovadosSelectedItems = combo.SelectedItems as ObservableCollection<QryAprovados>;
        vm.AprovadosSelectedItems.Clear();
        foreach (var item in combo.SelectedItems)
        {
            vm.AprovadosSelectedItems.Add((QryAprovados)item);
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
                    await vm.SaveVolumeAsync(new VolumeShoppinModel { Barcode = lookup.Barcode, Caminhao = "PRE", Data = DateTime.Now.Date, Resp = "PRE-CONFERENCIA" });
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
            }
        });
    }
}