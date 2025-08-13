var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.BookStore_Api>("booskstore-api");
builder.AddProject<Projects.BookStoreApp>("bookstoreapp");

builder.Build().Run();
