using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class RandomDungeonGenerator : Node2D
{
    [Export] public string DungeonPartsLocation;

    [ExportGroup("Tile Configuration")]
    [Export] public int RoomTileSize = 40;
    [Export] public int TileSize = 16;
    [Export] public Vector2I BaseGroundTile;
    [Export] public Array<Vector2I> AlternateGroundTiles = new Array<Vector2I>();
    [Export] public Vector2I BaseWallTile;
    [Export] public Array<Vector2I> AlternateWallTiles = new Array<Vector2I>();
    [Export] public Vector2I EnemySpawnTile;

    [ExportGroup("Dungeon Generator")]
    [Export] public int MaxRooms = 5;
    [Export] public int MaxContinuousPath = 5;
    [Export] public int MaxWidth = 5;
    [Export] public int MaxHeight = 5;

    public static RandomDungeonGenerator Instance;

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

        Instance = this;

        _numberRoomsToGenerate = MaxRooms;
        _rng = new RandomNumberGenerator();
        _rng.Randomize();

        _availableRooms = new RoomDefinitionList(_rng);
        LoadRoomDefinitions();
	}

    public void Clear()
    {
        Logger.Log("Clearing Map...");
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

        Logger.Log("Generating Map...");

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

                Logger.Log("Spawn Room Added");
                continue;
            }

            if (roomIdx == roomSize - 1)
            {
                // Finish Room
                Room finishRoom = (Room)_finishRoomPackedScene.Instantiate();
                AddRoom(finishRoom, _generateTrackPosition);
                finishRoom.SetConnection(Connection.GetOppositeDirection(_previousRoomConnection), true);

                Logger.Log("Finish Room Added at " + _generateTrackPosition.ToString());
                break;
            }

            if (_continuousPath >= MaxContinuousPath)
            {
                Logger.Log("Max track length achieved - resetting node");
                PickNewPathNode();
            }

            // Process additional rooms
            Direction entranceFromPreviousRoom = Connection.GetOppositeDirection(_previousRoomConnection);
            RoomDefinitionList possibleRooms = _availableRooms.GetRoomsWithAvailableConnection(entranceFromPreviousRoom);

            RoomDefinition newRoomDefinition = possibleRooms.GetRandom();
            Room newRoom = (Room)newRoomDefinition.PackedScene.Instantiate();
            AddRoom(newRoom, _generateTrackPosition);

            newRoom.SetConnection(entranceFromPreviousRoom, true);
            Logger.Log("Added Room: " + newRoomDefinition.Name + " at " + _generateTrackPosition.ToString());

            // Pick a random direction to travel
            _previousRoomConnection = GetRandomRoomDirection(newRoom);
            if (_previousRoomConnection == Direction.Unknown)
            {
                Logger.Log("No available next location - resetting node");
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

        if (Player.player is Player)
        {
            Marker2D spawnMarker = _rooms[0].GetNode<Marker2D>("PlayerSpawn");
            if (spawnMarker is Marker2D)
            {
                Player.player.GlobalPosition = spawnMarker.GlobalPosition;
            }
            else
            {
                Player.player.GlobalPosition = new Vector2(
                    RoomTileSize * TileSize / 2,
                    RoomTileSize * TileSize / 2
                );  
            }
        }
        
    }

    private void AddRoom(Room room, Vector2 mapPosition)
    {
        room.GlobalPosition = new Vector2(
            mapPosition.X * Room.TILE_SIZE * Room.ROOM_TILES, 
            mapPosition.Y * Room.TILE_SIZE * Room.ROOM_TILES
        );
        room.MapPosition = mapPosition;

        AddChild(room);

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
                Logger.Log("Could not determine a new room location after ten tries");
                break;
            }
        } while (newDirection == Direction.Unknown);

        _previousRoom.SetConnection(_previousRoomConnection, false);

        _previousRoomConnection = newDirection;
        _generateTrackPosition = GetRelativeLocation(newPathNode, _previousRoomConnection);
        _continuousPath = 0;
        newPathNode.SetConnection(_previousRoomConnection, true);
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
        Array<Direction> possibleDirections = new Array<Direction>();

        if (room.AvailableConnections.North && !room.UsedConnections.North)
        {
            if (!IsRoomAt(GetRelativeLocation(room, Direction.North)))
                possibleDirections.Add(Direction.North);
        }

        if (room.AvailableConnections.East && !room.UsedConnections.East)
        {
            if (!IsRoomAt(GetRelativeLocation(room, Direction.East)))
                possibleDirections.Add(Direction.East);
        }
            
        if (room.AvailableConnections.South && !room.UsedConnections.South)
        {
            if (!IsRoomAt(GetRelativeLocation(room, Direction.South)))
                possibleDirections.Add(Direction.South);
        }
            
        if (room.AvailableConnections.West && !room.UsedConnections.West)
        {
            if (!IsRoomAt(GetRelativeLocation(room, Direction.West)))
                possibleDirections.Add(Direction.West);
        }

        if (possibleDirections.Count == 0)
        {
            Logger.Log("There are no available connections for this room!");
            return Direction.Unknown;
        }
        
        return possibleDirections.PickRandom();
    }

    /// <summary>
    /// Load the dungeon prefab scenes from DungeonPartsLocation
    /// </summary>
    private void LoadRoomDefinitions()
    {
        DirAccess dirAccess = DirAccess.Open(DungeonPartsLocation);
        if (dirAccess == null)
        {
            Logger.Log("Level: Dungeon Parts Location Not Opened: " + DungeonPartsLocation);
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
