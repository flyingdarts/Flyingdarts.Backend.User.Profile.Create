
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using System.IO;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public static class ServiceFactory
{
    public static ServiceProvider GetServiceProvider()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddSystemsManager($"/{System.Environment.GetEnvironmentVariable("EnvironmentName")}")
            .Build();

        var services = new ServiceCollection();
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonDynamoDB>(configuration.GetAWSOptions("DynamoDb"));
        services.AddTransient<IDynamoDBContext, DynamoDBContext>();
        services.AddValidatorsFromAssemblyContaining<CreateUserProfileCommandValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateUserProfileCommand).Assembly));
        return services.BuildServiceProvider();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(x => x.AddServerHeader = false);
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureAppConfiguration((environment, app) =>
            {
                app.AddSystemsManager($"/{environment.HostingEnvironment.EnvironmentName}");
            });
}