using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RoomDefinition
{
    public string Name;
    public PackedScene PackedScene;
    public Connection AvailableConnections;
    public Connection UsedConnections;

    public RoomDefinition(string filePath)
    {
        PackedScene = ResourceLoader.Load<PackedScene>(filePath);
        Room room = (Room)PackedScene.Instantiate();

        Name = Path.GetFileNameWithoutExtension(filePath);
        AvailableConnections = new Connection(
            room.HasNorthConnection, 
            room.HasEastConnection, 
            room.HasSouthConnection, 
            room.HasWestConnection
        );
        UsedConnections = new Connection(
            room.IsNorthConnection, 
            room.IsEastConnection, 
            room.IsSouthConnection, 
            room.IsWestConnection
        );

        room.QueueFree();
    }
}

