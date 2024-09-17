using ExpedicaoApp.Model;
using SQLite;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;


namespace ExpedicaoApp.DataBaseLocal
{
    public class VolumeRepository : IDisposable
    {
        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await database.CreateTableAsync<VolumeShoppinModel>();
        }

        public async Task<bool> DeleteAllItens<T>()
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

        public async Task<int> SaveItemAsync(VolumeShoppinModel item)
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

        public async Task<int> UpdateItemAsync(VolumeShoppinModel item)
        {
            try
            {
                await Init();
                return await database.UpdateAsync(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<VolumeShoppinModel>> GetItensAsync()
        {
            try
            {
                await Init();
                return await database.Table<VolumeShoppinModel>().Where(t => t.Enviado == false).ToListAsync();
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
                await Init();
                return await database.Table<VolumeShoppinModel>().Where(t => t.Barcode == barcode).CountAsync() != 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<int>> GetTotItensAsync()
        {
            try
            {
                await Init();
                string query = @"SELECT COUNT(*) as Count FROM VolumeShoppinModel WHERE Enviado = 0 GROUP BY Barcode";
                var result = await database.QueryAsync<int>(query);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendVolumesAsync()
        {
            try
            {
                await Init();
                foreach (var item in await GetItensAsync())
                {
                    string apiUrl = "https://api.cipolatti.com.br:44366/api/ConfCargaGeral/GravarVolume"; //api/ConfCargaGeral/GravarVolume
                    JsonSerializerOptions options = new()
                    {
                        WriteIndented = true
                    };
                    string jsonParametro = JsonSerializer.Serialize(item, options);
                    HttpClientHandler handler = new()
                    {
                        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                    };

                    var content = new StringContent(jsonParametro, Encoding.UTF8, "application/json");
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    using HttpClient client = new(handler);
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        //Romaneio = JsonConvert.DeserializeObject<RomaneioModel>(responseBody);
                        item.Enviado = true;
                        await UpdateItemAsync(item);
                    }
                    else
                    {
                        // await App.Current.MainPage.DisplayAlert("Erro", $"Erro: {response.StatusCode} - {response.ReasonPhrase}", "OK");
                        throw new InvalidOperationException($"{response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task SendPreConferenciaAsync()
        {
            try
            {
                await Init();
                foreach (var item in await GetItensAsync())
                {
                    string apiUrl = "https://api.cipolatti.com.br:44366/api/ConfCargaGeral/GravarPreConferencia"; //api/ConfCargaGeral/GravarVolume
                    JsonSerializerOptions options = new()
                    {
                        WriteIndented = true
                    };
                    string jsonParametro = JsonSerializer.Serialize(item, options);
                    HttpClientHandler handler = new()
                    {
                        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                    };

                    var content = new StringContent(jsonParametro, Encoding.UTF8, "application/json");
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    using HttpClient client = new(handler);
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        //Romaneio = JsonConvert.DeserializeObject<RomaneioModel>(responseBody);
                        item.Enviado = true;
                        await UpdateItemAsync(item);
                    }
                    else
                    {
                        // await App.Current.MainPage.DisplayAlert("Erro", $"Erro: {response.StatusCode} - {response.ReasonPhrase}", "OK");
                        throw new InvalidOperationException($"{response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }





        public void Dispose() { }
    }
}
