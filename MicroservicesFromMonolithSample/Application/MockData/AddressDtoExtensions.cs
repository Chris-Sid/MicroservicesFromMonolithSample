using SharedFunctionalities.DTOs.Address;

namespace AddressService.Application.MockData
{
    public static class AddressDtoExtensions
    {
        public static AddressDto Clone(this AddressDto source)
        {
            return new AddressDto
            {
                AddressId = new DmsIdentifier
                {
                    InternalId = source.AddressId.InternalId,
                    ExternalId = source.AddressId.ExternalId
                },
                BusinessPartnerId = source.BusinessPartnerId,
                ContactPersonId = source.ContactPersonId,
                Street = source.Street,
                Block = source.Block,
                ZipCode = source.ZipCode,
                City = source.City,
                County = source.County,
                State = source.State,
                StreetNo = source.StreetNo,
                Longitude = source.Longitude,
                Latitude = source.Latitude,
                FederalTaxId = source.FederalTaxId,
                TaxCode = source.TaxCode,
                BuildingFloorRoom = source.BuildingFloorRoom,
                CountryId = source.CountryId,
                PlaceType = source.PlaceType,
                AddressType = source.AddressType,
                IsDefault = source.IsDefault,
                AltLanguageStreet = source.AltLanguageStreet,
                AltLanguageCity = source.AltLanguageCity,
                AltLanguageStateDescription = source.AltLanguageStateDescription,
                AltLanguageBlock = source.AltLanguageBlock,
                AltLanguageBuildingFloorRoom = source.AltLanguageBuildingFloorRoom
            };
        }
    }
}
