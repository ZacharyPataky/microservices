using FinShark.Domain.Stock;
using DB = FinShark.DAL.Models;

namespace FinShark.Domain.Interfaces;

public interface IStockRepository
{
    Task<List<DB.Stock>> GetStocksAsync(QueryObject queryObject);
    Task<DB.Stock?> GetStockByIdAsync(int stockId);
    Task<DB.Stock> CreateStockAsync(DB.Stock stockModel);
    Task<DB.Stock> UpdateStockAsync(int stockId, UpdateStockRequestDto stockDto);
    Task<DB.Stock?> DeleteStockAsync(int stockId);
    Task<bool> StockExists(int stockId);
}
