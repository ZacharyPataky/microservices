using Comments.DTOs;
using Comments.Models;

namespace Comments.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetCommentsAsync();
    Task<Comment?> GetCommentByIdAsync(int commentId);
    Task<Comment> CreateCommentAsync(Comment commentModel);
    Task<Comment?> UpdateCommentAsync(int commentId, UpdateCommentRequestDto commentModel);
    Task<Comment?> DeleteCommentAsync(int commentId);
}
