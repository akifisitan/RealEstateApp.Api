# Real Estate App Api

- Backend API for [Real Estate listing website](https://github.com/akifisitan/RealEstateApp) built using .NET 6 Web API and Entity Framework Core

## Usage

1. Setup [Docker](https://www.docker.com/)

2. Create a new file called .env and copy the contents of example-env into it or simply rename env.example to .env

3. Open up a terminal in this directory and build the web api image

```bash
docker build -t realestateapp-api .
```

4. Create and run the containers

```bash
docker compose up
```

## Info

- The app will automatically apply migrations and seed the database if the ApplyMigrationsOnBoot environment variable is set to "Y"
- Connection strings, passwords and default admin credentials can be set via the .env file.

## Development

Add migrations via the package manager console

```powershell
Add-Migration -Context RealEstateContext version_name
Add-Migration -Context RealEstateIdentityContext version_name
```

Apply the migrations to the database

```powershell
Update-Database -Context RealEstateContext
Update-Database -Context RealEstateIdentityContext
```
