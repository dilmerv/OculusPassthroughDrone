public static class DroneStatsExtension
{
    public static void UpdateStats(this DroneStats stats, DroneReponse droneReponse)
    {
        switch (droneReponse.Command)
        {
            case DroneCommand.battery:
                stats.battery = droneReponse.Response;
                break;
            case DroneCommand.speed:
                stats.speed = droneReponse.Response;
                break;
            case DroneCommand.height:
                stats.height = droneReponse.Response;
                break;
            case DroneCommand.temp:
                stats.temp = droneReponse.Response;
                break;
            case DroneCommand.time:
                stats.time = droneReponse.Response;
                break;
            case DroneCommand.tof:
                stats.tof = droneReponse.Response;
                break;
            case DroneCommand.acceleration:
                stats.acceleration = droneReponse.Response;
                break;
            case DroneCommand.baro:
                stats.baro = droneReponse.Response;
                break;
            case DroneCommand.attitude:
                stats.attitude = droneReponse.Response;
                break;

        }
    }

    public static bool IsStatsCommand(this DroneCommand command)
    {
        return command == DroneCommand.battery ||
            command == DroneCommand.speed ||
            command == DroneCommand.height ||
            command == DroneCommand.temp ||
            command == DroneCommand.time ||
            command == DroneCommand.tof ||
            command == DroneCommand.acceleration ||
            command == DroneCommand.baro ||
            command == DroneCommand.attitude;
    }
}
