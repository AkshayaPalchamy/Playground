using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Playground_Api>("PlayGround");

builder.Build().Run();
