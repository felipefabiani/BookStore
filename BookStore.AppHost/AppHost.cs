using BookStore.AppHost;

const string bookStoreAppName = "BookStore";
const string bookStoreApiName = $"{bookStoreAppName}-Api";
const string bookStoreDatabaseName = $"{bookStoreAppName}";
const string bookStoreSqlName = $"{bookStoreAppName}-Database";
const string bookStoreSeqName = $"{bookStoreAppName}-Seq";

var builder = DistributedApplication.CreateBuilder(args);
var saPassword = builder.AddParameter("sql-sa-password", secret: true)
    .ExcludeFromManifest();

// var cache = builder.AddRedis("cache");

var seq = builder
    .AddSeq(bookStoreSeqName, 5468)
    .ExcludeFromManifest()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithEnvironment("ACCEPT_EULA", "Y")
    .WithContainerName(bookStoreSeqName);

// Define a SQL Server resource
var sql = builder
    .AddSqlServer(bookStoreSqlName, saPassword, 1433)
    // .WithEndpoint(port: 1433, targetPort: 1433, name:"TESTE-DB")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithContainerName(bookStoreSqlName);

var db = sql.AddDatabase(bookStoreDatabaseName);

var api = builder.AddProject<Projects.BookStore_Api>(bookStoreApiName)
    .WithReference(seq)
    .WithReference(db)
    .WaitFor(seq)
    .WaitFor(db)
    .WithScalarUi();


//builder.AddProject<Projects.BookStoreApp>("bookstoreapp")
//    .WithReference(bsApi)
//    .WaitFor(bsApi);

builder.Build().Run();
