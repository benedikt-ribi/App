using MLZ2025.Core.Model;
using MLZ2025.Core.Services;
using MLZ2025.Shared.Model;
using MLZ2025.Shared.Services;

namespace MLZ2025.Core.Tests;

[TestFixture]
public class SqliteTests : TestsBase
{
    [Test]
    public void TestGetAndAddData()
    {
        var serviceProvider = CreateServiceProvider();
        using (var dataAccess = serviceProvider.GetRequiredService<DataAccess<DatabaseAddress>>())
        {
            dataAccess.DeleteAll();

            var addressCount = dataAccess.Table().Count();

            Assert.That(addressCount, Is.EqualTo(0));

            var max = dataAccess.Insert(new DatabaseAddress
            {
                FirstName = "Max",
                LastName = "Mustermann"
            });

            addressCount = dataAccess.Table().Count();

            Assert.That(addressCount, Is.EqualTo(1));
        }
    }
}
