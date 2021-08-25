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
    rc
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
    // core actions
    Connect,
    InitializeSDK,
    TakeOff,
    Landing,
    Emergency
}