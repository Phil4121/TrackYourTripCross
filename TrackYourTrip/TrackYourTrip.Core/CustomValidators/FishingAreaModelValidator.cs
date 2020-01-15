using FluentValidation;
using TrackYourTrip.Core.Models;

namespace TrackYourTrip.Core.CustomValidators
{
    public class FishingAreaModelValidator : AbstractValidator<FishingAreaModel>
    {
        public FishingAreaModelValidator()
        {
            RuleFor(fa => fa.FishingArea).NotEmpty().WithMessage("Bezeichnung darf nicht leer sein!");
            RuleFor(fa => fa.ID_WaterModel).NotEmpty().WithMessage("Gewässertyp darf nicht leer sein!");
            RuleFor(fa => fa.AreaLocation)
                .Must(l => l.Latitude != 0)
                .WithMessage("Standort des Revieres muss festgelegt werden!");
        }
    }
}
