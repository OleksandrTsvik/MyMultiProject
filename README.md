# Tsvik Oleksandr 


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