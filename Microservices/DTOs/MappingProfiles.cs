using AutoMapper;
using Shared.Comments;
using Shared.Models;
using Shared.Stocks;

namespace DTOs;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Stock, StockDto>();
        CreateMap<CreateStockRequestDto, Stock>();
        CreateMap<UpdateStockRequestDto, Stock>();
        CreateMap<Comment, CommentDto>();
        CreateMap<CreateCommentRequestDto, Comment>();
        CreateMap<UpdateCommentRequestDto, Comment>();
    }
}
