using Godot;
using System;
using static Godot.TextServer;


public enum Direction
{
    North,
    East,
    South,
    West,
    Unknown
};

public partial class Connection
{
    public bool North;
    public bool East;
    public bool South;
    public bool West;

    public Connection(bool north = false, bool east = false, bool south = false, bool west = false)
    {
        North = north;
        East = east;
        South = south;
        West = west;
    }

    public static Direction GetOppositeDirection(Direction direction)
    {
        if (direction == Direction.North) return Direction.South;
        if (direction == Direction.East) return Direction.West;
        if (direction == Direction.South) return Direction.North;
        if (direction == Direction.West) return Direction.East;

        GD.Print("Could not determine opposite direction!");
        return Direction.North;
    }

    public bool Get(Direction direction)
    {
        if (direction == Direction.North) return North;
        if (direction == Direction.East) return East;
        if (direction == Direction.South) return South;
        if (direction == Direction.West) return West;

        return false;
    }

    public void Set(Direction direction, bool state)
    {
        if (direction == Direction.North) North = state;
        if (direction == Direction.East) East = state;
        if (direction == Direction.South) South = state;
        if (direction == Direction.West) West = state;
    }
}
