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
