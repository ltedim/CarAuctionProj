using CarAuctionCommon.Enums;

namespace CarAuctionCommon.Dtos
{
    public record VehicleSearchDto(VehicleType? TypeId, string? Manufacturer, string? Model, int? Year)
    {
    }
}
