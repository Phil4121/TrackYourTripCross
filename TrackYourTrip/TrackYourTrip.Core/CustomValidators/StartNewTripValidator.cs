using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Models;

namespace TrackYourTrip.Core.CustomValidators
{
    public class StartNewTripValidator : AbstractValidator<TripModel>
    {
        public StartNewTripValidator()
        {
            RuleFor(tm => tm.FishingArea).NotEmpty().WithMessage("Revier darf nicht leer sein!");
            RuleFor(tm => tm.TripDateTime).NotEmpty().WithMessage("Startdatum darf nicht leer sein!");
        }
    }
}
