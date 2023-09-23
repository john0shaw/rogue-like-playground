using Godot;
using System;

public partial class Weapon : Resource
{
    public enum AttackTypeEnum
    {
        Swing,
        Stab
    }

    [Export] public Texture2D Texture { get; set; }
    [Export] public string Name;
    [Export] public AttackTypeEnum AttackType;
}
