using Core.Models;

namespace Core.Services;

public static class LocalStorageExtensions
{
    public static async Task<T> Load<T>(this ILocalStorage<T> localStorage, int id) where T : class
    {
        var item = await localStorage.TryLoad(id);
        if (item != null)
        {
            return item;
        }

        throw new InvalidOperationException($"Could not load object of type [{typeof(T)}] with id [{id}].");
    }
}