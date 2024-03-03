using Azure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Comments;
using Shared.Models;
using Shared.Stocks;

namespace Main.Controllers;

[ApiController]
[Route("api/main")]
public class MainController : ControllerBase
{
    private string _stocksBase;
    private string _commentsBase;

    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public MainController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;

        _stocksBase = _configuration.GetValue<string>("StocksBase")!;
        _commentsBase = _configuration.GetValue<string>("CommentsBase")!;
    }

    #region Stocks

    [HttpGet]
    [Route("stocks")]
    public async Task<IActionResult> GetStocks()
    {
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync(_stocksBase);

        if (!response.IsSuccessStatusCode)
            return BadRequest();

        string responseBody = await response.Content.ReadAsStringAsync();
        var stockDtos = JsonConvert.DeserializeObject<List<StockDto>>(responseBody);
        return Ok(stockDtos);
    }

    [HttpGet]
    [Route("stocks/{stockId:int}")]
    public async Task<IActionResult> GetStockById([FromRoute] int stockId)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync(_stocksBase + $"/{stockId}");

        if (!response.IsSuccessStatusCode)
            return BadRequest();

        string responseBody = await response.Content.ReadAsStringAsync();
        var stockDto = JsonConvert.DeserializeObject<StockDto>(responseBody);
        return Ok(stockDto);
    }

    [HttpPost]
    [Route("stocks")]
    public async Task<IActionResult> CreateStock([FromBody] CreateCommentRequestDto createStockRequestDto)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var httpContent = new StringContent(
            JsonConvert.SerializeObject(createStockRequestDto), 
            System.Text.Encoding.UTF8, 
            "application/json");
        var response = await httpClient.PostAsync(_stocksBase, httpContent);

        if (!response.IsSuccessStatusCode)
            return BadRequest();

        string responseBody = await response.Content.ReadAsStringAsync();
        var stockDto = JsonConvert.DeserializeObject<StockDto>(responseBody);
        return Ok(stockDto);
    }

    #endregion

    #region Comments

    [HttpGet]
    [Route("comments")]
    public async Task<IActionResult> GetComments()
    {
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync(_commentsBase);

        if (!response.IsSuccessStatusCode)
            return BadRequest();

        string responseBody = await response.Content.ReadAsStringAsync();
        var commentDtos = JsonConvert.DeserializeObject<List<CommentDto>>(responseBody);
        return Ok(commentDtos);
    }

    [HttpGet]
    [Route("comments/{commentId:int}")]
    public async Task<IActionResult> GetCommentById([FromRoute] int commentId)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync(_commentsBase + $"/{commentId}");

        if (!response.IsSuccessStatusCode)
            return BadRequest();

        string responseBody = await response.Content.ReadAsStringAsync();
        var commentDto = JsonConvert.DeserializeObject<CommentDto>(responseBody);
        return Ok(commentDto);
    }

    [HttpPost]
    [Route("comments/{stockId:int}")]
    public async Task<IActionResult> CreateComment([FromRoute] int stockId, [FromBody] CreateCommentRequestDto createCommentRequestDto)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var httpContent = new StringContent(
            JsonConvert.SerializeObject(createCommentRequestDto),
            System.Text.Encoding.UTF8,
            "application/json");
        var response = await httpClient.PostAsync(_commentsBase + $"/{stockId}", httpContent);

        if (!response.IsSuccessStatusCode)
            return BadRequest();

        string responseBody = await response.Content.ReadAsStringAsync();
        var commentDto = JsonConvert.DeserializeObject<CommentDto>(responseBody);
        return Ok(commentDto);
    }

    #endregion

    #region Both

    [HttpPost]
    [Route("both")]
    public async Task<IActionResult> CreateBoth([FromBody] CreateBothRequestDto createBothRequestDto)
    {
        var httpClient = _httpClientFactory.CreateClient();
     
        // Stock
        var stockHttpContent = new StringContent(
            JsonConvert.SerializeObject(createBothRequestDto.CreateStockRequestDto),
            System.Text.Encoding.UTF8,
            "application/json");
        var stockResponse = await httpClient.PostAsync(_stocksBase, stockHttpContent);

        if (!stockResponse.IsSuccessStatusCode)
            return BadRequest("Stock failed.");

        string stockResponseBody = await stockResponse.Content.ReadAsStringAsync();
        var stockDto = JsonConvert.DeserializeObject<StockDto>(stockResponseBody);
        var stockId = stockDto!.Id;

        // Comment
        var commentHttpContent = new StringContent(
            JsonConvert.SerializeObject(createBothRequestDto.CreateCommentRequestDto),
            System.Text.Encoding.UTF8,
            "application/json");
        var commentResponse = await httpClient.PostAsync(_commentsBase + $"/{stockId}", commentHttpContent);

        if (!commentResponse.IsSuccessStatusCode)
        {
            await httpClient.DeleteAsync(_stocksBase + $"/{stockId}");
            return BadRequest("Comment failed.");
        }

        return Ok("Both the stock and comment were created.");
    }

    #endregion
}
