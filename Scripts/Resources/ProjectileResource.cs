using Godot;
using System;

public partial class ProjectileResource : Resource
{
    [Export] public Texture2D Sprite;
    [Export] public float Speed;
    [Export] public float Range;
    [Export] public float Damage;
}
