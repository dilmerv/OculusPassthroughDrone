public class DroneRequest
{
    private string payload;

    public RequestType RequestType { get; set; } = RequestType.ControlCommand;

    public DroneCommand Command { get; set; }

    public string Payload
    {
        get
        {
            var addedParam = RequestType == RequestType.ReadCommand ? "?" : string.Empty;
            return string.IsNullOrEmpty(payload) ? $"{Command}{addedParam}" : $"{payload}{addedParam}";
        }
        set
        {
            payload = value;
        }
    }
}