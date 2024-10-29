using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Models.Requests;

namespace PaymentGateway.Api.Tests;

public class PaymentsControllerValidationTests : IAsyncLifetime
{
    private readonly Random _random = new();
    private HttpClient _client;
    public async Task InitializeAsync()
    {
        var webApplicationFactory = new WebApplicationFactory<PaymentsController>();
        _client = webApplicationFactory.CreateClient();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task PostingPayment_CardNumberIsNotSupplied_RejectRequest()
    {
        // Arrange
        var request = new PostPaymentRequest
        {
            ExpiryYear = _random.Next(2023, 2030),
            ExpiryMonth = _random.Next(1, 12).ToString(),
            Amount = _random.Next(1, 10000),
            Currency = "GBP",
            Cvv = _random.Next(100,999)
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/payments", request);

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Theory]
    [InlineData("1234567891011")] //Too short
    [InlineData("1234567891011121314")] //Too long
    public async Task PostingPayment_CardNumberIsNotValidLength_RejectRequest(string cardNumber)
    {
        // Arrange
        var request = new PostPaymentRequest
        {
            ExpiryYear = _random.Next(2023, 2030),
            ExpiryMonth = _random.Next(1, 12).ToString(),
            Amount = _random.Next(1, 10000),
            Currency = "GBP",
            Cvv = _random.Next(100, 999),
            CardNumber = cardNumber
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/payments", request);

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task PostingPayment_CardNumberIsContainsNonNumericalChars_RejectRequest()
    {
        // Arrange
        var request = new PostPaymentRequest
        {
            ExpiryYear = _random.Next(2023, 2030),
            ExpiryMonth = _random.Next(1, 12).ToString(),
            Amount = _random.Next(1, 10000),
            Currency = "GBP",
            Cvv = _random.Next(100, 999),
            CardNumber = "test"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/payments", request);

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    //[Fact]
    //public async Task PostingPayment_ExpiryMonthIsNotSupplied_RejectRequest()
    //{
    //    // Arrange
    //    var request = new PostPaymentRequest
    //    {
    //        ExpiryYear = _random.Next(2023, 2030),
    //        Amount = _random.Next(1, 10000),
    //        Currency = "GBP",
    //        Cvv = _random.Next(100, 999),
    //        CardNumber = "1234123412341234"
    //    };

    //    // Act
    //    var response = await _client.PostAsJsonAsync("/api/payment", request);

    //    // Assert
    //    Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    //}

    //[Fact]
    //public async Task PostingPayment_ExpiryMonthIsNotInValidRange_RejectRequest()
    //{
    //    // Arrange
    //    var request = new PostPaymentRequest
    //    {
    //        ExpiryYear = _random.Next(2023, 2030),
    //        Amount = _random.Next(1, 10000),
    //        Currency = "GBP",
    //        Cvv = _random.Next(100, 999),
    //        CardNumber = "1234123412341234"
    //    };

    //    // Act
    //    var response = await _client.PostAsJsonAsync("/api/payment", request);

    //    // Assert
    //    Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    //}

    //[Fact]
    //public async Task PostingPayment_ExpiryYearIsNotSupplied_RejectRequest()
    //{
    //    // Arrange

    //    // Act

    //    // Assert
    //}

    //[Fact]
    //public async Task PostingPayment_ExpiryYearIsInTheFuture_RejectRequest()
    //{
    //    // Arrange

    //    // Act

    //    // Assert
    //}

    //[Fact]
    //public async Task PostingPayment_CurrencyIsNotProvided_RejectRequest()
    //{
    //    // Arrange

    //    // Act

    //    // Assert
    //}

    //[Fact]
    //public async Task PostingPayment_CurrencyIsNotValid_RejectRequest()
    //{
    //    // Arrange

    //    // Act

    //    // Assert
    //}

    //[Fact]
    //public async Task PostingPayment_AmountIsNotProvided_RejectRequest()
    //{
    //    // Arrange

    //    // Act

    //    // Assert
    //}

    //[Fact]
    //public async Task PostingPayment_AmountIsNotAnInt_RejectRequest()
    //{
    //    // Arrange

    //    // Act

    //    // Assert
    //}

    //[Fact]
    //public async Task PostingPayment_CvvIsNotProvided_RejectRequest()
    //{
    //    // Arrange

    //    // Act

    //    // Assert
    //}

    //[Fact]
    //public async Task PostingPayment_CvvIsInvalidLength_RejectRequest()
    //{
    //    // Arrange

    //    // Act

    //    // Assert
    //}

    //[Fact]
    //public async Task PostingPayment_CvvIsNonNumerical_RejectRequest()
    //{
    //    // Arrange

    //    // Act

    //    // Assert
    //}
}