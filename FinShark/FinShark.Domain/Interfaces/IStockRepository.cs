using FinShark.Domain.Stock;
using DB = FinShark.DAL.Models;

namespace FinShark.Domain.Interfaces;

public interface IStockRepository
{
    Task<List<DB.Stock>> GetStocksAsync();
    Task<DB.Stock?> GetStockByIdAsync(int id);
    Task<DB.Stock> CreateStockAsync(DB.Stock stockModel);
    Task<DB.Stock> UpdateStockAsync(int id, UpdateStockRequestDto stockDto);
    Task<DB.Stock?> DeleteStockAsync(int id);
    Task<bool> StockExists(int id);
}
