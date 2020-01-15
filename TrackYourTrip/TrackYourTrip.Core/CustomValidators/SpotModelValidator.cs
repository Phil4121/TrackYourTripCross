using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Models;

namespace TrackYourTrip.Core.CustomValidators
{
    public class SpotModelValidator : AbstractValidator<SpotModel>
    {
        public SpotModelValidator()
        {
            RuleFor(s => s.Spot).NotEmpty().WithMessage("Bezeichnung darf nicht leer sein!");
            RuleFor(s => s.ID_SpotType).NotEmpty().WithMessage("Spottype darf nicht leer sein!");
        }
    }
}
