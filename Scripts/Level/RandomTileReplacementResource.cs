using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class RandomTileReplacementResource : Resource
{
    [Export] public Vector2I BaseTile;
    [Export] public Array<Vector2I> ReplacementTiles = new Array<Vector2I>();
    [Export] public float ReplacementChance;
}
