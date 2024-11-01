﻿namespace PaymentGateway.Api.Models.Requests;

public class PostPaymentRequest
{
    public string CardNumber{ get; set; }
    public string ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string Currency { get; set; }
    public int Amount { get; set; }
    public int Cvv { get; set; }
}