using System.Net;

using FluentValidation;

using PaymentGateway.Api.Models.Requests;

namespace PaymentGateway.Api.Validators
{
    public class PostPaymentRequestValidator : AbstractValidator<PostPaymentRequest>
    {
        public PostPaymentRequestValidator() { 
            RuleFor(x => x.CardNumber).NotEmpty().Length(14,19).WithErrorCode("Rejected");
            RuleFor(x => x.CardNumber).Must(x => x.All(char.IsDigit)).WithErrorCode("Rejected");
        }
    }
}
