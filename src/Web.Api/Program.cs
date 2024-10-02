using Web.Api.Configurations;

WebApplication
    .CreateBuilder(args)
    .AddServices()
    .CreateApp()
    .ConfigureApp()
    .Run();