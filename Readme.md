Steps to running via docker:

Open up a terminal, navigate to this directory and build the image by running:

```bash
docker build -t realestateapp-api .
docker compose up
```

Run the migration commands via the package manager console

```powershell
Update-Database -Context RealEstateContext
Update-Database -Context RealEstateIdentityContext
```

The app will automatically apply migrations and create a default admin user with the following credentials:
username: admin
password: Test123.

- These can be overridden by editing the contents of the .env file.
- Make sure to change the values of the .env file for production, these are just dummy values set for development.
- Other values like the connection strings and passwords can be set via the .env file as well.
