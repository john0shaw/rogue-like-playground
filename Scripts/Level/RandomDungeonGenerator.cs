using Godot;
using System;
using System.Collections.Generic;

public partial class RandomDungeonGenerator : Node2D
{
    [Export] public string DungeonPartsLocation;

    [ExportGroup("Tile Configuration")]
    [Export] public int RoomTileSize = 40;
    [Export] public int TileSize = 16;

    [ExportGroup("Dungeon Generator")]
    [Export] public int MaxRooms = 10;
    [Export] public int MaxContinuousPath = 5;
    [Export] public int MaxWidth = 5;
    [Export] public int MaxHeight = 5;

    RandomNumberGenerator _rng;
    RoomDefinitionList _availableRooms;
    List<Room> _rooms = new List<Room>();

    PackedScene _spawnRoomPackedScene;
    PackedScene _finishRoomPackedScene;

    Vector2 _generateTrackPosition = Vector2.Zero;
    Direction _previousRoomConnection = Direction.North;
    Room _previousRoom;
    int _continuousPath = 0;

    int _numberRoomsToGenerate;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _spawnRoomPackedScene = ResourceLoader.Load<PackedScene>("res://Scenes/Levels/SpecialRooms/Spawn.tscn");
        _finishRoomPackedScene = ResourceLoader.Load<PackedScene>("res://Scenes/Levels/SpecialRooms/Finish.tscn");

        _numberRoomsToGenerate = MaxRooms;
        _rng = new RandomNumberGenerator();
        _rng.Randomize();

        _availableRooms = new RoomDefinitionList(_rng);
        LoadRoomDefinitions();
	}

    public void Clear()
    {
        GD.Print("Clearing Map...");
        _continuousPath = 0;
        foreach (Room room in _rooms)
        {
            room.QueueFree();
        }
        _rooms.Clear();
    }

    public void Generate(int roomSize = -1)
    {
        if (roomSize == -1)
            roomSize = MaxRooms;

        GD.Print("Generating Map...");

        _generateTrackPosition = Vector2.Zero;
        _previousRoomConnection = Direction.North;

        for (int roomIdx = 0; roomIdx < roomSize; roomIdx++)
        {
            if (roomIdx == 0)
            {
                // Spawn room
                Room spawnRoom = (Room)_spawnRoomPackedScene.Instantiate();
                AddRoom(spawnRoom, Vector2.Zero);
                spawnRoom.SetConnection(Direction.South, true);

                // First room is always Y+1 from the spawn room
                _generateTrackPosition.Y += 1;
                _previousRoomConnection = Direction.South;

                GD.Print("Spawn Room Added");
                continue;
            }

            if (roomIdx == roomSize - 1)
            {
                // Finish Room
                Room finishRoom = (Room)_finishRoomPackedScene.Instantiate();
                AddRoom(finishRoom, _generateTrackPosition);
                finishRoom.SetConnection(Connection.GetOppositeDirection(_previousRoomConnection), true);

                GD.Print("Finish Room Added at " + _generateTrackPosition.ToString());
                break;
            }

            if (_continuousPath >= MaxContinuousPath)
            {
                GD.Print("Max track length achieved - resetting node");
                PickNewPathNode();
            }

            // Process additional rooms
            Direction entranceFromPreviousRoom = Connection.GetOppositeDirection(_previousRoomConnection);
            RoomDefinitionList possibleRooms = _availableRooms.GetRoomsWithAvailableConnection(entranceFromPreviousRoom);

            RoomDefinition newRoomDefinition = possibleRooms.GetRandom();
            Room newRoom = (Room)newRoomDefinition.PackedScene.Instantiate();
            AddRoom(newRoom, _generateTrackPosition);

            newRoom.SetConnection(entranceFromPreviousRoom, true);
            GD.Print("Added Room: " + newRoomDefinition.Name + " at " + _generateTrackPosition.ToString());

            // Pick a random direction to travel
            _previousRoomConnection = GetRandomRoomDirection(newRoom);
            if (_previousRoomConnection == Direction.Unknown)
            {
                GD.Print("No available next location - resetting node");
                PickNewPathNode();
                // subtract from roomIdx to not "lose" a room, as we'll run through the loop again
                roomIdx--;
                continue;
            }

            newRoom.SetConnection(_previousRoomConnection, true);

            switch (_previousRoomConnection)
            {
                case Direction.North:
                    _generateTrackPosition.Y -= 1;
                    break;
                case Direction.East:
                    _generateTrackPosition.X += 1;
                    break;
                case Direction.South:
                    _generateTrackPosition.Y += 1;
                    break;
                case Direction.West:
                    _generateTrackPosition.X -= 1;
                    break;
            }
        }

        Player.player.GlobalPosition = new Vector2(
            RoomTileSize * TileSize / 2,
            RoomTileSize * TileSize / 2
        );
    }

    private void AddRoom(Room room, Vector2 mapPosition)
    {
        if (IsRoomAt(mapPosition))
        {
            GD.Print("Duplicated room at " + mapPosition.ToString());
        }

        AddChild(room);
        room.MapPosition = _generateTrackPosition;
        room.GlobalPosition = GetRoomGlobalPosition(room);
        _continuousPath++;

        _rooms.Add(room);
        _previousRoom = room;
    }

    private void PickNewPathNode()
    {
        Direction newDirection = Direction.Unknown;

        int retries = 0;

        Room newPathNode;

        do
        {
            newPathNode = _rooms[_rng.RandiRange(1, _rooms.Count - 1)];
            newDirection = GetRandomRoomDirection(newPathNode);

            retries++;
            if (retries > 10)
            {
                GD.Print("Could not determine a new room location after ten tries");
                break;
            }
        } while (newDirection == Direction.Unknown);

        _previousRoom.SetConnection(_previousRoomConnection, false);

        _previousRoomConnection = newDirection;
        _generateTrackPosition = GetRelativeLocation(newPathNode, _previousRoomConnection);
        _continuousPath = 0;
        newPathNode.SetConnection(_previousRoomConnection, true);
    }

    private Vector2 GetRoomGlobalPosition(Room room)
    {
        return new Vector2(room.MapPosition.X * RoomTileSize * TileSize, room.MapPosition.Y * RoomTileSize * TileSize);
    }

    private bool IsRoomAt(Vector2 position)
    {
        foreach (Room room in _rooms)
        {
            if (room.MapPosition == position)
                return true;
        }

        return false;
    }

    private Vector2 GetRelativeLocation(Room room, Direction direction)
    {
        Vector2 location = room.MapPosition;
        switch(direction)
        {
            case Direction.North:
                location.Y--;
                break;
            case Direction.East:
                location.X++;
                break;
            case Direction.South:
                location.Y++;
                break;
            case Direction.West:
                location.X--;
                break;
        }

        return location;
    }

    private Direction GetRandomRoomDirection(Room room)
    {
        List<Direction> possibleDirections = new List<Direction>();

        if (room.AvailableConnections.North && !room.UsedConnections.North)
        {
            if (IsRoomAt(GetRelativeLocation(room, Direction.North)))
            {
                //GD.Print("Room already exist to north at: " + GetRelativeLocation(room, Direction.North).ToString());
            }
            else
            {
                possibleDirections.Add(Direction.North);
            }
        }

        if (room.AvailableConnections.East && !room.UsedConnections.East)
        {
            if (IsRoomAt(GetRelativeLocation(room, Direction.East)))
            {
                //GD.Print("Room already exists to east at: " + GetRelativeLocation(room, Direction.East).ToString());
            }
            else
            {
                possibleDirections.Add(Direction.East);
            }
        }
            
        if (room.AvailableConnections.South && !room.UsedConnections.South)
        {
            if (IsRoomAt(GetRelativeLocation(room, Direction.South)))
            {
                //GD.Print("Room already exists to south at: " + GetRelativeLocation(room, Direction.South).ToString());
            }
            else
            {
                possibleDirections.Add(Direction.South);
            }
        }
            
        if (room.AvailableConnections.West && !room.UsedConnections.West)
        {
            if (IsRoomAt(GetRelativeLocation(room, Direction.West)))
            {
                //GD.Print("Room already exists to west at: " + GetRelativeLocation(room, Direction.West).ToString());
            }
            else
            {
                possibleDirections.Add(Direction.West);
            }
        }

        if (possibleDirections.Count == 0)
        {
            GD.Print("There are no available connections for this room!");
            return Direction.Unknown;
        }
        
        return possibleDirections[_rng.RandiRange(0, possibleDirections.Count - 1)];
    }

    /// <summary>
    /// Load the dungeon prefab scenes from DungeonPartsLocation
    /// </summary>
    private void LoadRoomDefinitions()
    {
        DirAccess dirAccess = DirAccess.Open(DungeonPartsLocation);
        if (dirAccess == null)
        {
            GD.Print("Level: Dungeon Parts Location Not Opened: " + DungeonPartsLocation);
            return;
        }

        dirAccess.ListDirBegin();
        string filename = dirAccess.GetNext();
        while (filename != "")
        {
            _availableRooms.Add(new RoomDefinition(DungeonPartsLocation + "/" + filename));
            filename = dirAccess.GetNext();
        }
    }

    public void _on_button_pressed()
    {
        Clear();
        Generate(_numberRoomsToGenerate);
    }

    public void _on_num_rooms_value_changed(float value)
    {
        _numberRoomsToGenerate = (int)value;
    }
}
