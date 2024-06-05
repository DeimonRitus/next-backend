using next_backend.models;
using next_backend.services;

namespace TestProject2;

public class Tests
{
    [Test]
    public async Task GetStories()
    {
        RestClientService restClient = new RestClientService();
        StoryService storyService = new StoryService(restClient);

        //We send 10 as pageSize due to normal scenarios using the page
        var result = await storyService.GetStories(1, 10);
        var firstStory = result?.First();
        
        Assert.NotNull(result);
        Assert.IsAssignableFrom<Story>(firstStory);
    }

    [Test]
    public async Task GetStoryById()
    {
        RestClientService restClient = new RestClientService();
        StoryService storyService = new StoryService(restClient);
        
        var result = await storyService.GetStories(1, 5);
        var firstStory = result?.First();
        
        Assert.NotNull(result);
        Assert.NotNull(firstStory);
        
        if (firstStory != null)
        {
            var story = await storyService.GetStory(firstStory.Id);
            Assert.IsAssignableFrom<Story>(story);
        }
    }

    [Test]
    public async Task ShouldNotReceiveStoriesWithoutBothParams()
    {
        RestClientService restClient = new RestClientService();
        StoryService storyService = new StoryService(restClient);
        
        Assert.ThrowsAsync<StoryService.NotFoundException>(async() => await storyService.GetStories(0, 0));
    }
    
    [Test]
    public async Task ShouldNotReceiveStoriesWithoutPageSizeParam()
    {
        RestClientService restClient = new RestClientService();
        StoryService storyService = new StoryService(restClient);
        
        Assert.ThrowsAsync<StoryService.NotFoundException>(async() => await storyService.GetStories(1, 0));
    }
    
    [Test]
    public async Task ShouldThrowAnErrorWhenReceiveStoriesWithoutPageParam()
    {
        RestClientService restClient = new RestClientService();
        StoryService storyService = new StoryService(restClient);
        
        Assert.ThrowsAsync<StoryService.NotFoundException>(async() => await storyService.GetStories(0, 1));
    }
}