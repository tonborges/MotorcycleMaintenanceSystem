using System.Globalization;
using System.Text.Json.Serialization;
using Application;
using Application.Configurations;
using HealthChecks.UI.Client;
using Infrastructure.Configurations;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Web.Api.Consumers.Motorcycles;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using Web.Api.Transformers;

namespace Web.Api.Configurations;

public static class DependencyInjectionConfiguration
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

        builder.Services
               .AddPresentation()
               .AddApplication()
               .AddInfrastructure(builder.Configuration);

        builder.Services.AddOptions<AppSettings>()
               .BindConfiguration(nameof(MessageBusSettings));

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddHealthChecks();
        builder.Services.AddMassTransit(bus =>
        {
            var settings = new MessageBusSettings();
            builder.Configuration.Bind(nameof(MessageBusSettings), settings);

            if (settings.RabbitMqSettings is null)
                throw new ArgumentNullException(nameof(RabbitMqSettings), "RabbitMqSettings in MessageBusSettings needed");

            bus.AddConsumer<MotorcycleCreatedConsumer>();
            bus.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(settings.RabbitMqSettings.Host, h =>
                {
                    h.Username(settings.RabbitMqSettings!.Username ?? "");
                    h.Password(settings.RabbitMqSettings!.Password ?? "");
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ApplicationMediatREntryPoint>());
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "default",
                policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
        });

        return builder;
    }

    public static WebApplication CreateApp(this WebApplicationBuilder builder)
        => builder.Build();

    public static WebApplication ConfigureApp(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.ApplyMigrations();
        }

        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.UseCors("default");
        app.MapHealthChecks("/healthcheck");
        app.UseRouting();
        app.MapControllers();

        app.MapHealthChecks("health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }

    private static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services
            .AddControllers(options => { options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseRouteTransformer())); })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.DefaultThreadCurrentCulture;

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen((Action<SwaggerGenOptions>)(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Motorcycle Maintenance System",
                Version = "v1"
            });
        }));

        return services;
    }
}