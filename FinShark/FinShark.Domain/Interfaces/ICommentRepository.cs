using FinShark.Domain.Comment;
using DB = FinShark.DAL.Models;

namespace FinShark.Domain.Interfaces;

public interface ICommentRepository
{
    Task<List<DB.Comment>> GetCommentsAsync();
    Task<DB.Comment?> GetCommentByIdAsync(int commentId);
    Task<DB.Comment> CreateCommentAsync(DB.Comment commentModel);
    Task<DB.Comment?> UpdateCommentAsync(int commentId, UpdateCommentRequestDto commentModel);
    Task<DB.Comment?> DeleteCommentAsync(int commentId);
}
