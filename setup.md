# Setup

## .vscode

### settings.json

```json
{
  "terminal.integrated.persistentSessionReviveProcess": "never",
  "terminal.integrated.enablePersistentSessions": false,

  "workbench.iconTheme": "material-icon-theme",
  "material-icon-theme.activeIconPack": "nest",
  "material-icon-theme.files.associations": {},
  "material-icon-theme.folders.associations": {
    "db-schema": "jinja",

    "Api": "api",
    "Application": "app",
    "Domain": "dump",
    "Infrastructure": "tools",
    "Persistence": "database",
    "SharedKernel": "shared",

    "Properties": "private",
    "Migrations": "ci"
  }
}
```

### launch.json

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": "Docker .NET Attach (Preview)",
      "type": "docker",
      "request": "attach",
      "platform": "netCore",
      "containerName": "backend",
      "processName": "Api",
      "sourceFileMap": {
        "/app": "${workspaceFolder}/backend/src"
      }
    }
  ]
}
```

> If `"processName": "Api"` doesn't work, try using `"processId": "${command:pickProcess}"` instead.

### terminals.json

[Terminals Manager](https://marketplace.visualstudio.com/items?itemName=fabiospampinato.vscode-terminals)

```json
{
  "autorun": true,
  "autokill": true,
  "terminals": [
    {
      "name": "root",
      "icon": "file-directory",
      "color": "terminal.ansiBlue",
      "shellPath": "C:\\Program Files\\Git\\bin\\bash.exe",
      "execute": false,
      "commands": ["docker-compose -f docker-compose.dev.yml up --build -d"]
    },
    {
      "name": "backend",
      "icon": "server",
      "color": "terminal.ansiGreen",
      "shellPath": "C:\\Program Files\\Git\\bin\\bash.exe",
      "execute": false,
      "commands": [
        "cd backend",
        "dotnet watch --no-hot-reload --project src/Api/Api.csproj --launch-profile Development"
      ]
    }
  ]
}
```
