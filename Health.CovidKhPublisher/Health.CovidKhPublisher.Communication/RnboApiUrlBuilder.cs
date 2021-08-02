using System;
using Health.CovidKhPublisher.Business.Models.Domain;

namespace Health.CovidKhPublisher.Communication
{
    public static class RnboApiUrlBuilder
    {
        private const string BaseUrl = "https://api-covid19.rnbo.gov.ua";

        public static string BuildMainDataUrl(Location location)
        {
            if (location == Location.Kharkiv)
            {
                return $"{BaseUrl}/charts/main-data?mode=ukraine&country=4901";
            }

            throw new InvalidOperationException("Unsupported location");
        }
    }
}
