using FluentValidation;
using TrackYourTrip.Core.Models;

namespace TrackYourTrip.Core.CustomValidators
{
    public class SpotModelValidator : AbstractValidator<SpotModel>
    {
        public SpotModelValidator()
        {
            RuleFor(s => s.Spot).NotEmpty().WithMessage("Bezeichnung darf nicht leer sein!");
            RuleFor(s => s.ID_SpotType).NotEmpty().WithMessage("Spottype darf nicht leer sein!");
            RuleFor(s => s.SpotMarker).Must(l => l.Count > 0).WithMessage("Spot muss markiert werden!");
        }
    }
}
