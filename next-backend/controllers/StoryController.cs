using Microsoft.AspNetCore.Mvc;
using next_backend.models;
using next_backend.services;

namespace next_backend.controllers;

[ApiController]
[Route("api/[controller]")]
public class StoryController(StoryService storyService) : ControllerBase
{
    [HttpGet("/stories")]
    public async Task<List<Story>?> GetStories(int page, int pageSize)
    {
        return await storyService.GetStories(page, pageSize);
    }
    
    [HttpGet("/story/{id:int}")]
    public async Task<Story?> GetStory(int id)
    {
        return await storyService.GetStory(id);
    }
}