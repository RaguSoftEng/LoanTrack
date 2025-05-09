var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", "postgresUser");
var password = builder.AddParameter("password", "Postgres!123");

var postgres = builder.AddPostgres("loantrack-pg", username, password, port: 5432)
    .WithDataVolume("loantrack-pg-pv")
    .WithPgAdmin(containerName: "loantrack-pgadmin");

var postgresDb = postgres.AddDatabase("loantrackdb");

builder.AddProject<Projects.LoanTrack_Web>("loantrack-web")
    .WithReference(postgresDb)
    .WaitFor(postgresDb)
    .WithEnvironment("ConnectionStrings__DefaultConnection", postgresDb);

builder.Build().Run();
