using AutoMapper;
using FinShark.DAL.Models;
using FinShark.Domain.Comment;
using FinShark.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.API.Controllers;

[ApiController]
[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepo;
    private readonly IStockRepository _stockRepo;
    private readonly IMapper _mapper;

    public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, IMapper mapper)
    {
        _commentRepo = commentRepo;
        _stockRepo = stockRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetComments()
    {
        var commentModels = await _commentRepo.GetCommentsAsync();
        var commentDtos = commentModels.Select(_mapper.Map<CommentDto>);

        return Ok(commentDtos);
    }

    [HttpGet]
    [Route("{commentId:int}")]
    public async Task<IActionResult> GetCommentById([FromRoute] int commentId)
    {
        var commentModel = await _commentRepo.GetCommentByIdAsync(commentId);

        if (commentModel == null)
            return NotFound();

        var commentDto = _mapper.Map<CommentDto>(commentModel);
        return Ok(commentDto);
    }

    [HttpPost]
    [Route("{stockId:int}")]
    public async Task<IActionResult> CreateComment([FromRoute] int stockId, [FromBody] CreateCommentRequestDto createCommentRequestDto)
    {
        if (!await _stockRepo.StockExists(stockId))
            return BadRequest("The stock does not exist.");

        var commentModel = _mapper.Map<Comment>(createCommentRequestDto);
        commentModel.StockId = stockId;

        await _commentRepo.CreateCommentAsync(commentModel);
        var commentDto = _mapper.Map<CommentDto>(commentModel);
        return CreatedAtAction(
            nameof(GetCommentById),
            new { commentId = commentModel.Id },
            commentDto);
    }

    [HttpPut]
    [Route("{commentId:int}")]
    public async Task<IActionResult> UpdateComment([FromRoute] int commentId, [FromBody] UpdateCommentRequestDto updateCommentRequestDto)
    {
        var commentModel = await _commentRepo.UpdateCommentAsync(commentId, updateCommentRequestDto);

        if (commentModel == null)
            return NotFound();

        var commentDto = _mapper.Map<CommentDto>(commentModel);
        return Ok(commentDto);
    }

    [HttpDelete]
    [Route("{commentId:int}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
    {
        var commentModel = await _commentRepo.DeleteCommentAsync(commentId);
        
        if (commentModel == null) 
            return NotFound("The comment does not exist.");

        return Ok(commentModel);
    }
}
