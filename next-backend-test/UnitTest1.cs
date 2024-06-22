using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using next_backend.models;
using next_backend.services;
using Moq;

namespace TestProject2;

public class Tests
{
    private Mock<IStoryService> _mockMemoryCache;
    private RestClientService _restClientService;
    
    [SetUp]
    public void SetUp()
    {
        _mockMemoryCache = new Mock<IStoryService>();
        _restClientService = new RestClientService();
    }
    
    [Test]
    public async Task GetStory()
    {
        int key = 99;
        Story expectedStory = new Story { Id = key, Title = $"Story {key}" };

        _mockMemoryCache.Setup(s => s.GetStory(key)).Returns(Task.FromResult(expectedStory));
        
        var customOptions = new MemoryCacheOptions()
        {
            SizeLimit = 1024 
        };
        
        var cache = new MemoryCache(customOptions);
        var storyService = new StoryService(_restClientService, cache);

        var result = await storyService.GetStory(key);
        Assert.AreEqual(result!.Id, expectedStory.Id);
    }
    
    [Test]
    public async Task ShouldNotReceiveStoriesWithoutPageSizeParam()
    {
        var customOptions = new MemoryCacheOptions()
        {
            SizeLimit = 1024 
        };
        
        var cache = new MemoryCache(customOptions);
        var storyService = new StoryService(_restClientService, cache);
        
        Assert.ThrowsAsync<StoryService.NotFoundException>(async() => await storyService.GetStories(1, 0));
    }
    
    [Test]
    public async Task ShouldThrowAnErrorWhenReceiveStoriesWithoutPageParam()
    {
        var customOptions = new MemoryCacheOptions()
        {
            SizeLimit = 1024 
        };
        
        var cache = new MemoryCache(customOptions);
        var storyService = new StoryService(_restClientService, cache);
        
        Assert.ThrowsAsync<StoryService.NotFoundException>(async() => await storyService.GetStories(0, 1));
    }
    
}