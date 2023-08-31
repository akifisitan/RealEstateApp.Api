# Real Estate App Api

- Backend for a Real Estate listing website [(SvelteRE)](https://github.com/akifisitan/SvelteRE/) built using the .NET 6 Web API template

## Usage

1. Setup [Docker](https://www.docker.com/)

2. Create a new file called .env and copy the contents of example-env into it or simply rename example-env to .env

3. Open up a terminal in this directory and build the web api image

```bash
docker build -t realestateapp-api .
```

4. Create and run the containers

```bash
docker compose up
```

## Info

- The app will automatically apply migrations and create a default admin user with the following credentials:

  - username: admin
  - password: Test123.

- These can be overridden by editing the contents of the .env file.
- Other values like the connection strings and passwords can be set via the .env file as well.
- Make sure to change the values of the .env file for production, these are just dummy values set for development.

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
