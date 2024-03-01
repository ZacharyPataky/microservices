using AutoMapper;
using Comments.Models;

namespace Comments.DTOs;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Comment, CommentDto>();
        CreateMap<CreateCommentRequestDto, Comment>();
        CreateMap<UpdateCommentRequestDto, Comment>();
    }
}
