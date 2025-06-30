using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Services;

public class PersonService : IPersonService
{
    private readonly ILocalStorage<Person> _localStorage;

    public PersonService(ILocalStorage<Person> localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task Save(Person person)
    {
        if (person == null)
            throw new ArgumentNullException(nameof(person));

        await _localStorage.Save(person);
    }

    public async Task<List<Person>> Load()
    {
        await _localStorage.Initialize();
        var people = await _localStorage.LoadAll();
        return people;
    }
}