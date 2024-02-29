using AutoMapper;
using DB = FinShark.DAL.Models;

namespace FinShark.Domain.Stock;

public class MappingProfiles_Stock : Profile
{
    public MappingProfiles_Stock()
    {
        CreateMap<DB.Stock, StockDto>();
        CreateMap<CreateStockRequestDto, DB.Stock>();
        CreateMap<UpdateStockRequestDto, DB.Stock>();
    }
}
