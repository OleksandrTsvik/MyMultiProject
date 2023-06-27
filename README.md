# Multi-project of Oleksandr Tsvik
### Connect with me:

[![Email Badge](https://cdn.icon-icons.com/icons2/72/PNG/32/email_14410.png)](mailto:oleksandr.zwick@gmail.com)


## Install dependencies for the client
```sh
cd client
npm install
```

## Creating files for the server
#### In the `API` folder, create an `appsettings.json` file with the following required fields:
```json
{
  "Cloudinary": {
    "CloudName": "",
    "ApiKey": "",
    "ApiSecret": ""
  }
}
```

>**NOTE:** You must have a [Cloudinary](https://cloudinary.com/) account.

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

## *References*
- [Complete guide to building an app with .Net Core and React](https://www.udemy.com/course/complete-guide-to-building-an-app-with-net-core-and-react/)

## What's Included
1. Login and registration
2. Task Manager
3. Language and keyboard translator
4. Uploading images