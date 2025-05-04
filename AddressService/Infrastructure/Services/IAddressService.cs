using SharedFunctionalities.DTOs.Address;

namespace AddressService.Infrastructure.Services
{
    public interface IAddressService
    {
        Task<AddressDto> GetAddress(DmsIdentifier Address, AddressRequestHeaders headers, CancellationToken cancellationToken = default);
        Task<AddressDto> PostAddressAsync(AddressDto AddressDto, AddressRequestHeaders headers, CancellationToken cancellationToken = default);
    }
}
