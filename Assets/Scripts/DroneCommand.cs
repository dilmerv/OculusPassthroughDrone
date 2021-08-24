using System;

[Serializable]
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
    ccw
}