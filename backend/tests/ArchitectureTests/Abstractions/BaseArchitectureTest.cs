using System.Reflection;

namespace ArchitectureTests.Abstractions;

public abstract class BaseArchitectureTest
{
    protected static readonly Assembly ApiAssembly = Api.AssemblyReference.Assembly;
    protected static readonly Assembly ApplicationAssembly = Application.AssemblyReference.Assembly;
    protected static readonly Assembly DomainAssembly = Domain.AssemblyReference.Assembly;
    protected static readonly Assembly InfrastructureAssembly = Infrastructure.AssemblyReference.Assembly;
    protected static readonly Assembly PersistenceAssembly = Persistence.AssemblyReference.Assembly;
    protected static readonly Assembly SharedKernelAssembly = SharedKernel.AssemblyReference.Assembly;

    protected static readonly string ApiNamespace = nameof(Api);
    protected static readonly string ApplicationNamespace = nameof(Application);
    protected static readonly string DomainNamespace = nameof(Domain);
    protected static readonly string InfrastructureNamespace = nameof(Infrastructure);
    protected static readonly string PersistenceNamespace = nameof(Persistence);
    protected static readonly string SharedKernelNamespace = nameof(SharedKernel);
}
