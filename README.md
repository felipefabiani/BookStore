# BookStore

Setting up:
Add database password to the secret file. From solution folder run command.
dotnet user-secrets --project BookStore.AppHost set "Parameters:sql-sa-password" <password>


File is under folder:
C:\Users\<user>\AppData\Roaming\Microsoft\UserSecrets\<UserSecretsId>

<UserSecretsId> is defined on project file, in this case BookStore.AppHost.csproj

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
Running BookStore.Api applies any pending migrations and seed data to the database.
Or can run command below
dotnet ef database update --project BookStore.Database --startup-project BookStore.Api --context BookStoreContext

dotnet ef migrations add RemovingIndex --project BookStore.Database --namespace BookStore.Database.Migrations --startup-project BookStore.Api --context BookStoreContext

dotnet ef database update --project BookStore.Database --startup-project BookStore.Api --context BookStoreContext --connection "Data Source=127.0.0.1,1433;Initial Catalog=BookStore;User ID=sa;pwd=qwe@@123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"


dotnet ef migrations add SetUp_Roles_Claims --project BookStore.Database --namespace BookStore.Database.Migrations --startup-project BookStore.Api --context BookStoreContext


Serilog Seq (datalust)
docker pull datalust/seq:latest

docker run --name BookStore.Seq -d --restart unless-stopped -e ACCEPT_EULA=Y -e SEQ_PASSWORD=qwe@@123 -p 5341:80 datalust/seq:latest

open in browser http:\\localhost:5341
  user: admin

