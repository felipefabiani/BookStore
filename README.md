# BookStore

Using Sql Server docket
current image: docker pull mcr.microsoft.com/mssql/server:2022-latest
container: 
docker run --name BookStore.Database \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=qwe@@123" \
  -p 1433:1433 \
  -d mcr.microsoft.com/mssql/server:2022-latest

Connect to Database using Microsoft Sql Server Management Studio
Server Type: Database Engine
Server name: 127.0.0.1,1433
Austhentication: SQL SErver Authentication
Login: sa
Password: qwe@@123

Connection string set up on Environment Variables, to change go to:
    Right-click your project in Solution Explorer
    Select Properties
    Go to the Debug tab
    Find the section labeled Environment Variables
    Name: BOOKSTORE_DB_CONNECTION
    Value: Server=localhost;Database=BookStoreDb;User Id=sa;Password=qwe@@123;TrustServerCertificate=True;

How do run migrations for this project
dotnet ef database update --project BookStore.Database --startup-project BooskStore.Api --context BookStoreContext

dotnet ef migrations add Initial --project BookStore.Database --startup-project BooskStore.Api --context BookStoreContext