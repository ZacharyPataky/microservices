using Comments.DTOs;
using Comments.Interfaces;
using Comments.Models;
using Microsoft.EntityFrameworkCore;

namespace Comments.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Comment> CreateCommentAsync(Comment commentModel)
    {
        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }

    public async Task<Comment?> DeleteCommentAsync(int commentId)
    {
        var commentModel = await _context.Comments
            .FirstOrDefaultAsync(comment => comment.Id == commentId);

        if (commentModel == null)
            return null;

        _context.Comments.Remove(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }

    public async Task<Comment?> GetCommentByIdAsync(int commentId)
    {
        return await _context.Comments.FindAsync(commentId);
    }

    public async Task<List<Comment>> GetCommentsAsync()
    {
        return await _context.Comments.ToListAsync();
    }

    public async Task<Comment?> UpdateCommentAsync(int commentId, UpdateCommentRequestDto updateCommentRequestDto)
    {
        var existingComment = await _context.Comments.FindAsync(commentId);

        if (existingComment == null)
            return null;

        existingComment.Title = updateCommentRequestDto.Title;
        existingComment.Content = updateCommentRequestDto.Content;

        await _context.SaveChangesAsync(true);

        return existingComment;
    }
}

