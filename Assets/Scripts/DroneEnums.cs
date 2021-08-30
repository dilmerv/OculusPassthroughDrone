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
    speed,
    rc,
    battery
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