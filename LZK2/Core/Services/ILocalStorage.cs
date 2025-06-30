using Core.Models;

namespace Core.Services;

/// <summary>
/// Defines a generic interface for local storage operations.
/// </summary>
/// <typeparam name="T">The type of entity to store.</typeparam>
public interface ILocalStorage<T> where T : class
{
    /// <summary>
    /// Tries to load an entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<T?> TryLoad(int id);

    /// <summary>
    /// Saves the specified entity to local storage.
    /// </summary>
    /// <param name="entity">The entity to save.</param>
    /// <returns>True if the operation succeeded; otherwise, false.</returns>
    Task<bool> Save(T entity);

    /// <summary>
    /// Deletes the specified entity from local storage.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>True if the operation succeeded; otherwise, false.</returns>
    Task<bool> Delete(T entity);

    /// <summary>
    /// Loads all entities from local storage.
    /// </summary>
    /// <returns>A list of all entities.</returns>
    Task<List<T>> LoadAll();

    /// <summary>
    /// Deletes all entities from local storage.
    /// </summary>
    /// <returns>True if the operation succeeded; otherwise, false.</returns>
    Task<bool> DeleteAll();

    /// <summary>
    /// Initializes the local storage.
    /// </summary>
    Task Initialize();
}