using FinShark.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.API.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StockController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetStocks()
    {
        var stocks = _context.Stocks.ToList();
        return Ok(stocks);
    }

    [HttpGet("{id}")]
    public IActionResult GetStockById([FromRoute] int id)
    {
        var stock = _context.Stocks.Find(id);

        if (stock == null)
            return NotFound();

        return Ok(stock);
    }
}
