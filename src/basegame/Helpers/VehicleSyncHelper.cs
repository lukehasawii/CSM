using System.Collections.Generic;
using CSM.API;
using ColossalFramework;
using UnityEngine;

namespace CSM.BaseGame.Helpers
{
    public static class VehicleSyncHelper
    {
        public static void Send(IVehicleSyncSender sender)
        {
            // Only send from server
            if (!sender.IsServer())
                return;

            var list = new List<VehiclePositionData>();
            VehicleManager vm = VehicleManager.instance;
            var buffer = vm.m_vehicles.m_buffer;
            for (ushort i = 1; i < buffer.Length; i++)
            {
                if ((buffer[i].m_flags & Vehicle.Flags.Created) != 0)
                {
                    list.Add(new VehiclePositionData
                    {
                        VehicleId = i,
                        Position = buffer[i].GetLastFramePosition()
                    });
                }
            }

            if (list.Count > 0)
            {
                sender.SendVehiclePositions(list);
            }
        }
    }
}
