using AddressService.Application.MockData;
using AddressService.Infrastructure.Services;
using AddressServices.Protos;
using Grpc.Core;

namespace AddressService
{
    public class AddressGrpcService : AddressGrpc.AddressGrpcBase
    {
        private readonly IAddressService _addressService;

        public AddressGrpcService(IAddressService addressService)
        {
            _addressService = addressService;
        }

        public override async Task<AddressDto> GetAddress(AddressRequest request, ServerCallContext context)
        {
            var internalId = "int1";// request.Id.InternalId;

            var result = MockAddressData.Addresses
                .FirstOrDefault(x => x.AddressId.InternalId == internalId);

            if (result == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Address not found for ID: {internalId}"));
            }

            return new AddressDto
            {
                AddressId = new DmsIdentifier
                {
                    InternalId = result.AddressId.InternalId,
                    ExternalId = result.AddressId.ExternalId
                },
                Street = result.Street,
                City = result.City
            };
        }
    }
}
