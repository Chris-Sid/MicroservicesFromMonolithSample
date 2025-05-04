﻿using AddressService.Application.MockData;
using SharedFunctionalities.DTOs.Address;
using SharedFunctionalities.Helpers.Response;
using System.Net.Http;

namespace AddressService.Infrastructure.Services
{
    public class AddressServiceHub : IAddressService
    {

        private readonly IHttpClientFactory _httpClient;
        private readonly string _baseUrl;
        public AddressServiceHub(IHttpClientFactory httpClient, string baseUrl)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>
        /// Get Address
        /// </summary>
        /// <remarks>
        /// This operation is used to get an Address from DMS. {addressId} will be known to DMS.
        /// </remarks>
        /// <param name="addressId">Identifier Value</param>
        /// <param name="x_Authorization">Authorization Token</param>
        /// <param name="x_RequestId">Request Id</param>
        /// <param name="accept_Language">Accept Language. If not set default value is 'en'</param>
        /// <param name="x_EntityOwnerUserId">Entity Owner User Id</param>
        /// <param name="x_EntityOwnerCompanyId">Entity Owner Company Id</param>
        /// <param name="x_DimensionCompany">Dimension Company</param>
        /// <param name="x_DimensionLocation">Dimension Location</param>
        /// <param name="x_DimensionBranch">Dimension Branch</param>
        /// <param name="x_DimensionMake">Dimension Make</param>
        /// <param name="x_DimensionMarketSegment">Dimension Market Segment</param>
        /// <param name="x_TimezoneOffset">Timezone offset to handle datetimes</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<AddressDto> GetAddress(DmsIdentifier addressRequest, AddressRequestHeaders headers, CancellationToken cancellationToken = default)
        {
            ValidateInputs(addressRequest, headers);
            using (var client = _httpClient.CreateClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/api/dms/addresses/{Uri.EscapeDataString(addressRequest.InternalId)}"))
            {
                HeadersHelper.ConfigureHeaders(request, headers);

                HttpResponseMessage response;
                try
                {
                    //return Mock Data if requestID="mock" and internalID == int1 || int2 || int3
                    if (headers.RequestId == "mock" && MockAddressData.Addresses.Any(x => x.AddressId.InternalId.Equals(addressRequest.InternalId)))
                    {
                        var result = MockAddressData.Addresses.FirstOrDefault(x => x.AddressId.InternalId.Equals(addressRequest.InternalId));
                        if (result != null)
                            return result;
                    }
                    response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                }
                catch (Exception ex)
                {
                 //   _logger.LogError(ex, ex.Message);
                    throw;
                }

                return await ResponseProcessor.ProcessResponseAsync<AddressDto>(response);
            }
        }

        public async Task<AddressDto> PostAddressAsync(AddressDto addressRequest, AddressRequestHeaders headers, CancellationToken cancellationToken = default)
        {
            ValidateInputs(addressRequest.AddressId, headers);
            using (var client = _httpClient.CreateClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/api/dms/addresses/{addressRequest}"))
            {
                HeadersHelper.ConfigureHeaders(request, headers);

                HttpResponseMessage response;
                try
                {
                  
                        //return Mock Data if requestID="mock" and internalID == int1 || int2 || int3
                        if (headers.RequestId == "mock" && MockAddressData.Addresses.Any(x => x.AddressId.InternalId.Equals(addressRequest.AddressId.InternalId)))
                    {
                        var existing = MockAddressData.Addresses.FirstOrDefault(x =>
                   x.AddressId.InternalId.Equals(addressRequest.AddressId.InternalId, StringComparison.OrdinalIgnoreCase));
                        if (existing != null)
                        {
                            var index = MockAddressData.Addresses.IndexOf(existing);
                            MockAddressData.Addresses[index] = addressRequest;  //  Replace it with updated
                            return addressRequest;                              //  Return updated one
                        }
                        else
                        {
                            // Insert: add new item
                            MockAddressData.Addresses.Add(addressRequest);
                        }
                        return addressRequest; // return the upserted address
                    }
                    response = await client.PostAsync(_baseUrl, request.Content, cancellationToken);
                }
                catch (Exception ex)
                {
                   // _logger.LogError(ex, ex.Message);
                    throw;
                }

                return await ResponseProcessor.ProcessResponseAsync<AddressDto>(response);
            }
        }

        private static void ValidateInputs(DmsIdentifier addressId, AddressRequestHeaders headers)
        {
            if (string.IsNullOrWhiteSpace(addressId.InternalId))
                throw new ArgumentNullException(nameof(addressId));

            if (headers == null)
                throw new ArgumentNullException(nameof(headers));

            if (string.IsNullOrWhiteSpace(headers.Authorization))
                throw new ArgumentNullException(nameof(headers.Authorization));

            if (string.IsNullOrWhiteSpace(headers.RequestId))
                throw new ArgumentNullException(nameof(headers.RequestId));
        }
    }
}
