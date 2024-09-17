using ExpedicaoApp.Model;
using ExpedicaoApp.ViewModels;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ExpedicaoApp.DataBaseLocal;

namespace ExpedicaoApp.Views.Romaneio;

public partial class Romaneio : ContentPage
{

    RomaneioRepository RomaneioRepository;

    public Romaneio(RomaneioViewModel romaneioViewModel, RomaneioRepository romaneioRepository)
	{
		InitializeComponent();
        BindingContext = romaneioViewModel;
        RomaneioRepository = romaneioRepository;
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {

        await GetAprovadosTaskAsync();
        await GetTrasportadorasTaskAsync();


    }

    private async Task GetAprovadosTaskAsync()
    {
        RomaneioViewModel vm = (RomaneioViewModel)BindingContext;
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
                vm.Aprovados = JsonConvert.DeserializeObject<ObservableCollection<QryAprovados>>(responseBody);
            }
            else
            {
                Console.WriteLine($"Erro: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro ao carregar Siglas", ex.Message, "OK");
        }
    }

    private async Task GetTrasportadorasTaskAsync()
    {
        RomaneioViewModel vm = (RomaneioViewModel)BindingContext;
        try
        {
            string apiUrl = "https://api.cipolatti.com.br:44366/api/Tranportadora/Transportadoras";
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
                vm.Tranportadoras = JsonConvert.DeserializeObject<ObservableCollection<TranportadoraModel>>(responseBody);
            }
            else
            {
                Console.WriteLine($"Erro: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro ao carregar Siglas", ex.Message, "OK");
        }
    }
}