using ExpedicaoApp.Model;
using Newtonsoft.Json;
using SQLite;
using System.Collections.ObjectModel;

namespace ExpedicaoApp.DataBaseLocal
{
    public class LookupRepository : IDisposable
    {
        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await database.CreateTableAsync<LookupModel>();
        }

        public async Task<int> GetTotItemAsync()
        {
            try
            {
                await Init();
                return await database.Table<LookupModel>().CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LookupModel> GetItensAsync(string qrcode)
        {
            try
            {
                await Init();
                return await database.Table<LookupModel>().Where(t => t.Qrcode == qrcode).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LookupModel> GetItenCodigoAsync(string qrcode)
        {
            try
            {
                await Init();
                return await database.Table<LookupModel>().Where(t => t.Qrcode.Contains(qrcode)).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> SaveItemAsync(LookupModel item)
        {
            try
            {
                await Init();
                return await database.InsertAsync(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAllItems<T>()
        {
            try
            {
                await Init();
                return await database.DeleteAllAsync<T>() != 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task LookupAsync(string sigla)
        {
            try
            {
                await Init();
                HttpClientHandler handler = new()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };
                using HttpClient client = new(handler);
                string apiUrl = "https://api.cipolatti.com.br:44366/api/Lookup/LookupBySigla"; //api/Lookup/LookupBySigla?sigla=TOP
                //string parametro = qrCode;
                foreach (var item in sigla.Split(','))
                {
                    string urlComParametro = $"{apiUrl}?sigla={item}";
                    HttpResponseMessage response = await client.GetAsync(urlComParametro);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var lookups = JsonConvert.DeserializeObject<ObservableCollection<LookupModel>>(responseBody);
                        foreach (var lookup in lookups)
                        {
                            SaveItemAsync(lookup);
                        }
                    }
                    else
                    {
                        //await DisplayAlert("Erro", $"{response.StatusCode} - {response.ReasonPhrase}", "OK");
                        throw new InvalidOperationException($"{response.StatusCode} - {response.ReasonPhrase}");
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
        }


        public void Dispose() => throw new NotImplementedException();
    }
}
