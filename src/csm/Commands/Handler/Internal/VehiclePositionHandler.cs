using CSM.API.Commands;
using CSM.Commands.Data.Internal;
using ColossalFramework;
using UnityEngine;

namespace CSM.Commands.Handler.Internal
{
    public class VehiclePositionHandler : CommandHandler<VehiclePositionCommand>
    {
        public VehiclePositionHandler()
        {
            TransactionCmd = false;
        }

        protected override void Handle(VehiclePositionCommand command)
        {
            // Update vehicle positions on clients.
            foreach (var data in command.Vehicles)
            {
                try
                {
                    ref Vehicle vehicle = ref VehicleManager.instance.m_vehicles.m_buffer[data.VehicleId];
                    vehicle.SetPosition(data.Position);
                }
                catch
                {
                    // Ignore if vehicle does not exist or method fails.
                }
            }
        }
    }
}
