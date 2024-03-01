using AutoMapper;
using FinShark.DAL.Models;
using FinShark.Domain;
using FinShark.Domain.Interfaces;
using FinShark.Domain.Stock;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.API.Controllers;

[ApiController]
[Route("api/stock")]
public class StockController : ControllerBase
{
    private readonly IStockRepository _stockRepo;
    private readonly IMapper _mapper;

    public StockController(IStockRepository stockRepo, IMapper mapper)
    {
        _stockRepo = stockRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetStocks([FromQuery] QueryObject queryObject)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stocksModels = await _stockRepo.GetStocksAsync(queryObject);
        var stockDtos = stocksModels.Select(_mapper.Map<StockDto>);

        return Ok(stockDtos);
    }

    [HttpGet("{stockId:int}")]
    public async Task<IActionResult> GetStockById([FromRoute] int stockId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stockModel = await _stockRepo.GetStockByIdAsync(stockId);
        if (stockModel == null)
            return NotFound();

        var stockDto = _mapper.Map<StockDto>(stockModel);
        return Ok(stockDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto createStockRequestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stockModel = _mapper.Map<Stock>(createStockRequestDto);
        await _stockRepo.CreateStockAsync(stockModel);

        var StockDto = _mapper.Map<StockDto>(stockModel);
        return CreatedAtAction(
            nameof(GetStockById),
            new { stockId = stockModel.Id },
            StockDto);
    }

    [HttpPut]
    [Route("{stockId:int}")]
    public async Task<IActionResult> UpdateStock([FromRoute] int stockId, [FromBody] UpdateStockRequestDto updateStockRequestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stockModel = await _stockRepo.UpdateStockAsync(stockId, updateStockRequestDto);
        if (stockModel == null)
            return NotFound();

        var stockDto = _mapper.Map<StockDto>(stockModel);
        return Ok(stockDto);
    }

    [HttpDelete]
    [Route("{stockId:int}")]
    public async Task<IActionResult> DeleteStock([FromRoute] int stockId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stockModel = await _stockRepo.DeleteStockAsync(stockId);
        if (stockModel == null)
            return NotFound();

        return NoContent();
    }
}
