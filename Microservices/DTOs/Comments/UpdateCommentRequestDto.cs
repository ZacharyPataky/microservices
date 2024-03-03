using System.ComponentModel.DataAnnotations;

namespace Shared.Comments;

public class UpdateCommentRequestDto
{
    [Required]
    [MinLength(5, ErrorMessage = "Title must be 5+ characters.")]
    [MaxLength(280, ErrorMessage = "Title must not exceed 280 characters.")]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MinLength(5, ErrorMessage = "Content must be 5+ characters.")]
    [MaxLength(280, ErrorMessage = "Content must not exceed 280 characters.")]
    public string Content { get; set; } = string.Empty;
}
