dotnet ef migrations add Initial_ApplicationIdentityContext -c  ShoppingDbContext -o Migrations\ShoppingDbContext --project ..\..\Shopping
dotnet ef database update --project ..\..\Shopping