# Multi-project of Oleksandr Tsvik
### Connect with me:

[![Email Badge](https://cdn.icon-icons.com/icons2/72/PNG/32/email_14410.png)](mailto:oleksandr.zwick@gmail.com)


## What's Included
1. Login and registration (access_token and refresh_token)
2. Task Manager (drag and drop)
3. Dictionary
4. ~~Language and~~ keyboard translator
5. Uploading images
6. Followers / Following feature

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

## Run server [development]
```sh
cd server/API
dotnet watch --no-hot-reload
```

## Run client [development]
```sh
cd client
npm start
```

## Run server and client
```sh
cd client
npm run build-windows

cd ../server/API
dotnet run
```


## Useful commands

### Possible to fix server errors in VS Code
```sh
cd server/API
dotnet restore
dotnet build
```

### Drop database
```sh
cd server
dotnet ef database drop -s API -p Persistence
```

## *References*
- [Complete guide to building an app with .Net Core and React](https://www.udemy.com/course/complete-guide-to-building-an-app-with-net-core-and-react/)
- [Create React App](https://create-react-app.dev/)
- [semantic-ui](https://react.semantic-ui.com/usage/)
- [Formik](https://formik.org/)
- [date-fns#format](https://date-fns.org/v2.30.0/docs/format)
- [react-toastify](https://fkhadra.github.io/react-toastify/introduction/)
- [react-dropzone](https://react-dropzone.js.org/)
- [react-cropper](https://github.com/react-cropper/react-cropper)
- [react-infinite-scroller](https://github.com/danbovey/react-infinite-scroller)

- [*EditorConfig*](https://editorconfig.org/)