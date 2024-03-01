using AutoMapper;
using FinShark.Domain.Comment;
using FinShark.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.API.Controllers;

[ApiController]
[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepo;
    private readonly IMapper _mapper;

    public CommentController(ICommentRepository commentRepo, IMapper mapper)
    {
        _commentRepo = commentRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetComments()
    {
        var comments = await _commentRepo.GetCommentsAsync();
        var commentDtos = comments.Select(_mapper.Map<CommentDto>);

        return Ok(commentDtos);
    }
}
