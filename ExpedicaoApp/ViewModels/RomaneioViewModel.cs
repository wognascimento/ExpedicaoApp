using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpedicaoApp.Model;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text;
using ExpedicaoApp.DataBaseLocal;
using Newtonsoft.Json;
using System.Text.Json;

namespace ExpedicaoApp.ViewModels
{
    public partial class RomaneioViewModel : ObservableObject
    {
        RomaneioRepository RomaneioRepository;

        public RomaneioViewModel(RomaneioRepository romaneioRepository) 
        {
            Romaneio = new RomaneioModel();
            RomaneioRepository = romaneioRepository;
        }

        [ObservableProperty]
        ObservableCollection<QryAprovados> aprovados;

        [ObservableProperty]
        ObservableCollection<TranportadoraModel> tranportadoras;

        [ObservableProperty]
        ObservableCollection<string> condicao = ["BOA","REGULAR","PÉSSIMA"];

        //[ObservableProperty]

        private RomaneioModel _romaneio;
        public RomaneioModel Romaneio
        {
            get => _romaneio;
            set => SetProperty(ref _romaneio, value);
        }
        //RomaneioModel romaneio = new();

        [RelayCommand]
        async Task OnGravar()
        {

            if (Romaneio.ShoppingDestino == null)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "Informe a Sigla", "OK");
                return;
            }
            else if (Romaneio.Codtransportadora == 0)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "Informe a Transportadora", "OK");
                return;
            }
            else if (Romaneio.NomeMotorista == null)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "Informe o Motorista", "OK");
                return;
            }
            else if (Romaneio.TelefoneMotorista == null)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "Informe o Telefone do Motorista", "OK");
                return;
            }
            else if (Romaneio.NumeroCnh == null)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "Informe a CNH do Motorista", "OK");
                return;
            }
            else if (Romaneio.CondicaoCaminhao == null)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "Informe a Condição do Caminhão", "OK");
                return;
            }
            else if (Romaneio.PlacaCarroceria == null)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "Informe a Placa do Caminhão", "OK");
                return;
            }
            else if (Romaneio.PlacaCarroceriaCidade == null)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "Informe a Cidade do Caminhão", "OK");
                return;
            }
            else if (Romaneio.PlacaCarroceriaEstado == null)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "Informe o Estado do Caminhão", "OK");
                return;
            }
            else if (Romaneio.BauLargura == null)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "Informe a Larura do Caminhão", "OK");
                return;
            }
            else if (Romaneio.BauAltura == null)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "Informe a Altura do Caminhão", "OK");
                return;
            }
            else if (Romaneio.BauProfundidade == null)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "Informe a Profundidade do Caminhão", "OK");
                return;
            }
            else if (Romaneio.NomeConferente == null)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "Informe o Nome do Conferente", "OK");
                return;
            }

            if (Romaneio.CodRomaneiro > 0)
            {
                await RomaneioRepository.SaveItemAsync(Romaneio);
                await App.Current.MainPage.DisplayAlert("Sucesso", "Romaneio Salvo com Sucesso!!!", "OK");
                //Romaneio = new();
                return;
            }

            string apiUrl = "https://api.cipolatti.com.br:44366/api/Romaneio/romaneio";
            JsonSerializerOptions options = new()
            {
                WriteIndented = true
            };
            Romaneio.CodRomaneiro = 0;
            string jsonParametro = System.Text.Json.JsonSerializer.Serialize(Romaneio, options);
            HttpClientHandler handler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            var content = new StringContent(jsonParametro, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using HttpClient client = new(handler);
            try
            {
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Romaneio = JsonConvert.DeserializeObject<RomaneioModel>(responseBody);
                    //await RomaneioRepository.SaveItemAsync(Romaneio);
                    await App.Current.MainPage.DisplayAlert("Sucesso", "Romaneio Salvo com Sucesso!!!", "OK");

                    //bool lookup = await App.Current.MainPage.DisplayAlert("Romaneio Salvo com Sucesso!", "Deseja realizar Lookup?", "Sim", "Não");
                    //if (lookup)
                    //{
                       //await LookupRepository.DeleteAllItems<LookupModel>();
                    //}
                    //Debug.WriteLine("Answer: " + lookup);

                }
                else
                {
                    //Console.WriteLine($"Erro: {response.StatusCode} - {response.ReasonPhrase}");
                    //await App.Current.MainPage.DisplayAlert("Erro", $"Erro: {response.StatusCode} - {response.ReasonPhrase}", "OK");

                    string errorMessage = await response.Content.ReadAsStringAsync();
                    await App.Current.MainPage.DisplayAlert("Erro", errorMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ocorreu um erro:", ex.Message, "OK");
            }

        }


    }
}
