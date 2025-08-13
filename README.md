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




