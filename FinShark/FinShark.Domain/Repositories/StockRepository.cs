using FinShark.DAL.Models;
using FinShark.Domain.Interfaces;
using FinShark.Domain.Stock;
using Microsoft.EntityFrameworkCore;
using DB = FinShark.DAL.Models;

namespace FinShark.Domain.Repositories;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _context;

    public StockRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DB.Stock> CreateStockAsync(DB.Stock stockModel)
    {
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<DB.Stock?> DeleteStockAsync(int stockId)
    {
        var stockModel = await _context.Stocks
            .FirstOrDefaultAsync(stock => stock.Id == stockId);

        if (stockModel == null)
            return null;

        _context.Stocks.Remove(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<DB.Stock?> GetStockByIdAsync(int stockId)
    {
        return await _context.Stocks
            .Include(comment => comment.Comments)
            .FirstOrDefaultAsync(stock => stock.Id == stockId);
    }

    public async Task<List<DB.Stock>> GetStocksAsync()
    {
        return await _context.Stocks
            .Include(comment => comment.Comments)
            .ToListAsync();
    }

    public Task<bool> StockExists(int stockId)
    {
        return _context.Stocks.AnyAsync(stock => stock.Id == stockId);
    }

    public async Task<DB.Stock> UpdateStockAsync(int stockId, UpdateStockRequestDto stockDto)
    {
        var existingStock = await _context.Stocks
            .FirstOrDefaultAsync(stock => stock.Id == stockId);

        if (existingStock == null)
            return null;

        existingStock.Symbol = stockDto.Symbol;
        existingStock.CompanyName = stockDto.CompanyName;
        existingStock.Purchase = stockDto.Purchase;
        existingStock.LastDiv = stockDto.LastDiv;
        existingStock.Industry = stockDto.Industry;
        existingStock.MarketCap = stockDto.MarketCap;

        await _context.SaveChangesAsync();

        return existingStock;
    }
}
