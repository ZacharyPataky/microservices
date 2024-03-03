using Shared.Comments;
using Shared.Stocks;

namespace Main;

public class CreateBothRequestDto
{
    public CreateStockRequestDto CreateStockRequestDto { get; set; }
    public CreateCommentRequestDto CreateCommentRequestDto { get; set; }
}
