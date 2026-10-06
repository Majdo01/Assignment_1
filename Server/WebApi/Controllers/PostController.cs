using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepository;

    public PostsController(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost(
        [FromBody] CreatePostDto request)
    {
        Post post = new Post(
            request.Title,
            request.Body,
            request.UserId);

        Post created = await postRepository.AddAsync(post);

        PostDto dto = new PostDto
        {
            Id = created.Id,
            Title = created.Title,
            Body = created.Body,
            UserId = created.UserId
        };

        return Created($"/Posts/{dto.Id}", dto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetPost(int id)
    {
        try
        {
            Post post = await postRepository.GetSingleAsync(id);

            PostDto dto = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId
            };

            return Ok(dto);
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetPosts(
        [FromQuery] string? title,
        [FromQuery] int? userId)
    {
        IQueryable<Post> posts = postRepository.GetMany();

        if (!string.IsNullOrWhiteSpace(title))
        {
            posts = posts.Where(p =>
                p.Title.Contains(title,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (userId.HasValue)
        {
            posts = posts.Where(p => p.UserId == userId.Value);
        }

        List<PostDto> result = posts
            .Select(p => new PostDto
            {
                Id = p.Id,
                Title = p.Title,
                Body = p.Body,
                UserId = p.UserId
            })
            .ToList();

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdatePost(
        int id,
        [FromBody] CreatePostDto request)
    {
        Post post = new Post(
            request.Title,
            request.Body,
            request.UserId)
        {
            Id = id
        };

        try
        {
            await postRepository.UpdateAsync(post);
            return NoContent();
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletePost(int id)
    {
        try
        {
            await postRepository.DeleteAsync(id);
            return NoContent();
        }
        catch
        {
            return NotFound();
        }
    }
}