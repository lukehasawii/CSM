using CSM.API.Commands;
using ProtoBuf;
using System.Collections.Generic;
using UnityEngine;

namespace CSM.Commands.Data.Internal
{
    /// <summary>
    ///     Periodically sends vehicle position updates from the server.
    /// </summary>
    /// Sent by:
    /// - VehicleSyncHelper
    [ProtoContract]
    public class VehiclePositionCommand : CommandBase
    {
        [ProtoMember(1)]
        public List<VehicleData> Vehicles { get; set; }

        [ProtoContract]
        public class VehicleData
        {
            [ProtoMember(1)]
            public ushort VehicleId { get; set; }

            [ProtoMember(2)]
            public Vector3 Position { get; set; }
        }
    }
}
