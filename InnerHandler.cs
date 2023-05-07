using System;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Flyingdarts.Lambdas.Shared;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

public class InnerHandler
{
    private readonly IMediator _mediator;

    public InnerHandler()
    {
    }
    public InnerHandler(ServiceProvider serviceProvider)
    {
        _mediator = serviceProvider.GetRequiredService<IMediator>();
    }
    public async Task<APIGatewayProxyResponse> Handle(SocketMessage<CreateUserProfileCommand> request, ILambdaContext context)
    {
        try
        {
            if (request?.Message is null)
                throw new BadRequestException("Unable to parse request.", typeof(CreateUserProfileCommand));

            return await _mediator.Send(request.Message);
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"{ex.Message}\n{ex.StackTrace}");
            return new APIGatewayProxyResponse
            {
                StatusCode = 400,
                Body = ex.Message
            };
        }
    }
}