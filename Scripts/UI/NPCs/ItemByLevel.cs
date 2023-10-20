using Godot;
using System;


[GlobalClass]
public partial class ItemByLevel: Node
{
    [Export] public Weapon Weapon;
    [Export] public int Cost;
    [Export] public int MinLevel;
}
