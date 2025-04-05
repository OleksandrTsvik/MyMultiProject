using ArchitectureTests.Abstractions;

namespace ArchitectureTests;

public class LayerTests : BaseArchitectureTest
{
    [Fact]
    public void Application_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        string[] otherProjects =
        [
            ApiNamespace,
            InfrastructureNamespace,
            PersistenceNamespace,
        ];

        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        string[] otherProjects =
        [
            ApiNamespace,
            ApplicationNamespace,
            InfrastructureNamespace,
            PersistenceNamespace,
        ];

        // Act
        TestResult result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        string[] otherProjects =
        [
            ApiNamespace,
        ];

        // Act
        TestResult result = Types.InAssembly(InfrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Persistence_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        string[] otherProjects =
        [
            ApiNamespace,
            InfrastructureNamespace,
        ];

        // Act
        TestResult result = Types.InAssembly(PersistenceAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }
}
