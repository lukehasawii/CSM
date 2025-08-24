namespace CSM.API
{
    public interface IVehicleSyncSender
    {
        void SendVehiclePositions(IEnumerable<VehiclePositionData> vehicles);
    }

    public struct VehiclePositionData
    {
        public ushort VehicleId;
        public UnityEngine.Vector3 Position;
    }
}
