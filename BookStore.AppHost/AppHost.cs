const string bookStoreAppName = "BookStore";
const string bookStoreApiName = $"{bookStoreAppName}-Api";
const string bookStoreDatabaseName = $"{bookStoreAppName}-Database";
const string bookStoreSeqName = $"{bookStoreAppName}-Seq";

var builder = DistributedApplication.CreateBuilder(args);

#region Test to add containers instead of microsoft extensions
// var bsSeq = builder.AddContainer(bookStoreSeqName, "datalust/seq", "2025.2");
//    .WithHttpEndpoint(port: 5341, targetPort: 80)
//    .WithEnvironment("ACCEPT_EULA", "Y")
//    .WithEnvironment("SEQ_PASSWORD", "qwe@@123")
//    .WithLifetime(ContainerLifetime.Persistent)
//    .WithContainerName(bookStoreSeqName)
//    //.WithHealthCheck("/api/health")
//    ;

//var bsDatabase = builder.AddContainer("BookStore-DataBase", "mcr.microsoft.com/mssql/server:2022-latest")
//    .WithEndpoint(port: 1433, targetPort: 1433)
//    .WithEnvironment("ACCEPT_EULA", "Y")
//    .WithEnvironment("MSSQL_SA_PASSWORD", "qwe@@123")
//    .WithLifetime(ContainerLifetime.Persistent)
//    .WithContainerName("BookStore-DataBase");

#endregion


// var cache = builder.AddRedis("cache");
var saPassword = builder.AddParameter("sql-sa-password", secret: true);

var seq = builder
    .AddSeq(bookStoreSeqName, 5468)
    .ExcludeFromManifest()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithEnvironment("ACCEPT_EULA", "Y")
    .WithContainerName(bookStoreSeqName);

// Define a SQL Server resource
var db = builder
    .AddSqlServer(bookStoreDatabaseName)
    .WithPassword(saPassword) // pass parameter instead of hardcoded password
    .WithLifetime(ContainerLifetime.Persistent)
    .WithContainerName(bookStoreDatabaseName);

var api = builder.AddProject<Projects.BookStore_Api>(bookStoreApiName)
    .WithReference(seq)
    .WithReference(db)
    .WaitFor(seq)
    .WaitFor(db);

//builder.AddProject<Projects.BookStoreApp>("bookstoreapp")
//    .WithReference(bsApi)
//    .WaitFor(bsApi);

builder.Build().Run();
