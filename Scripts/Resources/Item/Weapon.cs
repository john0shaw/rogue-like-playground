using Godot;
using System;

public partial class Weapon : Item
{
    public enum AttackTypeEnum
    {
        Swing,
        Stab
    }

    [Export] public AttackTypeEnum AttackType;
    [Export] public int Damage = 1;
}
