
using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;
using Microsoft.Extensions.Configuration;

public static class ServiceFactory
{
    public static ServiceProvider GetServiceProvider(IConfiguration Configuration)
    {
        var services = new ServiceCollection();
        services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
        services.AddAWSService<IAmazonDynamoDB>(Configuration.GetAWSOptions("DynamoDb"));
        services.AddTransient<IDynamoDBContext, DynamoDBContext>();
        services.AddValidatorsFromAssemblyContaining<CreateUserProfileCommandValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateUserProfileCommand).Assembly));
        return services.BuildServiceProvider();
    }
}