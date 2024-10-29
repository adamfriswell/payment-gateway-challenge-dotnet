using System.Text.Json;

using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : Controller
{
    private readonly PaymentsRepository _paymentsRepository;
    private readonly IHttpClientFactory _httpClientFactory;

    public PaymentsController(PaymentsRepository paymentsRepository, IHttpClientFactory httpClientFactory)
    {
        _paymentsRepository = paymentsRepository;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PostPaymentResponse?>> GetPaymentAsync(Guid id)
    {
        var payment = _paymentsRepository.Get(id);

        return new OkObjectResult(payment);
    }

    [HttpPost("")]
    public async Task<ActionResult<PostPaymentResponse?>> PostPaymentAysnc([FromBody] PostPaymentRequest request) {

        var bankingSimulatorRequest = new BankingSimulatorRequest()
        {
            CardNumber = request.CardNumber,
            ExpiryDate = $"{request.ExpiryMonth}/{request.ExpiryYear}",
            Currency = request.Currency,
            Amount = request.Amount,
            Cvv = request.Cvv.ToString()
        };
        
        using var client = _httpClientFactory.CreateClient("BankingSimulator");

        try
        {
            var jsonRequest = JsonSerializer.Serialize(bankingSimulatorRequest);
            var response = await client.PostAsJsonAsync("payments", jsonRequest);
            var result = await response.Content.ReadAsStringAsync();
            return new OkObjectResult(response);
        }
        catch (Exception)
        {
            throw;
        }
    }
}