using Godot;
using Godot.Collections;
using System;

public partial class Item : Resource
{
    [Export] public int ID;
    [Export] public string Name;
    [Export] public int Value;
    [Export] public int Count = 1;
    [Export] public Texture2D Texture;
}
