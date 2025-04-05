using Application.Common.Messaging;
using ArchitectureTests.Abstractions;

namespace ArchitectureTests;

public class ApplicationTests : BaseArchitectureTest
{
    [Fact]
    public void Command_Should_HaveCommandPostfix()
    {
        // Arrange

        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommand))
            .Or()
            .ImplementInterface(typeof(ICommand<>))
            .Should()
            .HaveNameEndingWith("Command")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void CommandHandlers_Should_HaveCommandHandlerPostfix()
    {
        // Arrange

        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should()
            .HaveNameEndingWith("CommandHandler")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Query_Should_HaveQueryPostfix()
    {
        // Arrange

        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should()
            .HaveNameEndingWith("Query")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }
}
