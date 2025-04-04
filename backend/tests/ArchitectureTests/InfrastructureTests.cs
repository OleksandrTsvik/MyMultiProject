using Application.Common.Messaging;
using ArchitectureTests.Abstractions;

namespace ArchitectureTests;

public class InfrastructureTests : BaseArchitectureTest
{
    [Fact]
    public void Infrastructure_Should_Not_ImplementQueryInterface()
    {
        // Arrange

        // Act
        TestResult result = Types.InAssembly(InfrastructureAssembly)
            .ShouldNot()
            .ImplementInterface(typeof(IQuery<>))
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void QueryHandlers_Should_HaveQueryHandlerPostfix()
    {
        // Arrange

        // Act
        TestResult result = Types.InAssembly(InfrastructureAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .HaveNameEndingWith("QueryHandler")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
