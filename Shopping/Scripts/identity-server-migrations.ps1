dotnet ef migrations add Initial_PersistedGrantDbContext -c  PersistedGrantDbContext -o Migrations\PersistedGrantDbContext --project ..\..\Shopping.csproj
dotnet ef database update --project ..\..\Shopping.csproj

dotnet ef migrations add Initial_ConfigurationDbContext -c  ConfigurationDbContext -o Migrations\ConfigurationDbContext --project ..\..\Shopping.csproj
dotnet ef database update --project ..\..\Shopping.csproj