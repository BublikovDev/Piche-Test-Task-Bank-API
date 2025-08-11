# Piche Test Task (Bank API)
### 1. Install & Run SQL Server

### 2. Update Connection String
Server/appsettings.json

### 3. Apply Migrations
cd Server

dotnet ef migrations add InitialMigration

dotnet ef database update

### 4. Run application
dotnet run --project Server

### 5. Use Swagger to manual test application
Example:
http://localhost:5029/swagger
https://localhost:7103/swagger
