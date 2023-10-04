using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;


public class RoomDefinitionList : List<RoomDefinition>
{
    RandomNumberGenerator _rng;

    /// <summary>
    /// Create the room definition list using a provided RNG.  Create a new RNG and randomize if no rng provided from calling class
    /// </summary>
    /// <param name="rng"></param>
    public RoomDefinitionList(RandomNumberGenerator rng = null)
    {
        if (rng == null)
        {
            _rng = new RandomNumberGenerator();
            _rng.Randomize();
        }
        else
        {
            _rng = rng;
        }
    }

    public void SetRng(RandomNumberGenerator rng)
    {
        _rng = rng;
    }

    /// <summary>
    /// Get a new RoomDefinitionList including all rooms with an available connection in the given direction
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public RoomDefinitionList GetRoomsWithAvailableConnection(Direction direction)
    {
        RoomDefinitionList roomsWithAvailableConnection = new RoomDefinitionList();
        foreach (RoomDefinition roomDefinition in this)
        {
            if (roomDefinition.AvailableConnections.Get(direction))
                roomsWithAvailableConnection.Add(roomDefinition);
        }

        return roomsWithAvailableConnection;
    }

    /// <summary>
    /// Get a random room definition from the list
    /// </summary>
    /// <returns></returns>
    public RoomDefinition GetRandom()
    {
        return this[_rng.RandiRange(0, Count - 1)];
    }
}

