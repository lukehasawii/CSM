using System.Collections.Generic;
using CSM.API;
using CSM.Commands.Data.Internal;
using CSM.Networking;
using ColossalFramework;
using UnityEngine;

namespace CSM.BaseGame.Helpers
{
    /// <summary>
    ///     Helper to periodically sync vehicle positions from the server.
    /// </summary>
    public static class VehicleSyncHelper
    {
        public static void Send()
        {
            if (Command.CurrentRole != MultiplayerRole.Server)
                return;

            var list = new List<VehiclePositionCommand.VehicleData>();
            VehicleManager vm = VehicleManager.instance;
            var buffer = vm.m_vehicles.m_buffer;
            for (ushort i = 1; i < buffer.Length; i++)
            {
                if ((buffer[i].m_flags & Vehicle.Flags.Created) != 0)
                {
                    list.Add(new VehiclePositionCommand.VehicleData
                    {
                        VehicleId = i,
                        Position = buffer[i].GetLastFramePosition()
                    });
                }
            }

            if (list.Count > 0)
            {
                Command.SendToAll(new VehiclePositionCommand
                {
                    Vehicles = list
                });
            }
        }
    }
}
