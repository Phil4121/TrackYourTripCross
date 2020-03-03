using System;
using System.Collections.Generic;
using System.Text;

namespace TrackYourTrip.Core.Helpers
{
    public static class StatusHelper
    {
        public enum StatusPicEnum
        {
            STATUS_OK,
            STATUS_UNDEFINED,
            STATUS_ERROR,
            STATUS_WAITING
        }

        public static string GetPicForStatus(StatusPicEnum status)
        {
            try
            {
                switch (status)
                {
                    case StatusPicEnum.STATUS_OK:
                        return "Success.png";

                    case StatusPicEnum.STATUS_ERROR:
                        return "Error.png";

                    case StatusPicEnum.STATUS_WAITING:
                        return "Waiting.png";

                    default:
                        return "Unknown.png";
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException("Status not implemented");
            }
        }
    }
}
