using Core.Models;
using SQLite;

namespace Core.Services;

public class SqliteLocalStorage<T> : ILocalStorage<T> where T : class, new()
{
    private readonly SQLiteAsyncConnection _connection;

    public SqliteLocalStorage(LocalStorageSettings settings)
    {
        var options = new SQLiteConnectionString(settings.DatabasePath);
        _connection = new SQLiteAsyncConnection(options);
    }

    public async Task Initialize()
    {
        // Drop and recreate the table to ensure schema matches the model (for development only!)
        await _connection.DropTableAsync<T>();
        await _connection.CreateTableAsync<T>();
    }

    public async Task<bool> Delete(T entity)
    {
        var pkProp = typeof(T).GetProperty("Id");
        if (pkProp == null) throw new InvalidOperationException($"Type {typeof(T).Name} does not have an Id property.");
        var pk = pkProp.GetValue(entity);
        if (pk == null) return false;
        return await _connection.DeleteAsync<T>(pk) == 1;
    }

    public Task<List<T>> LoadAll()
    {
        return _connection.Table<T>().ToListAsync();
    }

    public async Task<bool> DeleteAll()
    {
        return await _connection.DeleteAllAsync<T>() >= 0;
    }

    public async Task<bool> Save(T entity)
    {
        // Anpassung: Wenn das Entity eine Eigenschaft "PLZ" hat und diese null ist, auf leeren String setzen
        var plzProp = typeof(T).GetProperty("PLZ");
        if (plzProp != null && plzProp.PropertyType == typeof(string))
        {
            var plzValue = plzProp.GetValue(entity) as string;
            if (plzValue == null)
            {
                plzProp.SetValue(entity, string.Empty);
            }
        }

        return await _connection.InsertOrReplaceAsync(entity) == 1;
    }

    public async Task<T?> TryLoad(int id)
    {
        return await _connection.FindAsync<T?>(id);
    }
}