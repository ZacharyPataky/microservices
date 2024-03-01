using AutoMapper;
using FinShark.DAL.Models;
using FinShark.Domain.Interfaces;
using FinShark.Domain.Stock;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.API.Controllers;

[ApiController]
[Route("api/stock")]
public class StockController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IStockRepository _stockRepo;
    private readonly IMapper _mapper;

    public StockController(ApplicationDbContext context, IStockRepository stockRepo, IMapper mapper)
    {
        _context = context;
        _stockRepo = stockRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetStocks()
    {
        var stocks = await _stockRepo.GetStocksAsync();
        return Ok(stocks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStockById([FromRoute] int id)
    {
        var stock = await _stockRepo.GetStockByIdAsync(id);

        if (stock == null)
            return NotFound();

        return Ok(_mapper.Map<StockDto>(stock));
    }

    [HttpPost]
    public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto stockDto)
    {
        var stockModel = _mapper.Map<Stock>(stockDto);
        await _stockRepo.CreateStockAsync(stockModel);
        return CreatedAtAction(
            nameof(GetStockById),
            new { id = stockModel.Id },
            _mapper.Map<StockDto>(stockModel));
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
    {
        var stockModel = await _stockRepo.UpdateStockAsync(id, stockDto);

        if (stockModel == null)
            return NotFound();

        return Ok(_mapper.Map<StockDto>(stockModel));
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteStock([FromRoute] int id)
    {
        var stockModel = await _stockRepo.DeleteStockAsync(id);

        if (stockModel == null)
            return NotFound();

        return NoContent();
    }
}
