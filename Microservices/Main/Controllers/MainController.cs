using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Comments;
using Shared.Stocks;

namespace Main.Controllers;

[ApiController]
[Route("api/main")]
public class MainController : ControllerBase
{
    private string _apiGateway_Stock;
    private string _apiGateway_Stock_Id;
    private string _apiGateway_Comment;
    private string _apiGateway_Comment_Id;

    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public MainController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;

        _apiGateway_Stock = _configuration.GetValue<string>("ApiGateway_Stock")!;
        _apiGateway_Stock_Id = _configuration.GetValue<string>("ApiGateway_Stock_Id")!;
        _apiGateway_Comment = _configuration.GetValue<string>("ApiGateway_Comment")!;
        _apiGateway_Comment_Id = _configuration.GetValue<string>("ApiGateway_Comment_Id")!;
    }

    #region Stocks

    [HttpGet]
    [Route("stocks")]
    public async Task<IActionResult> GetStocks()
    {
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync(_apiGateway_Stock);

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
        var reqUrl = _apiGateway_Stock_Id.Replace("###ID###", stockId.ToString());
        var response = await httpClient.GetAsync(reqUrl);

        if (!response.IsSuccessStatusCode)
            return BadRequest();

        string responseBody = await response.Content.ReadAsStringAsync();
        var stockDto = JsonConvert.DeserializeObject<StockDto>(responseBody);
        return Ok(stockDto);
    }

    [HttpPost]
    [Route("stocks")]
    public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto createStockRequestDto)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var httpContent = new StringContent(
            JsonConvert.SerializeObject(createStockRequestDto), 
            System.Text.Encoding.UTF8, 
            "application/json");
        var response = await httpClient.PostAsync(_apiGateway_Stock, httpContent);

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
        var response = await httpClient.GetAsync(_apiGateway_Comment);

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
        var reqUrl = _apiGateway_Comment_Id.Replace("###ID###", commentId.ToString());
        var response = await httpClient.GetAsync(reqUrl);

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
        var reqUrl = _apiGateway_Comment_Id.Replace("###ID###", stockId.ToString());
        var response = await httpClient.PostAsync(reqUrl, httpContent);

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
        var stockResponse = await httpClient.PostAsync(_apiGateway_Stock, stockHttpContent);

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
        var reqUrl = _apiGateway_Comment_Id.Replace("###ID###", stockId.ToString());
        var commentResponse = await httpClient.PostAsync(reqUrl, commentHttpContent);

        if (!commentResponse.IsSuccessStatusCode)
        {
            var reqUrlDel = _apiGateway_Stock_Id.Replace("###ID###", stockId.ToString());
            await httpClient.DeleteAsync(reqUrlDel);
            return BadRequest("Comment failed.");
        }

        return Ok("Both the stock and comment were created.");
    }

    #endregion
}
