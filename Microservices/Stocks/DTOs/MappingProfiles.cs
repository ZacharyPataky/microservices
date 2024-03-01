using AutoMapper;
using Stocks.Models;

namespace Stocks.DTOs;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Stock, StockDto>();
        CreateMap<Comment, CommentDto>();
        CreateMap<CreateStockRequestDto, Stock>();
        CreateMap<UpdateStockRequestDto, Stock>();
    }
}
