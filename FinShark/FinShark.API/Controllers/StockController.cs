﻿using AutoMapper;
using FinShark.DAL.Models;
using FinShark.Domain.Stock;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.API.Controllers;

[ApiController]
[Route("api/stock")]
public class StockController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public StockController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetStocks()
    {
        var stocks = _context.Stocks.ToList()
            .Select(stock => _mapper.Map<StockDto>(stock));
        return Ok(stocks);
    }

    [HttpGet("{id}")]
    public IActionResult GetStockById([FromRoute] int id)
    {
        var stock = _context.Stocks.Find(id);

        if (stock == null)
            return NotFound();

        return Ok(_mapper.Map<StockDto>(stock));
    }

    [HttpPost]
    public IActionResult CreateStock([FromBody] CreateStockRequestDto stockDto)
    {
        var stockModel = _mapper.Map<Stock>(stockDto);
        _context.Stocks.Add(stockModel);
        _context.SaveChanges();
        return CreatedAtAction(
            nameof(GetStockById),
            new { id = stockModel.Id },
            _mapper.Map<StockDto>(stockModel));
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
    {
        var stockModel = _context.Stocks.FirstOrDefault(stock => stock.Id == id);

        if (stockModel == null)
            return NotFound();

        stockModel.Symbol = stockDto.Symbol;
        stockModel.CompanyName= stockDto.CompanyName;
        stockModel.Purchase= stockDto.Purchase;
        stockModel.LastDiv = stockDto.LastDiv;
        stockModel.Industry = stockDto.Industry;
        stockModel.MarketCap = stockDto.MarketCap;

        _context.SaveChanges();
        return Ok(_mapper.Map<StockDto>(stockModel));
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeleteStock([FromRoute] int id)
    {
        var stockModel = _context.Stocks.FirstOrDefault(stock => stock.Id == id);

        if (stockModel == null)
            return NotFound();

        _context.Stocks.Remove(stockModel);
        _context.SaveChanges(true);

        return NoContent();
    }
}
