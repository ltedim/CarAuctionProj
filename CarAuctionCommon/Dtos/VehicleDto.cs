using CarAuctionCommon.Enums;

namespace CarAuctionCommon.Dtos
{
    public record VehicleDto(int VehicleId, string Plate, VehicleType TypeId, int NumDoors, int LoadCapacity, string Model, int Year, decimal StartingBid);
}
