using System.Collections.Generic;
using CSM.API;
using CSM.Commands.Data.Internal;
using CSM.Networking;

namespace CSM
{
    public class VehicleSyncSender : IVehicleSyncSender
    {
        public void SendVehiclePositions(IEnumerable<VehiclePositionData> vehicles)
        {
            var commandList = new List<VehiclePositionCommand.VehicleData>();

            foreach (var v in vehicles)
            {
                commandList.Add(new VehiclePositionCommand.VehicleData
                {
                    VehicleId = v.VehicleId,
                    Position = v.Position
                });
            }

            if (commandList.Count > 0)
            {
                Command.SendToAll(new VehiclePositionCommand
                {
                    Vehicles = commandList
                });
            }
        }

        public bool IsServer() => Command.CurrentRole == MultiplayerRole.Server;
    }
}
