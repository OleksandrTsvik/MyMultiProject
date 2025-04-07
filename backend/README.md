# MyMultiProject/backend

## 🚀 How-to run

1. Install .NET 8.0.
2. Install required dependencies:

```sh
dotnet restore
dotnet build
```

3. In the `src/Api` folder, fill in the `appsettings.json` file.
4. Run:

```sh
cd src/Api
dotnet watch --no-hot-reload
```

## 💾 Useful commands

### Create EF migration

```sh
dotnet ef migrations add InitialCreate -s src/Api -p src/Persistence
```

### List EF migrations

```sh
dotnet ef migrations list -s src/Api -p src/Persistence
```

### Remove last EF migration

```sh
dotnet ef migrations remove -s src/Api -p src/Persistence
```

> When used, the `--force` flag will remove the migration regardless of whether it has already been applied to the database.

### Updates the database to the last migration or to a specified migration

```sh
dotnet ef database update -s src/Api -p src/Persistence
```

### Drop database

```sh
dotnet ef database drop -s src/Api -p src/Persistence
```

### Run tests

```sh
dotnet test
```

### Possible to fix backend errors in vscode

```sh
cd src/Api
dotnet restore
dotnet build
```

### Сreate solution and projects

```sh
dotnet new list

dotnet new sln
dotnet new webapi -n Api
dotnet new classlib -n Application
dotnet new classlib -n Domain
```

### Add projects to the solution

```sh
dotnet sln add src/Api/Api.csproj
dotnet sln add src/Application/Application.csproj
dotnet sln add src/Domain/Domain.csproj

dotnet sln list
```

### Add project references

```sh
cd src/Api
dotnet add reference ../Infrastructure/Infrastructure.csproj
cd ../Application
dotnet add reference ../Domain/Domain.csproj
cd ../Domain
dotnet add reference ../SharedKernel/SharedKernel.csproj
cd ../Infrastructure
dotnet add reference ../Application/Application.csproj
dotnet add reference ../Persistence/Persistence.csproj
```

## 📝 Useful notes

### Unit test naming conventions

```
[ThingUnderTest]_Should_[ExpectedResult]_[Conditions]
```
