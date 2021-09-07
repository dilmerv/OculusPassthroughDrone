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
    RotateRight,
    RotateLeft,
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

public enum DroneStatsAttribute
{
    bat,
    pitch,
    roll,
    yaw,
    h,
    time,
    tof,
    baro,
    templ,
    temph,
    agx,
    agy,
    agz
}

public enum ResponseType
{ 
    ok,
    error
}