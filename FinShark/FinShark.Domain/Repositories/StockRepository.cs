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

    public async Task<DB.Stock?> DeleteStockAsync(int id)
    {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);

        if (stockModel == null)
            return null;

        _context.Stocks.Remove(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<DB.Stock?> GetStockByIdAsync(int id)
    {
        return await _context.Stocks.FindAsync(id);
    }

    public async Task<List<DB.Stock>> GetStocksAsync()
    {
        return await _context.Stocks.ToListAsync();
    }

    public async Task<DB.Stock> UpdateStockAsync(int id, UpdateStockRequestDto stockDto)
    {
        var existingStock = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);

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
