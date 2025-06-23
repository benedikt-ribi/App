using MLZ2025.Core.ViewModel;
using NUnit.Framework.Internal;

namespace MLZ2025.Core.Tests;

[TestFixture]
public class DetailViewModelTests : TestsBase
{
    [Test]
    public void TestDetailViewModel()
    {
        var serviceProvider = CreateServiceProvider();
        var viewModel = serviceProvider.GetRequiredService<DetailViewModel>();

        Assert.That(viewModel, Is.Not.Null);
    }
}
