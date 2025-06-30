using System.Threading.Tasks;
using Core.Models;

namespace Core.Services;

public interface IPersonService
{
    Task Save(Person person);
    Task<List<Person>> Load();
}