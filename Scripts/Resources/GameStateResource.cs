using Godot;
using Godot.Collections;
using System;

public partial class GameStateResource : Resource
{
    [ExportGroup("DungeonState")]
    [Export] public int Level;

    [ExportGroup("PlayerState")]
    [Export] public int Gold;
    [Export] public Array<Item> Inventory = new Array<Item>();
}
