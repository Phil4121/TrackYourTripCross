using FluentValidation;
using System;
using TrackYourTrip.Core.Models;

namespace TrackYourTrip.Core.CustomValidators
{
    public class FishedSpotBiteModelValidator : AbstractValidator<FishedSpotBiteModel>
    {
        public FishedSpotBiteModelValidator()
        {
            RuleFor(fsb => fsb.ID_BaitColor).NotEmpty().WithMessage("Köderfarbe darf nicht leer sein!");
            RuleFor(fsb => fsb.ID_BaitType).NotEmpty().WithMessage("Ködertyp darf nicht leer sein!");
            RuleFor(fsb => fsb.ID_BiteDistance).NotEmpty().WithMessage("Distanz darf nicht leer sein!");
            RuleFor(fsb => fsb.ID_FishedSpot).NotEmpty().WithMessage("Spot darf nicht leer sein!");
            RuleFor(fsb => fsb.ID_FishingArea).NotEmpty().WithMessage("Revier darf nicht leer sein!");
            RuleFor(fsb => fsb.ID_Trip).NotEmpty().WithMessage("Trip darf nicht leer sein!");
            RuleFor(fsb => fsb.BiteDateTime).NotEmpty().WithMessage("Uhrzeit darf nicht leer sein!");
        }
    }
}
