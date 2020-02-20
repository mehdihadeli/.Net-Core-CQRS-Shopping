dotnet ef migrations add Initial_PersistedGrantDbContext -c  PersistedGrantDbContext -o Migrations\PersistedGrantDbContext --project ..\..\Shopping
dotnet ef database update --project ..\..\Shopping

dotnet ef migrations add Initial_ConfigurationDbContext -c  ConfigurationDbContext -o Migrations\ConfigurationDbContext --project ..\..\Shopping
dotnet ef database update --project ..\..\Shopping