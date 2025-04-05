using ArchitectureTests.Abstractions;

namespace ArchitectureTests;

public class PersistenceTests : BaseArchitectureTest
{
    [Fact]
    public void Repositories_Should_HaveDependencyOnDomain()
    {
        // Arrange

        // Act
        TestResult result = Types.InAssembly(PersistenceAssembly)
            .That()
            .HaveNameEndingWith("Repository")
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }
}
