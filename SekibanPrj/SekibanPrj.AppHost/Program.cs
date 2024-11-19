var builder = DistributedApplication.CreateBuilder(args);


var postgressPassword = builder.AddParameter("postgres-password", true);
var postgress = builder.AddPostgres("solution2", password: postgressPassword)
    .WithDataVolume().WithPgAdmin().AddDatabase("SekibanPostgres");



var apiService = builder.AddProject<Projects.SekibanPrj_ApiService>("apiservice")
    .WithReference(postgress)
    .WithEndpoint("https", annotation => annotation.IsProxied = false);

builder.AddProject<Projects.SekibanPrj_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
