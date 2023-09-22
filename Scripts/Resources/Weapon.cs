using Godot;
using System;

public partial class Weapon : Resource
{
    [Export] public Texture2D Texture { get; set; }
    [Export] public string Name;
}
