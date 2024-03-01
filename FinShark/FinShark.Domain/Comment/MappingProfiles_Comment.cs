using AutoMapper;
using DB = FinShark.DAL.Models;

namespace FinShark.Domain.Comment;

public class MappingProfiles_Comment : Profile
{
    public MappingProfiles_Comment()
    {
        CreateMap<DB.Comment, CommentDto>();
        CreateMap<CreateCommentRequestDto, DB.Comment>();
        CreateMap<UpdateCommentRequestDto, DB.Comment>();
    }
}
