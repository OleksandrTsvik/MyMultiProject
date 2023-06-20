# Multi-project of Oleksandr Tsvik


## Install dependencies for the client
```sh
cd client
npm install
```

## Install dependencies for the server
```sh
cd server
dotnet restore
dotnet build
```

## Create EF Migrations
```sh
cd server
dotnet ef migrations add InitialCreate -s API -p Persistence
```

## Run server
```sh
cd server/API
dotnet watch --no-hot-reload
```

## Run client
```sh
cd client
npm start
```

## What's Included
1. Login and registration
2. Task Manager
3. Language and keyboard translator