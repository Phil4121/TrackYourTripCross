using FluentValidation;
using TrackYourTrip.Core.Models;

namespace TrackYourTrip.Core.CustomValidators
{
    public class FishedSpotCatchModelValidator : AbstractValidator<FishedSpotCatchModel>
    {
        public FishedSpotCatchModelValidator()
        {
            RuleFor(fsb => fsb.ID_BaitColor).NotEmpty().WithMessage("Köderfarbe darf nicht leer sein!");
            RuleFor(fsb => fsb.ID_BaitType).NotEmpty().WithMessage("Ködertyp darf nicht leer sein!");
            RuleFor(fsb => fsb.ID_BiteDistance).NotEmpty().WithMessage("Distanz darf nicht leer sein!");
            RuleFor(fsb => fsb.ID_FishedSpot).NotEmpty().WithMessage("Spot darf nicht leer sein!");
            RuleFor(fsb => fsb.ID_FishingArea).NotEmpty().WithMessage("Revier darf nicht leer sein!");
            RuleFor(fsb => fsb.ID_Trip).NotEmpty().WithMessage("Trip darf nicht leer sein!");
            RuleFor(fsb => fsb.CatchDateTime).NotEmpty().WithMessage("Uhrzeit darf nicht leer sein!");
            RuleFor(fsb => fsb.ID_Fish).NotEmpty().WithMessage("Fisch darf nicht leer sein!");
            RuleFor(fsb => fsb.FishLength).NotEmpty().WithMessage("Länge darf nicht leer sein!");
        }
    }
}
