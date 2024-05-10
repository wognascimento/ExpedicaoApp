using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpedicaoApp.Model;
using ExpedicaoApp.Views;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace ExpedicaoApp.ViewModels
{
    public partial class EnderecamentoViewModels : ObservableObject
    {
        [ObservableProperty]
        private EnderecamentoGalpaoModel? enderecamentoGalpao;

        [ObservableProperty]
        private LookupModel? lookup;

        [ObservableProperty]
        private ObservableCollection<LookupModel> movimentacoes = [];

        [ObservableProperty]
        private List<LeitorQRCodeModel> leitorQRCode =
            [
                new LeitorQRCodeModel("ENDERECO", false),
                new LeitorQRCodeModel("VOLUME", false),
            ];

        [ObservableProperty]
        string? acao;


        [RelayCommand]
        async Task OnImage1Tapped(string acao)
        {
            
            Acao = acao;
            var lFuncunc = LeitorQRCode.FirstOrDefault(w => w.Tipo == "ENDERECO");
            if (acao == "VOLUME" && lFuncunc.Leitura == false)
                return;

            var indexOf = LeitorQRCode.IndexOf(LeitorQRCode.Find(p => p.Tipo == acao));
            await Shell.Current.GoToAsync($"{nameof(QrCodeScanner)}", true,
                new Dictionary<string, object>
                {
                    {"Acao", acao }
                });
            LeitorQRCode[indexOf].Leitura = true;
            
        }

        [RelayCommand]
        async Task OnSend()
        {
            if (Movimentacoes.Count == 0)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "Não existe(m) volume(s) para ser indereçado(s)", "OK");
                return;
            }

            foreach (LookupModel item in Movimentacoes)
            {
                var movimentacao = new MovimentacaoVolumeShoppingModel { BarcodeEndereco = EnderecamentoGalpao.Barcode, BarcodeVolume = item.Barcode, InseridoPor = "APLICATIVO", InseridoEm = DateTime.Now };

                ///api/SaidaAlmox/almoxMovSaida
                string apiUrl = "https://api.cipolatti.com.br:44366/api/MovimentacaoVolumeShopping/GravarVolume";
                JsonSerializerOptions options = new()
                {
                    WriteIndented = true
                };
                string jsonParametro = JsonSerializer.Serialize(movimentacao, options);
                HttpClientHandler handler = new()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };

                var parametro = new
                {
                    movimentacao
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

                        await App.Current.MainPage.DisplayAlert("Sucesso", responseBody, "OK");
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        //Console.WriteLine($"Erro: {response.StatusCode} - {response.ReasonPhrase}");
                        await App.Current.MainPage.DisplayAlert("Erro", $"{response.StatusCode} - {response.ReasonPhrase}", "OK");
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                    await App.Current.MainPage.DisplayAlert("Erro", $"{ex.Message}", "OK");
                }
            }
        }

    }
}
