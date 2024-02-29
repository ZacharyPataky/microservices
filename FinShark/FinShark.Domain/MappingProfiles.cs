using AutoMapper;
using FinShark.Domain.Stock;
using DB = FinShark.DAL.Models;

namespace FinShark.Domain;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<DB.Stock, StockDto>();
        CreateMap<CreateStockRequestDto, DB.Stock>();
    }
}
