using System.Text.Json.Serialization;
using Amazon.Lambda.APIGatewayEvents;
using MediatR;

public class CreateUserProfileCommand : IRequest<APIGatewayProxyResponse>
{
    public string CognitoUserId { get; set; }
    public string UserName { get; set; }
    [JsonIgnore]
    public string Email { get; set; }
    public string Country { get; set; }
}