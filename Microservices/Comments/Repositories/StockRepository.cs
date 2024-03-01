using Comments.DTOs;
using Comments.Helpers;
using Comments.Interfaces;
using Comments.Models;
using Microsoft.EntityFrameworkCore;

namespace Comments.Repositories;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _context;

    public StockRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Stock> CreateStockAsync(Stock stockModel)
    {
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<Stock?> DeleteStockAsync(int stockId)
    {
        var stockModel = await _context.Stocks
            .FirstOrDefaultAsync(stock => stock.Id == stockId);

        if (stockModel == null)
            return null;

        _context.Stocks.Remove(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<Stock?> GetStockByIdAsync(int stockId)
    {
        return await _context.Stocks
            .Include(comment => comment.Comments)
            .FirstOrDefaultAsync(stock => stock.Id == stockId);
    }

    public async Task<List<Stock>> GetStocksAsync(QueryObject queryObject)
    {
        var stockModels = _context.Stocks
            .Include(comment => comment.Comments)
            .AsQueryable();

        // Query Params /////////////////////////////////////////////////////////

        // Company Name
        if (!string.IsNullOrWhiteSpace(queryObject.CompanyName))
            stockModels = stockModels
                .Where(stock => stock.CompanyName
                    .Contains(queryObject.CompanyName));

        // Symbol
        if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
            stockModels = stockModels
                .Where(stock => stock.Symbol
                    .Contains(queryObject.Symbol));

        // Sort
        if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
            if (queryObject.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                stockModels = queryObject.IsDescending ?
                    stockModels.OrderByDescending(stock => stock.Symbol) :
                    stockModels.OrderBy(stock => stock.Symbol);

        // Pagination
        var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;

        return await stockModels
            .Skip(skipNumber)
            .Take(queryObject.PageSize)
            .ToListAsync();
    }

    public Task<bool> StockExists(int stockId)
    {
        return _context.Stocks.AnyAsync(stock => stock.Id == stockId);
    }

    public async Task<Stock> UpdateStockAsync(int stockId, UpdateStockRequestDto stockDto)
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
