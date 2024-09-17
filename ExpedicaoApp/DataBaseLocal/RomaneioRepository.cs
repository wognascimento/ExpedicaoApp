using ExpedicaoApp.Model;
using SQLite;

namespace ExpedicaoApp.DataBaseLocal
{
    public class RomaneioRepository : IDisposable
    {

        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await database.CreateTableAsync<RomaneioModel>();
        }

        public async Task<int> SaveItemAsync(RomaneioModel item)
        {
            try
            {
                await Init();
                //if (item.CodRomaneiro != 0)
                    //return await database.UpdateAsync(item);
               // else
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

        public async Task<string> GetClientesAsync()
        {
            try
            {
                await Init();
                var result = await database.Table<RomaneioModel>().ToListAsync();
                var siglas = result.GroupBy(x => x.ShoppingDestino).Select(x=> x.Key).ToList();
                string s = string.Join(",", siglas);
                return s;
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
                await Init();
                return await database.Table<RomaneioModel>().FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose() { }
    }

    public static class Constants
    {
        public const string DatabaseFilename = "ColetorDB.db3"; //coletor.db

        public const SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}
