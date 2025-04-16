using AddressService.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Configuration;
using SharedFunctionalities.DTOs.Address;

namespace AddressService.API.Controllers
{
    [Route("api/dms/addresses/")]
    public class GetAddress : Controller
    {
        private readonly IAddressService _addressService;
        private readonly IConfiguration _configuration;

        public GetAddress(IAddressService addressService, IConfiguration configuration)
        {
            _addressService = addressService;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("api/dms/addresses/{internalId}")]
        public async Task<IActionResult> Address(string internalId, [FromHeader] AddressRequestHeaders headers)
        {
            var username = User.Identity?.Name;

            // You can access headers.RequestId or other dimensions here
            var address = await _addressService.GetAddressAsync(
                new DmsIdentifier { InternalId = internalId }, headers);
            return Ok(address);
        }


    }
}
