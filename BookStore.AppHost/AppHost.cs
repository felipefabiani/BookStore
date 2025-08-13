var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.BookStoreApp>("bookstoreapp");

builder.Build().Run();
