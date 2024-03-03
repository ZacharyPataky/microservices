namespace Shared.Models;

public class Comment
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public int? StockId { get; set; }

    // Navigation Property -> Let's us access the Stock directly using dot-notation
    public Stock? Stock { get; set; }
}

