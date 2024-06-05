using Microsoft.AspNetCore.Http.HttpResults;

namespace next_backend.services;

public class RestClientService
{
    private const string BaseUrl = $"https://hacker-news.firebaseio.com/v0";

    public async Task<HttpContent> GetTask(string path)
    {
        using var client = new HttpClient();
        try
        {
            var completeBaseUrl = $"{BaseUrl}{path}";
            var response = await client.GetAsync((completeBaseUrl));

            if (response.IsSuccessStatusCode)
                return response.Content;
            
            throw new Exception($"Failed to get resource. Status code: {response.StatusCode}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}