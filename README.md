# Prueba: API + Avalonia

Login, registro de usuarios y consulta de logs de login.

- apiprueba: API ASP.NET Core con SQLite.
- avaloniaprueba: aplicación de escritorio Avalonia.

Requisitos: .NET SDK 10, SQLite (comando sqlite3 disponible en PATH) y Git.

## Crear la base de datos

En PowerShell, desde la carpeta que contiene ambos proyectos:

```powershell
cd "C:\Users\Joaquin\Desktop\ITES\prueba"
New-Item -ItemType Directory -Force "C:\sqlite"
sqlite3 "C:/sqlite/prueba.db" ".read apiprueba/database.sql"
```

Ejecutar el script SQL una sola vez, sobre una base nueva. Crea las tablas usuario y login_log. La API ya está configurada para usar C:\sqlite\prueba.db.

## Ejecutar

Desde la carpeta prueba, iniciar la API:

```powershell
dotnet run --project .\apiprueba\apiprueba.csproj --launch-profile http
```

En otra terminal, desde la misma carpeta, iniciar Avalonia:

```powershell
dotnet run --project .\avaloniaprueba\avaloniaprueba.csproj
```

La API se ejecuta en Development en http://localhost:5197. Crear una cuenta desde Registrarme y luego iniciar sesión para ver los logs. Cerrar sesión vuelve al login.

## Subir ambos proyectos a GitHub

Crear en GitHub un repositorio vacío llamado prueba, sin README, licencia ni .gitignore. Reemplazar TU_USUARIO en el comando por tu usuario de GitHub.

Desde PowerShell:

```powershell
cd "C:\Users\Joaquin\Desktop\ITES\prueba"
git init
git add .
git commit -m "Agregar API y aplicacion Avalonia"
git branch -M main
git remote add origin https://github.com/TU_USUARIO/prueba.git
git push -u origin main
```

Git solicitará autenticación si hace falta. Ambos proyectos quedan dentro del mismo repositorio. El .gitignore excluye compilados, archivos del IDE y bases de datos locales.
