using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using next_backend.models;

namespace next_backend.services;

public interface IStoryService
{
    Task<Story> GetStory(int id);
    Task<List<Story>> GetStories(int page, int pageSize);
}

public class StoryService(RestClientService restClient, IMemoryCache? memoryCache) : IStoryService
{

    public async Task<Story?> GetStory(int id)
    {
        var path = $"/item/{id}.json";
        
        try
        {
            var response =  restClient.GetTask(path);
            var result = await response.Result.ReadAsStringAsync();
            Story? story = JsonConvert.DeserializeObject<Story>(result);
                
            return story;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while fetching item {id}: {ex.Message}");
            throw;
        }
    }
    
    public async Task<List<Story>?> GetStories(int page, int pageSize)
    {
        List<Story> stories = new List<Story>();
        var path = $"/newstories.json";

        try
        {
            if (page <= 0)
                throw new NotFoundException($"Page data {page} was not provided");
            
            if (pageSize <= 0)
                throw new NotFoundException($"PageSize data {pageSize} was not provided");
            
            var response = restClient.GetTask(path);
            var resultIds = await response.Result.ReadAsStringAsync();
            var currentIds = resultIds.Skip((page - 1) * pageSize).Take(pageSize);

            foreach (var id in currentIds)
            {
                string key = $"story_{id}";
                if (!memoryCache.TryGetValue(key, out Story? cachedData))
                {
                    cachedData = await GetStory(id);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                    memoryCache.Set(key, cachedData, cacheEntryOptions);
                    
                    if (cachedData != null) stories.Add(cachedData);
                } else 
                    stories.Add(cachedData);
            }
            
            return stories;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public class NotFoundException(string message) : Exception(message);
}