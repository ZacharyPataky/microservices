using DB = FinShark.DAL.Models;

namespace FinShark.Domain.Interfaces;

public interface ICommentRepository
{
    Task<List<DB.Comment>> GetCommentsAsync();
    Task<DB.Comment?> GetCommentByIdAsync(int id);
    Task<DB.Comment> CreateCommentAsync(DB.Comment commentModel);
    Task<DB.Comment?> DeleteCommentAsync(int id);
}
