using Shared;
using Shared.Models;
using Shared.Stocks;

namespace Shared.Interfaces;

public interface IStockRepository
{
    Task<List<Stock>> GetStocksAsync(QueryObject queryObject);
    Task<Stock?> GetStockByIdAsync(int stockId);
    Task<Stock> CreateStockAsync(Stock stockModel);
    Task<Stock> UpdateStockAsync(int stockId, UpdateStockRequestDto stockDto);
    Task<Stock?> DeleteStockAsync(int stockId);
    Task<bool> StockExists(int stockId);
}
