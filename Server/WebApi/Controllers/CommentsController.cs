using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment(
        [FromBody] CreateCommentDto request)
    {
        Comment comment = new Comment(
            request.Body,
            request.PostId,
            request.UserId);

        Comment created = await commentRepository.AddAsync(comment);

        CommentDto dto = new CommentDto
        {
            Id = created.Id,
            Body = created.Body,
            PostId = created.PostId,
            UserId = created.UserId
        };

        return Created($"/Comments/{dto.Id}", dto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetComment(int id)
    {
        try
        {
            Comment comment =
                await commentRepository.GetSingleAsync(id);

            CommentDto dto = new CommentDto
            {
                Id = comment.Id,
                Body = comment.Body,
                PostId = comment.PostId,
                UserId = comment.UserId
            };

            return Ok(dto);
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetComments(
        [FromQuery] int? userId,
        [FromQuery] int? postId)
    {
        IQueryable<Comment> comments =
            commentRepository.GetMany();

        if (userId.HasValue)
        {
            comments =
                comments.Where(c => c.UserId == userId.Value);
        }

        if (postId.HasValue)
        {
            comments =
                comments.Where(c => c.PostId == postId.Value);
        }

        List<CommentDto> result = comments
            .Select(c => new CommentDto
            {
                Id = c.Id,
                Body = c.Body,
                PostId = c.PostId,
                UserId = c.UserId
            })
            .ToList();

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateComment(
        int id,
        [FromBody] CreateCommentDto request)
    {
        Comment comment = new Comment(
            request.Body,
            request.PostId,
            request.UserId)
        {
            Id = id
        };

        try
        {
            await commentRepository.UpdateAsync(comment);
            return NoContent();
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteComment(int id)
    {
        try
        {
            await commentRepository.DeleteAsync(id);
            return NoContent();
        }
        catch
        {
            return NotFound();
        }
    }
}