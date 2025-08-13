var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.BookStoreApp>("bookstoreapp");

builder.AddProject<Projects.BooskStore_Api>("booskstore-api");

builder.Build().Run();
