using System.Collections.Generic;

namespace CSM.API
{
    public interface IVehicleSyncSender
    {
        void SendVehiclePositions(IEnumerable<VehiclePositionData> vehicles);
        bool IsServer();
    }

    public struct VehiclePositionData
    {
        public ushort VehicleId;
        public float X;
        public float Y;
        public float Z;

        public VehiclePositionData(float x, float y, float z, ushort id)
        {
            X = x; Y = y; Z = z; VehicleId = id;
        }
    }
}
