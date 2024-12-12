using CarAuctionCommon.Entities;
using CarAuctionCommon.Enums;

namespace CarAuctionCommon.Dtos
{
    public record VehicleDto(int VehicleId, string Plate, VehicleType TypeId, int NumDoors, int LoadCapacity, string Model, int Year, decimal StartingBid)
    {
        public Vehicle ToVehicle()
        {
            return new Vehicle { Id = VehicleId, Plate = Plate, TypeId = TypeId, NumDoors  = NumDoors, LoadCapacity = LoadCapacity, Model = Model, Year = Year, StartingBid = StartingBid };  
        }
    }
}
