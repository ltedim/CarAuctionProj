using VehicleAuctionCommon.Entities;
using VehicleAuctionCommon.Enums;

namespace VehicleAuctionCommon.Dtos
{
    public record VehicleDto (int VehicleId, string Plate, VehicleType TypeId, int NumDoors, int LoadCapacity, string Model, int Year, decimal StartingBid, string Manufacturer)
    {
        public Vehicle ToVehicle()
        {
            return new Vehicle { Id = VehicleId, Plate = Plate, TypeId = TypeId, NumDoors  = NumDoors, LoadCapacity = LoadCapacity, Model = Model, Year = Year, StartingBid = StartingBid, Manufacturer = Manufacturer };  
        }
    }
}
