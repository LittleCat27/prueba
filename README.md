# Prueba: API + Avalonia

Login, registro de usuarios y consulta de logs de login.

- apiprueba: API ASP.NET Core con SQLite.
- avaloniaprueba: aplicación de escritorio Avalonia.

Requisitos: .NET SDK 10, SQLite (comando sqlite3 disponible en PATH) y Git.

## Crear la base de datos

Usar query database.sql que se encuentra en apiprueba (Para usar sqlite hay que instalarlo y colocar la direccion de la base de datos en "C:\sqlite\prueba.db")


Para instalar sqlite acceder a:

```
https://www.sqlite.org/download.html
```

## Ejecutar

Iniciar primero el proyecto apiprueba y luego el proyecto avalonia

Si los quieren iniciar desde la carpeta base (la que contiene ambos proyectos, pueden usar dotnet run y especificar la carpeta de cada proyecto)

```powershell
dotnet run --project .\apiprueba\apiprueba.csproj
```

```powershell
dotnet run --project .\avaloniaprueba\avaloniaprueba.csproj
```
