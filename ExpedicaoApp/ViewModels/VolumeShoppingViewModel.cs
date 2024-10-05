using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpedicaoApp.DataBaseLocal;
using ExpedicaoApp.Model;
using Newtonsoft.Json;
using Plugin.Maui.Audio;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ExpedicaoApp.ViewModels
{
    public partial class VolumeShoppingViewModel : ObservableObject
    {
        readonly VolumeRepository volume;
        readonly LookupRepository lookup;
        readonly RomaneioRepository romaneio;

        private readonly IAudioManager audioManager;

        public VolumeShoppingViewModel(VolumeRepository volume, LookupRepository lookup, RomaneioRepository romaneio, IAudioManager audioManager)
        {
            this.volume = volume;
            this.lookup = lookup;
            this.romaneio = romaneio;

            LookupModel = new LookupModel();
            this.audioManager = audioManager;
        }

        [ObservableProperty]
        ObservableCollection<QryAprovados> aprovados;

        [ObservableProperty]
        string siglas;

        [ObservableProperty]
        string caminhao;

        [ObservableProperty]
        ObservableCollection<QryAprovados> aprovadosSelectedItems = [];

        [ObservableProperty]
        bool isObjectScanner;

        [ObservableProperty]
        bool isLoading;

        [ObservableProperty]
        LookupModel? lookupModel;

        [ObservableProperty]
        RomaneioModel? romaneioModel;

        private int _x;
        private int _y;

        public int X
        {
            get => _x;
            set
            {
                SetProperty(ref _x, value);
                OnPropertyChanged(nameof(ButtonText)); // Atualiza o texto do botão quando X mudar
            }
        }

        public int Y
        {
            get => _y;
            set
            {
                SetProperty(ref _y, value);
                OnPropertyChanged(nameof(ButtonText)); // Atualiza o texto do botão quando Y mudar
            }
        }

        public string ButtonText => $"Enviar {X} de {Y} volumes";

        public async Task SiglasAsync()
        {
            try
            {
                Siglas = await romaneio.GetClientesAsync();
                Y = await lookup.GetTotItemAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RomaneioModel> GetRomaneioAsync()
        {
            try
            {
                return await romaneio.GetRomaneioAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<int>> GetTotItensCarregadosAsync()
        {
            try
            {
                return await volume.GetTotItensAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<LookupModel> GetVolumeAsync(string qrcode)
        {
            try
            {
               return await lookup.GetItensAsync(qrcode);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveVolumeAsync(VolumeShoppinModel item)
        {
            try
            {
                await volume.SaveItemAsync(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> GetItemsAsync(string barcode)
        {
            try
            {
                return await volume.GetItemsAsync(barcode);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GetAprovadosTaskAsync()
        {
            try
            {
                string apiUrl = "https://api.cipolatti.com.br:44366/api/Aprovado/SelecionarTodos";
                HttpClientHandler handler = new()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };
                using HttpClient client = new(handler);
                string urlComParametro = $"{apiUrl}";
                HttpResponseMessage response = await client.GetAsync(urlComParametro);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Aprovados = JsonConvert.DeserializeObject<ObservableCollection<QryAprovados>>(responseBody);
                }
                else
                {
                    Console.WriteLine($"Erro: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro ao carregar Siglas", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task FinalizarAsync()
        {
            var confim = await App.Current.MainPage.DisplayAlert("Finalização", "Deseja finalizar o Carregamento?", "Sim", "Não");
            if (confim)
            {
                await romaneio.DeleteAllItems<RomaneioModel>();
                await lookup.DeleteAllItems<LookupModel>();
                await volume.DeleteAllItens<VolumeShoppinModel>();
                await Shell.Current.GoToAsync("..");
            }
        }

        [RelayCommand]
        async Task VolumeAsync()
        {
            try
            {
                IsObjectScanner = true;
                IsLoading = true;

                if(AprovadosSelectedItems.Count > 0)
                    Siglas = string.Join(",", AprovadosSelectedItems.Select(a => a.SiglaServ));

                //Debug.WriteLine(Siglas);
                await lookup.DeleteAllItems<LookupModel>();

                //await App.Current.MainPage.DisplayPromptAsync("Lookup", "Lookup finalizado!!!", "OK");

                caminhao = await App.Current.MainPage.DisplayPromptAsync("Informe o número do caminhão separado por ','", "Por exemplo 1,2,3...");

                await lookup.LookupAsync(Siglas, caminhao);
                Y = await lookup.GetTotItemAsync();
                await App.Current.MainPage.DisplayAlert("Lookup", "Lookup finalizado!!!", "OK");

                IsObjectScanner = false;
                IsLoading = false;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                IsObjectScanner = false;
                IsLoading = false;
            }
        }

        [RelayCommand]
        async Task SendVolumesAsync()
        {
            try
            {
                IsObjectScanner = true;
                IsLoading = true;

                await volume.SendVolumesAsync();
                await lookup.DeleteAllItems<LookupModel>();
                await lookup.LookupAsync(Siglas, caminhao);
                Y = await lookup.GetTotItemAsync();
                var tot = await GetTotItensCarregadosAsync();
                X = tot.Count;
                await App.Current.MainPage.DisplayAlert("Envio", "Volumes Enviados com Sucesso!!!", "OK");

                IsObjectScanner = false;
                IsLoading = false;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                IsObjectScanner = false;
                IsLoading = false;
            }
        }

        [RelayCommand]
        async Task SaveVolumeManualAsync()
        {
            try
            {
                if (LookupModel.Qrcode.Length != 5) //30394
                {
                    App.Current.MainPage.DisplayAlert("Volume", "Quantidade de caracteres inválida para o volume.", "OK");
                    return;
                }
                var l = await lookup.GetItenCodigoAsync(LookupModel.Qrcode);
                if (l != null)
                {
                    var vol = new VolumeShoppinModel { Barcode = l.Barcode, Caminhao = RomaneioModel.PlacaCaminhao, Data = RomaneioModel.DataCarregamento, Resp = RomaneioModel.NomeConferente, manual = true };
                    await volume.SaveItemAsync(vol);
                    var pSucess = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("sucess.mp3"));
                    pSucess.Play();
                    X++;
                }
                else 
                {
                    var pError = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("error.mp3"));
                    pError.Play();
                    await App.Current.MainPage.DisplayAlert("Result", "Volume não Presednte no Lookup", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task SendPreconferenciaAsync()
        {
            try
            {
                IsObjectScanner = true;
                IsLoading = true;

                await volume.SendPreConferenciaAsync();
                await lookup.DeleteAllItems<LookupModel>();
                await lookup.LookupAsync(Siglas, caminhao);
                Y = await lookup.GetTotItemAsync();
                var tot = await GetTotItensCarregadosAsync();
                X = tot.Count;
                await App.Current.MainPage.DisplayAlert("Envio", "Volumes Enviados com Sucesso!!!", "OK");

                IsObjectScanner = false;
                IsLoading = false;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                IsObjectScanner = false;
                IsLoading = false;
            }
        }
    }
}
