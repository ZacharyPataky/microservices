using AutoMapper;
using Shared.Comments;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces;
using Shared.Models;

namespace Comments.Controllers;

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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var commentModels = await _commentRepo.GetCommentsAsync();
        var commentDtos = commentModels.Select(_mapper.Map<CommentDto>);

        return Ok(commentDtos);
    }

    [HttpGet]
    [Route("{commentId:int}")]
    public async Task<IActionResult> GetCommentById([FromRoute] int commentId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var commentModel = await _commentRepo.DeleteCommentAsync(commentId);
        if (commentModel == null)
            return NotFound("The comment does not exist.");

        return Ok(commentModel);
    }
}
