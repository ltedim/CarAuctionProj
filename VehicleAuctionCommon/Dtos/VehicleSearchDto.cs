using VehicleAuctionCommon.Enums;

namespace VehicleAuctionCommon.Dtos
{
    public record VehicleSearchDto(VehicleType? TypeId, string? Manufacturer, string? Model, int? Year)
    {
    }
}
