public enum DroneCommand
{
    command,
    takeoff,
    land,
    streamon, 
    streamoff,
    emergency,
    up,
    down,
    left,
    right,
    forward,
    back,
    cw,
    ccw,
    rc,

    // get commands
    battery,
    speed,
    time,
    height,
    temp,
    attitude,
    baro,
    acceleration,
    tof,
    wifi

}

public enum DroneAction
{
    // move actions
    Left,
    Right,
    Forward,
    Backward,
    Up,
    Down,
    YawLeft,
    YawRight,
    // core actions
    Connect,
    InitializeSDK,
    TakeOff,
    Landing,
    Emergency
}

public enum DroneSpeedType
{ 
    Positive,
    Negative
}

public enum RequestType
{
    ControlCommand,
    ReadCommand,
}