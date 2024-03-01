using DB = FinShark.DAL.Models;

namespace FinShark.Domain.Interfaces;

public interface ICommentRepository
{
    Task<List<DB.Comment>> GetCommentsAsync();
}
