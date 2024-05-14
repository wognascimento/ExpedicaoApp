using ExpedicaoApp.Model;
using ExpedicaoApp.ViewModels;
using Newtonsoft.Json;

namespace ExpedicaoApp.Views.Enderecamento;

[QueryProperty(nameof(QrCode), "qrCode")]
public partial class Enderecamento : ContentPage
{

    public string QrCode
    {
        set { LoadQrCode(value); }
    }
    
    public Enderecamento(EnderecamentoViewModels enderecamentoViewModels)
	{
		InitializeComponent();
        BindingContext = enderecamentoViewModels;
    }

    async Task LoadQrCode(string qrCode)
    {

        string parametro = qrCode;
        HttpClientHandler handler = new()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        };

        EnderecamentoViewModels vm = (EnderecamentoViewModels)BindingContext;
        using HttpClient client = new(handler);
        try
        {

            if (vm.Acao == "Endereço")
            {
                string apiUrl = "https://api.cipolatti.com.br:44366/api/MovimentacaoVolumeShopping/Endereco"; 
                string urlComParametro = $"{apiUrl}?Barcode={parametro}";
                HttpResponseMessage response = await client.GetAsync(urlComParametro);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    vm.EnderecamentoGalpao = JsonConvert.DeserializeObject<EnderecamentoGalpaoModel>(responseBody);
                }
                else
                {
                    await DisplayAlert("Erro", $"{response.StatusCode} - {response.ReasonPhrase}", "OK");
                }
            }
            else if (vm.Acao == "Volume")
            {
                string apiUrl = "https://api.cipolatti.com.br:44366/api/MovimentacaoVolumeShopping/Volume";
                string urlComParametro = $"{apiUrl}?qrcode={parametro}";
                HttpResponseMessage response = await client.GetAsync(urlComParametro);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    vm.Lookup = JsonConvert.DeserializeObject<LookupModel>(responseBody);
                    if (vm.Lookup != null)
                    {
                        var found = vm.Movimentacoes.FirstOrDefault(x => x.Qrcode == vm.Lookup.Qrcode);
                        if (found != null)
                        {
                            int indice = vm.Movimentacoes.IndexOf(found);
                            listView.EndRefresh();
                        }
                        else
                            vm.Movimentacoes.Add(vm.Lookup);
                    }
                }
                else
                {
                    await DisplayAlert("Erro", $"{response.StatusCode} - {response.ReasonPhrase}", "OK");
                }
            }


        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro: {ex.Message}");
        }

    }
}