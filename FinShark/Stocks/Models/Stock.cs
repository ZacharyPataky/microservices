﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Stocks.Models;

public class Stock
{
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Purchase { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal LastDiv { get; set; }
    public string Industry { get; set; } = string.Empty;
    public long MarketCap { get; set; }

    // 1 -> M Relationships
    public List<Comment> Comments { get; set; } = new List<Comment>();
}
