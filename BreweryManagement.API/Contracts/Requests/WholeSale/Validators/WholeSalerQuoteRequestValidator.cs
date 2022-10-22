using FluentValidation;

namespace BreweryManagement.API.Contracts.Requests.WholeSale.Validators
{
    public class WholeSalerQuoteRequestValidator : AbstractValidator<WholeSalerQuoteRequest>
    {
        public WholeSalerQuoteRequestValidator()
        {
            RuleFor(request => request).NotEmpty().WithMessage("The order cannot be empty");
            RuleFor(request => request.WholeSalerId).NotEmpty().WithMessage("The wholesaler must exist");
            RuleFor(request => request.QuoteDetails).NotEmpty().WithMessage("The order cannot be empty");
            RuleFor(request => request.QuoteDetails).Must(list =>
            {
                return list.Select(d => d.BeerId).Distinct().Count() == list.Count();
            }).WithMessage("There can't be any duplicate in the order");
        }
    }
}
