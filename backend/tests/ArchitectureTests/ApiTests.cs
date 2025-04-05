using Api.Abstractions;
using ArchitectureTests.Abstractions;

namespace ArchitectureTests;

public class ApiTests : BaseArchitectureTest
{
    [Fact]
    public void Controllers_Should_InheritFromBaseApiController()
    {
        // Arrange

        // Act
        TestResult result = Types.InAssembly(ApiAssembly)
            .That()
            .HaveNameEndingWith("Controller")
            .And()
            .DoNotHaveName("BaseApiController")
            .Should()
            .Inherit(typeof(BaseApiController))
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Controllers_Should_Not_HaveDependencyOnRepositories()
    {
        // Arrange

        // Act
        TestResult result = Types.InAssembly(ApiAssembly)
            .That()
            .HaveNameEndingWith("Controller")
            .ShouldNot()
            .HaveDependencyOn($"{PersistenceNamespace}.Repositories")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }
}
