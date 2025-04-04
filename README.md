# Multi-project of Oleksandr Tsvik

Studying and testing new technologies and approaches in programming for my personal learning and growth.

## 🌠 Features

- Login and registration (access_token and refresh_token)
- Task Manager (drag and drop)
- Dictionary
- ~~Language and~~ keyboard translator
- Uploading images
- Followers / Following feature
- Clean Architecture with ASP.NET Core Web API

## 🐳 Docker

### up

```sh
docker-compose -f docker-compose.dev.yml up --build
```

> Be sure to fill in the `backend/src/Api/appsettings.json` file!

### down

```sh
docker-compose -f docker-compose.dev.yml down
```

### Watch logs

```sh
docker logs backend --follow
```

### Connect to container

```sh
docker exec -it backend bash
```
