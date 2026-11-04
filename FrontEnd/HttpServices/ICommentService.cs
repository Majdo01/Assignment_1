using ApiContracts;

namespace FrontEnd.HttpServices;

public interface ICommentService
{
    Task<CommentDto> AddCommentAsync(CreateCommentDto request);
    Task<List<CommentDto>> GetCommentsForPostAsync(int postId);
}