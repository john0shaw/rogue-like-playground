using Godot;
using Godot.Collections;
using System;

public partial class EnemyResource : Resource
{
    [Export] public string Name;

    [ExportGroup("Stats")]
    [Export] public int MaxHealth;
    [Export] public float MoveSpeed;

    [ExportGroup("Media")]
    [Export] public SpriteFrames SpriteFrames;
    [Export] public AnimationLibrary AnimationLibrary;

    [ExportSubgroup("Combat")]
    [Export] public float DetectionDistance;
    [Export] public int AttackDamage;
    [Export] public float AttackRange;
    [Export] public float AttackSpeed;
    [Export] public bool IsRanged;
    [Export] public ProjectileResource ProjectileResource;
    
    [ExportGroup("Loot")]
    [Export] public Array<Loot> Loot;
}
