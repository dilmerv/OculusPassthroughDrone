using System;
using System.Linq;

public static class DroneStatsExtension
{
    public static void UpdateStats(this DroneStats stats, string droneReponse)
    {
        var statsDelimited = droneReponse.Split(';');

        foreach (var stat in statsDelimited)
        {
            var statSplit = stat.Split(':');
            if (Enum.TryParse(statSplit.First(), true, out DroneStatsAttribute attribute))
            {
                var value = statSplit.Last();
                switch (attribute)
                {
                    case DroneStatsAttribute.bat:
                        stats.battery = value;
                        break;
                    case DroneStatsAttribute.pitch:
                        stats.pitch = value;
                        break;
                    case DroneStatsAttribute.roll:
                        stats.roll = value;
                        break;
                    case DroneStatsAttribute.yaw:
                        stats.yaw = value;
                        break;
                    case DroneStatsAttribute.h:
                        stats.height = value;
                        break;
                    case DroneStatsAttribute.time:
                        stats.time = value;
                        break;
                    case DroneStatsAttribute.tof:
                        stats.tof = value;
                        break;
                    case DroneStatsAttribute.baro:
                        stats.baro = value;
                        break;
                    case DroneStatsAttribute.templ:
                        stats.templ = value;
                        break;
                    case DroneStatsAttribute.temph:
                        stats.temph = value;
                        break;
                    case DroneStatsAttribute.agx:
                        stats.agx = value;
                        break;
                    case DroneStatsAttribute.agy:
                        stats.agy = value;
                        break;
                    case DroneStatsAttribute.agz:
                        stats.agz = value;
                        break;
                }
            }
        }
    }
}
