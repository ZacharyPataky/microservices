using FinShark.DAL.Models;
using FinShark.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using DB = FinShark.DAL.Models;

namespace FinShark.Domain.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;    
    }

    public async Task<DB.Comment> CreateCommentAsync(DB.Comment commentModel)
    {
        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }

    public async Task<DB.Comment?> DeleteCommentAsync(int commentId)
    {
        var commentModel = await _context.Comments
            .FirstOrDefaultAsync(comment => comment.Id == commentId);

        if (commentModel == null)
            return null;

        _context.Comments.Remove(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }

    public async Task<DB.Comment?> GetCommentByIdAsync(int commentId)
    {
        return await _context.Comments.FindAsync(commentId);
    }

    public async Task<List<DB.Comment>> GetCommentsAsync()
    {
        return await _context.Comments.ToListAsync();
    }
}
