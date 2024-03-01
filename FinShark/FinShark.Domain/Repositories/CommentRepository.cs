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

    public async Task<List<DB.Comment>> GetCommentsAsync()
    {
        return await _context.Comments.ToListAsync();
    }
}
