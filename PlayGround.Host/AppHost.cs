using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Playground_Api>("PlayGround");

builder.AddProject<Playground_web>("playground-web");

builder.Build().Run();
