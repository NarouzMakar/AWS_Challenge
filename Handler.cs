using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using narouz_makar_aws_challenge;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AwsDotnetCsharp;
public class Handler
{
    public async Task<APIGatewayProxyResponse> LoadTodos(APIGatewayProxyRequest request)
    {
        Console.WriteLine($"LoadTodos Started");
        var response = await GetTodos();
        APIGatewayProxyResponse apiResponse = new APIGatewayProxyResponse()
        {
            Body = response,
            StatusCode = (int)HttpStatusCode.OK,
            IsBase64Encoded = false,
            Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } }
        };
        Console.WriteLine($"LoadTodos Completed");
        return apiResponse;
    }

    static async Task<string> GetTodos()
    {
        string todoUrl = Environment.GetEnvironmentVariable("ToDo_External_Url");
        Console.WriteLine($"todoUrl: {todoUrl}");
        using HttpClient todoClient = new()
        {
            BaseAddress = new Uri(todoUrl)
        };
        using HttpResponseMessage responsed = await todoClient.GetAsync("todos");
        var jsonResponse = await responsed.Content.ReadAsStringAsync();
        UploaderHelper.PrepareDataForFileUpload(jsonResponse);
        return jsonResponse;
    }

}