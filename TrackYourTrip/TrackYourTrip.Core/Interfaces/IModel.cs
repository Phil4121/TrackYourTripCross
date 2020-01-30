using System;

namespace TrackYourTrip.Core.Interfaces
{
    public interface IModel
    {
        Guid Id { get; set; }
        bool IsNew { get; set; }
        bool IsValid { get; set; }
    }
}
