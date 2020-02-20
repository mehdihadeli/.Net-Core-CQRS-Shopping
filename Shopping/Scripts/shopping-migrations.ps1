dotnet ef migrations add Initial_ShoppingContext -c  ShoppingDbContext -o Migrations\ShoppingDbContext --project ..\..\Shopping
dotnet ef database update --project ..\..\Shopping