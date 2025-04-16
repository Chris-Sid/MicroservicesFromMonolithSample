using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedFunctionalities.Helpers.Response
{
    public class HeadersHelper
    {
        /// <summary>
        /// Adds a header to the HttpRequestMessage if the header value is not null or empty.
        /// </summary>
        /// <param name="request">The HttpRequestMessage to which the header will be added.</param>
        /// <param name="headerName">The name of the header.</param>
        /// <param name="headerValue">The value of the header.</param>
        public static void AddHeaderIfNotEmpty(HttpRequestMessage request, string headerName, string headerValue)
        {
            if (!string.IsNullOrEmpty(headerValue))
            {
                request.Headers.TryAddWithoutValidation(headerName, headerValue);
            }
        }

        public static void ConfigureHeaders<T>(HttpRequestMessage request, T headers)
        {
            var headerProperties = typeof(T).GetProperties();
            foreach (var property in headerProperties)
            {
                var propertyValue = property.GetValue(headers)?.ToString();
                var headerName = MapPropertyToHeaderName(property.Name);
                if (!string.IsNullOrWhiteSpace(headerName) && !string.IsNullOrWhiteSpace(propertyValue))
                {
                    request.Headers.TryAddWithoutValidation(headerName, propertyValue);
                }
            }
        }

        private static string MapPropertyToHeaderName(string propertyName)
        {
            // Map property names to header names, e.g., using a dictionary or a switch statement
            return propertyName switch
            {
                "Authorization" => "X-Authorization",
                "RequestId" => "X-RequestId",
                "DimensionCompany" => "X-DimensionCompany",
                "DimensionLocation" => "X-DimensionLocation",
                "DimensionBranch" => "X-DimensionBranch",
                "AcceptLanguage" => "Accept-Language",
                "EntityOwnerUserId" => "X-EntityOwnerUserId",
                "EntityOwnerCompanyId" => "X-EntityOwnerCompanyId",
                "DimensionMake" => "X-DimensionMake",
                "DimensionMarketSegment" => "X-DimensionMarketSegment",
                "TimezoneOffset" => "X-TimezoneOffset",
                _ => null, // Return null for unmapped properties
            };
        }
    }
}
